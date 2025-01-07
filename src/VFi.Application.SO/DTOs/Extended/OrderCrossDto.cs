namespace VFi.Application.SO.DTOs.Extended;

public class OrderCrossDto
{
    private DateTime? _paymentExpiryDate;

    public Guid? Id { get; set; }
    public string Code { get; set; } = null!;
    public string OrderType { get; set; }
    public DateTime? OrderDate { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerCode { get; set; }
    public Guid? StoreId { get; set; }
    public string? StoreCode { get; set; }
    public string? StoreName { get; set; }
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
    public string? DeliveryName { get; set; }
    public string? DeliveryNote { get; set; }
    public string? DeliveryPhone { get; set; }
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
    public DateTime? PaymentExpiryDate
    {
        get
        {
            if (_paymentExpiryDate.HasValue)
            {
                return _paymentExpiryDate;
            }
            if (CreatedDate.HasValue)
            {
                return CreatedDate.Value.AddDays(2);
            }
            return null;
        }
        set => _paymentExpiryDate = value;
    }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public decimal? TotalDiscountAmount { get; set; }
    public decimal? TotalAmount { get; set; }
    public List<OrderProductDto>? Details { get; set; }
    public List<OrderServiceAddDto>? OrderServiceAdd { get; set; }
    public List<PaymentInvoiceDto>? PaymentInvoice { get; set; }
    public List<OrderTrackingDto>? OrderTracking { get; set; }
    public string? Image { get; set; }
}

