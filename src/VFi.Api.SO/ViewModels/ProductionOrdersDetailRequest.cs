using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class ProductionOrdersDetailRequest
{
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public Guid? OrderProductId { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? ProductImage { get; set; }
    public string? Sku { get; set; }
    public string? Gtin { get; set; }
    public string? Origin { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public double? Quantity { get; set; }
    public string? Note { get; set; }
    public double? EstimatedDeliveryQuantity { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public bool? IsWorkOrdered { get; set; }
    public string? ProductionOrdersCode { get; set; }
    public bool? IsEstimated { get; set; }
    public bool? IsBom { get; set; }
    public int? Status { get; set; }
    public DateTime? EstimatedDate { get; set; }
    public int? EstimateStatus { get; set; }
    public string? Solution { get; set; }
    public string? Transport { get; set; }
    public double? Height { get; set; }
    public double? Package { get; set; }
    public double? Volume { get; set; }
    public double? Length { get; set; }
    public double? Weight { get; set; }
    public double? Width { get; set; }
    public string? CancelReason { get; set; }
    public int? DisplayOrder { get; set; }
}
public class AddProductionOrdersDetailRequest : ProductionOrdersDetailRequest
{
}
public class EditProductionOrdersDetailRequest : ProductionOrdersDetailRequest
{
    public Guid? Id { get; set; }
}
public class EditProductionOrdersDetailRequestNew : ProductionOrdersDetailRequest
{
    public string? Id { get; set; }
}
public class EditPackingProductionOrdersDetailRequest
{
    public Guid Id { get; set; }
    public Guid? ProductId { get; set; }
    public string? Solution { get; set; }
    public string? Transport { get; set; }
    public double? Height { get; set; }
    public double? Package { get; set; }
    public double? Volume { get; set; }
    public double? Length { get; set; }
    public double? Weight { get; set; }
    public double? Width { get; set; }

}
public class CancelProductionOrdersDetailRequest
{
    public Guid Id { get; set; }
    public string? CancelReason { get; set; }
    public int Status { get; set; }

}
public class CompleteProductionOrdersDetailRequest
{
    public Guid Id { get; set; }
    public int Status { get; set; }
}
