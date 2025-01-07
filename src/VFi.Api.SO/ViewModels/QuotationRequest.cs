using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddQuotationRequest
{
    public Guid? Id { get; set; }
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
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
    public List<FileRequest>? File { get; set; }
    public List<AddOrderProductRequest>? OrderProduct { get; set; }
    public List<AddOrderServiceAddRequest>? OrderServiceAdd { get; set; }
}
public class EditQuotationRequest : AddQuotationRequest
{
    public Guid Id { get; set; }
}
public class QuotationListBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$date")]
    public DateTime? Date { get; set; }
    [FromQuery(Name = "$customerId")]
    public Guid? CustomerId { get; set; }
    [FromQuery(Name = "$isContract")]
    public int? IsContract { get; set; }
    [FromQuery(Name = "$isOrder")]
    public int? IsOrder { get; set; }
    [FromQuery(Name = "$fromDate")]
    public DateTime? FromDate { get; set; }
    [FromQuery(Name = "$toDate")]
    public DateTime? ToDate { get; set; }
    [FromQuery(Name = "$keyword")]
    public string? Keyword { get; set; }
    [FromQuery(Name = "$pageSize")]
    public int PageSize { get; set; }
    [FromQuery(Name = "$pageIndex")]
    public int PageIndex { get; set; }
    [FromQuery(Name = "$currency")]
    public string? Currency { get; set; }

    public QuotationParams ToBaseQuery() => new QuotationParams
    {
        Status = Status,
        Date = Date,
        CustomerId = CustomerId,
        IsContract = IsContract,
        IsOrder = IsOrder,
        FromDate = FromDate,
        ToDate = ToDate,
        Currency = Currency,
    };
}
public class QuotationPagingRequest : FilterQuery
{
    [FromQuery(Name = "$employeeId")]
    public string? EmployeeId { get; set; }
}
public class UpdateStatusQuotationRequest
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public string? ApproveComment { get; set; }
    public string? Description { get; set; }
}
public class ValidateExcelQuotationRequest
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

public class QuotationSendTransactionRequest
{
    public string? Keyword { get; set; }
    public string? Quotation { get; set; }
}
