using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryCreatePanelController : MonoBehaviour
{
    [Header("Refs")]
    public AppFlow flow;
    public DeliveryService service;

    [Header("Inputs (TMP)")]
    public TMP_InputField storeNameInput;
    public TMP_InputField customerNameInput;
    public TMP_InputField phoneInput;
    public TMP_InputField addressInput;
    public TMP_InputField pointsInput;
    public TMP_InputField noteInput;

    [Header("Buttons")]
    public Button submitButton;
    public Button cancelButton;

    [Header("Optional")]
    public TMP_Text resultText;

    void Awake()
    {
        if (submitButton) submitButton.onClick.AddListener(() => StartCoroutine(CoSubmit()));
        if (cancelButton) cancelButton.onClick.AddListener(() => flow.ShowDeliveryListAdmin());
    }

    IEnumerator CoSubmit()
    {
        string storeName = storeNameInput ? storeNameInput.text.Trim() : "";
        string customerName = customerNameInput ? customerNameInput.text.Trim() : "";
        string phone = phoneInput ? phoneInput.text.Trim() : "";
        string address = addressInput ? addressInput.text.Trim() : "";
        string note = noteInput ? noteInput.text.Trim() : "";

        int points = 0;
        if (pointsInput && !string.IsNullOrWhiteSpace(pointsInput.text))
            int.TryParse(pointsInput.text.Trim(), out points);

        // MVP: lat/lng는 비워둬도 됨 (나중에 주소→좌표 붙이기)
        string lat = "";
        string lng = "";

        ApiResponse resp = null;
        yield return service.CreateDelivery(storeName, customerName, phone, address, lat, lng, points, note, r => resp = r);

        if (resp == null || resp.result != "OK")
        {
            if (resultText) resultText.text = $"실패: {resp?.msg}";
            Debug.LogWarning($"[DeliveryCreate] FAIL: {resp?.msg}");
            yield break;
        }

        if (resultText) resultText.text = "등록 완료!";
        flow.ShowDeliveryListAdmin();
    }
}
