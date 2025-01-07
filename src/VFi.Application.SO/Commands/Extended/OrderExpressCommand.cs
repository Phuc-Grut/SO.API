using System.Data;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands.Extended;

public class OrderExpressCommand : Command
{
    public Guid Id { get; set; }
    public string? OrderType { get; set; }
    public string? Code { get; set; }
    public DateTime? OrderDate { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerCode { get; set; }
    public Guid? StoreId { get; set; }
    public string? StoreCode { get; set; }
    public string? StoreName { get; set; }
    public Guid? ContractId { get; set; }
    public string? ContractName { get; set; }
    public string? Currency { get; set; }
    public decimal? ExchangeRate { get; set; }
    public Guid? ShippingMethodId { get; set; }
    public string? ShippingMethodCode { get; set; }
    public string? ShippingMethodName { get; set; }
    public string? RouterShipping { get; set; }
    public string? DomesticTracking { get; set; }
    public string? DomesticCarrier { get; set; }
    public int? Status { get; set; }
    public Guid? PaymentTermId { get; set; }
    public string? PaymentTermName { get; set; }
    public string? PaymentMethodName { get; set; }
    public Guid? PaymentMethodId { get; set; }
    public int? PaymentStatus { get; set; }
    public string? ShipperName { get; set; }
    public string? ShipperPhone { get; set; }
    public string? ShipperZipCode { get; set; }
    public string? ShipperAddress { get; set; }
    public string? ShipperCountry { get; set; }
    public string? ShipperProvince { get; set; }
    public string? ShipperDistrict { get; set; }
    public string? ShipperWard { get; set; }
    public string? ShipperNote { get; set; }
    public string? DeliveryName { get; set; }
    public string? DeliveryPhone { get; set; }
    public string? DeliveryZipCode { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryCountry { get; set; }
    public string? DeliveryProvince { get; set; }
    public string? DeliveryDistrict { get; set; }
    public string? DeliveryWard { get; set; }
    public string? DeliveryNote { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public Guid? DeliveryMethodId { get; set; }
    public string? DeliveryMethodCode { get; set; }
    public string? DeliveryMethodName { get; set; }
    public int? DeliveryStatus { get; set; }
    public bool? IsBill { get; set; }
    public string? BillName { get; set; }
    public string? BillAddress { get; set; }
    public string? BillCountry { get; set; }
    public string? BillProvince { get; set; }
    public string? BillDistrict { get; set; }
    public string? BillWard { get; set; }
    public int? BillStatus { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public Guid? GroupEmployeeId { get; set; }
    public string? GroupEmployeeName { get; set; }
    public string? CommodityGroup { get; set; }
    public decimal? AirFreight { get; set; }
    public decimal? SeaFreight { get; set; }
    public decimal? Surcharge { get; set; }
    public int? Weight { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Length { get; set; }
    public string? Image { get; set; }
    public decimal? Paid { get; set; }
    public decimal? Total { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public Guid? RouterShippingId { get; set; }
    public string? ShippingCodePost { get; set; }
    public string? TrackingCode { get; set; }
    public string? TrackingCarrier { get; set; }
    public int? Package { get; set; }
    public DateTime? ToDeliveryDate { get; set; }
    public List<OrderExpressDetailDto>? OrderExpressDetail { get; set; } = new List<OrderExpressDetailDto>();
    public List<OrderServiceAddDto>? OrderServiceAdd { get; set; }
    public List<PaymentInvoiceDto>? PaymentInvoice { get; set; }
    public List<OrderInvoiceDto>? OrderInvoice { get; set; }
}

public class OrderExpressAddCommand : OrderExpressCommand
{
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CreatedByName { get; set; }

    public OrderExpressAddCommand(Guid id,
                                    string? orderType,
                                    string? code,
                                    DateTime? orderDate,
                                    Guid? customerId,
                                    string? customerName,
                                    string? customerCode,
                                    Guid? storeId,
                                    string? storeCode,
                                    string? storeName,
                                    Guid? contractId,
                                    string? contractName,
                                    string? currency,
                                    decimal? exchangeRate,
                                    Guid? shippingMethodId,
                                    string? shippingMethodCode,
                                    string? shippingMethodName,
                                    string? routerShipping,
                                    string? domesticTracking,
                                    string? domesticCarrier,
                                    int? status,
                                    Guid? paymentTermId,
                                    string? paymentTermName,
                                    string? paymentMethodName,
                                    Guid? paymentMethodId,
                                    int? paymentStatus,
                                    string? shipperName,
                                    string? shipperPhone,
                                    string? shipperZipCode,
                                    string? shipperAddress,
                                    string? shipperCountry,
                                    string? shipperProvince,
                                    string? shipperDistrict,
                                    string? shipperWard,
                                    string? shipperNote,
                                    string? deliveryName,
                                    string? deliveryPhone,
                                    string? deliveryZipCode,
                                    string? deliveryAddress,
                                    string? deliveryCountry,
                                    string? deliveryProvince,
                                    string? deliveryDistrict,
                                    string? deliveryWard,
                                    string? deliveryNote,
                                    DateTime? estimatedDeliveryDate,
                                    Guid? deliveryMethodId,
                                    string? deliveryMethodCode,
                                    string? deliveryMethodName,
                                    int? deliveryStatus,
                                    bool? isBill,
                                    string? billName,
                                    string? billAddress,
                                    string? billCountry,
                                    string? billProvince,
                                    string? billDistrict,
                                    string? billWard,
                                    int? billStatus,
                                    string? description,
                                    string? note,
                                    Guid? groupEmployeeId,
                                    string? groupEmployeeName,
                                    string? commodityGroup,
                                    decimal? airFreight,
                                    decimal? seaFreight,
                                    decimal? surcharge,
                                    int? weight,
                                    int? width,
                                    int? height,
                                    int? length,
                                    string? image,
                                    decimal? paid,
                                    decimal? total,
                                    Guid? accountId,
                                    string? accountName,
                                    Guid? createdBy,
                                    DateTime? createdDate,
                                    string? createdByName,
                                    Guid? routerShippingId,
                                    string? shippingCodePost,
                                    string? trackingCode,
                                    string? trackingCarrier,
                                    int? package,
                                    DateTime? toDeliveryDate,
                                    List<OrderExpressDetailDto>? orderExpressDetail,
                                    List<OrderServiceAddDto>? orderServiceAdd,
                                    List<PaymentInvoiceDto>? paymentInvoice,
                                    List<OrderInvoiceDto>? orderInvoice)
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
        ContractId = contractId;
        ContractName = contractName;
        Currency = currency;
        ExchangeRate = exchangeRate;
        ShippingMethodId = shippingMethodId;
        ShippingMethodCode = shippingMethodCode;
        ShippingMethodName = shippingMethodName;
        RouterShipping = routerShipping;
        DomesticTracking = domesticTracking;
        DomesticCarrier = domesticCarrier;
        Status = status;
        PaymentTermId = paymentTermId;
        PaymentTermName = paymentTermName;
        PaymentMethodName = paymentMethodName;
        PaymentMethodId = paymentMethodId;
        PaymentStatus = paymentStatus;
        ShipperName = shipperName;
        ShipperPhone = shipperPhone;
        ShipperZipCode = shipperZipCode;
        ShipperAddress = shipperAddress;
        ShipperCountry = shipperCountry;
        ShipperProvince = shipperProvince;
        ShipperDistrict = shipperDistrict;
        ShipperWard = shipperWard;
        ShipperNote = shipperNote;
        DeliveryName = deliveryName;
        DeliveryPhone = deliveryPhone;
        DeliveryZipCode = deliveryZipCode;
        DeliveryAddress = deliveryAddress;
        DeliveryCountry = deliveryCountry;
        DeliveryProvince = deliveryProvince;
        DeliveryDistrict = deliveryDistrict;
        DeliveryWard = deliveryWard;
        DeliveryNote = deliveryNote;
        EstimatedDeliveryDate = estimatedDeliveryDate;
        DeliveryMethodId = deliveryMethodId;
        DeliveryMethodCode = deliveryMethodCode;
        DeliveryMethodName = deliveryMethodName;
        DeliveryStatus = deliveryStatus;
        IsBill = isBill;
        BillName = billName;
        BillAddress = billAddress;
        BillCountry = billCountry;
        BillProvince = billProvince;
        BillDistrict = billDistrict;
        BillWard = billWard;
        BillStatus = billStatus;
        Description = description;
        Note = note;
        GroupEmployeeId = groupEmployeeId;
        GroupEmployeeName = groupEmployeeName;
        CommodityGroup = commodityGroup;
        AirFreight = airFreight;
        SeaFreight = seaFreight;
        Surcharge = surcharge;
        Weight = weight;
        Width = width;
        Height = height;
        Length = length;
        Image = image;
        Paid = paid;
        Total = total;
        AccountId = accountId;
        AccountName = accountName;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
        CreatedByName = createdByName;
        OrderExpressDetail = orderExpressDetail;
        OrderServiceAdd = orderServiceAdd;
        PaymentInvoice = paymentInvoice;
        OrderInvoice = orderInvoice;
        RouterShippingId = routerShippingId;
        ShippingCodePost = shippingCodePost;
        TrackingCode = trackingCode;
        TrackingCarrier = trackingCarrier;
        Package = package;
        ToDeliveryDate = toDeliveryDate;
    }

    public bool IsValid(IOrderExpressRepository _context)
    {
        ValidationResult = new OrderExpressAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class OrderExpressEditCommand : OrderExpressCommand
{
    public OrderExpressEditCommand(Guid id,
                                    string? orderType,
                                    string? code,
                                    DateTime? orderDate,
                                    Guid? customerId,
                                    string? customerName,
                                    string? customerCode,
                                    Guid? storeId,
                                    string? storeCode,
                                    string? storeName,
                                    Guid? contractId,
                                    string? contractName,
                                    string? currency,
                                    decimal? exchangeRate,
                                    Guid? shippingMethodId,
                                    string? shippingMethodCode,
                                    string? shippingMethodName,
                                    string? routerShipping,
                                    string? domesticTracking,
                                    string? domesticCarrier,
                                    int? status,
                                    Guid? paymentTermId,
                                    string? paymentTermName,
                                    string? paymentMethodName,
                                    Guid? paymentMethodId,
                                    int? paymentStatus,
                                    string? shipperName,
                                    string? shipperPhone,
                                    string? shipperZipCode,
                                    string? shipperAddress,
                                    string? shipperCountry,
                                    string? shipperProvince,
                                    string? shipperDistrict,
                                    string? shipperWard,
                                    string? shipperNote,
                                    string? deliveryName,
                                    string? deliveryPhone,
                                    string? deliveryZipCode,
                                    string? deliveryAddress,
                                    string? deliveryCountry,
                                    string? deliveryProvince,
                                    string? deliveryDistrict,
                                    string? deliveryWard,
                                    string? deliveryNote,
                                    DateTime? estimatedDeliveryDate,
                                    Guid? deliveryMethodId,
                                    string? deliveryMethodCode,
                                    string? deliveryMethodName,
                                    int? deliveryStatus,
                                    bool? isBill,
                                    string? billName,
                                    string? billAddress,
                                    string? billCountry,
                                    string? billProvince,
                                    string? billDistrict,
                                    string? billWard,
                                    int? billStatus,
                                    string? description,
                                    string? note,
                                    Guid? groupEmployeeId,
                                    string? groupEmployeeName,
                                    string? commodityGroup,
                                    decimal? airFreight,
                                    decimal? seaFreight,
                                    decimal? surcharge,
                                    int? weight,
                                    int? width,
                                    int? height,
                                    int? length,
                                    string? image,
                                    decimal? paid,
                                    decimal? total,
                                    Guid? accountId,
                                    string? accountName,
                                    Guid? updatedBy,
                                    DateTime? updatedDate,
                                    string? updatedByName,
                                    Guid? routerShippingId,
                                    string? shippingCodePost,
                                    string? trackingCode,
                                    string? trackingCarrier,
                                    int? package,
                                    DateTime? toDeliveryDate,
                                    List<OrderExpressDetailDto>? orderExpressDetail,
                                    List<OrderServiceAddDto>? orderServiceAdd,
                                    List<PaymentInvoiceDto>? paymentInvoice,
                                    List<OrderInvoiceDto>? orderInvoice)
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
        ContractId = contractId;
        ContractName = contractName;
        Currency = currency;
        ExchangeRate = exchangeRate;
        ShippingMethodId = shippingMethodId;
        ShippingMethodCode = shippingMethodCode;
        ShippingMethodName = shippingMethodName;
        RouterShipping = routerShipping;
        DomesticTracking = domesticTracking;
        DomesticCarrier = domesticCarrier;
        Status = status;
        PaymentTermId = paymentTermId;
        PaymentTermName = paymentTermName;
        PaymentMethodName = paymentMethodName;
        PaymentMethodId = paymentMethodId;
        PaymentStatus = paymentStatus;
        ShipperName = shipperName;
        ShipperPhone = shipperPhone;
        ShipperZipCode = shipperZipCode;
        ShipperAddress = shipperAddress;
        ShipperCountry = shipperCountry;
        ShipperProvince = shipperProvince;
        ShipperDistrict = shipperDistrict;
        ShipperWard = shipperWard;
        ShipperNote = shipperNote;
        DeliveryName = deliveryName;
        DeliveryPhone = deliveryPhone;
        DeliveryZipCode = deliveryZipCode;
        DeliveryAddress = deliveryAddress;
        DeliveryCountry = deliveryCountry;
        DeliveryProvince = deliveryProvince;
        DeliveryDistrict = deliveryDistrict;
        DeliveryWard = deliveryWard;
        DeliveryNote = deliveryNote;
        EstimatedDeliveryDate = estimatedDeliveryDate;
        DeliveryMethodId = deliveryMethodId;
        DeliveryMethodCode = deliveryMethodCode;
        DeliveryMethodName = deliveryMethodName;
        DeliveryStatus = deliveryStatus;
        IsBill = isBill;
        BillName = billName;
        BillAddress = billAddress;
        BillCountry = billCountry;
        BillProvince = billProvince;
        BillDistrict = billDistrict;
        BillWard = billWard;
        BillStatus = billStatus;
        Description = description;
        Note = note;
        GroupEmployeeId = groupEmployeeId;
        GroupEmployeeName = groupEmployeeName;
        CommodityGroup = commodityGroup;
        AirFreight = airFreight;
        SeaFreight = seaFreight;
        Surcharge = surcharge;
        Weight = weight;
        Width = width;
        Height = height;
        Length = length;
        Image = image;
        Paid = paid;
        Total = total;
        AccountId = accountId;
        AccountName = accountName;
        UpdatedBy = updatedBy;
        UpdatedDate = updatedDate;
        UpdatedByName = updatedByName;
        OrderExpressDetail = orderExpressDetail;
        OrderServiceAdd = orderServiceAdd;
        PaymentInvoice = paymentInvoice;
        OrderInvoice = orderInvoice;
        RouterShippingId = routerShippingId;
        ShippingCodePost = shippingCodePost;
        TrackingCode = trackingCode;
        TrackingCarrier = trackingCarrier;
        Package = package;
        ToDeliveryDate = toDeliveryDate;
    }

    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedByName { get; set; }
    public bool IsValid(IOrderExpressRepository _context)
    {
        ValidationResult = new OrderExpressEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ApprovalOrderExpressCommand : Command
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public string? ApproveComment { get; set; }
    public ApprovalOrderExpressCommand(
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
public class OrderExpressCreateByCustomerCommand : OrderExpressCommand
{
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public string? Code { get; set; }
    public string? DeliveryName { get; set; }
    public string? DeliveryPhone { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryCountry { get; set; }
    public string? DeliveryProvince { get; set; }
    public string? DeliveryDistrict { get; set; }
    public string? DeliveryWard { get; set; }
    public string? DeliveryNote { get; set; }
    public Guid? ShippingMethodId { get; set; }
    public string? ShippingMethodName { get; set; }
    public string? ShippingMethodCode { get; set; }
    public string? Note { get; set; }
    public string? RouterShipping { get; set; }
    public string? Currency { get; set; }
    public int? Weight { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Length { get; set; }
    public string? Image { get; set; }
    public string? DomesticTracking { get; set; }
    public List<OrderExpressDetailDto>? Detail { get; set; }
    public List<OrderServiceAddDto>? OrderServiceAdd { get; set; }

    public OrderExpressCreateByCustomerCommand(Guid id,
                                    Guid? accountId,
                                    string? accountName,
                                    string? code,
                                    string? deliveryName,
                                    string? deliveryPhone,
                                    string? deliveryAddress,
                                    string? deliveryCountry,
                                    string? deliveryProvince,
                                    string? deliveryDistrict,
                                    string? deliveryWard,
                                    string? deliveryNote,
                                    Guid? shippingMethodId,
                                    string? shippingMethodName,
                                    string? shippingMethodCode,
                                    string? note,
                                    string? routerShipping,
                                    string? currency,
                                    int? weight,
                                    int? width,
                                    int? height,
                                    int? length,
                                    string? image,
                                    string? domesticTracking,
                                    List<OrderExpressDetailDto>? detail,
                                    List<OrderServiceAddDto>? orderServiceAdd,
                                    Guid? createdBy,
                                    DateTime? createdDate,
                                    string? createdByName)
    {
        Id = id;
        AccountId = accountId;
        AccountName = accountName;
        Code = code;
        DeliveryName = deliveryName;
        DeliveryPhone = deliveryPhone;
        DeliveryAddress = deliveryAddress;
        DeliveryCountry = deliveryCountry;
        DeliveryProvince = deliveryProvince;
        DeliveryDistrict = deliveryDistrict;
        DeliveryWard = deliveryWard;
        DeliveryNote = deliveryNote;
        ShippingMethodId = shippingMethodId;
        ShippingMethodName = shippingMethodName;
        ShippingMethodCode = shippingMethodCode;
        Note = note;
        RouterShipping = routerShipping;
        Currency = currency;
        Weight = weight;
        Width = width;
        Height = height;
        Length = length;
        Image = image;
        DomesticTracking = domesticTracking;
        Detail = detail;
        OrderServiceAdd = orderServiceAdd;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
        CreatedByName = createdByName;
    }

    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public bool IsValid(IOrderExpressRepository _context)
    {
        ValidationResult = new OrderExpressCreateByCustomerCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class OrderExpressAddByCustomerCommand : Command
{
    public OrderExpressAddByCustomerCommand(Guid? id,
                                            string code,
                                            Guid? customerId,
                                            string storeCode,
                                            string currencyCode,
                                            string shippingMethodCode,
                                            string routerShipping,
                                            string trackingCode,
                                            string trackingCarrier,
                                            int? weight,
                                            int? width,
                                            int? height,
                                            int? length,
                                            string deliveryCountry,
                                            string deliveryProvince,
                                            string deliveryDistrict,
                                            string deliveryWard,
                                            string deliveryAddress,
                                            string deliveryName,
                                            string deliveryPhone,
                                            string deliveryNote,
                                            DataTable products,
                                            DataTable serviceAdd,
                                            string image,
                                            string images,
                                            string description,
                                            string note)
    {
        Id = id;
        Code = code;
        CustomerId = customerId;
        StoreCode = storeCode;
        CurrencyCode = currencyCode;
        ShippingMethodCode = shippingMethodCode;
        RouterShipping = routerShipping;
        TrackingCode = trackingCode;
        TrackingCarrier = trackingCarrier;
        Weight = weight;
        Width = width;
        Height = height;
        Length = length;
        DeliveryCountry = deliveryCountry;
        DeliveryProvince = deliveryProvince;
        DeliveryDistrict = deliveryDistrict;
        DeliveryWard = deliveryWard;
        DeliveryAddress = deliveryAddress;
        DeliveryName = deliveryName;
        DeliveryPhone = deliveryPhone;
        DeliveryNote = deliveryNote;
        Products = products;
        ServiceAdd = serviceAdd;
        Image = image;
        Images = images;
        Description = description;
        Note = note;
    }

    public Guid? Id { get; set; }
    public string Code { get; set; }
    public Guid? CustomerId { get; set; }
    public string StoreCode { get; set; }
    public string CurrencyCode { get; set; }
    public string ShippingMethodCode { get; set; }
    public string RouterShipping { get; set; }
    public string TrackingCode { get; set; }
    public string TrackingCarrier { get; set; }
    public int? Weight { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Length { get; set; }
    public string DeliveryCountry { get; set; }
    public string DeliveryProvince { get; set; }
    public string DeliveryDistrict { get; set; }
    public string DeliveryWard { get; set; }
    public string DeliveryAddress { get; set; }
    public string DeliveryName { get; set; }
    public string DeliveryPhone { get; set; }
    public string DeliveryNote { get; set; }
    public DataTable Products { get; set; }
    public DataTable ServiceAdd { get; set; }
    public string Image { get; set; }
    public string Images { get; set; }
    public string Description { get; set; }
    public string Note { get; set; }
}

public class OrderExpressDeleteCommand : OrderExpressCommand
{
    public OrderExpressDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IOrderExpressRepository _context)
    {
        ValidationResult = new OrderExpressDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ManagePaymentOrderExpressCommand : Command
{
    public Guid Id { get; set; }
    public int? PaymentStatus { get; set; }
    public List<PaymentInvoiceDto>? PaymentInvoice { get; set; }
    public ManagePaymentOrderExpressCommand(
        Guid id,
        int? paymentStatus,
        List<PaymentInvoiceDto>? paymentInvoice)
    {
        Id = id;
        PaymentStatus = paymentStatus;
        PaymentInvoice = paymentInvoice;
    }
}
public class ManageServiceOrderExpressCommand : Command
{
    public Guid Id { get; set; }
    public List<OrderServiceAddDto>? OrderServiceAdd { get; set; }
    public ManageServiceOrderExpressCommand(
        Guid id,
        List<OrderServiceAddDto>? orderServiceAdd)
    {
        Id = id;
        OrderServiceAdd = orderServiceAdd;
    }
}
public class NoteOrderExpressCommand : Command
{
    public Guid Id { get; set; }
    public List<OrderTrackingDto>? OrderTracking { get; set; }
    public NoteOrderExpressCommand(
        Guid id,
        List<OrderTrackingDto>? orderTracking)
    {
        Id = id;
        OrderTracking = orderTracking;
    }
}

public class CreateInvoicePayOrderExpressCommand : Command
{
    public Guid Id { get; set; }
    public decimal TotalPay { get; set; }
    public Guid AccountId { get; set; }
    public Guid? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
}
