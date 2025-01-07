namespace VFi.Application.SO.DTOs;

public class ProductionOrdersDetailDto
{
    public Guid Id { get; set; }
    public Guid ProductionOrdersId { get; set; }
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
    public string? CustomerName { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public DateTime? EstimateDate { get; set; }
    public bool? IsEstimated { get; set; }
    public bool? IsBom { get; set; }
    public int? Status { get; set; }
    public DateTime? EstimatedDate { get; set; }
    public int? EstimateStatus { get; set; }
    public string? OrderNumber { get; set; }
    public DateTime? DateNeed { get; set; }
    public string? CancelReason { get; set; }
    public string? Solution { get; set; }
    public string? Transport { get; set; }
    public double? Height { get; set; }
    public double? Package { get; set; }
    public double? Volume { get; set; }
    public double? Length { get; set; }
    public double? Weight { get; set; }
    public double? Width { get; set; }
    public int? DisplayOrder { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }

}

public class ProductionOrdersDetailCountTotalDto : ProductionOrdersDetailDto
{
    public decimal? Total { get; set; }
}
public class TotalCostEstimateOfProductOrderDetailDto
{
    public Guid Id { get; set; }
    public decimal? Total { get; set; }
}
