using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class PaymentInvoiceCommand : Command
{

    public Guid Id { get; set; }
    public string? Type { get; set; }
    public string? Code { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public Guid? SaleDiscountId { get; set; }
    public Guid? ReturnOrderId { get; set; }
    public string? Description { get; set; }
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public string? Calculation { get; set; }
    public decimal? ExchangeRate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? PaymentMethodName { get; set; }
    public string? PaymentMethodCode { get; set; }
    public Guid? PaymentMethodId { get; set; }
    public string? BankName { get; set; }
    public string? BankAccount { get; set; }
    public string? BankNumber { get; set; }
    public string? PaymentCode { get; set; }
    public string? PaymentNote { get; set; }
    public string? Note { get; set; }
    public int? Status { get; set; }
    public int? Locked { get; set; }
    public int? PaymentStatus { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? File { get; set; }
}

public class PaymentInvoiceAddCommand : PaymentInvoiceCommand
{
    public PaymentInvoiceAddCommand(
        Guid id,
        string? type,
        string? code,
        Guid? orderId,
        string? orderCode,
        Guid? saleDiscountId,
        Guid? returnOrderId,
        string? description,
        decimal? amount,
        string? currency,
        string? currencyName,
        string? calculation,
        decimal? exchangeRate,
        DateTime? paymentDate,
        string? paymentMethodName,
        string? paymentMethodCode,
        Guid? paymentMethodId,
        string? bankName,
        string? bankAccount,
        string? bankNumber,
        string? paymentCode,
        string? paymentNote,
        string? note,
        int? status,
        int? locked,
        int? paymentStatus,
        Guid? accountId,
        string? accountName,
        Guid? customerId,
        string? customerName,
        string? file
        )
    {
        Id = id;
        Type = type;
        Code = code;
        OrderId = orderId;
        OrderCode = orderCode;
        SaleDiscountId = saleDiscountId;
        ReturnOrderId = returnOrderId;
        Description = description;
        Amount = amount;
        Currency = currency;
        CurrencyName = currencyName;
        Calculation = calculation;
        ExchangeRate = exchangeRate;
        PaymentDate = paymentDate;
        PaymentMethodName = paymentMethodName;
        PaymentMethodCode = paymentMethodCode;
        PaymentMethodId = paymentMethodId;
        BankName = bankName;
        BankAccount = bankAccount;
        BankNumber = bankNumber;
        PaymentCode = paymentCode;
        PaymentNote = paymentNote;
        Note = note;
        Status = status;
        Locked = locked;
        PaymentStatus = paymentStatus;
        AccountId = accountId;
        AccountName = accountName;
        CustomerId = customerId;
        CustomerName = customerName;
        File = file;
    }
    public bool IsValid(IPaymentInvoiceRepository _context)
    {
        ValidationResult = new PaymentInvoiceAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PaymentInvoiceEditCommand : PaymentInvoiceCommand
{
    public PaymentInvoiceEditCommand(
        Guid id,
        string? type,
        string? code,
        Guid? orderId,
        string? orderCode,
        Guid? saleDiscountId,
        Guid? returnOrderId,
        string? description,
        decimal? amount,
        string? currency,
        string? currencyName,
        string? calculation,
        decimal? exchangeRate,
        DateTime? paymentDate,
        string? paymentMethodName,
        string? paymentMethodCode,
        Guid? paymentMethodId,
        string? bankName,
        string? bankAccount,
        string? bankNumber,
        string? paymentCode,
        string? paymentNote,
        string? note,
        int? status,
        int? locked,
        int? paymentStatus,
        Guid? accountId,
        string? accountName,
        Guid? customerId,
        string? customerName,
        string? file
        )
    {
        Id = id;
        Type = type;
        Code = code;
        OrderId = orderId;
        OrderCode = orderCode;
        SaleDiscountId = saleDiscountId;
        ReturnOrderId = returnOrderId;
        Description = description;
        Amount = amount;
        Currency = currency;
        CurrencyName = currencyName;
        Calculation = calculation;
        ExchangeRate = exchangeRate;
        PaymentDate = paymentDate;
        PaymentMethodName = paymentMethodName;
        PaymentMethodCode = paymentMethodCode;
        PaymentMethodId = paymentMethodId;
        BankName = bankName;
        BankAccount = bankAccount;
        BankNumber = bankNumber;
        PaymentCode = paymentCode;
        PaymentNote = paymentNote;
        Note = note;
        Status = status;
        Locked = locked;
        PaymentStatus = paymentStatus;
        AccountId = accountId;
        AccountName = accountName;
        CustomerId = customerId;
        CustomerName = customerName;
        File = file;
    }
    public bool IsValid(IPaymentInvoiceRepository _context)
    {
        ValidationResult = new PaymentInvoiceEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PaymentInvoiceDeleteCommand : PaymentInvoiceCommand
{
    public PaymentInvoiceDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IPaymentInvoiceRepository _context)
    {
        ValidationResult = new PaymentInvoiceDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class PaymentInvoiceChangeLockedCommand : Command
{
    public Guid Id { get; set; }
    public int? Locked { get; set; }
    public PaymentInvoiceChangeLockedCommand(
        Guid id,
        int? locked
        )
    {
        Id = id;
        Locked = locked;
    }
}
