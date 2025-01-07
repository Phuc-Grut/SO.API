using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using MassTransit.Internals.GraphValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class ReturnOrderCommand : Command
{
    public Guid Id { get; set; }
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
    public string? File { get; set; }
    public List<ReturnOrderProductDto>? ReturnOrderProduct { get; set; }
}

public class AddReturnOrderCommand : ReturnOrderCommand
{
    public AddReturnOrderCommand(
        Guid id,
        string? code,
        Guid? customerId,
        string? customerCode,
        string? customerName,
        Guid? orderId,
        string? orderCode,
        string? address,
        string? country,
        string? province,
        string? district,
        string? ward,
        string? description,
        int? status,
        Guid? warehouseId,
        string? warehouseCode,
        string? warehouseName,
        DateTime? returnDate,
        Guid? accountId,
        string? accountName,
        Guid? currencyId,
        string? currency,
        string? currencyName,
        string? calculation,
        decimal? exchangeRate,
        int? typeDiscount,
        double? discountRate,
        int? typeCriteria,
        decimal? amountDiscount,
        Guid? approveBy,
        DateTime? approveDate,
        string? approveByName,
        string? approveComment,
        string? file,
        List<ReturnOrderProductDto>? returnOrderProduct)
    {
        Id = id;
        Code = code;
        CustomerId = customerId;
        CustomerCode = customerCode;
        CustomerName = customerName;
        OrderId = orderId;
        OrderCode = orderCode;
        Address = address;
        Country = country;
        Province = province;
        District = district;
        Ward = ward;
        Description = description;
        Status = status;
        WarehouseId = warehouseId;
        WarehouseCode = warehouseCode;
        WarehouseName = warehouseName;
        ReturnDate = returnDate;
        AccountId = accountId;
        AccountName = accountName;
        CurrencyId = currencyId;
        Currency = currency;
        CurrencyName = currencyName;
        Calculation = calculation;
        ExchangeRate = exchangeRate;
        TypeDiscount = typeDiscount;
        DiscountRate = discountRate;
        TypeCriteria = typeCriteria;
        AmountDiscount = amountDiscount;
        ApproveBy = approveBy;
        ApproveDate = approveDate;
        ApproveByName = approveByName;
        ApproveComment = approveComment;
        File = file;
        ReturnOrderProduct = returnOrderProduct;
    }

    public bool IsValid(IReturnOrderRepository _context)
    {
        ValidationResult = new AddReturnOrderValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class EditReturnOrderCommand : ReturnOrderCommand
{
    public EditReturnOrderCommand(
        Guid id,
        string? code,
        Guid? customerId,
        string? customerCode,
        string? customerName,
        Guid? orderId,
        string? orderCode,
        string? address,
        string? country,
        string? province,
        string? district,
        string? ward,
        string? description,
        int? status,
        Guid? warehouseId,
        string? warehouseCode,
        string? warehouseName,
        DateTime? returnDate,
        Guid? accountId,
        string? accountName,
        Guid? currencyId,
        string? currency,
        string? currencyName,
        string? calculation,
        decimal? exchangeRate,
        int? typeDiscount,
        double? discountRate,
        int? typeCriteria,
        decimal? amountDiscount,
        Guid? approveBy,
        DateTime? approveDate,
        string? approveByName,
        string? approveComment,
        string? file,
        List<ReturnOrderProductDto>? returnOrderProduct)
    {
        Id = id;
        Code = code;
        CustomerId = customerId;
        CustomerCode = customerCode;
        CustomerName = customerName;
        OrderId = orderId;
        OrderCode = orderCode;
        Address = address;
        Country = country;
        Province = province;
        District = district;
        Ward = ward;
        Description = description;
        Status = status;
        WarehouseId = warehouseId;
        WarehouseCode = warehouseCode;
        WarehouseName = warehouseName;
        ReturnDate = returnDate;
        AccountId = accountId;
        AccountName = accountName;
        CurrencyId = currencyId;
        Currency = currency;
        CurrencyName = currencyName;
        Calculation = calculation;
        ExchangeRate = exchangeRate;
        TypeDiscount = typeDiscount;
        DiscountRate = discountRate;
        TypeCriteria = typeCriteria;
        AmountDiscount = amountDiscount;
        ApproveBy = approveBy;
        ApproveDate = approveDate;
        ApproveByName = approveByName;
        ApproveComment = approveComment;
        File = file;
        ReturnOrderProduct = returnOrderProduct;
    }

    public bool IsValid(IReturnOrderRepository _context)
    {
        ValidationResult = new EditReturnOrderValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class DeleteReturnOrderCommand : ReturnOrderCommand
{
    public DeleteReturnOrderCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IReturnOrderRepository _context)
    {
        ValidationResult = new DeteleReturnOrderValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ReturnOrderProcessCommand : ReturnOrderCommand
{
    public ReturnOrderProcessCommand(
         Guid id,
         string? approveComment,
         int? status
      )
    {
        Id = id;
        ApproveComment = approveComment;
        Status = status;
    }
}
public class ReturnOrderDuplicateCommand : ReturnOrderCommand
{
    public Guid ReturnOrderId { get; set; }
    public ReturnOrderDuplicateCommand(
        Guid id,
        Guid returnOrderId,
        string code)
    {
        Id = id;
        Code = code;
        ReturnOrderId = returnOrderId;
    }
    public bool IsValidReturnOrder(IReturnOrderRepository _context)
    {
        ValidationResult = new ReturnOrderDuplicateCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ManagePaymentReturnCommand : Command
{
    public Guid Id { get; set; }
    public int? PaymentStatus { get; set; }
    public List<PaymentInvoiceDto>? PaymentInvoice { get; set; }
    public ManagePaymentReturnCommand(
        Guid id,
        int? paymentStatus,
        List<PaymentInvoiceDto>? paymentInvoice)
    {
        Id = id;
        PaymentStatus = paymentStatus;
        PaymentInvoice = paymentInvoice;
    }
}

public class ReturnOrderUploadFileCommand : Command
{
    public Guid Id { get; set; }
    public string? File { get; set; }
    public ReturnOrderUploadFileCommand(
        Guid id,
        string? file
    )
    {
        Id = id;
        File = file;
    }
}

public class ValidateExcelReturnOrder
{
    public IFormFile File { get; set; } = null!;
    public string SheetId { get; set; } = null!;
    public int HeaderRow { get; set; }
    public int? ProductCode { get; set; }
    public int? ProductName { get; set; }
    public int? UnitCode { get; set; }
    public int? UnitPrice { get; set; }
    public int? UnitName { get; set; }
    public int? Tax { get; set; }
    public int? QuantityReturn { get; set; }
    public int? ReasonName { get; set; }
    public int? DiscountPercent { get; set; }

}
