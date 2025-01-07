using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using MassTransit.Internals.GraphValidation;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class QuotationCommand : Command
{
    public Guid Id { get; set; }
    public string? Code { get; set; } = null!;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public Guid? StoreId { get; set; }
    public string? StoreCode { get; set; }
    public string? StoreName { get; set; }
    public Guid? ChannelId { get; set; }
    public string? ChannelName { get; set; }
    public string? DeliveryNote { get; set; }
    public string? DeliveryName { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryCountry { get; set; }
    public string? DeliveryProvince { get; set; }
    public string? DeliveryDistrict { get; set; }
    public string? DeliveryWard { get; set; }
    public int? DeliveryStatus { get; set; }
    public bool? IsBill { get; set; }
    public string? BillName { get; set; }
    public string? BillAddress { get; set; }
    public string? BillCountry { get; set; }
    public string? BillProvince { get; set; }
    public string? BillDistrict { get; set; }
    public string? BillWard { get; set; }
    public int? BillStatus { get; set; }
    public Guid? ShippingMethodId { get; set; }
    public string? ShippingMethodCode { get; set; }
    public string? ShippingMethodName { get; set; }
    public Guid? DeliveryMethodId { get; set; }
    public string? DeliveryMethodCode { get; set; }
    public string? DeliveryMethodName { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public string? Calculation { get; set; }
    public decimal? ExchangeRate { get; set; }
    public Guid? PriceListId { get; set; }
    public string? PriceListName { get; set; }
    public Guid? RequestQuoteId { get; set; }
    public string? RequestQuoteCode { get; set; }
    public Guid? ContractId { get; set; }
    public Guid? SaleOrderId { get; set; }
    public Guid? QuotationTermId { get; set; }
    public string? QuotationTermContent { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? ExpiredDate { get; set; }
    public Guid? GroupEmployeeId { get; set; }
    public string? GroupEmployeeName { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public int? TypeDiscount { get; set; }
    public double? DiscountRate { get; set; }
    public int? TypeCriteria { get; set; }
    public decimal? AmountDiscount { get; set; }
    public string? Note { get; set; }
    public Guid? OldId { get; set; }
    public string? OldCode { get; set; }
    public string? File { get; set; }
    public List<OrderProductDto>? OrderProduct { get; set; }
    public List<OrderServiceAddDto>? OrderServiceAdd { get; set; }
}

public class AddQuotationCommand : QuotationCommand
{
    public AddQuotationCommand(
        Guid id,
        string? code,
        string? name,
        string? description,
        int? status,
        Guid? customerId,
        string? customerCode,
        string? customerName,
        string? email,
        string? phone,
        string? address,
        Guid? storeId,
        string? storeCode,
        string? storeName,
        Guid? channelId,
        string? channelName,
        string? deliveryNote,
        string? deliveryName,
        string? deliveryAddress,
        string? deliveryCountry,
        string? deliveryProvince,
        string? deliveryDistrict,
        string? deliveryWard,
        int? deliveryStatus,
        bool? isBill,
        string? billName,
        string? billAddress,
        string? billCountry,
        string? billProvince,
        string? billDistrict,
        string? billWard,
        int? billStatus,
        Guid? shippingMethodId,
        string? shippingMethodCode,
        string? shippingMethodName,
        Guid? deliveryMethodId,
        string? deliveryMethodCode,
        string? deliveryMethodName,
        DateTime? expectedDate,
        string? currency,
        string? currencyName,
        string? calculation,
        decimal? exchangeRate,
        Guid? priceListId,
        string? priceListName,
        Guid? requestQuoteId,
        string? requestQuoteCode,
        Guid? contractId,
        Guid? saleOrderId,
        Guid? quotationTermId,
        string? quotationTermContent,
        DateTime? date,
        DateTime? expiredDate,
        Guid? groupEmployeeId,
        string? groupEmployeeName,
        Guid? accountId,
        string? accountName,
        int? typeDiscount,
        double? discountRate,
        int? typeCriteria,
        decimal? amountDiscount,
        string? note,
        Guid? oldId,
        string? oldCode,
        string? file,
        List<OrderProductDto>? orderProduct,
        List<OrderServiceAddDto>? orderServiceAdd
        )
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        CustomerId = customerId;
        CustomerCode = customerCode;
        CustomerName = customerName;
        Email = email;
        Phone = phone;
        Address = address;
        StoreId = storeId;
        StoreCode = storeCode;
        StoreName = storeName;
        ChannelId = channelId;
        ChannelName = channelName;
        DeliveryNote = deliveryNote;
        DeliveryName = deliveryName;
        DeliveryAddress = deliveryAddress;
        DeliveryCountry = deliveryCountry;
        DeliveryProvince = deliveryProvince;
        DeliveryDistrict = deliveryDistrict;
        DeliveryWard = deliveryWard;
        DeliveryStatus = deliveryStatus;
        IsBill = isBill;
        BillName = billName;
        BillAddress = billAddress;
        BillCountry = billCountry;
        BillProvince = billProvince;
        BillDistrict = billDistrict;
        BillWard = billWard;
        BillStatus = billStatus;
        ShippingMethodId = shippingMethodId;
        ShippingMethodCode = shippingMethodCode;
        ShippingMethodName = shippingMethodName;
        DeliveryMethodId = deliveryMethodId;
        DeliveryMethodCode = deliveryMethodCode;
        DeliveryMethodName = deliveryMethodName;
        ExpectedDate = expectedDate;
        Currency = currency;
        CurrencyName = currencyName;
        Calculation = calculation;
        ExchangeRate = exchangeRate;
        PriceListId = priceListId;
        PriceListName = priceListName;
        RequestQuoteId = requestQuoteId;
        RequestQuoteCode = requestQuoteCode;
        ContractId = contractId;
        SaleOrderId = saleOrderId;
        QuotationTermId = quotationTermId;
        QuotationTermContent = quotationTermContent;
        Date = date;
        ExpiredDate = expiredDate;
        GroupEmployeeId = groupEmployeeId;
        GroupEmployeeName = groupEmployeeName;
        AccountId = accountId;
        AccountName = accountName;
        TypeDiscount = typeDiscount;
        DiscountRate = discountRate;
        TypeCriteria = typeCriteria;
        AmountDiscount = amountDiscount;
        Note = note;
        OldId = oldId;
        OldCode = oldCode;
        File = file;
        OrderProduct = orderProduct;
        OrderServiceAdd = orderServiceAdd;
    }

    public bool IsValid(IQuotationRepository _context)
    {
        ValidationResult = new AddQuotationValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class EditQuotationCommand : QuotationCommand
{
    public EditQuotationCommand(
        Guid id,
        string code,
        string? name,
        string? description,
        int? status,
        Guid? customerId,
        string? customerCode,
        string? customerName,
        string? email,
        string? phone,
        string? address,
        Guid? storeId,
        string? storeCode,
        string? storeName,
        Guid? channelId,
        string? channelName,
        string? deliveryNote,
        string? deliveryName,
        string? deliveryAddress,
        string? deliveryCountry,
        string? deliveryProvince,
        string? deliveryDistrict,
        string? deliveryWard,
        int? deliveryStatus,
        bool? isBill,
        string? billName,
        string? billAddress,
        string? billCountry,
        string? billProvince,
        string? billDistrict,
        string? billWard,
        int? billStatus,
        Guid? shippingMethodId,
        string? shippingMethodCode,
        string? shippingMethodName,
        Guid? deliveryMethodId,
        string? deliveryMethodCode,
        string? deliveryMethodName,
        DateTime? expectedDate,
        string? currency,
        string? currencyName,
        string? calculation,
        decimal? exchangeRate,
        Guid? priceListId,
        string? priceListName,
        Guid? requestQuoteId,
        string? requestQuoteCode,
        Guid? contractId,
        Guid? saleOrderId,
        Guid? quotationTermId,
        string? quotationTermContent,
        DateTime? date,
        DateTime? expiredDate,
        Guid? groupEmployeeId,
        string? groupEmployeeName,
        Guid? accountId,
        string? accountName,
        int? typeDiscount,
        double? discountRate,
        int? typeCriteria,
        decimal? amountDiscount,
        string? note,
        string? file,
        List<OrderProductDto>? orderProduct,
        List<OrderServiceAddDto>? orderServiceAdd
        )
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        CustomerId = customerId;
        CustomerCode = customerCode;
        CustomerName = customerName;
        Email = email;
        Phone = phone;
        Address = address;
        StoreId = storeId;
        StoreCode = storeCode;
        StoreName = storeName;
        ChannelId = channelId;
        ChannelName = channelName;
        DeliveryNote = deliveryNote;
        DeliveryName = deliveryName;
        DeliveryAddress = deliveryAddress;
        DeliveryCountry = deliveryCountry;
        DeliveryProvince = deliveryProvince;
        DeliveryDistrict = deliveryDistrict;
        DeliveryWard = deliveryWard;
        DeliveryStatus = deliveryStatus;
        IsBill = isBill;
        BillName = billName;
        BillAddress = billAddress;
        BillCountry = billCountry;
        BillProvince = billProvince;
        BillDistrict = billDistrict;
        BillWard = billWard;
        BillStatus = billStatus;
        ShippingMethodId = shippingMethodId;
        ShippingMethodCode = shippingMethodCode;
        ShippingMethodName = shippingMethodName;
        DeliveryMethodId = deliveryMethodId;
        DeliveryMethodCode = deliveryMethodCode;
        DeliveryMethodName = deliveryMethodName;
        ExpectedDate = expectedDate;
        Currency = currency;
        CurrencyName = currencyName;
        Calculation = calculation;
        ExchangeRate = exchangeRate;
        PriceListId = priceListId;
        PriceListName = priceListName;
        RequestQuoteId = requestQuoteId;
        RequestQuoteCode = requestQuoteCode;
        ContractId = contractId;
        SaleOrderId = saleOrderId;
        QuotationTermId = quotationTermId;
        QuotationTermContent = quotationTermContent;
        Date = date;
        ExpiredDate = expiredDate;
        GroupEmployeeId = groupEmployeeId;
        GroupEmployeeName = groupEmployeeName;
        AccountId = accountId;
        AccountName = accountName;
        TypeDiscount = typeDiscount;
        DiscountRate = discountRate;
        TypeCriteria = typeCriteria;
        AmountDiscount = amountDiscount;
        Note = note;
        File = file;
        OrderProduct = orderProduct;
        OrderServiceAdd = orderServiceAdd;
    }

    public override bool IsValid()
    {
        ValidationResult = new EditQuotationValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class DeleteQuotationCommand : QuotationCommand
{
    public DeleteQuotationCommand(Guid id)
    {
        Id = id;
    }
    public override bool IsValid()
    {
        ValidationResult = new DeteleQuotationValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class UpdateStatusQuotationCommand : Command
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public string? ApproveComment { get; set; }
    public UpdateStatusQuotationCommand(
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

public class QuotationUploadFileCommand : Command
{
    public Guid Id { get; set; }
    public string? File { get; set; }
    public QuotationUploadFileCommand(
        Guid id,
        string? file
    )
    {
        Id = id;
        File = file;
    }
}
public class QuotationEmailNotifyCommand : Command
{
    public string SenderCode { get; set; }
    public string SenderName { get; set; }
    public string Subject { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string CC { get; set; }
    public string BCC { get; set; }
    public string Body { get; set; }
    public string TemplateCode { get; set; }

    public QuotationEmailNotifyCommand(
        string senderCode,
        string senderName,
        string subject,
        string from,
        string to,
        string cC,
        string bCC,
        string body,
        string templateCode)
    {
        SenderCode = senderCode;
        SenderName = senderName;
        Subject = subject;
        From = from;
        To = to;
        CC = cC;
        BCC = bCC;
        Body = body;
        TemplateCode = templateCode;
    }
}
