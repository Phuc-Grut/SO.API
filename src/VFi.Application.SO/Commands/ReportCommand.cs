using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using FluentValidation;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class ReportCommand : Command
{
    public Guid Id { get; set; }
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

public class ReportAddCommand : ReportCommand
{
    public ReportAddCommand(
        Guid id,
        string? name,
        Guid? reportTypeId,
        string? reportTypeCode,
        DateTime? fromDate,
        DateTime? toDate,
        string? customerId,
        string? customerCode,
        string? customerName,
        string? employeeId,
        string? employeeCode,
        string? employeeName,
        string? categoryRootId,
        string? categoryRootName,
        string? productId,
        string? productCode,
        string? productName,
        string? customerGroupId,
        string? customerGroupCode,
        string? customerGroupName,
        string? currencyCode,
        int? status,
        DateTime? loadDate)
    {
        Id = id;
        Name = name;
        ReportTypeId = reportTypeId;
        ReportTypeCode = reportTypeCode;
        FromDate = fromDate;
        ToDate = toDate;
        CustomerId = customerId;
        CustomerCode = customerCode;
        CustomerName = customerName;
        EmployeeId = employeeId;
        EmployeeCode = employeeCode;
        EmployeeName = employeeName;
        CategoryRootId = categoryRootId;
        CategoryRootName = categoryRootName;
        ProductId = productId;
        ProductCode = productCode;
        ProductName = productName;
        CustomerGroupId = customerGroupId;
        CustomerGroupCode = customerGroupCode;
        CustomerGroupName = customerGroupName;
        CurrencyCode = currencyCode;
        Status = status;
        LoadDate = loadDate;
    }
    public bool IsValid(IReportRepository _context)
    {
        ValidationResult = new ReportAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ReportEditCommand : ReportCommand
{
    public ReportEditCommand(
        Guid id,
        string? name,
        Guid? reportTypeId,
        string? reportTypeCode,
        DateTime? fromDate,
        DateTime? toDate,
        string? customerId,
        string? customerCode,
        string? customerName,
        string? employeeId,
        string? employeeCode,
        string? employeeName,
        string? categoryRootId,
        string? categoryRootName,
        string? productId,
        string? productCode,
        string? productName,
        string? customerGroupId,
        string? customerGroupCode,
        string? customerGroupName,
        string? currencyCode,
        int? status,
        DateTime? loadDate)
    {
        Id = id;
        Name = name;
        ReportTypeId = reportTypeId;
        ReportTypeCode = reportTypeCode;
        FromDate = fromDate;
        ToDate = toDate;
        CustomerId = customerId;
        CustomerCode = customerCode;
        CustomerName = customerName;
        EmployeeId = employeeId;
        EmployeeCode = employeeCode;
        EmployeeName = employeeName;
        CategoryRootId = categoryRootId;
        CategoryRootName = categoryRootName;
        ProductId = productId;
        ProductCode = productCode;
        ProductName = productName;
        CustomerGroupId = customerGroupId;
        CustomerGroupCode = customerGroupCode;
        CustomerGroupName = customerGroupName;
        CurrencyCode = currencyCode;
        Status = status;
        LoadDate = loadDate;
    }
    public bool IsValid(IReportRepository _context)
    {
        ValidationResult = new ReportEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ReportDeleteCommand : ReportCommand
{
    public ReportDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IReportRepository _context)
    {
        ValidationResult = new ReportDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ReportLoadDataCommand : Command
{
    public ReportLoadDataCommand(Guid reportId)
    {
        ReportId = reportId;
    }
    public Guid ReportId { get; set; }
}
