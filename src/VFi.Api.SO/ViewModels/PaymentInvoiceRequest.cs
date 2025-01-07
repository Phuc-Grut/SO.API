using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddPaymentInvoiceRequest
{
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
    public Guid? Id { get; set; }
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
    public List<FileRequest>? File { get; set; }
}
public class EditPaymentInvoiceRequest : AddPaymentInvoiceRequest
{
}

public class DeletePaymentInvoiceRequest
{
    public Guid? Id { get; set; }
}
public class PaymentInvoiceChangeLockedRequest
{
    public Guid Id { get; set; }
    public int? Locked { get; set; }
}
public class PaymentInvoiceListBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$orderId")]
    public Guid? OrderId { get; set; }
    public PaymentInvoiceQueryParams ToBaseQuery() => new PaymentInvoiceQueryParams
    {
        Status = Status,
        OrderId = OrderId
    };
}
public class ManagePaymentRequest
{
    public Guid Id { get; set; }
    public int? PaymentStatus { get; set; }
    public List<AddPaymentInvoiceRequest>? PaymentInvoice { get; set; }
}
