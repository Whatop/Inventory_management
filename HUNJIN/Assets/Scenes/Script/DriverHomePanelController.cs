using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DriverHomePanelController : MonoBehaviour
{
    [Header("Refs")]
    public AppFlow flow;
    public DeliveryService service;
    public NaverMapLink naverMapLink;

    [Header("Confirm UI")]
    public ChoicePanelController choicePanel;

    [Header("Login")]
    public TMP_InputField driverIdInput;
    public TMP_InputField driverNameInput;
    public Button loginButton;
    public Button logoutButton; // 로그인 상태일 때만 활성화

    [Header("View Mode (Optional)")]
    public Toggle availableToggle;
    public Toggle myToggle;

    [Header("Driver Status")]
    public TMP_Dropdown statusDropdown; // IDLE/ON_DELIVERY/BREAK
    public Button refreshButton;

    [Header("List")]
    public Transform contentRoot;
    public GameObject driverDeliveryItemPrefab;

    [Header("Heartbeat")]
    public float heartbeatIntervalSec = 15f;

    Coroutine _heartbeatCo;

    enum ViewMode { Available, My }
    ViewMode mode = ViewMode.My;

    void Awake()
    {
        if (loginButton) loginButton.onClick.AddListener(() => StartCoroutine(CoLogin()));
        if (logoutButton) logoutButton.onClick.AddListener(RequestLogout);

        if (refreshButton) refreshButton.onClick.AddListener(Refresh);

        if (statusDropdown)
            statusDropdown.onValueChanged.AddListener(_ => OnStatusChanged());

        if (availableToggle)
            availableToggle.onValueChanged.AddListener(isOn => { if (isOn) { mode = ViewMode.Available; Refresh(); } });

        if (myToggle)
            myToggle.onValueChanged.AddListener(isOn => { if (isOn) { mode = ViewMode.My; Refresh(); } });

        RefreshLoginUI();
    }

    void OnEnable()
    {
        RefreshLoginUI();
        // 자동로그인 세션이 있으면 UI 반영
        if (service && service.IsLoggedIn())
        {
            if (driverIdInput) driverIdInput.text = service.currentDriverId;
            if (driverNameInput) driverNameInput.text = service.currentDriverName;

            if (statusDropdown)
                SetDropdownValueByText(statusDropdown, service.currentDriverStatus);

            StartHeartbeat();
            Refresh();
        }

        if (availableToggle && myToggle)
        {
            if (!availableToggle.isOn && !myToggle.isOn) myToggle.isOn = true;
            mode = availableToggle.isOn ? ViewMode.Available : ViewMode.My;
        }

        RefreshLoginUI();
    }

    void OnDisable()
    {
        StopHeartbeat();
    }

    void RefreshLoginUI()
    {
        bool loggedIn = service && service.IsLoggedIn();

        if (logoutButton) logoutButton.gameObject.SetActive(loggedIn);
        if (loginButton) loginButton.gameObject.SetActive(!loggedIn);

        if (driverIdInput) driverIdInput.interactable = !loggedIn;
        if (driverNameInput) driverNameInput.interactable = !loggedIn;
    }

    IEnumerator CoLogin()
    {
        string id = driverIdInput ? driverIdInput.text.Trim() : "";
        string name = driverNameInput ? driverNameInput.text.Trim() : "";

        if (string.IsNullOrEmpty(id))
        {
            RefreshLoginUI();
            Debug.LogWarning("[Driver] driverId is empty");
            yield break;
        }

        // 세션 저장(자동로그인)
        service.SetDriver(id, name);

        ApiResponse resp = null;
        yield return service.UpsertDriver(id, name, r => resp = r);

        if (resp == null || resp.result != "OK")
        {
            Debug.LogWarning($"[DriverUpsert] FAIL: {resp?.msg}");
            yield break;
        }

        OnStatusChanged();
        StartHeartbeat();
        Refresh();
        RefreshLoginUI();
    }

    void RequestLogout()
    {
        if (choicePanel)
        {
            choicePanel.Show(
                "로그아웃 하시겠습니까?",
                onYes: () => LogoutConfirmed(),
                onNo: null
            );
        }
        else
        {
            // 확인창 없으면 즉시 로그아웃
            LogoutConfirmed();
        }
    }

    void LogoutConfirmed()
    {
        StopHeartbeat();
        if (service) service.LogoutDriver();

        if (driverIdInput) driverIdInput.text = "";
        if (driverNameInput) driverNameInput.text = "";
        if (statusDropdown) SetDropdownValueByText(statusDropdown, "IDLE");

        ClearChildren(contentRoot);
        RefreshLoginUI();
    }

    void OnStatusChanged()
    {
        if (!statusDropdown || !service) return;
        string status = statusDropdown.options[statusDropdown.value].text;
        service.SetDriverStatus(status);
    }

    public void Refresh()
    {
        if (!service || !service.IsLoggedIn())
        {
            Debug.LogWarning("[Driver] Not logged in");
            return;
        }

        if (mode == ViewMode.Available)
            StartCoroutine(CoFetchAvailable());
        else
            StartCoroutine(CoFetchMyDeliveries());
    }

    IEnumerator CoFetchAvailable()
    {
        ApiResponse resp = null;
        yield return service.FetchAvailableDeliveries(r => resp = r);

        if (resp == null || resp.result != "OK")
        {
            Debug.LogWarning($"[AvailableDeliveries] FAIL: {resp?.msg}");
            yield break;
        }

        var rows = service.ParseDeliveryRows(resp.value);
        Render(rows, isAvailableList: true);
    }

    IEnumerator CoFetchMyDeliveries()
    {
        ApiResponse resp = null;
        yield return service.FetchDeliveriesForDriver(service.currentDriverId, "", r => resp = r);

        if (resp == null || resp.result != "OK")
        {
            Debug.LogWarning($"[MyDeliveries] FAIL: {resp?.msg}");
            yield break;
        }

        var rows = service.ParseDeliveryRows(resp.value);
        Render(rows, isAvailableList: false);
    }

    void Render(List<DeliveryDto> rows, bool isAvailableList)
    {
        ClearChildren(contentRoot);

        foreach (var d in rows)
        {
            var go = Instantiate(driverDeliveryItemPrefab, contentRoot);

            // ... (여기 Render는 네 기존 버전 그대로 두고)
            // “배차 수락(Claim)” 호출부만 아래처럼 Confirm로 감싸면 됨.

            var claimBtn = FindButton(go, "ClaimButton");
            if (claimBtn)
            {
                claimBtn.gameObject.SetActive(isAvailableList);
                claimBtn.onClick.RemoveAllListeners();

                claimBtn.onClick.AddListener(() =>
                {
                    RequestClaimConfirm(d.deliveryId);
                });
            }
        }
    }

    void RequestClaimConfirm(string deliveryId)
    {
        if (choicePanel)
        {
            choicePanel.Show(
                "배차 수락하시겠습니까?",
                onYes: () => StartCoroutine(CoClaim(deliveryId)),
                onNo: null
            );
        }
        else
        {
            StartCoroutine(CoClaim(deliveryId));
        }
    }

    IEnumerator CoClaim(string deliveryId)
    {
        ApiResponse resp = null;
        yield return service.Claim(deliveryId, service.currentDriverId, r => resp = r);

        if (resp == null || resp.result != "OK")
        {
            Debug.LogWarning($"[Claim] FAIL: {resp?.msg}");
            yield break;
        }

        if (myToggle) myToggle.isOn = true;
        mode = ViewMode.My;
        Refresh();
    }

    void StartHeartbeat()
    {
        if (_heartbeatCo != null) StopCoroutine(_heartbeatCo);
        _heartbeatCo = StartCoroutine(CoHeartbeat());
    }

    void StopHeartbeat()
    {
        if (_heartbeatCo != null)
        {
            StopCoroutine(_heartbeatCo);
            _heartbeatCo = null;
        }
    }

    IEnumerator CoHeartbeat()
    {
        while (true)
        {
            ApiResponse resp = null;
            yield return service.Heartbeat(
                service.currentDriverId,
                service.currentDriverName,
                service.currentDriverStatus,
                "", "",
                r => resp = r
            );

            yield return new WaitForSeconds(heartbeatIntervalSec);
        }
    }

    static void ClearChildren(Transform t)
    {
        if (!t) return;
        for (int i = t.childCount - 1; i >= 0; i--)
            Destroy(t.GetChild(i).gameObject);
    }

    static Button FindButton(GameObject root, string childName)
    {
        var tf = root.transform.Find(childName);
        return tf ? tf.GetComponent<Button>() : null;
    }

    static void SetDropdownValueByText(TMP_Dropdown dd, string text)
    {
        if (!dd) return;
        text = (text ?? "").Trim();
        for (int i = 0; i < dd.options.Count; i++)
        {
            if ((dd.options[i].text ?? "").Trim() == text)
            {
                dd.SetValueWithoutNotify(i);
                return;
            }
        }
    }
}
