using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryService : MonoBehaviour
{
    [Header("API Client")]
    [SerializeField] private SheetApiClient client;

    public DeliveryApi DeliveryApi { get; private set; }
    public DriverApi DriverApi { get; private set; }

    [Header("Driver Session")]
    public string currentDriverId;
    public string currentDriverName;
    public string currentDriverStatus = "IDLE";

    // PlayerPrefs keys (auto-login)
    const string PREF_DRIVER_ID = "driver.session.id";
    const string PREF_DRIVER_NAME = "driver.session.name";
    const string PREF_DRIVER_STATUS = "driver.session.status";

    void Awake()
    {
        EnsureInitialized();
        LoadDriverSession(); // <-- 자동 로그인
    }

    void OnEnable()
    {
        EnsureInitialized();
    }

    private void EnsureInitialized()
    {
        if (DeliveryApi != null && DriverApi != null) return;

        if (!client) client = FindObjectOfType<SheetApiClient>(true);

        if (!client)
        {
            Debug.LogError("[DeliveryService] SheetApiClient가 없습니다. AppRoot에 SheetApiClient가 활성화되어 있는지 확인하세요.");
            return;
        }

        DeliveryApi = new DeliveryApi(client);
        DriverApi = new DriverApi(client);
    }

    private bool IsReady(Action<ApiResponse> onDone, string context)
    {
        EnsureInitialized();

        if (DeliveryApi == null || DriverApi == null)
        {
            Debug.LogError($"[DeliveryService] API 초기화 실패: {context}");
            onDone?.Invoke(new ApiResponse
            {
                order = context,
                result = "FAIL",
                msg = "API 초기화 실패(DeliveryApi/DriverApi null)"
            });
            return false;
        }

        return true;
    }

    // -------------------------
    // Session (Auto-login)
    // -------------------------

    public void SetDriver(string driverId, string driverName)
    {
        currentDriverId = (driverId ?? "").Trim();
        currentDriverName = (driverName ?? "").Trim();
        SaveDriverSession();
    }

    public void SetDriverStatus(string status)
    {
        currentDriverStatus = string.IsNullOrWhiteSpace(status) ? "IDLE" : status.Trim();
        SaveDriverSession();
    }

    public bool IsLoggedIn()
    {
        return !string.IsNullOrEmpty(currentDriverId);
    }

    public void LogoutDriver()
    {
        currentDriverId = "";
        currentDriverName = "";
        currentDriverStatus = "IDLE";

        PlayerPrefs.DeleteKey(PREF_DRIVER_ID);
        PlayerPrefs.DeleteKey(PREF_DRIVER_NAME);
        PlayerPrefs.DeleteKey(PREF_DRIVER_STATUS);
        PlayerPrefs.Save();
    }

    void LoadDriverSession()
    {
        string id = PlayerPrefs.GetString(PREF_DRIVER_ID, "").Trim();
        if (string.IsNullOrEmpty(id)) return;

        currentDriverId = id;
        currentDriverName = PlayerPrefs.GetString(PREF_DRIVER_NAME, "").Trim();
        currentDriverStatus = PlayerPrefs.GetString(PREF_DRIVER_STATUS, "IDLE").Trim();
        if (string.IsNullOrEmpty(currentDriverStatus)) currentDriverStatus = "IDLE";
    }

    void SaveDriverSession()
    {
        if (string.IsNullOrEmpty(currentDriverId)) return;

        PlayerPrefs.SetString(PREF_DRIVER_ID, currentDriverId);
        PlayerPrefs.SetString(PREF_DRIVER_NAME, currentDriverName ?? "");
        PlayerPrefs.SetString(PREF_DRIVER_STATUS, currentDriverStatus ?? "IDLE");
        PlayerPrefs.Save();
    }

    // -------------------------
    // API wrappers
    // -------------------------

    public IEnumerator CreateDelivery(string storeName, string customerName, string phone, string address,
        string lat, string lng, int points, string note, Action<ApiResponse> onDone)
    {
        if (!IsReady(onDone, "delivery_create")) yield break;
        yield return DeliveryApi.Create(storeName, customerName, phone, address, lat, lng, points, note, onDone);
    }

    public IEnumerator Assign(string deliveryId, string driverId, Action<ApiResponse> onDone)
    {
        if (!IsReady(onDone, "delivery_assign")) yield break;
        yield return DeliveryApi.Assign(deliveryId, driverId, onDone);
    }

    public IEnumerator Accept(string deliveryId, string driverId, Action<ApiResponse> onDone)
    {
        if (!IsReady(onDone, "delivery_accept")) yield break;
        yield return DeliveryApi.Accept(deliveryId, driverId, onDone);
    }

    public IEnumerator Claim(string deliveryId, string driverId, Action<ApiResponse> onDone)
    {
        if (!IsReady(onDone, "delivery_claim")) yield break;
        yield return DeliveryApi.Claim(deliveryId, driverId, onDone);
    }

    public IEnumerator FetchAvailableDeliveries(Action<ApiResponse> onDone)
    {
        if (!IsReady(onDone, "delivery_available_list")) yield break;
        yield return DeliveryApi.AvailableList(onDone);
    }

    public IEnumerator UpdateStatus(string deliveryId, string driverId, string status, Action<ApiResponse> onDone)
    {
        if (!IsReady(onDone, "delivery_update_status")) yield break;
        yield return DeliveryApi.UpdateStatus(deliveryId, driverId, status, onDone);
    }

    public IEnumerator FetchDeliveriesAdmin(string statusOrEmpty, Action<ApiResponse> onDone)
    {
        if (!IsReady(onDone, "delivery_list")) yield break;
        yield return DeliveryApi.List("", statusOrEmpty, onDone);
    }

    public IEnumerator FetchDeliveriesForDriver(string driverId, string statusOrEmpty, Action<ApiResponse> onDone)
    {
        if (!IsReady(onDone, "delivery_list")) yield break;
        yield return DeliveryApi.List(driverId, statusOrEmpty, onDone);
    }

    public IEnumerator UpsertDriver(string driverId, string driverName, Action<ApiResponse> onDone)
    {
        if (!IsReady(onDone, "driver_upsert")) yield break;
        yield return DriverApi.Upsert(driverId, driverName, onDone);
    }

    public IEnumerator FetchDrivers(Action<ApiResponse> onDone)
    {
        if (!IsReady(onDone, "driver_list")) yield break;
        yield return DriverApi.List(onDone);
    }

    public IEnumerator Heartbeat(string driverId, string driverName, string status, string lat, string lng, Action<ApiResponse> onDone)
    {
        if (!IsReady(onDone, "driver_heartbeat")) yield break;
        yield return DriverApi.Heartbeat(driverId, driverName, status, lat, lng, onDone);
    }

    public List<DeliveryDto> ParseDeliveryRows(string valueJson)
    {
        if (string.IsNullOrEmpty(valueJson)) return new List<DeliveryDto>();
        var obj = JsonHelper.FromJson<DeliveryListResponse>(valueJson);
        return (obj != null && obj.rows != null) ? obj.rows : new List<DeliveryDto>();
    }

    public List<DriverDto> ParseDriverRows(string valueJson)
    {
        if (string.IsNullOrEmpty(valueJson)) return new List<DriverDto>();
        var obj = JsonHelper.FromJson<DriverListResponse>(valueJson);
        return (obj != null && obj.rows != null) ? obj.rows : new List<DriverDto>();
    }
}
