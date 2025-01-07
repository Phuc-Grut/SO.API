namespace VFi.Domain.SO.Models.Spider.YahooAucton;

public class AuctionProductPublish
{
    public int ProductType { get; set; }
    public Guid? Id { get; set; }
    public string Code { get; set; }
    /// <summary>
    /// New = 0, Refurbished = 10,20 = LikeNew, Used = 30, Used40 = 31, Damaged = 40
    /// </summary>
    public int Condition { get; set; }
    public string LinkInfo { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string FullDescription { get; set; }
    public string Origin { get; set; }
    public string Source { get; set; }
    public string Brand { get; set; }
    public string Manufacturer { get; set; }
    public string Image { get; set; }
    public string Gtin { get; set; }
    public List<string> Images { get; set; } = new List<string>();
    public decimal? Price { get; set; }
    public string Currency { get; set; }
    public bool? IsTaxExempt { get; set; }
    public int TaxRate { get; set; }
    public string Unit { get; set; }
    public int? StockQuantity { get; set; }
    public bool? IsStocking { get; set; }
    public bool? IsShipEnabled { get; set; } = true;
    public bool? IsFreeShipping { get; set; } = false;
    public decimal? AdditionalShippingCharge { get; set; }
    public bool? CanReturn { get; set; }
    public string Tags { get; set; }
    public bool? LotSerial { get; set; }
    public bool? IsVariant { get; set; }
    public string? AttributesJson { get; set; }
    public int? VariantCount { get; set; }
    public string Sku { get; set; }

    public bool? MultiPacking { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }


    public string OriginCategoryId { get; set; }
    public string OriginCategory { get; set; }
    public string OriginCategoryIdPath { get; set; }
    public string OriginCategoryPath { get; set; }

    public bool IsFreeDomesticShipping { get; set; } = false;
    public decimal? DomesticShippingFee { get; set; }


    public AuctionInfo Auction { get; set; } = new AuctionInfo();

    public class AuctionInfo
    {

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? StartPrice { get; set; }
        public decimal? HighestBid { get; set; }
        public decimal? BuyNowPrice { get; set; }
        public bool? IsAutomaticExtension { get; set; }
        public bool? IsEarlyClosing { get; set; }
        public int Bids { get; set; }
        public bool? IsClosed { get; set; }
        public bool AuctionEnded { get; set; }

    }
}
