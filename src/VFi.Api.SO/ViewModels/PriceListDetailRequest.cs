using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddPriceListDetailRequest
{
    public string? Id { get; set; }
    public Guid? PriceListId { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencyName { get; set; }
    public int? QuantityMin { get; set; }
    public int? Type { get; set; }
    public decimal? FixPrice { get; set; }
    public int? TypeDiscount { get; set; }
    public double? DiscountRate { get; set; }
    public decimal? DiscountValue { get; set; }
    public int? DisplayOrder { get; set; }
}
public class EditPriceListDetailRequest : AddPriceListDetailRequest
{
    public Guid Id { get; set; }
}
