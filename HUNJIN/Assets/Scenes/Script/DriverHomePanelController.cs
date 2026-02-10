using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Button logoutButton;

    [Header("Login-Only UI")]
    public GameObject myPointsPanelRoot;
    public GameObject myDeliveriesRoot;

    [Header("Points Text (Optional)")]
    public TMP_Text todayPointsText;
    public TMP_Text totalPointsText;

    [Header("View Mode (Optional)")]
    public Toggle availableToggle;
    public Toggle myToggle;

    [Header("Driver Status")]
    public TMP_Dropdown statusDropdown;
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

        if (myPointsPanelRoot) myPointsPanelRoot.SetActive(loggedIn);
        if (myDeliveriesRoot) myDeliveriesRoot.SetActive(loggedIn);
    }

    IEnumerator CoLogin()
    {
        string id = driverIdInput ? driverIdInput.text.Trim() : "";
        string name = driverNameInput ? driverNameInput.text.Trim() : "";

        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("[Driver] driverId is empty");
            yield break;
        }

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
            choicePanel.Open(
                "로그 아웃",
                "로그아웃 하시겠습니까?",
                onYes: () => LogoutConfirmed(),
                onNo: null
            );
        }
        else LogoutConfirmed();
    }

    void LogoutConfirmed()
    {
        StopHeartbeat();
        if (service) service.LogoutDriver();

        if (driverIdInput) driverIdInput.text = "";
        if (driverNameInput) driverNameInput.text = "";
        if (statusDropdown) SetDropdownValueByText(statusDropdown, "IDLE");

        ClearChildren(contentRoot);
        SetPointsText(null);
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
            RefreshLoginUI();
            return;
        }

        RefreshLoginUI();
        StartCoroutine(CoRefreshPoints());

        if (mode == ViewMode.Available)
            StartCoroutine(CoFetchAvailable());
        else
            StartCoroutine(CoFetchMyDeliveries());
    }

    IEnumerator CoRefreshPoints()
    {
        ApiResponse resp = null;
        yield return service.FetchDrivers(r => resp = r);
        if (resp == null || resp.result != "OK") yield break;

        var list = service.ParseDriverRows(resp.value);
        var me = list.FirstOrDefault(x => (x.driverId ?? "") == service.currentDriverId);
        SetPointsText(me);
    }

    void SetPointsText(DriverDto me)
    {
        string today = me != null ? me.todayPoints : "";
        string total = me != null ? me.totalPoints : "";
        if (todayPointsText) todayPointsText.text = string.IsNullOrEmpty(today) ? "0" : today;
        if (totalPointsText) totalPointsText.text = string.IsNullOrEmpty(total) ? "0" : total;
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

        // 내 배달 안전 필터
        string myId = (service.currentDriverId ?? "").Trim();
        string myName = (service.currentDriverName ?? "").Trim();

        rows = rows.Where(d =>
        {
            string assignedId = (d.assignedDriverId ?? "").Trim();
            string acceptedName = (d.acceptedDriverName ?? "").Trim();
            bool byId = !string.IsNullOrEmpty(myId) && assignedId == myId;
            bool byName = !string.IsNullOrEmpty(myName) && acceptedName == myName;
            return byId || byName;
        }).ToList();

        Render(rows, isAvailableList: false);
    }

    static string ToKoreanDeliveryStatus(string status)
    {
        switch ((status ?? "").Trim())
        {
            case "CREATED": return "등록됨";
            case "ASSIGNED": return "배차됨";
            case "ACCEPTED": return "수락완료";
            case "PICKED_UP": return "픽업완료";
            case "DELIVERED": return "배달완료";
            case "CANCELED": return "취소됨";
            default: return status ?? "";
        }
    }

    void Render(List<DeliveryDto> rows, bool isAvailableList)
    {
        ClearChildren(contentRoot);

        foreach (var d in rows)
        {
            var go = Instantiate(driverDeliveryItemPrefab, contentRoot);

            SetTMP(go, "CustomerNameText", d.customerName);
            SetTMP(go, "AddressText", d.address);
            SetTMP(go, "StatusText", ToKoreanDeliveryStatus(d.status));
            SetTMP(go, "PointsText", d.points);

            // ✅ 버튼 딥 탐색 (경로 문제 해결)
            var acceptBtn = FindButtonAnyDeep(go.transform, "AcceptButton");
            var pickupBtn = FindButtonAnyDeep(go.transform, "PickupButton");
            var deliverBtn = FindButtonAnyDeep(go.transform, "DeliverButton", "DeliveredButton", "DeliveredtButton");
            var routeBtn = FindButtonAnyDeep(go.transform, "RouteButton");
            var addressBtn = FindButtonAnyDeep(go.transform, "AddressButton");
            var cancelBtn = FindButtonAnyDeep(go.transform, "CancelButton");

            // 등록 배달(available)에서만 수락 노출
            if (acceptBtn)
            {
                bool canAccept = isAvailableList && (d.status ?? "").Trim() == "CREATED";
                acceptBtn.gameObject.SetActive(canAccept);
                acceptBtn.onClick.RemoveAllListeners();
                if (canAccept)
                {
                    SetButtonLabel(acceptBtn, "수락");
                    acceptBtn.onClick.AddListener(() => RequestClaimConfirm(d.deliveryId));
                }
            }

            bool isMyList = !isAvailableList;
            string st = (d.status ?? "").Trim();

            SetActive(pickupBtn, isMyList && st == "ACCEPTED");
            SetActive(deliverBtn, isMyList && st == "PICKED_UP");

            // 내 배달: ACCEPTED / PICKED_UP 에서 길안내/주소복사/취소 활성
            SetActive(routeBtn, isMyList && (st == "ACCEPTED" || st == "PICKED_UP"));
            SetActive(addressBtn, isMyList && (st == "ACCEPTED" || st == "PICKED_UP"));
            SetActive(cancelBtn, isMyList && (st == "ACCEPTED" || st == "PICKED_UP"));

            if (pickupBtn)
            {
                pickupBtn.onClick.RemoveAllListeners();
                pickupBtn.onClick.AddListener(() => StartCoroutine(CoUpdate(d.deliveryId, "PICKED_UP")));
                SetButtonLabel(pickupBtn, "픽업완료");
            }

            if (deliverBtn)
            {
                deliverBtn.onClick.RemoveAllListeners();
                deliverBtn.onClick.AddListener(() => StartCoroutine(CoUpdate(d.deliveryId, "DELIVERED")));
                SetButtonLabel(deliverBtn, "배달완료");
            }

            if (routeBtn)
            {
                routeBtn.onClick.RemoveAllListeners();
                routeBtn.onClick.AddListener(() =>
                {
                    if (!naverMapLink) return;
                    naverMapLink.OpenRoute(d.lat, d.lng, d.address, d.customerName);
                });
                SetButtonLabel(routeBtn, "경로 버튼");
            }

            if (addressBtn)
            {
                addressBtn.onClick.RemoveAllListeners();
                addressBtn.onClick.AddListener(() =>
                {
                    GUIUtility.systemCopyBuffer = d.address ?? "";
                    Debug.Log($"[Address Copied] {GUIUtility.systemCopyBuffer}");
                });
                SetButtonLabel(addressBtn, "주소 복사");
            }

            if (cancelBtn)
            {
                cancelBtn.onClick.RemoveAllListeners();
                cancelBtn.onClick.AddListener(() => RequestCancelConfirm(d.deliveryId));
                SetButtonLabel(cancelBtn, "배달 취소");
            }
        }
    }

    void RequestClaimConfirm(string deliveryId)
    {
        if (choicePanel)
        {
            choicePanel.Open(
                "배차 수락",
                "배차를 수락하시겠습니까?",
                onYes: () => StartCoroutine(CoClaim(deliveryId)),
                onNo: null
            );
        }
        else StartCoroutine(CoClaim(deliveryId));
    }

    void RequestCancelConfirm(string deliveryId)
    {
        if (choicePanel)
        {
            choicePanel.Open(
                "배달 취소",
                "이 배달을 취소하시겠습니까?",
                onYes: () => StartCoroutine(CoUpdate(deliveryId, "CANCELED")),
                onNo: null
            );
        }
        else StartCoroutine(CoUpdate(deliveryId, "CANCELED"));
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

    IEnumerator CoUpdate(string deliveryId, string status)
    {
        ApiResponse resp = null;
        yield return service.UpdateStatus(deliveryId, service.currentDriverId, status, r => resp = r);

        if (resp == null || resp.result != "OK")
        {
            Debug.LogWarning($"[UpdateStatus:{status}] FAIL: {resp?.msg}");
            yield break;
        }

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

    // -----------------------
    // Helpers
    // -----------------------

    static void ClearChildren(Transform t)
    {
        if (!t) return;
        for (int i = t.childCount - 1; i >= 0; i--)
            Destroy(t.GetChild(i).gameObject);
    }

    static void SetActive(Button btn, bool active)
    {
        if (!btn) return;
        btn.gameObject.SetActive(active);
    }

    static void SetTMP(GameObject root, string childName, string text)
    {
        // ✅ 이건 기존대로(직계) 두되, 필요하면 여기도 딥탐색으로 바꿔도 됨
        var tf = root.transform.Find(childName);
        if (!tf) return;

        TMP_Text tmp = tf.GetComponent<TMP_Text>();
        if (!tmp) tmp = tf.GetComponentInChildren<TMP_Text>(true);

        if (!tmp) return;
        tmp.text = text ?? "";
    }

    static void SetButtonLabel(Button btn, string label)
    {
        if (!btn) return;
        var tmp = btn.GetComponentInChildren<TMP_Text>(true);
        if (tmp) tmp.text = label ?? "";
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

    // ✅ 핵심: 버튼 이름으로 "자식 전체"에서 찾기 (inactive 포함)
    static Button FindButtonAnyDeep(Transform root, params string[] names)
    {
        if (!root || names == null || names.Length == 0) return null;

        var all = root.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < names.Length; i++)
        {
            string target = names[i];
            if (string.IsNullOrEmpty(target)) continue;

            for (int j = 0; j < all.Length; j++)
            {
                if (all[j].name == target)
                {
                    var btn = all[j].GetComponent<Button>();
                    if (btn) return btn;
                }
            }
        }
        return null;
    }
}
