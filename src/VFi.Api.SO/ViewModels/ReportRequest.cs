using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddReportRequest
{
    public string? Name { get; set; }
    public Guid? ReportTypeId { get; set; }
    public string? ReportTypeCode { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? EmployeeId { get; set; }
    public string? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }
    public string? CategoryRootId { get; set; }
    public string? CategoryRootName { get; set; }
    public string? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? CustomerGroupId { get; set; }
    public string? CustomerGroupCode { get; set; }
    public string? CustomerGroupName { get; set; }
    public string? CurrencyCode { get; set; }
    public int? Status { get; set; }
    public DateTime? LoadDate { get; set; }
}
public class EditReportRequest : AddReportRequest
{
    public Guid Id { get; set; }
}
public class ReportPagingDetailRequest : FopPagingRequest
{

    [FromQuery(Name = "$typeReport")]
    public string? TypeReport { get; set; }
}
public class LoadDataReportRequest
{
    public Guid ReportId { get; set; }
    public string ReportType { get; set; } = null!;
    public string? CustomerCode { get; set; }
    public string? CustomerGroupId { get; set; }
    public string? EmployeeId { get; set; }
    public string? CategoryRootId { get; set; }
    public string? ProductCode { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? Status { get; set; }
    public int? DiferenceStatus { get; set; }
}
