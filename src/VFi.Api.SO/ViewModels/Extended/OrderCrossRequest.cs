namespace VFi.Api.SO.ViewModels.Extended;

public class AddOrderCrossRequest
{
    public bool? IsAutoPayment { get; set; }
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; } = null!;
    public string? Code { get; set; } = null!;
    public DateTime? OrderDate { get; set; }
    public Guid? CustomerId { get; set; }
    public int? Status { get; set; }
    public string? Currency { get; set; }
    public string? Note { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public string PurchaseGroup { get; set; } = null!;
    public string ChannelCode { get; set; } = null!;
    public List<AddOrderProductRequest>? Details { get; set; }
    public List<AddOrderServiceAddRequest>? OrderServiceAdd { get; set; }
    public List<AddPaymentInvoiceRequest>? PaymentInvoice { get; set; }
}

public class CreateOrderCrossRequest
{
    public Guid? AccountId { get; set; }
    public Guid? CustomerId { get; set; }
    public string PurchaseGroup { get; set; }
    public string? OrderType { get; set; }
    public string StoreCode { get; set; }
    public string ChannelCode { get; set; }
    public string CurrencyCode { get; set; }
    public decimal? ExchangeRate { get; set; }
    public string PaymentTermCode { get; set; }
    public string PaymentMethodCode { get; set; }
    public string ShippingMethodCode { get; set; }
    public int? BuyFee { get; set; }
    public decimal? DomesticShipping { get; set; }
    public string RouterShipping { get; set; }

    public string? DeliveryCountry { get; set; }
    public string? DeliveryProvince { get; set; }
    public string? DeliveryDistrict { get; set; }
    public string? DeliveryWard { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryName { get; set; }
    public string? DeliveryPhone { get; set; }
    public string? DeliveryNote { get; set; }

    public decimal? TotalPay { get; set; }
    public bool PayNow { get; set; }

    public List<ProductCrossRequest> Products { get; set; }

    public List<ServiceAddCrossRequest> ServiceAdd { get; set; }

    public string Image { get; set; }
    public string? Images { get; set; }
    public string Description { get; set; }
    public string Note { get; set; }
}

public class ProductCrossRequest
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; }
    public string SourceLink { get; set; }
    public string? SourceCode { get; set; }
    public string? Image { get; set; }
    public string? Images { get; set; }
    public string? Origin { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public float TaxRate { get; set; }
    public string? Note { get; set; }
    public string? CategoryCode { get; set; }
    public string? GroupCategoryCode { get; set; }
    public string Attributes { get; set; }
    public string? BidUsername { get; set; }
    public string? SellerId { get; set; }
}

public class ServiceAddCrossRequest
{

    public Guid Id { get; set; }
    public Guid ServiceAddId { get; set; }
    public string ServiceAddName { get; set; }
    public decimal Price { get; set; }
    public int Status { get; set; }
    public string Currency { get; set; }
    public decimal ExchangeRate { get; set; } = 1;
    public string? Note { get; set; }
}


public class CreateOrderPaymentInvoiceRequest
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public decimal TotalPay { get; set; }
}

public class OrderInfoRequest
{
    public Guid AccountId { get; set; }
    public List<string> OrderCode { get; set; }
}

public class OrderUpdateAddressRequest
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string? DeliveryCountry { get; set; }
    public string? DeliveryProvince { get; set; }
    public string? DeliveryDistrict { get; set; }
    public string? DeliveryWard { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryName { get; set; }
    public string? DeliveryPhone { get; set; }
    public string? DeliveryNote { get; set; }
}

public class OrderUpdateBidUsernameRequest
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string? BidUsername { get; set; }
}

public class OrderCancelRequest
{
    public Guid Id { get; set; }
    public bool IsPayFee { get; set; }
}

public class OrderCrossAddServiceRequest
{
    public Guid Id { get; set; }
    public List<ServiceAddCrossRequest> ServiceAdd { get; set; }
}

public class UpdateTrackingOrderRequest
{
    public Guid Id { get; set; }
    public int? DomesticStatus { get; set; }
    public string? DomesticTracking { get; set; }
    public string? DomesticCarrier { get; set; }
}
public class UpdateShipmentRoutingOrderRequest
{
    public Guid Id { get; set; }
    public string? ShipmentRouting { get; set; }
}
public class UpdateOrderInternalNoteRequest
{
    public Guid Id { get; set; }
    public string? InternalNote { get; set; }
}
