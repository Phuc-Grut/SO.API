using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class ReportDto
{
    public Guid? Id { get; set; }
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
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedByName { get; set; }
}
