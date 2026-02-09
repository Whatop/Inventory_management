using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryApi
{
    private readonly SheetApiClient client;
    public DeliveryApi(SheetApiClient client) { this.client = client; }

    public IEnumerator Create(string storeName, string customerName, string phone, string address,
                              string lat, string lng, int points, string note,
                              Action<ApiResponse> onDone)
    {
        var form = new Dictionary<string, string>
        {
            ["order"] = "delivery_create",
            ["storeName"] = storeName,
            ["customerName"] = customerName,
            ["phone"] = phone,
            ["address"] = address,
            ["lat"] = lat,
            ["lng"] = lng,
            ["points"] = points.ToString(),
            ["note"] = note
        };
        yield return client.Post(form, onDone);
    }

    // (구버전) 관리자 배차 - 안 쓰더라도 남겨둠
    public IEnumerator Assign(string deliveryId, string driverId, Action<ApiResponse> onDone)
    {
        var form = new Dictionary<string, string>
        {
            ["order"] = "delivery_assign",
            ["deliveryId"] = deliveryId,
            ["driverId"] = driverId
        };
        yield return client.Post(form, onDone);
    }

    // (구버전) 배차된건 수락 - 안 쓰더라도 남겨둠
    public IEnumerator Accept(string deliveryId, string driverId, Action<ApiResponse> onDone)
    {
        var form = new Dictionary<string, string>
        {
            ["order"] = "delivery_accept",
            ["deliveryId"] = deliveryId,
            ["driverId"] = driverId
        };
        yield return client.Post(form, onDone);
    }

    /// <summary>등록됨(CREATED) 공개 목록</summary>
    public IEnumerator AvailableList(Action<ApiResponse> onDone)
    {
        var form = new Dictionary<string, string>
        {
            ["order"] = "delivery_available_list",
        };
        yield return client.Post(form, onDone);
    }

    /// <summary>선점(Claim): CREATED -> ACCEPTED + assignedDriverId</summary>
    public IEnumerator Claim(string deliveryId, string driverId, Action<ApiResponse> onDone)
    {
        var form = new Dictionary<string, string>
        {
            ["order"] = "delivery_claim",
            ["deliveryId"] = deliveryId,
            ["driverId"] = driverId
        };
        yield return client.Post(form, onDone);
    }

    public IEnumerator UpdateStatus(string deliveryId, string driverId, string status, Action<ApiResponse> onDone)
    {
        var form = new Dictionary<string, string>
        {
            ["order"] = "delivery_update_status",
            ["deliveryId"] = deliveryId,
            ["driverId"] = driverId,
            ["status"] = status
        };
        yield return client.Post(form, onDone);
    }

    public IEnumerator List(string driverIdOrEmpty, string statusOrEmpty, Action<ApiResponse> onDone)
    {
        var form = new Dictionary<string, string>
        {
            ["order"] = "delivery_list",
            ["driverId"] = driverIdOrEmpty ?? "",
            ["status"] = statusOrEmpty ?? ""
        };
        yield return client.Post(form, onDone);
    }
}
