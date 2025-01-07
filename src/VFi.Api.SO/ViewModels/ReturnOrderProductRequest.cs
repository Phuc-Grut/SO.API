using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddReturnOrderProductRequest
{
    public Guid? Id { get; set; }
    public Guid? ReturnOrderId { get; set; }
    public Guid? OrderProductId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductCode { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public decimal? QuantityReturn { get; set; }
    public decimal? UnitPrice { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public decimal? DiscountAmountDistribution { get; set; }
    public int? DiscountType { get; set; }
    public double? DiscountPercent { get; set; }
    public decimal? AmountDiscount { get; set; }
    public double? TaxRate { get; set; }
    public string? Tax { get; set; }
    public string? TaxCode { get; set; }
    public Guid? ReasonId { get; set; }
    public string? ReasonName { get; set; }
    public Guid? WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
    public int? DisplayOrder { get; set; }
}
public class EditReturnOrderProductRequest : AddReturnOrderProductRequest
{
    public string? Id { get; set; }
}
