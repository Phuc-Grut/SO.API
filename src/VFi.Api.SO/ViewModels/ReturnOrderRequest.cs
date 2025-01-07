using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class ReturnOrderRequest
{
    public string? Code { get; set; } = null!;
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public Guid? WarehouseId { get; set; }
    public string? WarehouseCode { get; set; }
    public string? WarehouseName { get; set; }
    public DateTime? ReturnDate { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public string? Calculation { get; set; }
    public decimal? ExchangeRate { get; set; }
    public int? TypeDiscount { get; set; }
    public double? DiscountRate { get; set; }
    public int? TypeCriteria { get; set; }
    public decimal? AmountDiscount { get; set; }
    public Guid? ApproveBy { get; set; }
    public DateTime? ApproveDate { get; set; }
    public string? ApproveByName { get; set; }
    public string? ApproveComment { get; set; }
    public List<FileRequest>? File { get; set; }
    public List<AddReturnOrderProductRequest>? ReturnOrderProduct { get; set; }
}
public class AddReturnOrderRequest : ReturnOrderRequest
{
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
}
public class EditReturnOrderRequest : ReturnOrderRequest
{
    public Guid Id { get; set; }
}

public class ReturnOrderPagingRequest : FilterQuery
{
    public Guid? CustomerId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    [FromQuery(Name = "$employeeId")]
    public string? EmployeeId { get; set; }
}

public class ProcessReturnOrderRequest
{
    public string Id { get; set; }
    public string? ApproveComment { get; set; }
    public int? Status { get; set; }
}

public class DuplicateReturnOrder
{
    public Guid ReturnOrderId { get; set; }
    public string? Code { get; set; }
    public int? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
}
