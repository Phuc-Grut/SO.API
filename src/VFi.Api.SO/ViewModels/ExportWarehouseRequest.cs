using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class ExportWarehouseRequest : FopPagingRequest
{
    [FromQuery(Name = "$employeeId")]
    public string? EmployeeId { get; set; }
    [FromQuery(Name = "$customerId")]
    public Guid? CustomerId { get; set; }
}
public class AddExportWarehouseRequest
{
    public string? Code { get; set; }
    public string? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public string? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Description { get; set; }
    /// <summary>
    /// Kho
    /// </summary>
    public string? WarehouseId { get; set; }
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
    public string? DeliveryMethodId { get; set; }
    public string? DeliveryMethodCode { get; set; }
    public string? DeliveryMethodName { get; set; }
    public string? ShippingMethodId { get; set; }
    public string? ShippingMethodCode { get; set; }
    public string? ShippingMethodName { get; set; }
    public int? Status { get; set; }
    public string? Note { get; set; }
    public Guid? RequestBy { get; set; }
    public string? RequestByName { get; set; }
    public DateTime? RequestDate { get; set; }
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
    public List<FileRequest>? File { get; set; }
    public List<AddExportWarehouseProductRequest>? Details { get; set; }
}
public class EditExportWarehouseRequest : AddExportWarehouseRequest
{
    public Guid Id { get; set; }
    public List<DeleteOrderProductRequest>? Deletes { get; set; }
}
public class ExportWarehouseAddOrdersRequest
{
    public Guid Id { get; set; }
    public List<Guid>? OrderIds { get; set; }
}
public class ApprovalExportWarehouseRequest
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public string? ApproveComment { get; set; }
    public int? Type { get; set; }
    public string? WarehouseCode { get; set; }
    public string? Sonumber { get; set; }
    public string? CustomerCode { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? DeliveryName { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryCountry { get; set; }
    public string? DeliveryProvince { get; set; }
    public string? DeliveryDistrict { get; set; }
    public string? DeliveryWard { get; set; }
    public string? DeliveryNote { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public string? ShippingMethodCode { get; set; }
    public string? DeliveryMethodCode { get; set; }
    public int? WMSStatus { get; set; }
    public string? Note { get; set; }
    public string? RequestByName { get; set; }
    public Guid? RequestBy { get; set; }
    public List<ProductItemListAddExt>? Details { get; set; }
}
public class DuplicateExportWarehouse
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
    public string? ModuleCodeExport { get; set; }
}

public class UpdateServiceFeesRequest
{
    public Guid Id { get; set; }
    public Guid ServiceAddId { get; set; }
    public Guid? ServiceAddCurrencyId { get; set; }
    public string? ServiceAddCurrencyCode { get; set; }
    public string? ServiceAddName { get; set; }
    public decimal PriceTotal { get; set; }
}