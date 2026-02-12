using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryCreatePanelController : MonoBehaviour
{
    [Header("Refs")]
    public AppFlow flow;
    public DeliveryService service;

    [Header("Loading (Optional)")]
    public GameObject loadingRoot;
    public float loadingMinDuration = 0.25f;

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

    [Header("Enrollment Popup (Interaction_UI)")]
    public GameObject enrollmentRoot;          // Interaction_UI/Enrollment
    public TMP_Text enrollmentText;            // Enrollment/.../Text(TMP)
    public Button enrollmentOkButton;          // Enrollment/Button

    float _loadingShownAt = -1f;

    void Awake()
    {
        if (submitButton) submitButton.onClick.AddListener(() => StartCoroutine(CoSubmit()));
        if (cancelButton) cancelButton.onClick.AddListener(() => flow.ShowDeliveryListAdmin());

        if (enrollmentOkButton)
        {
            enrollmentOkButton.onClick.RemoveAllListeners();
            enrollmentOkButton.onClick.AddListener(() => SetEnrollment(false));
        }

        SetLoading(false);
        SetEnrollment(false);
    }

    IEnumerator CoSubmit()
    {
        if (!service)
        {
            Debug.LogWarning("[DeliveryCreate] service is null");
            yield break;
        }

        string storeName = storeNameInput ? storeNameInput.text.Trim() : "";
        string customerName = customerNameInput ? customerNameInput.text.Trim() : "";
        string phone = phoneInput ? phoneInput.text.Trim() : "";
        string address = addressInput ? addressInput.text.Trim() : "";
        string note = noteInput ? noteInput.text.Trim() : "";

        int points = 0;
        if (pointsInput && !string.IsNullOrWhiteSpace(pointsInput.text))
            int.TryParse(pointsInput.text.Trim(), out points);

        // MVP: lat/lng는 비워둬도 됨
        string lat = "";
        string lng = "";

        _loadingShownAt = Time.unscaledTime;
        SetLoading(true);

        ApiResponse resp = null;
        yield return service.CreateDelivery(storeName, customerName, phone, address, lat, lng, points, note, r => resp = r);

        // 로딩 최소 노출
        float elapsed = Time.unscaledTime - _loadingShownAt;
        float wait = Mathf.Max(0f, loadingMinDuration - elapsed);
        if (wait > 0f) yield return new WaitForSecondsRealtime(wait);
        SetLoading(false);

        if (resp == null || resp.result != "OK")
        {
            ShowEnrollment($"실패했습니다.\n{resp?.msg}");
            yield break;
        }

        ShowEnrollment("등록되었습니다.");
        // 확인 버튼 누르면 닫히고, 목록으로 이동시키고 싶으면 아래처럼:
        // enrollmentOkButton.onClick.AddListener(() => flow.ShowDeliveryListAdmin());
        // 근데 AddListener 누적 방지하려면 아래 방식 추천:
        if (enrollmentOkButton)
        {
            enrollmentOkButton.onClick.RemoveAllListeners();
            enrollmentOkButton.onClick.AddListener(() =>
            {
                SetEnrollment(false);
                flow.ShowDeliveryListAdmin();
            });
        }
    }

    void ShowEnrollment(string msg)
    {
        if (enrollmentText) enrollmentText.text = msg ?? "";
        SetEnrollment(true);
    }

    void SetEnrollment(bool on)
    {
        if (!enrollmentRoot) return;
        enrollmentRoot.SetActive(on);
    }

    void SetLoading(bool on)
    {
        if (!loadingRoot) return;
        loadingRoot.SetActive(on);
    }
}
