using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddRequestPurchaseRequest
{
    public Guid Id { get; set; }
    public string? Code { get; set; } = null!;
    public Guid? RequestBy { get; set; }
    public string? RequestByName { get; set; }
    public string? RequestByEmail { get; set; }
    public DateTime? RequestDate { get; set; }
    /// <summary>
    /// Mục đích đề nghị
    /// </summary>
    public string? CurrencyCode { get; set; }
    public string? CurrencyName { get; set; }
    public string? Calculation { get; set; }
    public decimal? ExchangeRate { get; set; }
    public string? Proposal { get; set; }
    public string? Note { get; set; }
    public DateTime? ApproveDate { get; set; }
    public Guid? ApproveBy { get; set; }
    public string? ApproveByName { get; set; }
    public string? ApproveComment { get; set; }
    /// <summary>
    /// Trạng thái: 1-Sử dụng, 0-Không sử dụng
    /// </summary>
    public int? Status { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    /// <summary>
    /// Số lượng đề nghị
    /// </summary>
    public double? QuantityRequest { get; set; }
    /// <summary>
    /// Số lượng duyệt
    /// </summary>
    public double? QuantityApproved { get; set; }
    public int? StatusPurchase { get; set; }
    public string? ModuleCode { get; set; }
    public bool? IsAuto { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public List<FileRequest>? File { get; set; }
    public List<RequestPurchaseProductRequest>? Details { get; set; }
}
public class EditRequestPurchaseRequest : AddRequestPurchaseRequest
{
    public List<DeleteOrderProductRequest>? Deletes { get; set; }
}
public class ProcessRequestPurchaseRequest
{
    public string Id { get; set; }
    public int? Status { get; set; }
    public string? ApproveComment { get; set; }
    public int? POStatus { get; set; }
}
public class ApprovalRequestPurchaseRequest
{
    public string Id { get; set; }
    public List<RequestPurchaseProductRequest>? Details { get; set; }
}
public class RequestPurchasePagingRequest : FilterQuery
{
    [FromQuery(Name = "$employeeId")]
    public string? EmployeeId { get; set; }
}
public class DuplicateRequestPurchase
{
    public Guid RequestPurchaseId { get; set; }
    public string? Code { get; set; }
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
}

public class PurchaseRequestPurchase
{
    public Guid Id { get; set; }
    public int? POStatus { get; set; }
}

public class PurchaseRequestUpdateQuantityRequest
{
    public List<POPurchaseProductRequest>? ListUpdate { get; set; }
}
public class POPurchaseProductRequest
{
    public int? StatusPurchase { get; set; }
    public Guid? ProductId { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public double? Quantity { get; set; }
    public string? PurchaseRequestCode { get; set; }
}
public class UpdatePurchaseQtyRequest
{
    public int? NotCancel { get; set; }
    public Guid Id { get; set; }
    public string? PurchaseRequestCode { get; set; }
}
public class ImportExcelRequestPurchase
{
    public IFormFile File { get; set; } = null!;
    public string SheetId { get; set; } = null!;
    public int HeaderRow { get; set; }
    public int? RequestPurchaseId { get; set; }
    public int? OrderProductId { get; set; }
    public int? ProductId { get; set; }
    public int? ProductCode { get; set; }
    public int? ProductName { get; set; }
    public int? ProductImage { get; set; }
    public int? Origin { get; set; }
    public int? UnitType { get; set; }
    public int? UnitCode { get; set; }
    public int? UnitName { get; set; }
    public double? QuantityRequest { get; set; }
    public double? QuantityApproved { get; set; }
    public int? UnitPrice { get; set; }
    public int? Currency { get; set; }
    public int? DeliveryDate { get; set; }
    public int? PriorityLevel { get; set; }
    public int? Note { get; set; }
    public int? VendorCode { get; set; }
    public int? VendorName { get; set; }
    public int? Status { get; set; }
    public int? StatusPurchase { get; set; }
    public double? QuantityPurchased { get; set; }
    public int? DisplayOrder { get; set; }
}
