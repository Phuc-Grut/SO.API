using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddPromotionProductBuyRequest
{
    public string? Id { get; set; }
    public Guid? PromotionId { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public double? Quantity { get; set; }
}
public class EditPromotionProductBuyRequest : AddPromotionProductBuyRequest
{
    public Guid Id { get; set; }
}

public class DeletePromotionProductBuyRequest
{
    public Guid? Id { get; set; }
}
