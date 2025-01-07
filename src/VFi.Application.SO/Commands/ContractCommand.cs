using Consul;
using MassTransit.Internals.GraphValidation;
using Microsoft.AspNetCore.Http;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class ContractCommand : Command
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public Guid? ContractTypeId { get; set; }
    public string? ContractTypeName { get; set; }
    public Guid? QuotationId { get; set; }
    public string? QuotationName { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? SignDate { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? Address { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public string? Calculation { get; set; }
    public decimal? ExchangeRate { get; set; }
    public int? Status { get; set; }
    public int? TypeDiscount { get; set; }
    public double? DiscountRate { get; set; }
    public int? TypeCriteria { get; set; }
    public decimal? AmountDiscount { get; set; }
    public string? AccountName { get; set; }
    public Guid? GroupEmployeeId { get; set; }
    public string? GroupEmployeeName { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? ContractTermId { get; set; }
    public string? ContractTermName { get; set; }
    public string? ContractTermContent { get; set; }
    public DateTime? PaymentDueDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? Buyer { get; set; }
    public string? Saler { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public string? File { get; set; }
    public bool? HasPreviousContract { get; set; }
    public decimal? Paid { get; set; }
    public decimal? Received { get; set; }
    public List<OrderProductDto>? OrderProduct { get; set; }
}
public class ContractAddCommand : ContractCommand
{
    public ContractAddCommand(
        Guid id,
        string? code,
        string? name,
        Guid? contractTypeId,
        string? contractTypeName,
        Guid? quotationId,
        string? quotationName,
        Guid? orderId,
        string? orderCode,
        DateTime? startDate,
        DateTime? endDate,
        DateTime? signDate,
        Guid? customerId,
        string? customerCode,
        string? customerName,
        string? country,
        string? province,
        string? district,
        string? ward,
        string? address,
        string? currency,
        string? currencyname,
        string? calculation,
        decimal? exchangeRate,
        int? status,
        int? typeDiscount,
        double? discountRate,
        int? typeCriteria,
        decimal? amountDiscount,
        string? accountName,
        Guid? groupEmployeeId,
        string? groupEmployeeName,
        Guid? accountId,
        Guid? contractTermId,
        string? contractTermName,
        string? contractTermContent,
        DateTime? paymentDueDate,
        DateTime? deliveryDate,
        string? buyer,
        string? saler,
        string? description,
        string? note,
        string? file,
        bool? hasPreviousContract,
        decimal? paid,
        decimal? received,
        List<OrderProductDto>? orderProduct
        )
    {
        Id = id;
        Code = code;
        Name = name;
        ContractTypeId = contractTypeId;
        ContractTypeName = contractTypeName;
        QuotationId = quotationId;
        QuotationName = quotationName;
        OrderId = orderId;
        OrderCode = orderCode;
        StartDate = startDate;
        EndDate = endDate;
        SignDate = signDate;
        CustomerId = customerId;
        CustomerCode = customerCode;
        CustomerName = customerName;
        Country = country;
        Province = province;
        District = district;
        Ward = ward;
        Address = address;
        Currency = currency;
        CurrencyName = currencyname;
        Calculation = calculation;
        ExchangeRate = exchangeRate;
        Status = status;
        TypeDiscount = typeDiscount;
        DiscountRate = discountRate;
        TypeCriteria = typeCriteria;
        AmountDiscount = amountDiscount;
        AccountName = accountName;
        GroupEmployeeId = groupEmployeeId;
        GroupEmployeeName = groupEmployeeName;
        AccountId = accountId;
        ContractTermId = contractTermId;
        ContractTermName = contractTermName;
        ContractTermContent = contractTermContent;
        PaymentDueDate = paymentDueDate;
        DeliveryDate = deliveryDate;
        Buyer = buyer;
        Saler = saler;
        Description = description;
        Note = note;
        File = file;
        HasPreviousContract = hasPreviousContract;
        Paid = paid;
        Received = received;
        OrderProduct = orderProduct;
    }
    public bool IsValid(IContractRepository _context)
    {
        ValidationResult = new ContractAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ContractEditCommand : ContractCommand
{
    public ContractEditCommand(
        Guid id,
        string? code,
        string? name,
        Guid? contractTypeId,
        string? contractTypeName,
        Guid? quotationId,
        string? quotationName,
        Guid? orderId,
        string? orderCode,
        DateTime? startDate,
        DateTime? endDate,
        DateTime? signDate,
        Guid? customerId,
        string? customerCode,
        string? customerName,
        string? country,
        string? province,
        string? district,
        string? ward,
        string? address,
        string? currency,
        string? currencyname,
        string? calculation,
        decimal? exchangeRate,
        int? status,
        int? typeDiscount,
        double? discountRate,
        int? typeCriteria,
        decimal? amountDiscount,
        string? accountName,
        Guid? groupEmployeeId,
        string? groupEmployeeName,
        Guid? accountId,
        Guid? contractTermId,
        string? contractTermName,
        string? contractTermContent,
        DateTime? paymentDueDate,
        DateTime? deliveryDate,
        string? buyer,
        string? saler,
        string? description,
        string? note,
        string? file,
        bool? hasPreviousContract,
        decimal? paid,
        decimal? received,
        List<OrderProductDto>? orderProduct
        )
    {
        Id = id;
        Code = code;
        Name = name;
        ContractTypeId = contractTypeId;
        ContractTypeName = contractTypeName;
        QuotationId = quotationId;
        QuotationName = quotationName;
        OrderId = orderId;
        OrderCode = orderCode;
        StartDate = startDate;
        EndDate = endDate;
        SignDate = signDate;
        CustomerId = customerId;
        CustomerCode = customerCode;
        CustomerName = customerName;
        Country = country;
        Province = province;
        District = district;
        Ward = ward;
        Address = address;
        Currency = currency;
        CurrencyName = currencyname;
        Calculation = calculation;
        ExchangeRate = exchangeRate;
        Status = status;
        TypeDiscount = typeDiscount;
        DiscountRate = discountRate;
        TypeCriteria = typeCriteria;
        AmountDiscount = amountDiscount;
        AccountName = accountName;
        GroupEmployeeId = groupEmployeeId;
        GroupEmployeeName = groupEmployeeName;
        AccountId = accountId;
        ContractTermId = contractTermId;
        ContractTermName = contractTermName;
        ContractTermContent = contractTermContent;
        PaymentDueDate = paymentDueDate;
        DeliveryDate = deliveryDate;
        Buyer = buyer;
        Saler = saler;
        Description = description;
        Note = note;
        File = file;
        HasPreviousContract = hasPreviousContract;
        Paid = paid;
        Received = received;
        OrderProduct = orderProduct;
    }
    public bool IsValid(IContractRepository _context)
    {
        ValidationResult = new ContractEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ContractDeleteCommand : ContractCommand
{
    public ContractDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IContractRepository _context)
    {
        ValidationResult = new ContractDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ApprovalContractCommand : Command
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public string? ApproveComment { get; set; }
    public ApprovalContractCommand(
        Guid id,
        int? status,
        string? approveComment
    )
    {
        Id = id;
        Status = status;
        ApproveComment = approveComment;
    }
}

public class LiquidationContractCommand : Command
{
    public Guid Id { get; set; }
    public decimal? AmountLiquidation { get; set; }
    public DateTime? LiquidationDate { get; set; }
    public string? LiquidationReason { get; set; }
    public LiquidationContractCommand(
        Guid id,
        decimal? amountLiquidation,
        DateTime? liquidationDate,
        string? liquidationReason
    )
    {
        Id = id;
        AmountLiquidation = amountLiquidation;
        LiquidationDate = liquidationDate;
        LiquidationReason = liquidationReason;
    }
}

public class ContractUploadFileCommand : Command
{
    public Guid Id { get; set; }
    public string? File { get; set; }
    public ContractUploadFileCommand(
        Guid id,
        string? file
    )
    {
        Id = id;
        File = file;
    }
}
public class ValidateExcelContract
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
    public int? Quantity { get; set; }
    public int? Note { get; set; }
    public int? DiscountPercent { get; set; }

}
