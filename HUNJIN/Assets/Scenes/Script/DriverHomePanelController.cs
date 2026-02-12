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

    [Header("Loading (GameManager 방식)")]
    public GameObject loadingRoot;
    [Tooltip("로딩이 최소로 떠있어야 하는 시간(초)")]
    public float loadingMinDuration = 0.25f;
    [Tooltip("너무 오래 걸릴 때 안전장치(초). 0이면 미사용")]
    public float loadingMaxTimeout = 0f;

    [Header("Login")]
    public TMP_InputField driverIdInput;
    public TMP_InputField driverNameInput;
    public Button loginButton;
    public Button logoutButton;

    [Header("Login-Only UI (필요한 것만 연결)")]
    [Tooltip("내 포인트 패널 루트(로그인 시에만 보이게)")]
    public GameObject myPointsPanelRoot;
    [Tooltip("내 배달목록 루트(GameObject0 같은 루트, 로그인 시에만 보이게)")]
    public GameObject myDeliveriesRoot;

    [Header("Points Text (Optional)")]
    public TMP_Text todayPointsText;
    public TMP_Text totalPointsText;

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

    // loading state
    int _loadingRef = 0;
    float _loadingShownAt = -1f;
    Coroutine _loadingTimeoutCo;

    void Awake()
    {
        if (loginButton) loginButton.onClick.AddListener(() => StartCoroutine(CoWrapLoading(CoLogin())));
        if (logoutButton) logoutButton.onClick.AddListener(RequestLogout);

        if (refreshButton) refreshButton.onClick.AddListener(Refresh);

        if (statusDropdown)
            statusDropdown.onValueChanged.AddListener(_ => OnStatusChanged());

        if (availableToggle)
            availableToggle.onValueChanged.AddListener(isOn => { if (isOn) { mode = ViewMode.Available; Refresh(); } });

        if (myToggle)
            myToggle.onValueChanged.AddListener(isOn => { if (isOn) { mode = ViewMode.My; Refresh(); } });

        RefreshLoginUI();
        SetLoading(false, force: true);
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
        SetLoading(false, force: true);
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
            RefreshLoginUI();
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
        else
        {
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
            Debug.LogWarning("[Driver] Not logged in");
            RefreshLoginUI();
            return;
        }

        RefreshLoginUI();

        // 포인트/목록 동시 호출 → 로딩 2개 겹쳐도 refcount로 안전
        StartCoroutine(CoWrapLoading(CoRefreshPoints()));

        if (mode == ViewMode.Available)
            StartCoroutine(CoWrapLoading(CoFetchAvailable()));
        else
            StartCoroutine(CoWrapLoading(CoFetchMyDeliveries()));
    }

    IEnumerator CoRefreshPoints()
    {
        ApiResponse resp = null;
        yield return service.FetchDrivers(r => resp = r);

        if (resp == null || resp.result != "OK")
            yield break;

        var list = service.ParseDriverRows(resp.value);
        var me = list.FirstOrDefault(x => (x.driverId ?? "") == service.currentDriverId);

        SetPointsText(me);
    }

    void SetPointsText(DriverDto me)
    {
        if (!todayPointsText && !totalPointsText) return;

        string today = "";
        string total = "";

        if (me != null)
        {
            today = me.todayPoints;
            total = me.totalPoints;
        }

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

        // ✅ 안전 필터: "기사ID 또는 기사이름"으로 내 배달만 남김
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
            SetTMP(go, "AcceptedDriverNameText", d.acceptedDriverName);

            var acceptBtn = FindButton(go, "AcceptButton");
            var pickupBtn = FindButton(go, "PickupButton");
            var deliverBtn = FindButton(go, "DeliverButton");
            var routeBtn = FindButton(go, "RouteButton");
            var cancelBtn = FindButton(go, "CancelButton");

            // "등록 배달" 목록일 때만 수락 버튼 노출
            if (acceptBtn)
            {
                bool canAccept = isAvailableList && d.status == "CREATED";
                acceptBtn.gameObject.SetActive(canAccept);

                if (canAccept) SetButtonLabel(acceptBtn, "수락");

                acceptBtn.onClick.RemoveAllListeners();
                if (canAccept)
                    acceptBtn.onClick.AddListener(() => RequestClaimConfirm(d.deliveryId));
            }

            bool isMyList = !isAvailableList;

            SetActive(pickupBtn, isMyList && d.status == "ACCEPTED");
            SetActive(deliverBtn, isMyList && d.status == "PICKED_UP");
            SetActive(routeBtn, isMyList && (d.status == "ACCEPTED" || d.status == "PICKED_UP"));

            bool canCancel = isMyList && (d.status == "ACCEPTED" || d.status == "PICKED_UP");
            SetActive(cancelBtn, canCancel);

            if (pickupBtn)
            {
                pickupBtn.onClick.RemoveAllListeners();
                pickupBtn.onClick.AddListener(() => StartCoroutine(CoWrapLoading(CoUpdate(d.deliveryId, "PICKED_UP"))));
                SetButtonLabel(pickupBtn, "픽업완료");
            }

            if (deliverBtn)
            {
                deliverBtn.onClick.RemoveAllListeners();
                deliverBtn.onClick.AddListener(() => StartCoroutine(CoWrapLoading(CoUpdate(d.deliveryId, "DELIVERED"))));
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
                SetButtonLabel(routeBtn, "길안내");
            }

            if (cancelBtn)
            {
                cancelBtn.onClick.RemoveAllListeners();
                cancelBtn.onClick.AddListener(() => RequestCancelConfirm(d.deliveryId));
                SetButtonLabel(cancelBtn, "취소");
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
                onYes: () => StartCoroutine(CoWrapLoading(CoClaim(deliveryId))),
                onNo: null
            );
        }
        else
        {
            StartCoroutine(CoWrapLoading(CoClaim(deliveryId)));
        }
    }

    void RequestCancelConfirm(string deliveryId)
    {
        if (choicePanel)
        {
            choicePanel.Open(
                "배달 취소",
                "이 배달을 취소하시겠습니까?\n(취소 시 다시 등록됨 상태로 돌아갑니다)",
                onYes: () => StartCoroutine(CoWrapLoading(CoUpdate(deliveryId, "CANCELED"))),
                onNo: null
            );
        }
        else
        {
            StartCoroutine(CoWrapLoading(CoUpdate(deliveryId, "CANCELED")));
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
    // Loading helpers
    // -----------------------
    IEnumerator CoWrapLoading(IEnumerator routine)
    {
        BeginLoading();
        while (routine != null && routine.MoveNext())
            yield return routine.Current;
        EndLoading();
    }

    void BeginLoading()
    {
        _loadingRef = Mathf.Max(0, _loadingRef + 1);
        if (_loadingRef == 1)
        {
            _loadingShownAt = Time.unscaledTime;
            SetLoading(true);

            if (loadingMaxTimeout > 0f)
            {
                if (_loadingTimeoutCo != null) StopCoroutine(_loadingTimeoutCo);
                _loadingTimeoutCo = StartCoroutine(CoLoadingTimeout(loadingMaxTimeout));
            }
        }
    }

    void EndLoading()
    {
        _loadingRef = Mathf.Max(0, _loadingRef - 1);
        if (_loadingRef == 0)
        {
            if (_loadingTimeoutCo != null)
            {
                StopCoroutine(_loadingTimeoutCo);
                _loadingTimeoutCo = null;
            }
            StartCoroutine(CoHideLoadingMinDuration());
        }
    }

    IEnumerator CoHideLoadingMinDuration()
    {
        float elapsed = Time.unscaledTime - _loadingShownAt;
        float wait = Mathf.Max(0f, loadingMinDuration - elapsed);
        if (wait > 0f) yield return new WaitForSecondsRealtime(wait);
        SetLoading(false);
    }

    IEnumerator CoLoadingTimeout(float timeout)
    {
        yield return new WaitForSecondsRealtime(timeout);
        Debug.LogWarning("[DriverHome] Loading timeout reached. Forcing hide.");
        _loadingRef = 0;
        SetLoading(false, force: true);
    }

    void SetLoading(bool on, bool force = false)
    {
        if (!loadingRoot) return;
        if (!force && loadingRoot.activeSelf == on) return;
        loadingRoot.SetActive(on);
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

    static Button FindButton(GameObject root, string childName)
    {
        var tf = root.transform.Find(childName);
        return tf ? tf.GetComponent<Button>() : null;
    }

    static void SetActive(Button btn, bool active)
    {
        if (!btn) return;
        btn.gameObject.SetActive(active);
    }

    static void SetTMP(GameObject root, string containerName, string text)
    {
        var tf = root.transform.Find(containerName);
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
}
