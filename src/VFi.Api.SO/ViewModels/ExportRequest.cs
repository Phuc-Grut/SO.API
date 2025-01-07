using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class ExportRequest : FopPagingRequest
{
}
public class AddExportRequest
{
    public string? ExportWarehouseId { get; set; }
    public string? Code { get; set; }
    public DateTime? ExportDate { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }
    public int? Status { get; set; }
    public double? TotalQuantity { get; set; }
    public string? Note { get; set; }
    public List<FileRequest>? File { get; set; }
    public Guid? ApproveBy { get; set; }
    public DateTime? ApproveDate { get; set; }
    public string? ApproveByName { get; set; }
    public string? ApproveComment { get; set; }
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
    public List<AddExportProductRequest>? Details { get; set; }
}
public class EditExportRequest : AddExportRequest
{
    public Guid Id { get; set; }
}
public class ApprovalExportRequest
{
    public Guid Id { get; set; }
    public string? ExportWarehouseId { get; set; }
    public string? Code { get; set; }
    public DateTime? ExportDate { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }
    public int? Status { get; set; }
    public double? TotalQuantity { get; set; }
    public string Note { get; set; }
    public Guid? ApproveBy { get; set; }
    public DateTime? ApproveDate { get; set; }
    public string ApproveByName { get; set; }
    public string ApproveComment { get; set; }
}
