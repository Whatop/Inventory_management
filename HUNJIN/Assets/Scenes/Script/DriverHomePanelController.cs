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

    [Header("Login")]
    public TMP_InputField driverIdInput;
    public TMP_InputField driverNameInput;
    public Button loginButton;

    [Header("Driver Status")]
    public TMP_Dropdown statusDropdown; // IDLE/ON_DELIVERY/BREAK
    public Button refreshButton;

    [Header("Points UI (Optional)")]
    public TMP_Text todayPointsText;
    public TMP_Text totalPointsText;

    [Header("List")]
    public Transform contentRoot;
    public GameObject driverDeliveryItemPrefab;

    [Header("Heartbeat")]
    public float heartbeatIntervalSec = 15f;

    Coroutine _heartbeatCo;

    void Awake()
    {
        if (loginButton) loginButton.onClick.AddListener(() => StartCoroutine(CoLogin()));
        if (refreshButton) refreshButton.onClick.AddListener(Refresh);

        if (statusDropdown)
        {
            statusDropdown.onValueChanged.AddListener(_ => OnStatusChanged());
        }
    }

    void OnEnable()
    {
        // 로그인 되어 있으면 바로 목록 갱신
        if (!string.IsNullOrEmpty(service.currentDriverId))
        {
            Refresh();
            StartHeartbeat();
        }
    }

    void OnDisable()
    {
        StopHeartbeat();
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
        Refresh();
        StartHeartbeat();
    }

    void OnStatusChanged()
    {
        if (!statusDropdown) return;

        // Dropdown 옵션 텍스트: IDLE / ON_DELIVERY / BREAK 로 맞추는 걸 추천
        string status = statusDropdown.options[statusDropdown.value].text;
        service.currentDriverStatus = status;
    }

    public void Refresh()
    {
        if (string.IsNullOrEmpty(service.currentDriverId))
        {
            Debug.LogWarning("[Driver] Not logged in");
            return;
        }
        StartCoroutine(CoFetchMyDeliveries());
    }

    IEnumerator CoFetchMyDeliveries()
    {
        ApiResponse resp = null;
        yield return service.FetchDeliveriesForDriver(service.currentDriverId, "", r => resp = r);

        if (resp == null || resp.result != "OK")
        {
            Debug.LogWarning($"[DriverDeliveries] FAIL: {resp?.msg}");
            yield break;
        }

        var rows = service.ParseDeliveryRows(resp.value);
        Render(rows);
    }

    void Render(List<DeliveryDto> rows)
    {
        ClearChildren(contentRoot);

        foreach (var d in rows)
        {
            var go = Instantiate(driverDeliveryItemPrefab, contentRoot);

            SetTMP(go, "CustomerNameText", d.customerName);
            SetTMP(go, "AddressText", d.address);
            SetTMP(go, "StatusText", d.status);
            SetTMP(go, "PointsText", d.points);

            var acceptBtn = FindButton(go, "AcceptButton");
            var pickupBtn = FindButton(go, "PickupButton");
            var deliverBtn = FindButton(go, "DeliverButton");
            var routeBtn = FindButton(go, "RouteButton");

            // 상태별 버튼 노출
            SetActive(acceptBtn, d.status == "ASSIGNED");
            SetActive(pickupBtn, d.status == "ACCEPTED");
            SetActive(deliverBtn, d.status == "PICKED_UP");
            SetActive(routeBtn, d.status == "PICKED_UP" || d.status == "ACCEPTED" || d.status == "ASSIGNED");

            if (acceptBtn)
            {
                acceptBtn.onClick.RemoveAllListeners();
                acceptBtn.onClick.AddListener(() => StartCoroutine(CoAccept(d.deliveryId)));
            }
            if (pickupBtn)
            {
                pickupBtn.onClick.RemoveAllListeners();
                pickupBtn.onClick.AddListener(() => StartCoroutine(CoUpdate(d.deliveryId, "PICKED_UP")));
            }
            if (deliverBtn)
            {
                deliverBtn.onClick.RemoveAllListeners();
                deliverBtn.onClick.AddListener(() => StartCoroutine(CoUpdate(d.deliveryId, "DELIVERED")));
            }
            if (routeBtn)
            {
                routeBtn.onClick.RemoveAllListeners();
                routeBtn.onClick.AddListener(() =>
                {
                    if (!naverMapLink) return;
                    naverMapLink.OpenRoute(d.lat, d.lng, d.address, d.customerName);
                });
            }
        }
    }

    IEnumerator CoAccept(string deliveryId)
    {
        ApiResponse resp = null;
        yield return service.Accept(deliveryId, service.currentDriverId, r => resp = r);

        if (resp == null || resp.result != "OK")
        {
            Debug.LogWarning($"[Accept] FAIL: {resp?.msg}");
            yield break;
        }
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

    // ---------- Heartbeat ----------
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
            // MVP: 위치는 비워도 됨. 나중에 GPS 붙이면 lat/lng 넣기
            string lat = "";
            string lng = "";

            ApiResponse resp = null;
            yield return service.Heartbeat(
                service.currentDriverId,
                service.currentDriverName,
                service.currentDriverStatus,
                lat, lng,
                r => resp = r
            );

            // 실패해도 끊지 않고 다음 루프
            yield return new WaitForSeconds(heartbeatIntervalSec);
        }
    }

    // ---------- Helpers ----------
    static void ClearChildren(Transform t)
    {
        if (!t) return;
        for (int i = t.childCount - 1; i >= 0; i--)
            Destroy(t.GetChild(i).gameObject);
    }

    static void SetTMP(GameObject root, string childName, string text)
    {
        var tf = root.transform.Find(childName);
        if (!tf) return;
        var tmp = tf.GetComponent<TMP_Text>();
        if (!tmp) return;
        tmp.text = text ?? "";
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
}
