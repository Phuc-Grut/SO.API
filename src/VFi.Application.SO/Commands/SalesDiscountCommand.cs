using Microsoft.AspNetCore.Http;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class SalesDiscountCommand : Command
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public Guid? CustomerAddressId { get; set; }
    public string? SalesOrderCode { get; set; }
    public Guid? SalesOrderId { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencyName { get; set; }
    public decimal? ExchangeRate { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }
    public string? Note { get; set; }
    public int? Status { get; set; }
    public int? TypeDiscount { get; set; }
    public DateTime? DiscountDate { get; set; }
    public string File { get; set; }
    public Guid? ApproveBy { get; set; }
    public DateTime? ApproveDate { get; set; }
    public string? ApproveByName { get; set; }
    public string? ApproveComment { get; set; }
    public Boolean? IsUploadFile { get; set; }
    public List<SalesDiscountProductDto>? ListDetail { get; set; }

}

public class SalesDiscountUploadFileCommand : SalesDiscountCommand
{
    public SalesDiscountUploadFileCommand(
     Guid id,
     string? file
    )
    {
        Id = id;
        File = file;
    }
    public bool IsValidSalesDiscount(ISalesDiscountRepository _context)
    {
        ValidationResult = new SalesDiscountUploadFileCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class SalesDiscountAddCommand : SalesDiscountCommand
{
    public SalesDiscountAddCommand(
                                    Guid id,
                                    string code,
                                    Guid? customerId,
                                    string? customerName,
                                    string? customerCode,
                                    Guid? customerAddressId,
                                    string? salesOrderCode,
                                    Guid? salesOrderId,
                                    Guid? currencyId,
                                    string? currencyCode,
                                    string? currencyName,
                                    decimal? exchangeRate,
                                    Guid? employeeId,
                                    string? employeeName,
                                    string? employeeCode,
                                    string? note,
                                    int? status,
                                    int? typeDiscount,
                                    DateTime? discountDate,
                                    string? file,
     List<SalesDiscountProductDto>? listDetail)
    {
        Id = id;
        Code = code;
        CustomerId = customerId;
        CustomerCode = customerCode;
        CustomerName = customerName;
        CustomerAddressId = customerAddressId;
        SalesOrderCode = salesOrderCode;
        SalesOrderId = salesOrderId;
        CurrencyId = currencyId;
        CurrencyCode = currencyCode;
        CurrencyName = currencyName;
        ExchangeRate = exchangeRate;
        EmployeeId = employeeId;
        EmployeeName = employeeName;
        EmployeeCode = employeeCode;
        Note = note;
        Status = status;
        TypeDiscount = typeDiscount;
        File = file;
        DiscountDate = discountDate;
        ListDetail = listDetail;
    }
    public bool IsValidSalesDiscount(ISalesDiscountRepository _context)
    {
        ValidationResult = new SalesDiscountAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class SalesDiscountEditCommand : SalesDiscountCommand
{
    public SalesDiscountEditCommand(
                                    Guid id,
                                    string code,
                                    Guid? customerId,
                                    string? customerName,
                                    string? customerCode,
                                    Guid? customerAddressId,
                                    string salesOrderCode,
                                    Guid? salesOrderId,
                                    Guid? currencyId,
                                    string currencyCode,
                                    string currencyName,
                                    decimal? exchangeRate,
                                    Guid? employeeId,
                                    string? employeeName,
                                    string? employeeCode,
                                    string note,
                                    int? status,
                                    int? typeDiscount,
                                    DateTime? discountDate,
                                    string? file,
                                    List<SalesDiscountProductDto>? listDetail)
    {
        Id = id;
        Code = code;
        CustomerId = customerId;
        CustomerName = customerName;
        CustomerCode = customerCode;
        CustomerAddressId = customerAddressId;
        SalesOrderCode = salesOrderCode;
        SalesOrderId = salesOrderId;
        CurrencyId = currencyId;
        CurrencyCode = currencyCode;
        CurrencyName = currencyName;
        ExchangeRate = exchangeRate;
        EmployeeId = employeeId;
        EmployeeName = employeeName;
        EmployeeCode = employeeCode;
        Note = note;
        Status = status;
        TypeDiscount = typeDiscount;
        DiscountDate = discountDate;
        File = file;
        ListDetail = listDetail;
    }
    public bool IsValidSalesDiscount(ISalesDiscountRepository _context)
    {
        ValidationResult = new SalesDiscountEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class SalesDiscountDuplicateCommand : SalesDiscountCommand
{
    public Guid SalesDiscountId { get; set; }
    public SalesDiscountDuplicateCommand(
        Guid id,
        Guid purchaseDiscountId,
        string code)
    {
        Id = id;
        Code = code;
        SalesDiscountId = purchaseDiscountId;
    }
    public bool IsValidSalesDiscount(ISalesDiscountRepository _context)
    {
        ValidationResult = new SalesDiscountDuplicateCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class SalesDiscountDeleteCommand : SalesDiscountCommand
{
    public SalesDiscountDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValidSalesDiscount(ISalesDiscountRepository _context)
    {
        ValidationResult = new SalesDiscountDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ManagePaymentSDCommand : Command
{
    public Guid Id { get; set; }
    public int? PaymentStatus { get; set; }
    public List<PaymentInvoiceDto>? PaymentInvoice { get; set; }
    public ManagePaymentSDCommand(
        Guid id,
        int? paymentStatus,
        List<PaymentInvoiceDto>? paymentInvoice)
    {
        Id = id;
        PaymentStatus = paymentStatus;
        PaymentInvoice = paymentInvoice;
    }
}

public class SalesDiscountProcessCommand : SalesDiscountCommand
{
    public SalesDiscountProcessCommand(
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
public class ValidateExcelExportSalesDiscount
{
    public IFormFile File { get; set; } = null!;
    public string SheetId { get; set; } = null!;
    public int HeaderRow { get; set; }
    public int? ProductCode { get; set; }
    public int? ProductName { get; set; }
    public int? UnitCode { get; set; }
    public int? UnitName { get; set; }
    public int? UnitPrice { get; set; }
    public int? DiscountPercent { get; set; }
    public int? Tax { get; set; }
    public int? Quantity { get; set; }
    public int? ReasonDiscount { get; set; }
}
