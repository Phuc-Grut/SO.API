namespace VFi.Domain.SO.Models.Spider.Mercari;
public class MercariProductPublish
{
    public bool IsBlockTransport { get; set; }
    public bool IsBlockBargain { get; set; }
    public int ProductType { get; set; }
    public Guid? Id { get; set; }
    public string? Code { get; set; }
    /// <summary>
    /// New = 0, Refurbished = 10,20 = LikeNew, Used = 30, Used40 = 31, Damaged = 40
    /// </summary>
    public int Condition { get; set; }
    public string? LinkInfo { get; set; }
    public string? Name { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? Origin { get; set; }
    public string? Source { get; set; }
    public string? Brand { get; set; }
    public string? Manufacturer { get; set; }
    public string? Image { get; set; }
    public string? Gtin { get; set; }
    public List<string> Images { get; set; } = new List<string>();
    public decimal? Price { get; set; }

    public string? Currency { get; set; }
    public bool? IsTaxExempt { get; set; }
    public int TaxRate { get; set; }
    public string? Unit { get; set; }
    public int? StockQuantity { get; set; }
    public bool? IsStocking { get; set; }
    public bool? IsShipEnabled { get; set; } = true;
    public bool? IsFreeShipping { get; set; } = false;
    public decimal? AdditionalShippingCharge { get; set; }
    public bool? CanReturn { get; set; }
    public string? Tags { get; set; }
    public bool? LotSerial { get; set; }
    public bool? IsVariant { get; set; }
    public string? AttributesJson { get; set; }
    public int? VariantCount { get; set; }
    public string? Sku { get; set; }

    public bool? MultiPacking { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal? DomesticShippingFee { get; set; }




    public decimal MaxBargainPercent { get; set; }
    public decimal MinBargain { get; set; }
    public long? TransactionEvidenceId { get; set; }
    public string? TransactionEvidenceStatus { get; set; }
    public string? DeliveryCarrier { get; set; }
}
