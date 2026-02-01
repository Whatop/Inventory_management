using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SheetApiClient : MonoBehaviour
{
    [SerializeField] private string webAppUrl; // Apps Script 배포 URL
    public string WebAppUrl => webAppUrl;

    public IEnumerator Post(Dictionary<string, string> form, Action<ApiResponse> onDone)
    {
        if (string.IsNullOrEmpty(webAppUrl))
        {
            onDone?.Invoke(new ApiResponse { result = "FAIL", msg = "webAppUrl is empty" });
            yield break;
        }

        WWWForm wwwForm = new WWWForm();
        foreach (var kv in form)
            wwwForm.AddField(kv.Key, kv.Value ?? "");

        using (UnityWebRequest req = UnityWebRequest.Post(webAppUrl, wwwForm))
        {
            req.timeout = 15;
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                onDone?.Invoke(new ApiResponse { result = "FAIL", msg = $"Network error: {req.error}" });
                yield break;
            }

            // Apps Script가 JSON을 내려줌
            string json = req.downloadHandler.text;

            ApiResponse resp;
            try
            {
                resp = JsonUtility.FromJson<ApiResponse>(json);
                if (resp == null) resp = new ApiResponse { result = "FAIL", msg = "Json parse failed(null)" };
                if (string.IsNullOrEmpty(resp.result)) resp.result = "FAIL";
            }
            catch (Exception ex)
            {
                resp = new ApiResponse { result = "FAIL", msg = "Json parse exception: " + ex.Message, value = json };
            }

            onDone?.Invoke(resp);
        }
    }
}
