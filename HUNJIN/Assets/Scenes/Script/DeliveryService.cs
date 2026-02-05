using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryService : MonoBehaviour
{
    [Header("API Client")]
    public SheetApiClient client;

    public DeliveryApi DeliveryApi { get; private set; }
    public DriverApi DriverApi { get; private set; }

    [Header("Driver Session")]
    public string currentDriverId;
    public string currentDriverName;
    public string currentDriverStatus = "IDLE";

    void Awake()
    {
        if (!client) client = FindObjectOfType<SheetApiClient>();
        DeliveryApi = new DeliveryApi(client);
        DriverApi = new DriverApi(client);
    }

    public void SetDriver(string driverId, string driverName)
    {
        currentDriverId = (driverId ?? "").Trim();
        currentDriverName = (driverName ?? "").Trim();
    }

    public IEnumerator CreateDelivery(string storeName, string customerName, string phone, string address,
        string lat, string lng, int points, string note, Action<ApiResponse> onDone)
    {
        yield return DeliveryApi.Create(storeName, customerName, phone, address, lat, lng, points, note, onDone);
    }

    public IEnumerator Assign(string deliveryId, string driverId, Action<ApiResponse> onDone)
    {
        yield return DeliveryApi.Assign(deliveryId, driverId, onDone);
    }

    public IEnumerator Accept(string deliveryId, string driverId, Action<ApiResponse> onDone)
    {
        yield return DeliveryApi.Accept(deliveryId, driverId, onDone);
    }

    public IEnumerator UpdateStatus(string deliveryId, string driverId, string status, Action<ApiResponse> onDone)
    {
        yield return DeliveryApi.UpdateStatus(deliveryId, driverId, status, onDone);
    }

    public IEnumerator FetchDeliveriesAdmin(string statusOrEmpty, Action<ApiResponse> onDone)
    {
        yield return DeliveryApi.List("", statusOrEmpty, onDone);
    }

    public IEnumerator FetchDeliveriesForDriver(string driverId, string statusOrEmpty, Action<ApiResponse> onDone)
    {
        yield return DeliveryApi.List(driverId, statusOrEmpty, onDone);
    }

    public IEnumerator UpsertDriver(string driverId, string driverName, Action<ApiResponse> onDone)
    {
        yield return DriverApi.Upsert(driverId, driverName, onDone);
    }

    public IEnumerator FetchDrivers(Action<ApiResponse> onDone)
    {
        yield return DriverApi.List(onDone);
    }

    public IEnumerator Heartbeat(string driverId, string driverName, string status, string lat, string lng, Action<ApiResponse> onDone)
    {
        yield return DriverApi.Heartbeat(driverId, driverName, status, lat, lng, onDone);
    }

    // 응답 파싱 헬퍼 (서버에서 value를 JSON string으로 보내는 전제)
    public List<DeliveryDto> ParseDeliveryRows(string valueJson)
    {
        if (string.IsNullOrEmpty(valueJson)) return new List<DeliveryDto>();
        var obj = JsonHelper.FromJson<DeliveryListResponse>(valueJson);
        return obj != null && obj.rows != null ? obj.rows : new List<DeliveryDto>();
    }

    public List<DriverDto> ParseDriverRows(string valueJson)
    {
        if (string.IsNullOrEmpty(valueJson)) return new List<DriverDto>();
        var obj = JsonHelper.FromJson<DriverListResponse>(valueJson);
        return obj != null && obj.rows != null ? obj.rows : new List<DriverDto>();
    }
}
