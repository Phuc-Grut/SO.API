namespace VFi.Application.SO.DTOs;

public partial class OrderProductDto
{
    public decimal? DiscountAmount { get; set; }
    public string? BidUsername { get; set; }
    public string? SourceLink { get; set; }
    public string SourceCode { get; set; }
    public string? Attributes { get; set; }
    public string SellerId { get; set; }
}
