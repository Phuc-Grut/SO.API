using System.Data;
using Microsoft.AspNetCore.Http;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class OrderCommand : Command
{
    public Guid Id { get; set; }
    public string? OrderType { get; set; }
    public string Code { get; set; } = null!;
    public DateTime? OrderDate { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerCode { get; set; }
    public Guid? StoreId { get; set; }
    public string? StoreCode { get; set; }
    public string? StoreName { get; set; }
    public int? TypeDocument { get; set; }
    public Guid? ContractId { get; set; }
    public string? ContractName { get; set; }
    public Guid? QuotationId { get; set; }
    public string? QuotationName { get; set; }
    public Guid? ChannelId { get; set; }
    public string? ChannelName { get; set; }
    public int? Status { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public string? Calculation { get; set; }
    public decimal? ExchangeRate { get; set; }
    public Guid? PriceListId { get; set; }
    public string? PriceListName { get; set; }
    public Guid? PaymentTermId { get; set; }
    public string? PaymentTermName { get; set; }
    public string? PaymentMethodName { get; set; }
    public Guid? PaymentMethodId { get; set; }
    public int? PaymentStatus { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryPhone { get; set; }
    public string? DeliveryName { get; set; }
    public string? DeliveryCountry { get; set; }
    public string? DeliveryProvince { get; set; }
    public string? DeliveryDistrict { get; set; }
    public string? DeliveryWard { get; set; }
    public string? DeliveryNote { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public string? DeliveryTracking { get; set; }
    public string? DeliveryCarrier { get; set; }
    public int? DeliveryPackage { get; set; }
    public bool? IsBill { get; set; }
    public string? BillAddress { get; set; }
    public string? BillCountry { get; set; }
    public string? BillProvince { get; set; }
    public string? BillDistrict { get; set; }
    public string? BillWard { get; set; }
    public int? BillStatus { get; set; }
    public Guid? DeliveryMethodId { get; set; }
    public string? DeliveryMethodCode { get; set; }
    public string? DeliveryMethodName { get; set; }
    public int? DeliveryStatus { get; set; }
    public Guid? ShippingMethodId { get; set; }
    public string? ShippingMethodCode { get; set; }
    public string? ShippingMethodName { get; set; }
    public int? TypeDiscount { get; set; }
    public double? DiscountRate { get; set; }
    public int? TypeCriteria { get; set; }
    public decimal? AmountDiscount { get; set; }
    public string? Note { get; set; }
    public Guid? GroupEmployeeId { get; set; }
    public string? GroupEmployeeName { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public string? File { get; set; }
    public string? RouterShipping { get; set; }
    public string? DomesticTracking { get; set; }
    public string? DomesticCarrier { get; set; }
    public int? DomesticPackage { get; set; }
    public int? Weight { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Length { get; set; }
    public List<OrderProductDto>? OrderProduct { get; set; }
    public List<OrderServiceAddDto>? OrderServiceAdd { get; set; }
    public List<PaymentInvoiceDto>? PaymentInvoice { get; set; }
    public List<OrderInvoiceDto>? OrderInvoice { get; set; }
    public List<DeliveryProductDto>? ListExpectedDelivery { get; set; }
}

public class AddOrderCommand : OrderCommand
{
    public AddOrderCommand(
        Guid id,
        string? orderType,
        string code,
        DateTime? orderDate,
        Guid? customerId,
        string? customerName,
        string? customerCode,
        Guid? storeId,
        string? storeCode,
        string? storeName,
        int? typeDocument,
        Guid? contractId,
        string? contractName,
        Guid? quotationId,
        string? quotationName,
        Guid? channelId,
        string? channelName,
        int? status,
        string? currency,
        string? currencyName,
        string? calculation,
        decimal? exchangeRate,
        Guid? priceListId,
        string? priceListName,
        Guid? paymentTermId,
        string? paymentTermName,
        string? paymentMethodName,
        Guid? paymentMethodId,
        int? paymentStatus,
        string? deliveryAddress,
        string? deliveryPhone,
        string? deliveryName,
        string? deliveryCountry,
        string? deliveryProvince,
        string? deliveryDistrict,
        string? deliveryWard,
        string? deliveryNote,
        DateTime? estimatedDeliveryDate,
        bool? isBill,
        string? billAddress,
        string? billCountry,
        string? billProvince,
        string? billDistrict,
        string? billWard,
        int? billStatus,
        Guid? deliveryMethodId,
        string? deliveryMethodCode,
        string? deliveryMethodName,
        int? deliveryStatus,
        Guid? shippingMethodId,
        string? shippingMethodCode,
        string? shippingMethodName,
        int? typeDiscount,
        double? discountRate,
        int? typeCriteria,
        decimal? amountDiscount,
        string? note,
        Guid? groupEmployeeId,
        string? groupEmployeeName,
        Guid? accountId,
        string? accountName,
        string? image,
        string? description,
        string? file,
        List<OrderProductDto>? orderProduct,
        List<OrderServiceAddDto>? orderServiceAdd,
        List<PaymentInvoiceDto>? paymentInvoice,
        List<OrderInvoiceDto>? orderInvoice,
        List<DeliveryProductDto>? listExpectedDelivery
        )
    {
        Id = id;
        OrderType = orderType;
        Code = code;
        OrderDate = orderDate;
        CustomerId = customerId;
        CustomerName = customerName;
        CustomerCode = customerCode;
        StoreId = storeId;
        StoreCode = storeCode;
        StoreName = storeName;
        TypeDocument = typeDocument;
        ContractId = contractId;
        ContractName = contractName;
        QuotationId = quotationId;
        QuotationName = quotationName;
        ChannelId = channelId;
        ChannelName = channelName;
        Status = status;
        Currency = currency;
        CurrencyName = currencyName;
        Calculation = calculation;
        ExchangeRate = exchangeRate;
        PriceListId = priceListId;
        PriceListName = priceListName;
        PaymentTermId = paymentTermId;
        PaymentTermName = paymentTermName;
        PaymentMethodName = paymentMethodName;
        PaymentMethodId = paymentMethodId;
        PaymentStatus = paymentStatus;
        DeliveryAddress = deliveryAddress;
        DeliveryPhone = deliveryPhone;
        DeliveryName = deliveryName;
        DeliveryCountry = deliveryCountry;
        DeliveryProvince = deliveryProvince;
        DeliveryDistrict = deliveryDistrict;
        DeliveryWard = deliveryWard;
        DeliveryNote = deliveryNote;
        EstimatedDeliveryDate = estimatedDeliveryDate;
        IsBill = isBill;
        BillAddress = billAddress;
        BillCountry = billCountry;
        BillProvince = billProvince;
        BillDistrict = billDistrict;
        BillWard = billWard;
        BillStatus = billStatus;
        DeliveryMethodId = deliveryMethodId;
        DeliveryMethodCode = deliveryMethodCode;
        DeliveryMethodName = deliveryMethodName;
        DeliveryStatus = deliveryStatus;
        ShippingMethodId = shippingMethodId;
        ShippingMethodCode = shippingMethodCode;
        ShippingMethodName = shippingMethodName;
        TypeDiscount = typeDiscount;
        DiscountRate = discountRate;
        TypeCriteria = typeCriteria;
        AmountDiscount = amountDiscount;
        Note = note;
        GroupEmployeeId = groupEmployeeId;
        GroupEmployeeName = groupEmployeeName;
        AccountId = accountId;
        AccountName = accountName;
        Image = image;
        Description = description;
        File = file;
        OrderProduct = orderProduct;
        OrderServiceAdd = orderServiceAdd;
        PaymentInvoice = paymentInvoice;
        OrderInvoice = orderInvoice;
        ListExpectedDelivery = listExpectedDelivery;

    }

    public bool IsValid(IOrderRepository _context)
    {
        ValidationResult = new AddOrderValidation(_context).ValidateAsync(this).Result;
        return ValidationResult.IsValid;
    }
}

public class EditOrderCommand : OrderCommand
{
    public EditOrderCommand(
        Guid id,
        string? orderType,
        string code,
        DateTime? orderDate,
        Guid? customerId,
        string? customerName,
        string? customerCode,
        Guid? storeId,
        string? storeCode,
        string? storeName,
        int? typeDocument,
        Guid? contractId,
        string? contractName,
        Guid? quotationId,
        string? quotationName,
        Guid? channelId,
        string? channelName,
        int? status,
        string? currency,
        string? currencyName,
        string? calculation,
        decimal? exchangeRate,
        Guid? priceListId,
        string? priceListName,
        Guid? paymentTermId,
        string? paymentTermName,
        string? paymentMethodName,
        Guid? paymentMethodId,
        int? paymentStatus,
        string? deliveryAddress,
        string? deliveryPhone,
        string? deliveryName,
        string? deliveryCountry,
        string? deliveryProvince,
        string? deliveryDistrict,
        string? deliveryWard,
        string? deliveryNote,
        DateTime? estimatedDeliveryDate,
        bool? isBill,
        string? billAddress,
        string? billCountry,
        string? billProvince,
        string? billDistrict,
        string? billWard,
        int? billStatus,
        Guid? deliveryMethodId,
        string? deliveryMethodCode,
        string? deliveryMethodName,
        int? deliveryStatus,
        Guid? shippingMethodId,
        string? shippingMethodCode,
        string? shippingMethodName,
        int? typeDiscount,
        double? discountRate,
        int? typeCriteria,
        decimal? amountDiscount,
        string? note,
        Guid? groupEmployeeId,
        string? groupEmployeeName,
        Guid? accountId,
        string? accountName,
        string? image,
        string? description,
        string? file,
        List<OrderProductDto>? orderProduct,
        List<OrderServiceAddDto>? orderServiceAdd,
        List<PaymentInvoiceDto>? paymentInvoice,
        List<OrderInvoiceDto>? orderInvoice,
        List<DeliveryProductDto>? listExpectedDelivery
        )
    {
        Id = id;
        OrderType = orderType;
        Code = code;
        OrderDate = orderDate;
        CustomerId = customerId;
        CustomerName = customerName;
        CustomerCode = customerCode;
        StoreId = storeId;
        StoreCode = storeCode;
        StoreName = storeName;
        TypeDocument = typeDocument;
        ContractId = contractId;
        ContractName = contractName;
        QuotationId = quotationId;
        QuotationName = quotationName;
        ChannelId = channelId;
        ChannelName = channelName;
        Status = status;
        Currency = currency;
        CurrencyName = currencyName;
        Calculation = calculation;
        ExchangeRate = exchangeRate;
        PriceListId = priceListId;
        PriceListName = priceListName;
        PaymentTermId = paymentTermId;
        PaymentTermName = paymentTermName;
        PaymentMethodName = paymentMethodName;
        PaymentMethodId = paymentMethodId;
        PaymentStatus = paymentStatus;
        DeliveryAddress = deliveryAddress;
        DeliveryPhone = deliveryPhone;
        DeliveryName = deliveryName;
        DeliveryCountry = deliveryCountry;
        DeliveryProvince = deliveryProvince;
        DeliveryDistrict = deliveryDistrict;
        DeliveryWard = deliveryWard;
        DeliveryNote = deliveryNote;
        EstimatedDeliveryDate = estimatedDeliveryDate;
        IsBill = isBill;
        BillAddress = billAddress;
        BillCountry = billCountry;
        BillProvince = billProvince;
        BillDistrict = billDistrict;
        BillWard = billWard;
        BillStatus = billStatus;
        DeliveryMethodId = deliveryMethodId;
        DeliveryMethodName = deliveryMethodName;
        DeliveryMethodCode = deliveryMethodCode;
        DeliveryStatus = deliveryStatus;
        ShippingMethodId = shippingMethodId;
        ShippingMethodCode = shippingMethodCode;
        ShippingMethodName = shippingMethodName;
        TypeDiscount = typeDiscount;
        DiscountRate = discountRate;
        TypeCriteria = typeCriteria;
        AmountDiscount = amountDiscount;
        Note = note;
        GroupEmployeeId = groupEmployeeId;
        GroupEmployeeName = groupEmployeeName;
        AccountId = accountId;
        AccountName = accountName;
        Image = image;
        Description = description;
        File = file;
        OrderProduct = orderProduct;
        OrderServiceAdd = orderServiceAdd;
        PaymentInvoice = paymentInvoice;
        ListExpectedDelivery = listExpectedDelivery;
        OrderInvoice = orderInvoice;
    }


    public bool IsValid(IOrderRepository _context)
    {
        ValidationResult = new EditOrderValidation(_context).ValidateAsync(this).Result;
        return ValidationResult.IsValid;
    }
}

public class DeleteOrderCommand : OrderCommand
{
    public DeleteOrderCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IOrderRepository _context)
    {
        ValidationResult = new DeteleOrderValidation(_context).ValidateAsync(this).Result;
        return ValidationResult.IsValid;
    }
}

public class ApprovalOrderCommand : Command
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public string? ApproveComment { get; set; }

    public ApprovalOrderCommand(
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

public class ApprovalOrdersCommand : Command
{
    public IList<Guid> Ids { get; set; }
    public int? Status { get; set; }
    public string? ApproveComment { get; set; }
    public ApprovalOrdersCommand(
        IList<Guid> ids,
        int? status,
        string? approveComment
    )
    {
        Ids = ids;
        Status = status;
        ApproveComment = approveComment;

    }
}

public class CreateOrderCommand : Command
{
    public CreateOrderCommand()
    {

    }
    public Guid Id { get; set; }
    public string Code { get; set; }
    public Guid CustomerId { get; set; }
    public string OrderCode { get; set; }
    public string StoreCode { get; set; }
    public string ChannelCode { get; set; }
    public string CurrencyCode { get; set; }
    public decimal? ExchangeRate { get; set; }
    public string PaymentTermCode { get; set; }
    public string PaymentMethodCode { get; set; }
    public string ShippingMethodCode { get; set; }
    public int? BuyFee { get; set; }
    public string RouterShipping { get; set; }
    public DataTable Products { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public string Note { get; set; }
    public Guid? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }


    public bool IsValid(IOrderRepository _context)
    {
        ValidationResult = new CreateOrderValidation(_context).ValidateAsync(this).Result;
        return ValidationResult.IsValid;
    }
}
public class ManagePaymentOrderCommand : Command
{
    public Guid Id { get; set; }
    public int? PaymentStatus { get; set; }
    public List<PaymentInvoiceDto>? PaymentInvoice { get; set; }
    public ManagePaymentOrderCommand(
        Guid id,
        int? paymentStatus,
        List<PaymentInvoiceDto>? paymentInvoice)
    {
        Id = id;
        PaymentStatus = paymentStatus;
        PaymentInvoice = paymentInvoice;
    }
}
public class ManageServiceOrderCommand : Command
{
    public Guid Id { get; set; }
    public List<OrderServiceAddDto>? OrderServiceAdd { get; set; }
    public ManageServiceOrderCommand(
        Guid id,
        List<OrderServiceAddDto>? orderServiceAdd)
    {
        Id = id;
        OrderServiceAdd = orderServiceAdd;
    }
}
public class NoteOrderCommand : Command
{
    public Guid Id { get; set; }
    public List<OrderTrackingDto>? OrderTracking { get; set; }
    public NoteOrderCommand(
        Guid id,
        List<OrderTrackingDto>? orderTracking)
    {
        Id = id;
        OrderTracking = orderTracking;
    }
}

public class OrderUploadFileCommand : Command
{
    public Guid Id { get; set; }
    public string? File { get; set; }
    public OrderUploadFileCommand(
        Guid id,
        string? file
    )
    {
        Id = id;
        File = file;
    }
}
public class ValidateExcelOrder
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
    public int? DiscountPercent { get; set; }
    public int? Note { get; set; }
}

public class OrderEmailNotifyCommand : Command
{
    public string Order { get; set; }
    public string SenderCode { get; set; }
    public string SenderName { get; set; }
    public string Subject { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string CC { get; set; }
    public string BCC { get; set; }
    public string Body { get; set; }
    public string TemplateCode { get; set; }

    public OrderEmailNotifyCommand(
        string order,
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
        Order = order;
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
public class RecalculatePriceCommand : Command
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public string UserName { get; }
    public RecalculatePriceCommand(Guid id, Guid userId, string userName)
    {
        Id = id;
        UserId = userId;
        UserName = userName;
    }
}
