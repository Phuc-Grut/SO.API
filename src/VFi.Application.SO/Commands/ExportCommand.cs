using Consul;
using MassTransit.Internals.GraphValidation;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class ExportCommand : Command
{
    public Guid Id { get; set; }
    public Guid? ExportWarehouseId { get; set; }
    public string? Code { get; set; }
    public DateTime? ExportDate { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }
    public int? Status { get; set; }
    public double? TotalQuantity { get; set; }
    public string? Note { get; set; }
    public string? File { get; set; }
    public Guid? ApproveBy { get; set; }
    public DateTime? ApproveDate { get; set; }
    public string? ApproveByName { get; set; }
    public string? ApproveComment { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public List<ExportProductDto>? Detail { get; set; }
    public List<ListId>? Delete { get; set; }
}
public class ExportAddCommand : ExportCommand
{
    public ExportAddCommand(
        Guid id,
        Guid? exportWarehouseId,
        string? code,
        DateTime? exportDate,
        Guid? employeeId,
        string? employeeCode,
        string? employeeName,
        int? status,
        double? totalQuantity,
        string? note,
        string? file,
        Guid? approveBy,
        DateTime? approveDate,
        string? approveByName,
        string? approveComment,
        Guid? createdBy,
        string? createdByName,
        List<ExportProductDto>? detail
        )
    {
        Id = id;
        ExportWarehouseId = exportWarehouseId;
        Code = code;
        ExportDate = exportDate;
        EmployeeId = employeeId;
        EmployeeCode = employeeCode;
        EmployeeName = employeeName;
        Status = status;
        TotalQuantity = totalQuantity;
        Note = note;
        File = file;
        ApproveBy = approveBy;
        ApproveDate = approveDate;
        ApproveByName = approveByName;
        ApproveComment = approveComment;
        CreatedBy = createdBy;
        CreatedByName = createdByName;
        Detail = detail;
    }
    public bool IsValid(IExportRepository _context)
    {
        ValidationResult = new ExportAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ExportEditCommand : ExportCommand
{
    public ExportEditCommand(
        Guid id,
        Guid? exportWarehouseId,
        string? code,
        DateTime? exportDate,
        Guid? employeeId,
        string? employeeCode,
        string? employeeName,
        int? status,
        double? totalQuantity,
        string? note,
        string? file,
        Guid? approveBy,
        DateTime? approveDate,
        string? approveByName,
        string? approveComment,
        Guid? updatedBy,
        string? updatedByName,
        List<ExportProductDto>? detail,
        List<ListId>? delete
        )
    {
        Id = id;
        ExportWarehouseId = exportWarehouseId;
        Code = code;
        ExportDate = exportDate;
        EmployeeId = employeeId;
        EmployeeCode = employeeCode;
        EmployeeName = employeeName;
        Status = status;
        TotalQuantity = totalQuantity;
        Note = note;
        File = file;
        ApproveBy = approveBy;
        ApproveDate = approveDate;
        ApproveByName = approveByName;
        ApproveComment = approveComment;
        UpdatedBy = updatedBy;
        UpdatedByName = updatedByName;
        Detail = detail;
        Delete = delete;
    }
    public bool IsValid(IExportRepository _context)
    {
        ValidationResult = new ExportEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ExportDeleteCommand : ExportCommand
{
    public ExportDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IExportRepository _context)
    {
        ValidationResult = new ExportDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ApprovalExportCommand : Command
{
    public Guid Id { get; set; }
    public Guid? ExportWarehouseId { get; set; }
    public string? Code { get; set; }
    public DateTime? ExportDate { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }
    public int? Status { get; set; }
    public double? TotalQuantity { get; set; }
    public string? Note { get; set; }
    public Guid? ApproveBy { get; set; }
    public DateTime? ApproveDate { get; set; }
    public string? ApproveByName { get; set; }
    public string? ApproveComment { get; set; }
    public ApprovalExportCommand(
        Guid id,
        string? code,
        DateTime? exportDate,
        Guid? employeeId,
        string? employeeCode,
        string? employeeName,
        int? status,
        double? totalQuantity,
        string? note,
        Guid? approveBy,
        DateTime? approveDate,
        string? approveByName,
        string? approveComment
    )
    {
        Id = id;
        Code = code;
        ExportDate = exportDate;
        EmployeeId = employeeId;
        EmployeeCode = employeeCode;
        EmployeeName = employeeName;
        Status = status;
        TotalQuantity = totalQuantity;
        Note = note;
        ApproveBy = approveBy;
        ApproveDate = approveDate;
        ApproveByName = approveByName;
        ApproveComment = approveComment;
    }
}
