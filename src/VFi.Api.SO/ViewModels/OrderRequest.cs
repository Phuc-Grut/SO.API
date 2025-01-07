using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;

namespace VFi.Api.SO.ViewModels;

public class AddOrderRequest
{
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
    public string? OrderType { get; set; }
    public string? Code { get; set; } = null!;
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

    public string? RouterShipping { get; set; }
    public string? DomesticTracking { get; set; }
    public string? DomesticCarrier { get; set; }
    public int? DomesticPackage { get; set; }
    public int? Weight { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Length { get; set; }

    public List<FileRequest>? File { get; set; }
    public List<AddOrderProductRequest>? OrderProduct { get; set; }
    public List<AddOrderServiceAddRequest>? OrderServiceAdd { get; set; }
    public List<AddPaymentInvoiceRequest>? PaymentInvoice { get; set; }
    public List<OrderInvoiceRequest>? OrderInvoice { get; set; }
    public List<DeliveryProductDto>? ListExpectedDelivery { get; set; }
}
public class EditOrderRequest : AddOrderRequest
{
    public Guid Id { get; set; }
}
public class ApprovalOrderRequest
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public string? ApproveComment { get; set; }
}

public class ApprovalOrdersRequest
{
    public List<Guid> Ids { get; set; }
    public int? Status { get; set; }
    public string? ApproveComment { get; set; }
}
public class OrderPagingRequest : FilterQuery
{

    [FromQuery(Name = "$employeeId")]
    public string? EmployeeId { get; set; }
}
public class OrderReferenceRequest
{
    [FromQuery(Name = "$status")]
    public string? Status { get; set; }
    [FromQuery(Name = "$diferenceStatus")]
    public string? DiferenceStatus { get; set; }
    [FromQuery(Name = "$contractId")]
    public Guid? ContractId { get; set; }
    [FromQuery(Name = "$productId")]
    public Guid? ProductId { get; set; }
    [FromQuery(Name = "$orderDate")]
    public DateTime? OrderDate { get; set; }
    [FromQuery(Name = "$customerId")]
    public Guid? CustomerId { get; set; }
    [FromQuery(Name = "$isContract")]
    public int? IsContract { get; set; }
    [FromQuery(Name = "$isQuotation")]
    public int? IsQuotation { get; set; }
    [FromQuery(Name = "$statusExport")]
    public int? StatusExport { get; set; }
    [FromQuery(Name = "$statusPurchase")]
    public int? StatusPurchase { get; set; }
    [FromQuery(Name = "$statusReturn")]
    public int? StatusReturn { get; set; }
    [FromQuery(Name = "$statusSales")]
    public int? StatusSales { get; set; }
    [FromQuery(Name = "$fromDate")]
    public DateTime? FromDate { get; set; }
    [FromQuery(Name = "$toDate")]
    public DateTime? ToDate { get; set; }
    [FromQuery(Name = "$warehouseId")]
    public Guid? WarehouseId { get; set; }
    [FromQuery(Name = "$orderType")]
    public string? OrderType { get; set; }
    [FromQuery(Name = "$keyword")]
    public string? Keyword { get; set; }
    [FromQuery(Name = "$pageSize")]
    public int PageSize { get; set; }
    [FromQuery(Name = "$pageIndex")]
    public int PageIndex { get; set; }
    [FromQuery(Name = "$currency")]
    public string? Currency { get; set; }
    [FromQuery(Name = "$statusProduction")]
    public int? StatusProduction { get; set; }
    public OrderParams ToBaseQuery() => new OrderParams
    {
        Status = Status,
        DiferenceStatus = DiferenceStatus,
        ContractId = ContractId,
        ProductId = ProductId,
        OrderDate = OrderDate,
        CustomerId = CustomerId,
        IsContract = IsContract,
        IsQuotation = IsQuotation,
        StatusExport = StatusExport,
        StatusPurchase = StatusPurchase,
        StatusReturn = StatusReturn,
        StatusSales = StatusSales,
        FromDate = FromDate,
        ToDate = ToDate,
        WarehouseId = WarehouseId,
        OrderType = OrderType,
        Currency = Currency,
        StatusProduction = StatusProduction,
    };
}

public class CreateOrderExtRequest
{
    public Guid? AccountId { get; set; }
    public Guid? CustomerId { get; set; }
    //public string OrderCode { get; set; }
    public string StoreCode { get; set; }
    public string ChannelCode { get; set; }
    public string CurrencyCode { get; set; }
    public decimal? ExchangeRate { get; set; }
    public string PaymentTermCode { get; set; }
    public string PaymentMethodCode { get; set; }
    public string ShippingMethodCode { get; set; }
    public int? BuyFee { get; set; }
    public string RouterShipping { get; set; }
    public List<ProductRequest> Products { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public string Note { get; set; }
}
public class ManageServiceOrderRequest
{
    public Guid Id { get; set; }
    public List<AddOrderServiceAddRequest>? OrderServiceAdd { get; set; }

}
public class NoteOrderRequest
{
    public Guid Id { get; set; }
    public List<AddOrderTrackingRequest>? OrderTracking { get; set; }

}
public partial class OrderProductPagingRequest
{
    [FromQuery(Name = "$keyword")]
    public string? Keyword { get; set; }

    [FromQuery(Name = "$filter")]
    public string? Filter { get; set; }

    [FromQuery(Name = "$order")]
    public string? Order { get; set; }

    [FromQuery(Name = "$pageNumber")]
    public int PageNumber { get; set; }

    [FromQuery(Name = "$pageSize")]
    public int PageSize { get; set; }
    [FromQuery(Name = "$customerId")]
    public Guid? CustomerId { get; set; }

    [FromQuery(Name = "$diferenceStatus")]
    public string? DiferenceStatus { get; set; }

    [FromQuery(Name = "$orderType")]
    public string? OrderType { get; set; }

    [FromQuery(Name = "$status")]
    public string? Status { get; set; }
    [FromQuery(Name = "$fromDate")]
    public DateTime? FromDate { get; set; }
    [FromQuery(Name = "$toDate")]
    public DateTime? ToDate { get; set; }
    [FromQuery(Name = "$statusReturn")]
    public int? StatusReturn { get; set; }
    [FromQuery(Name = "$warehouseId")]
    public Guid? WarehouseId { get; set; }

    [FromQuery(Name = "$currency")]
    public string? Currency { get; set; }
    [FromQuery(Name = "$statusPurchase")]
    public int? StatusPurchased { get; set; }
    [FromQuery(Name = "$statusProduction")]
    public int? StatusProduction { get; set; }

    [FromQuery(Name = "$statusExport")]
    public int? StatusExport { get; set; }
    [FromQuery(Name = "$statusSales")]
    public int? StatusSales { get; set; }
    public OrderParams ToBaseQuery() => new OrderParams
    {
        CustomerId = CustomerId,
        DiferenceStatus = DiferenceStatus,
        OrderType = OrderType,
        Status = Status,
        FromDate = FromDate,
        ToDate = ToDate,
        StatusReturn = StatusReturn,
        StatusSales = StatusSales,
        WarehouseId = WarehouseId,
        Currency = Currency,
        StatusPurchase = StatusPurchased,
        StatusProduction = StatusProduction,
        StatusExport = StatusExport,
        Filter = Filter

    };

    public class OrderEmailNotifyRequest : EmailNotifyRequest
    {
        public string Order { get; set; }
    }



    public class OrderSendTransactionRequest
    {
        public string? Keyword { get; set; }
        public string Order { get; set; }
    }
}
public class RecalculatePriceRequest
{
    public Guid Id { get; set; }
}
