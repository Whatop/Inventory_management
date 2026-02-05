using System;
using UnityEngine;

public class NaverMapLink : MonoBehaviour
{
    [Tooltip("안드로이드 패키지명(선택). appname 파라미터에 사용하면 좋음")]
    public string appName = "com.yourcompany.yourapp";

    // 좌표 있으면 route, 없으면 search
    public void OpenRoute(string lat, string lng, string address, string placeName = "")
    {
        lat = (lat ?? "").Trim();
        lng = (lng ?? "").Trim();
        address = address ?? "";
        placeName = string.IsNullOrWhiteSpace(placeName) ? "목적지" : placeName;

        // 1) 좌표 있으면 길찾기(목적지)
        if (double.TryParse(lat, out var dlat) && double.TryParse(lng, out var dlng))
        {
            // nmap://route/public?dlat=...&dlng=...&dname=...&appname=...
            string uri = $"nmap://route/public?dlat={dlat}&dlng={dlng}&dname={Uri.EscapeDataString(placeName)}&appname={Uri.EscapeDataString(appName)}";
            Application.OpenURL(uri);

            // 웹 fallback 링크도 같이 준비(유저가 앱이 없을 수도 있으니)
            // (앱 실행 안 됐는지 확인은 Unity에서 어려움 → 필요하면 fallback 버튼을 따로 두는 게 가장 확실)
            return;
        }

        // 2) 좌표 없으면 주소 검색
        if (!string.IsNullOrWhiteSpace(address))
        {
            string q = Uri.EscapeDataString(address);
            string uri = $"nmap://search?query={q}&appname={Uri.EscapeDataString(appName)}";
            Application.OpenURL(uri);
            return;
        }

        Debug.LogWarning("[NaverMapLink] No lat/lng and no address.");
    }
}
