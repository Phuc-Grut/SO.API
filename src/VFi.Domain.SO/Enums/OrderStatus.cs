namespace VFi.Domain.SO.Enums;
public enum OrderStatus
{
    PendingConfirm = 0,
    WaitForPurchase = 10,
    Purchased = 20,
    DomesticShipping = 30,
    InWareHouse = 40,
    WaitForSettlement = 50,
    Delivering = 60,
    Delivered = 70,
    Returned = 80,
    Canceled = 90
}
