using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;

namespace VFi.Domain.SO.Models;

public class FulfillmentRequestAddExt
{
    public int? Type { get; set; }
    public string? WarehouseCode { get; set; }
    public string? Sonumber { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public Guid? VendorId { get; set; }
    public string? VendorCode { get; set; }
    public string? VendorName { get; set; }
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
    public int? Status { get; set; }
    public string? Note { get; set; }
    public string? RequestByName { get; set; }
    public Guid? RequestBy { get; set; }
    public string? Code { get; set; }
    public int? IsAuto { get; set; }
    public List<ProductItemListAddExt>? Details { get; set; }
    public List<FileRequest>? Files { get; set; }
}
public class ProductItemListAddExt
{
    public Guid? WarehouseId { get; set; }
    public string? WarehouseCode { get; set; }
    public string? WarehouseName { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? ProductImage { get; set; }
    public string? Origin { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public double? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public double? TaxRate { get; set; }
    public string? Tax { get; set; }
    public string? Note { get; set; }
    public string? OrderCode { get; set; }
    public int? SortOrder { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public Guid? CustomerId { get; set; }
    public int? IsQC { get; set; }
    public int? DisplayOrder { get; set; }
}
public class FileRequest
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Path { get; set; }
    public decimal? Size { get; set; }
    public bool? Status { get; set; }
    public string? Title { get; set; }
    public string? Type { get; set; }
    public string? VirtualPath { get; set; }
}
