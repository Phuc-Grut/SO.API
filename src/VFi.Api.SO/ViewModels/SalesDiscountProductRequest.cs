

namespace VFi.Api.SO.ViewModels;

public class SalesDiscountProductRequest
{
    public Guid? Id { get; set; }
    public Guid? SalesDiscountId { get; set; }
    public Guid SalesOrderId { get; set; }
    public Guid? OrderProductId { get; set; }
    public Guid ProductId { get; set; }
    public string? SalesOrderCode { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? ProductImage { get; set; }
    public double? Quantity { get; set; }
    public Guid? UnitId { get; set; }
    public string? UnitName { get; set; }
    public string? UnitCode { get; set; }
    public Guid? GroupUnitId { get; set; }
    public string? UnitType { get; set; }
    public decimal? UnitPrice { get; set; }
    public Guid? TaxCategoryId { get; set; }
    public double? TaxRate { get; set; }
    public string? Tax { get; set; }
    public string? ReasonDiscount { get; set; }
    public int? DisplayOrder { get; set; }
    public decimal? DiscountAmountDistribution { get; set; }
    public decimal? AmountDiscount { get; set; }
    public double? DiscountPercent { get; set; }
}
public class AddSalesDiscountProductRequest : SalesDiscountProductRequest
{
}
public class EditSalesDiscountProductRequest : SalesDiscountProductRequest
{
    public string? Id { get; set; }
}
public class DeleteSalesDiscountProductRequest
{
    public Guid Id { get; set; }
}
