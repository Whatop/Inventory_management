using System;

[Serializable]
public class DriverDto
{
    public string driverId;
    public string driverName;
    public string status;     // IDLE/ON_DELIVERY/BREAK/OFF
    public string lastLat;
    public string lastLng;
    public string lastSeenAt;
    public string todayPoints;
    public string totalPoints;
}
