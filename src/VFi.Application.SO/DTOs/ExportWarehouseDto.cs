namespace VFi.Application.SO.DTOs;

public class ExportWarehouseDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Description { get; set; }
    /// <summary>
    /// Kho
    /// </summary>
    public Guid? WarehouseId { get; set; }
    /// <summary>
    /// Kho
    /// </summary>
    public string? WarehouseCode { get; set; }
    public string? WarehouseName { get; set; }
    public int? DeliveryStatus { get; set; }
    public string? DeliveryName { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryCountry { get; set; }
    public string? DeliveryProvince { get; set; }
    public string? DeliveryDistrict { get; set; }
    public string? DeliveryWard { get; set; }
    public string? DeliveryNote { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public Guid? DeliveryMethodId { get; set; }
    public string? DeliveryMethodCode { get; set; }
    public string? DeliveryMethodName { get; set; }
    public Guid? ShippingMethodId { get; set; }
    public string? ShippingMethodCode { get; set; }
    public string? ShippingMethodName { get; set; }
    public int? Status { get; set; }
    public int? StatusExport { get; set; }
    public string? Note { get; set; }
    public string? RequestByName { get; set; }
    public DateTime? RequestDate { get; set; }
    public Guid? RequestBy { get; set; }
    public DateTime? ApproveDate { get; set; }
    public Guid? ApproveBy { get; set; }
    public string? ApproveByName { get; set; }
    public string? ApproveComment { get; set; }
    public string? FulfillmentRequestCode { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public int? TypeDiscount { get; set; }
    public double? DiscountRate { get; set; }
    public int? TypeCriteria { get; set; }
    public int? ExportsStatus { get; set; }
    public decimal? AmountDiscount { get; set; }
    public List<FileDto>? File { get; set; }
    public List<ExportWarehouseProductDto>? Details { get; set; }
}
public class ExportWarehouseValidateDto
{
    public UInt32 Row { get; set; }
    public bool IsValid
    {
        get
        {
            return Errors.Count == 0;
        }
    }
    public List<string> Errors { get; set; } = new List<string>();
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public Guid? UnitId { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public string? QuantityRequest { get; set; }
    public string? StockQuantity { get; set; }
    public string? Note { get; set; }
    public string? UnitType { get; set; }
}
