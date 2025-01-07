using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;

namespace VFi.Api.SO.ViewModels;

public class AddContractRequest
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? ContractTypeId { get; set; }
    public string? ContractTypeName { get; set; }
    public string? QuotationId { get; set; }
    public string? QuotationName { get; set; }
    public string? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? SignDate { get; set; }
    public string? CustomerId { get; set; }
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
    public string? GroupEmployeeId { get; set; }
    public string? GroupEmployeeName { get; set; }
    public string? AccountId { get; set; }
    public string? ContractTermId { get; set; }
    public string? ContractTermName { get; set; }
    public string? ContractTermContent { get; set; }
    public DateTime? PaymentDueDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? Buyer { get; set; }
    public string? Saler { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public List<FileRequest>? File { get; set; }
    public bool? HasPreviousContract { get; set; }
    public decimal? Paid { get; set; }
    public decimal? Received { get; set; }
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
    public List<AddOrderProductRequest>? OrderProduct { get; set; }
}
public class EditContractRequest : AddContractRequest
{
    public Guid Id { get; set; }
}
public class ContractListBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$date")]
    public DateTime? Date { get; set; }
    [FromQuery(Name = "$customerId")]
    public Guid? CustomerId { get; set; }
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
    public ContractParams ToBaseQuery() => new ContractParams
    {
        Status = Status,
        Date = Date,
        CustomerId = CustomerId,
        IsOrder = IsOrder,
        FromDate = FromDate,
        ToDate = ToDate
    };
}
public class ContractPagingRequest : FilterQuery
{
    [FromQuery(Name = "$employeeId")]
    public string? EmployeeId { get; set; }
}
public class ApprovalContractRequest
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public string? ApproveComment { get; set; }
}
public class LiquidationContractRequest
{
    public Guid Id { get; set; }
    public decimal? AmountLiquidation { get; set; }
    public DateTime? LiquidationDate { get; set; }
    public string? LiquidationReason { get; set; }
}
