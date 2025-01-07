namespace VFi.Api.SO.ViewModels;

public class OrderFetchOrderTrackingEventRequest
{
    public int? MinDays { get; set; }
    public int? MaxDays { get; set; }
    public int? MaxItems { get; set; }
    public string? OrderType { get; set; }
    public string? AuthorizationToken { get; set; }
}

public class OrderFetchDomesticDeliveryEventRequest
{
    public int? MinDays { get; set; }
    public int? MaxDays { get; set; }
    public int? MaxItems { get; set; }
}
