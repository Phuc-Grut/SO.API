﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.Api.SO.ViewModels;

namespace VFi.Application.SO.DTOs
{
    public class OrderExpressRequest : FilterQuery
    {
        public int? Status { get; set; }
    }
    public class CreateOrderExpressRequest
    {
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
        public List<ProductExpressRequest> Products { get; set; }
        public List<ServiceAddExpressRequest> ServiceAdd { get; set; }
    }
    public class AddOrderExpressRequest
    {
        public bool? IsAuto { get; set; }
        public string? ModuleCode { get; set; }
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
        public List<OrderExpressDetailRequest>? OrderExpressDetail { get; set; }
        public List<AddOrderServiceAddRequest>? OrderServiceAdd { get; set; }
        public List<AddPaymentInvoiceRequest>? PaymentInvoice { get; set; }
        public List<OrderInvoiceRequest>? OrderInvoice { get; set; }
    }

    public class EditOrderExpressRequest : AddOrderExpressRequest
    {
        public Guid Id { get; set; }
    }

    public class ApprovalOrderExpressRequest
    {
        public Guid Id { get; set; }
        public int? Status { get; set; }
        public string? ApproveComment { get; set; }
    }

    public class AddOrderExpressByCustomerRequest
    {
        public string Code { get; set; } = string.Empty;
        public Guid? AccountId { get; set; }
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
        public List<ProductExpressRequest> Products { get; set; } = new List<ProductExpressRequest>();
        public List<ServiceAddExpressRequest> ServiceAdd { get; set; } = new List<ServiceAddExpressRequest>();
        public string Image { get; set; }
        public string Images { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }
    public class ProductExpressRequest
    {
        public Guid? Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string Image { get; set; } = "";
        public string Images { get; set; } = "";
        public string Origin { get; set; } = "";
        public string UnitName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Note { get; set; } = "";
        public string CommodityGroup { get; set; } = "";
    }
    public class ServiceAddExpressRequest
    {
        public Guid? Id { get; set; }
        public Guid ServiceAddId { get; set; }
        public string ServiceAddName { get; set; } = "";
        public decimal Price { get; set; }
        public int Status { get; set; }
        public string Currency { get; set; } = "";
        public decimal ExchangeRate { get; set; } = 1;
        public string Note { get; set; } = "";
    }
    public class OrderExpressByAccountPagingRequest : FilterQuery
    {
        public Guid AccountId { get; set; }
    }

    public class OrderExpressDetailRequest
    {
        public Guid? Id { get; set; }
        public Guid? OrderExpressId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public string? ProductLink { get; set; }
        public string? Origin { get; set; }
        public string? UnitName { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? DisplayOrder { get; set; }
        public string? Note { get; set; }
        public string? CommodityGroup { get; set; }
        public string? SurchargeGroup { get; set; }
        public decimal? Surcharge { get; set; }
    }

    public class ManagePaymentOrderExpressRequest
    {
        public Guid Id { get; set; }
        public int? PaymentStatus { get; set; }
        public List<AddPaymentInvoiceRequest>? PaymentInvoice { get; set; }
    }

    public class ManageServiceOrderExpressRequest
    {
        public Guid Id { get; set; }
        public List<AddOrderServiceAddRequest>? OrderServiceAdd { get; set; }

    }

    public class NoteOrderExpressRequest
    {
        public Guid Id { get; set; }
        public List<AddOrderTrackingRequest>? OrderTracking { get; set; }

    }


    public class CreateOrderExpressPaymentInvoiceRequest
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public decimal TotalPay { get; set; }
    }
}
