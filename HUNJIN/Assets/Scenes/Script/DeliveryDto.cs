using System;

[Serializable]
public class DeliveryDto
{
    public string deliveryId;
    public string storeName;
    public string customerName;
    public string phone;
    public string address;
    public string lat;
    public string lng;
    public string status; // CREATED/ASSIGNED/ACCEPTED/PICKED_UP/DELIVERED/CANCELED
    public string assignedDriverId;

    // Deliveries 시트 P열: 수락한 기사 이름(표시용)
    public string acceptedDriverName;

    public string acceptedAt;
    public string pickedUpAt;
    public string deliveredAt;
    public string points;
    public string note;
}
