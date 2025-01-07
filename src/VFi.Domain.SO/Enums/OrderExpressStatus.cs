namespace VFi.Domain.SO.Enums;
public enum OrderExpressStatus
{
    New = 0,
    DomesticShipping = 30,
    InWareHouse = 40,
    WaitForSettlement = 50,
    Delivering = 60,
    Delivered = 70,
    Returned = 80,
    Canceled = 90
}
