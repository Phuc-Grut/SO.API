using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class ProductionOrderRequest : FopPagingRequest
{
}
public class ADDProductionOrderRequest
{
    public string Code { get; set; } = null!;
    public string? Note { get; set; }
    public int? Status { get; set; }
    public DateTime? RequestDate { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime? DateNeed { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderNumber { get; set; }
    public Guid? SaleEmployeeId { get; set; }
    public string? SaleEmployeeCode { get; set; }
    public string? SaleEmployeeName { get; set; }
    public int? Type { get; set; }
    public DateTime? EstimateDate { get; set; }
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
    public List<FileRequest>? File { get; set; }

}
public class AddProductionOrderRequest : ADDProductionOrderRequest
{
    public List<AddProductionOrdersDetailRequest>? ListProductionOrdersDetail { get; set; }
}
public class EditProductionOrderRequest : ADDProductionOrderRequest
{
    public string Id { get; set; }
    public List<EditProductionOrdersDetailRequest>? ListProductionOrdersDetail { get; set; }
    public List<IdRequest>? DeleteProductionOrdersDetail { get; set; }
}
public class EditProductionOrderRequestNew : ADDProductionOrderRequest
{
    public string Id { get; set; }
    public List<EditProductionOrdersDetailRequestNew>? ListProductionOrdersDetail { get; set; }
    public List<IdRequest>? DeleteProductionOrdersDetail { get; set; }
}

public class ConfirmProductionOrdersRequest
{
    public Guid Id { get; set; }
    public int Status { get; set; }

}
public class ImportProductionOrdersRequest
{
    [FromQuery(Name = "$type")]
    public int Type { get; set; }

}
