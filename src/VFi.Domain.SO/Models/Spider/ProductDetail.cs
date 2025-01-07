namespace VFi.Domain.SO.Models.Spider;

public class ProductDetail
{
    public string ProductSource { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public string ProductLink { get; set; }
    public string PreviewImage { get; set; }

    public string? Description { get; set; }

    public string? SellerId { get; set; }

    /// <summary>
    /// Include meta infomation also Seller Name, Seller Ratting, Seller Point
    /// </summary>
    public IDictionary<string, string?>? ProductMeta { get; set; }

    public string? CategoryId { get; set; }

    public string Route { get; set; }
    public string Currency { get; set; }
    public decimal Price { get; set; }
    public decimal? BuyNowPrice { get; set; }
    public int? Tax { get; set; }

    /// <summary>
    /// ProductStatus.cs
    /// </summary>
    public int Status { get; set; }

    public int? Weight { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Length { get; set; }

    public int? MaxAvaiable { get; set; }

    public decimal? ShippingFee { get; set; }
    public IList<ProductAttribute>? Attributes { get; set; }
    public IList<string>? Images { get; set; }
    public int? Bids { get; set; }
    public DateTime? EndTime { get; set; }
    public bool? IsFreeDelivery { get; set; }

    public class ProductAttribute
    {
        public string Name { get; set; } // Type of Attribute
        public IList<ProductAttributeItem>? Items { get; set; }
    }
    public class ProductAttributeItem
    {
        public string? Value { get; set; }
        public string? Link { get; set; }
        public string? Icon { get; set; }
        public string? Image { get; set; }
    }
}
