namespace VFi.Domain.SO.Models.Spider.Mercari;

public class MercariTrackingInfo
{
    public long? TransactionEvidenceId { get; set; }
    public string? TransactionEvidenceStatus { get; set; }
    public string? DeliveryCarrier { get; set; }
    public string? DomesticTracking { get; set; }
    public string? DomesticStatus { get; set; }
    public decimal? ShippingFee { get; set; }
}
