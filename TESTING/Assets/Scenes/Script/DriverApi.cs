using System;
using System.Collections;
using System.Collections.Generic;

public class DriverApi
{
    private readonly SheetApiClient client;
    public DriverApi(SheetApiClient client) { this.client = client; }

    public IEnumerator Upsert(string driverId, string driverName, Action<ApiResponse> onDone)
    {
        var form = new Dictionary<string, string>
        {
            ["order"] = "driver_upsert",
            ["driverId"] = driverId,
            ["driverName"] = driverName
        };
        yield return client.Post(form, onDone);
    }

    public IEnumerator Heartbeat(string driverId, string driverName, string status, string lat, string lng, Action<ApiResponse> onDone)
    {
        var form = new Dictionary<string, string>
        {
            ["order"] = "driver_heartbeat",
            ["driverId"] = driverId,
            ["driverName"] = driverName ?? "",
            ["status"] = status ?? "IDLE",
            ["lat"] = lat ?? "",
            ["lng"] = lng ?? ""
        };
        yield return client.Post(form, onDone);
    }

    public IEnumerator List(Action<ApiResponse> onDone)
    {
        var form = new Dictionary<string, string>
        {
            ["order"] = "driver_list",
        };
        yield return client.Post(form, onDone);
    }
}
