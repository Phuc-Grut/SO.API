using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddPromotionByValueRequest
{
    public string? Id { get; set; }
    public Guid? PromotionId { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public int? Type { get; set; }
    public decimal? MinOrderPrice { get; set; }
    public decimal? LimitTotalValue { get; set; }
    public double? DiscountPercent { get; set; }
    public decimal? ReduceAmount { get; set; }
    public decimal? FixPrice { get; set; }
    public int? TypeBonus { get; set; }
    public int? TypeBuy { get; set; }
    public double? Quantity { get; set; }
    public double? QuantityBuy { get; set; }
    public List<AddPromotionProductRequest>? ProductBonus { get; set; }
    public List<AddPromotionProductBuyRequest>? ProductBuy { get; set; }
}
public class EditPromotionByValueRequest : AddPromotionByValueRequest
{
    public Guid Id { get; set; }
}

public class DeletePromotionByValueRequest
{
    public Guid? Id { get; set; }
}
