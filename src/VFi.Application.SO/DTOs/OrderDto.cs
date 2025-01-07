namespace VFi.Application.SO.DTOs;

public class OrderDto
{
    public Guid? Id { get; set; }
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
    public string? ContractCode { get; set; }
    public string? ContractName { get; set; }
    public Guid? QuotationId { get; set; }
    public string? QuotationCode { get; set; }
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
    public string? DeliveryCountry { get; set; }
    public string? DeliveryProvince { get; set; }
    public string? DeliveryDistrict { get; set; }
    public string? DeliveryWard { get; set; }
    public string? DeliveryNote { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
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
    public string? DeliveryTracking { get; set; }
    public string? DeliveryCarrier { get; set; }
    public int? DeliveryPackage { get; set; }
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
    public DateTime? ApproveDate { get; set; }
    public Guid? ApproveBy { get; set; }
    public string? ApproveComment { get; set; }
    public bool? IsReturned { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public string? PurchaseGroup { get; set; }
    public int? BuyFee { get; set; }
    public string? RouterShipping { get; set; }
    public string? CommodityGroup { get; set; }
    public decimal? AirFreight { get; set; }
    public decimal? SeaFreight { get; set; }
    public decimal? Surcharge { get; set; }
    public string? DomesticTracking { get; set; }
    public string? DomesticCarrier { get; set; }
    public int? DomesticPackage { get; set; }
    public decimal? DomesticShiping { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public decimal? Total { get; set; }
    public decimal? Paid { get; set; }
    public int? Weight { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Length { get; set; }
    public decimal? TotalAmountTax { get; set; }
    public decimal? TotalDiscountAmount { get; set; }
    public decimal? TotalServiceAmount { get; set; }
    public decimal? TotalAmountCollected { get; set; }

    public decimal? TotalAmountConvert { get; set; }
    public decimal? TotalAmountNeedPay { get; set; }

    //public decimal? TotalAmountConvert
    //    => TotalAmountTax.GetValueOrDefault() * ExchangeRate.GetValueOrDefault()
    //        + OrderServiceAdd?.Sum(x => x.Price * x.ExchangeRate).GetValueOrDefault();

    //public decimal? TotalAmountNeedPay
    //{
    //    get
    //    {
    //        var payable = TotalAmountConvert.GetValueOrDefault() - TotalAmountCollected.GetValueOrDefault();
    //        return payable >= 0 ? payable : 0;
    //    }
    //}

    public List<FileDto>? File { get; set; }
    public List<DocumentDto>? Reference { get; set; }
    public List<OrderProductDto>? OrderProduct { get; set; }
    public List<OrderServiceAddDto>? OrderServiceAdd { get; set; }
    public List<PaymentInvoiceDto>? PaymentInvoice { get; set; }
    public List<OrderTrackingDto>? OrderTracking { get; set; }
    public List<OrderInvoiceDto>? OrderInvoice { get; set; }
    public List<DeliveryProductDto>? DeliveryProduct { get; set; }
    public string? ProductLink { get; internal set; }
    public decimal? TotalAmountOriginal { get; internal set; }
    public decimal? ShippingFee { get; internal set; }
    public string? DeliveryPhone { get; internal set; }
    public string? DeliveryName { get; internal set; }
    public string BillName { get; internal set; }
    public string? BidUsername { get; internal set; }
    public string? SellerId { get; internal set; }
    public string? ProductSourceCode { get; internal set; }
    public int? DomesticStatus { get; internal set; }
    public string? ShipmentRouting { get; internal set; }
    public string? CustomerEmail { get; internal set; }
    public string InternalNote { get; internal set; }
    public int CountPaymentInvoice { get; internal set; }
    public DateTime? DomesticDeliveryDate { get; internal set; }
    public Guid? ExportWarehouseProductId { get; set; }
    public Guid? ExportWarehouseId { get; set; }
    public string? ExportWarehouseCode { get; set; }
    public DateTime? ExportWarehouseCreatedDate { get; set; }
    public int? ExportWarehouseStatus { get; set; }
    public Guid? RequestPurchaseId { get; set; }
    public string? RequestPurchaseCode { get; set; }
    public int? RequestPurchaseStatus { get; set; }
    public DateTime? RequestPurchaseCreateDate { get; set; }
}

public class OrderParams
{
    public string? Status { get; set; }
    public string? DiferenceStatus { get; set; }
    public Guid? ContractId { get; set; }
    public Guid? ProductId { get; set; }
    public DateTime? OrderDate { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? WarehouseId { get; set; }
    public int? IsContract { get; set; }
    public int? IsQuotation { get; set; }
    public int? StatusExport { get; set; }
    public int? StatusPurchase { get; set; }
    public int? StatusSales { get; set; }
    public int? StatusReturn { get; set; }
    public int? StatusProduction { get; set; }
    public int? StatusExported { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? OrderType { get; set; }
    public string? Filter { get; set; }
    public string? Currency { get; set; }
}

public class OrderReferenceDto
{
    public Guid? Id { get; set; }
    public string? Code { get; set; }
    public string? CustomerName { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public DateTime? OrderDate { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
}

public class OrderPrintDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? OrderDate { get; set; }
    public string? OrderDateLabel { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountPhone { get; set; }
    public string? AccountName { get; set; }
    public string? Note { get; set; }
    public string? CreatedDateString { get; set; }
    public string? PrintedDateString { get; set; }
    public string? TotalAmountNoTax { get; set; }
    public string? TotalTaxValue { get; set; }
    public string? TotalDiscount { get; set; }
    public string? TotalAmount { get; set; }
    public string? TotalAmountText { get; set; }
    public byte[]? BarCode { get; set; }
    public byte[]? QrCode { get; set; }
    public string? StoreAddress { get; set; }
    public string? StoreName { get; set; }
    public string? StorePhone { get; set; }
    public string? Total { get; set; }
    public List<FileDto>? Files { get; set; }
    public List<OrderDetailPrintDto>? Details { get; set; }
    public List<OrderServiceAddPrintDto>? OrderServiceAdd { get; set; }
    public string? Tracking { get; set; }
}

public class OrderDetailPrintDto
{
    public Guid? Id { get; set; }
    public string ProductCode { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string? UnitName { get; set; }
    public string? Tax { get; set; }
    public string Quantity { get; set; }
    public string? UnitPrice { get; set; }
    public string? AmountNoVat { get; set; }
    public decimal? AmountNoDiscount { get; set; }
    public decimal? DiscountAmountDistribution { get; set; }
    public decimal? TotalAmountDiscount { get; set; }
    public decimal? AmountNoTax { get; set; }
    public decimal? AmountTax { get; set; }
    public decimal? TotalAmountTax { get; set; }
    public int? SortOrder { get; set; }
    public int? STT { get; set; }
    public int? DisplayOrder { get; set; }
}

public class OrderServiceAddPrintDto
{
    public Guid? Id { get; set; }
    public string ServiceAddName { get; set; } = null!;
    public decimal? Price { get; set; }
    public string? PriceString { get; set; }
    public int? SortOrder { get; set; }
}

public class Relate
{
    public Guid? Id { get; set; }
    public string Code { get; set; }
    public string Type { get; set; }
}

public class OrderListBoxDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? AccountName { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public string? Calculation { get; set; }
    public int? TypeDiscount { get; set; }
    public double? DiscountRate { get; set; }
    public int? TypeCriteria { get; set; }
    public decimal? AmountDiscount { get; set; }
    public decimal? TotalAmountTax { get; set; }
    public int? Status { get; set; }
    public string? Description { get; set; }
    public decimal? ExchangeRate { get; set; }
}

public class OrderValidateDto
{
    public UInt32 Row { get; set; }

    public bool IsValid
    {
        get { return Errors.Count == 0; }
    }

    public List<string> Errors { get; set; } = new List<string>();
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public Guid? UnitId { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public string? UnitPrice { get; set; }
    public string? Quantity { get; set; }
    public string? StockQuantity { get; set; }
    public string? DiscountPercent { get; set; }
    public string? Note { get; set; }
    public string? UnitType { get; set; }
    public string? ReasonName { get; set; }
    public Guid? ReasonId { get; set; }
    public string? TaxCategoryId { get; set; }
    public string? Tax { get; set; }
    public string? TaxCode { get; set; }
    public string? TaxRate { get; set; }
}