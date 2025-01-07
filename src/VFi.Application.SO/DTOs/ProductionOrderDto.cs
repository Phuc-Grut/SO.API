namespace VFi.Application.SO.DTOs;

public class ProductionOrderDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;
    public string? Note { get; set; }
    public int? Status { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public DateTime? RequestDate { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime? DateNeed { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderNumber { get; set; }
    public Guid? SaleEmployeeId { get; set; }
    public string? SaleEmployeeCode { get; set; }
    public string? SaleEmployeeName { get; set; }
    public int? Type { get; set; }
    public DateTime? EstimateDate { get; set; }
    public List<FileDto>? File { get; set; }
    public List<ProductionOrdersDetailDto>? ListProductionOrdersDetail { get; set; }
}
public class ProductionOrdersValidateDto
{
    public UInt32 Row { get; set; }
    public bool IsValid
    {
        get
        {
            return Errors.Count == 0;
        }
    }
    public List<string> Errors { get; set; } = new List<string>();
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public Guid? UnitId { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public string? QuantityRequest { get; set; }
    public string? Note { get; set; }
    public string? UnitType { get; set; }
}
