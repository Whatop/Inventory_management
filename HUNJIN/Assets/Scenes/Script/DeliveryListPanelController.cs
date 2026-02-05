using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryListPanelController : MonoBehaviour
{
    [Header("Refs")]
    public AppFlow flow;
    public DeliveryService service;

    [Header("Filter")]
    public string statusFilter = ""; // ""=ALL
    public Button allButton;
    public Button createdButton;
    public Button assignedButton;
    public Button acceptedButton;
    public Button pickedUpButton;
    public Button deliveredButton;

    [Header("Search (Optional)")]
    public TMP_InputField searchInput;

    [Header("List")]
    public Transform contentRoot;
    public GameObject deliveryItemPrefab;
    public Button refreshButton;
    public Button openCreateButton;

    [Header("Driver Select Popup")]
    public GameObject driverSelectPopupRoot;
    public Transform driverPopupContentRoot;
    public GameObject driverItemPrefab;
    public Button popupCloseButton;

    private string _pendingAssignDeliveryId = null;

    void Awake()
    {
        if (refreshButton) refreshButton.onClick.AddListener(Refresh);
        if (openCreateButton) openCreateButton.onClick.AddListener(() => flow.ShowDeliveryCreate());

        if (allButton) allButton.onClick.AddListener(() => SetFilter(""));
        if (createdButton) createdButton.onClick.AddListener(() => SetFilter("CREATED"));
        if (assignedButton) assignedButton.onClick.AddListener(() => SetFilter("ASSIGNED"));
        if (acceptedButton) acceptedButton.onClick.AddListener(() => SetFilter("ACCEPTED"));
        if (pickedUpButton) pickedUpButton.onClick.AddListener(() => SetFilter("PICKED_UP"));
        if (deliveredButton) deliveredButton.onClick.AddListener(() => SetFilter("DELIVERED"));

        if (popupCloseButton) popupCloseButton.onClick.AddListener(CloseDriverPopup);
        if (driverSelectPopupRoot) driverSelectPopupRoot.SetActive(false);
    }

    void OnEnable()
    {
        Refresh();
    }

    void SetFilter(string filter)
    {
        statusFilter = filter ?? "";
        Refresh();
    }

    public void Refresh()
    {
        StartCoroutine(CoFetch());
    }

    IEnumerator CoFetch()
    {
        ApiResponse resp = null;
        yield return service.FetchDeliveriesAdmin(statusFilter, r => resp = r);

        if (resp == null || resp.result != "OK")
        {
            Debug.LogWarning($"[DeliveryList] FAIL: {resp?.msg}");
            yield break;
        }

        var rows = service.ParseDeliveryRows(resp.value);
        if (searchInput && !string.IsNullOrWhiteSpace(searchInput.text))
        {
            string q = searchInput.text.Trim();
            rows = rows.FindAll(x =>
                (x.customerName ?? "").Contains(q) ||
                (x.address ?? "").Contains(q) ||
                (x.deliveryId ?? "").Contains(q));
        }

        Render(rows);
    }

    void Render(List<DeliveryDto> rows)
    {
        ClearChildren(contentRoot);

        foreach (var d in rows)
        {
            var go = Instantiate(deliveryItemPrefab, contentRoot);

            SetTMP(go, "CustomerNameText", d.customerName);
            SetTMP(go, "AddressText", d.address);
            SetTMP(go, "StatusText", d.status);
            SetTMP(go, "PointsText", d.points);

            var assignBtn = FindButton(go, "AssignButton");
            if (assignBtn)
            {
                assignBtn.onClick.RemoveAllListeners();
                assignBtn.onClick.AddListener(() => OpenDriverPopup(d.deliveryId));
            }
        }
    }

    // ---------- Driver popup ----------
    void OpenDriverPopup(string deliveryId)
    {
        _pendingAssignDeliveryId = deliveryId;
        if (driverSelectPopupRoot) driverSelectPopupRoot.SetActive(true);
        StartCoroutine(CoFetchDrivers());
    }

    void CloseDriverPopup()
    {
        _pendingAssignDeliveryId = null;
        if (driverSelectPopupRoot) driverSelectPopupRoot.SetActive(false);
        if (driverPopupContentRoot) ClearChildren(driverPopupContentRoot);
    }

    IEnumerator CoFetchDrivers()
    {
        ApiResponse resp = null;
        yield return service.FetchDrivers(r => resp = r);

        if (resp == null || resp.result != "OK")
        {
            Debug.LogWarning($"[DriverList] FAIL: {resp?.msg}");
            yield break;
        }

        var drivers = service.ParseDriverRows(resp.value);
        RenderDrivers(drivers);
    }

    void RenderDrivers(List<DriverDto> rows)
    {
        ClearChildren(driverPopupContentRoot);

        foreach (var dr in rows)
        {
            var go = Instantiate(driverItemPrefab, driverPopupContentRoot);

            SetTMP(go, "DriverNameText", dr.driverName);
            SetTMP(go, "StatusText", dr.status);
            SetTMP(go, "SeenText", dr.lastSeenAt);

            var selectBtn = FindButton(go, "SelectButton");
            if (selectBtn)
            {
                selectBtn.onClick.RemoveAllListeners();
                selectBtn.onClick.AddListener(() => StartCoroutine(CoAssign(_pendingAssignDeliveryId, dr.driverId)));
            }
        }
    }

    IEnumerator CoAssign(string deliveryId, string driverId)
    {
        if (string.IsNullOrEmpty(deliveryId) || string.IsNullOrEmpty(driverId))
            yield break;

        ApiResponse resp = null;
        yield return service.Assign(deliveryId, driverId, r => resp = r);

        if (resp == null || resp.result != "OK")
        {
            Debug.LogWarning($"[Assign] FAIL: {resp?.msg}");
            yield break;
        }

        CloseDriverPopup();
        Refresh();
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
}
