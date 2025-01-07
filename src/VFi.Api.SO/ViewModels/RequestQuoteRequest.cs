using System.Xml.Linq;
using Consul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddRequestQuoteRequest
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? StoreId { get; set; }
    public string? StoreCode { get; set; }
    public string? StoreName { get; set; }
    public string? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? Note { get; set; }
    public int? Status { get; set; }
    public Guid? ChannelId { get; set; }
    public string? ChannelCode { get; set; }
    public string? ChannelName { get; set; }
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
}
public class EditRequestQuoteRequest : AddRequestQuoteRequest
{
    public string Id { get; set; }
}
public class RequestQuotePagingRequest : FilterQuery
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$employeeId")]
    public string? EmployeeId { get; set; }
}
public class UpdateStatusRequestQuoteRequest
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
}
public class RequestQuoteListBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$customerId")]
    public Guid? CustomerId { get; set; }
    public RequestQuoteQueryParams ToBaseQuery() => new RequestQuoteQueryParams
    {
        Status = Status,
        CustomerId = CustomerId
    };
}
