using System;
using UnityEngine;

/// <summary>
/// 네이버지도 앱(설치 시) → 네이버지도 웹(미설치 시)으로 열기.
/// - Android: PackageManager.queryIntentActivities 로 설치 여부 확인(공식 가이드 방식)
/// - iOS: Application.CanOpenURL("nmap://") 사용(Info.plist에 LSApplicationQueriesSchemes 필요)
/// </summary>
public class NaverMapLink : MonoBehaviour
{
    [Header("App Identity")]
    [Tooltip("appname 파라미터. Android=applicationId, iOS=bundleId, 웹=URL (공식 문서 참고)")]
    public string appName = "com.yourcompany.yourapp";

    /// <summary>
    /// 좌표가 있으면 길찾기(route/public), 없으면 검색(search)로 연다.
    /// </summary>
    public void OpenRoute(string lat, string lng, string address, string placeName = "목적지")
    {
        lat = (lat ?? "").Trim();
        lng = (lng ?? "").Trim();
        address = address ?? "";
        placeName = string.IsNullOrWhiteSpace(placeName) ? "목적지" : placeName;

        // 1) 좌표 있으면 길찾기(목적지)
        if (double.TryParse(lat, out var dlat) && double.TryParse(lng, out var dlng))
        {
            string schemeUrl =
                $"nmap://route/public?dlat={dlat}&dlng={dlng}&dname={Uri.EscapeDataString(placeName)}&appname={Uri.EscapeDataString(appName)}";

            // 앱이 없으면 웹 검색으로 fallback(주소 우선, 없으면 "lat,lng" 검색)
            string webQuery = !string.IsNullOrWhiteSpace(address) ? address : $"{dlat},{dlng}";
            string webUrl = BuildWebSearchUrl(webQuery);

            TryOpenNaverMapOrFallback(schemeUrl, webUrl);
            return;
        }

        // 2) 좌표 없으면 주소 검색
        if (!string.IsNullOrWhiteSpace(address))
        {
            string q = Uri.EscapeDataString(address);
            string schemeUrl = $"nmap://search?query={q}&appname={Uri.EscapeDataString(appName)}";
            string webUrl = BuildWebSearchUrl(address);

            TryOpenNaverMapOrFallback(schemeUrl, webUrl);
            return;
        }

        Debug.LogWarning("[NaverMapLink] No lat/lng and no address.");
    }

    static string BuildWebSearchUrl(string query)
    {
        query = query ?? "";
        return $"https://map.naver.com/v5/search/{Uri.EscapeDataString(query)}";
    }

    void TryOpenNaverMapOrFallback(string schemeUrl, string fallbackWebUrl)
    {
        if (CanOpenNaverMapScheme(schemeUrl))
        {
            Application.OpenURL(schemeUrl);
            return;
        }

        // 요구사항: 앱이 없으면 웹페이지로
        if (!string.IsNullOrWhiteSpace(fallbackWebUrl))
            Application.OpenURL(fallbackWebUrl);
        else
            Debug.LogWarning("[NaverMapLink] fallbackWebUrl is empty.");
    }

    bool CanOpenNaverMapScheme(string schemeUrl)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var pm = activity.Call<AndroidJavaObject>("getPackageManager"))
            using (var uriClass = new AndroidJavaClass("android.net.Uri"))
            using (var uri = uriClass.CallStatic<AndroidJavaObject>("parse", schemeUrl))
            using (var intent = new AndroidJavaObject("android.content.Intent", "android.intent.action.VIEW", uri))
            {
                intent.Call<AndroidJavaObject>("addCategory", "android.intent.category.BROWSABLE");

                // PackageManager.MATCH_DEFAULT_ONLY = 0x00010000
                const int MATCH_DEFAULT_ONLY = 0x00010000;
                var list = pm.Call<AndroidJavaObject>("queryIntentActivities", intent, MATCH_DEFAULT_ONLY);
                return list != null && list.Call<int>("size") > 0;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("[NaverMapLink] CanOpenNaverMapScheme(Android) failed: " + e.Message);
            return false;
        }
#elif UNITY_IOS && !UNITY_EDITOR
        try
        {
            // iOS: Info.plist에 LSApplicationQueriesSchemes -> nmap 추가 필요
            return Application.CanOpenURL("nmap://");
        }
        catch (Exception e)
        {
            Debug.LogWarning("[NaverMapLink] CanOpenNaverMapScheme(iOS) failed: " + e.Message);
            return false;
        }
#else
        // 에디터/기타 플랫폼: 웹으로 테스트 가능하게 false
        return false;
#endif
    }
}
