using FluentValidation;
using Microsoft.AspNetCore.Http;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class ProductionOrderCommand : Command
{
    public Guid Id { get; set; }
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
    public string? File { get; set; }
    public List<ProductionOrdersDetailDto>? ListProductionOrdersDetail { get; set; }

}

public class ProductionOrderAddCommand : ProductionOrderCommand
{
    public ProductionOrderAddCommand(
        Guid Id,
        string Code,
        string? Note,
        int? Status,
        DateTime? RequestDate,
        Guid? CustomerId,
        string? CustomerCode,
        string? CustomerName,
        string? Email,
        string? Phone,
        string? Address,
        Guid? EmployeeId,
        string? EmployeeCode,
        string? EmployeeName,
        DateTime? DateNeed,
        Guid? OrderId,
        string? OrderNumber,
        Guid? SaleEmployeeId,
        string? SaleEmployeeCode,
        string? SaleEmployeeName,
        int? Type,
        DateTime? estimateDate,
        string? file,
        List<ProductionOrdersDetailDto>? ListProductionOrdersDetail
        )
    {
        this.Id = Id;
        this.Code = Code;
        this.Note = Note;
        this.Status = Status;
        this.RequestDate = RequestDate;
        this.CustomerId = CustomerId;
        this.CustomerCode = CustomerCode;
        this.CustomerName = CustomerName;
        this.Email = Email;
        this.Phone = Phone;
        this.Address = Address;
        this.EmployeeId = EmployeeId;
        this.EmployeeCode = EmployeeCode;
        this.EmployeeName = EmployeeName;
        this.DateNeed = DateNeed;
        this.OrderId = OrderId;
        this.OrderNumber = OrderNumber;
        this.SaleEmployeeId = SaleEmployeeId;
        this.SaleEmployeeCode = SaleEmployeeCode;
        this.SaleEmployeeName = SaleEmployeeName;
        this.Type = Type;
        this.EstimateDate = estimateDate;
        this.File = file;
        this.ListProductionOrdersDetail = ListProductionOrdersDetail;
    }
    public bool IsValid(IProductionOrderRepository _context)
    {
        ValidationResult = new ProductionOrderAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ProductionOrderEditCommand : ProductionOrderCommand
{
    public List<ListId>? DeleteProductionOrdersDetail { get; set; }
    public ProductionOrderEditCommand(
        Guid Id,
        string Code,
        string? Note,
        int? Status,
        DateTime? RequestDate,
        Guid? CustomerId,
        string? CustomerCode,
        string? CustomerName,
        string? Email,
        string? Phone,
        string? Address,
        Guid? EmployeeId,
        string? EmployeeCode,
        string? EmployeeName,
        DateTime? DateNeed,
        Guid? OrderId,
        string? OrderNumber,
        Guid? SaleEmployeeId,
        string? SaleEmployeeCode,
        string? SaleEmployeeName,
        int? Type,
        DateTime? estimateDate,
        string? file,
        List<ProductionOrdersDetailDto>? ListProductionOrdersDetail,
        List<ListId>? DeleteProductionOrdersDetail
                                )
    {
        this.Id = Id;
        this.Code = Code;
        this.Note = Note;
        this.Status = Status;
        this.RequestDate = RequestDate;
        this.CustomerId = CustomerId;
        this.CustomerCode = CustomerCode;
        this.CustomerName = CustomerName;
        this.Email = Email;
        this.Phone = Phone;
        this.Address = Address;
        this.EmployeeId = EmployeeId;
        this.EmployeeCode = EmployeeCode;
        this.EmployeeName = EmployeeName;
        this.DateNeed = DateNeed;
        this.OrderId = OrderId;
        this.OrderNumber = OrderNumber;
        this.SaleEmployeeId = SaleEmployeeId;
        this.SaleEmployeeCode = SaleEmployeeCode;
        this.SaleEmployeeName = SaleEmployeeName;
        this.Type = Type;
        this.EstimateDate = estimateDate;
        this.File = file;
        this.ListProductionOrdersDetail = ListProductionOrdersDetail;
        this.DeleteProductionOrdersDetail = DeleteProductionOrdersDetail;
    }
    public bool IsValid(IProductionOrderRepository _context)
    {
        ValidationResult = new ProductionOrderEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ProductionOrderDeleteCommand : ProductionOrderCommand
{
    public ProductionOrderDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IProductionOrderRepository _context)
    {
        ValidationResult = new ProductionOrderDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ProductionOrdersConfirmCommand : ProductionOrderCommand
{
    public ProductionOrdersConfirmCommand(Guid Id, int? Status)
    {
        this.Id = Id;
        this.Status = Status;
    }
    public bool IsValid(IProductionOrderRepository _context)
    {
        ValidationResult = new ProductionOrdersConfirmCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ValidateProductionOrders
{
    public IFormFile File { get; set; } = null!;
    public string SheetId { get; set; } = null!;
    public int HeaderRow { get; set; }
    public int? ProductCode { get; set; }
    public int? ProductName { get; set; }
    public int? UnitCode { get; set; }
    public int? UnitName { get; set; }
    public int? QuantityRequest { get; set; }
    public int? Note { get; set; }
}
