using FluentValidation;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class ProductionOrdersDetailCommand : Command
{
    public Guid Id { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public Guid? ProductionOrdersId { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? ProductImage { get; set; }
    public string? Sku { get; set; }
    public string? Gtin { get; set; }
    public string? Origin { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public double? Quantity { get; set; }
    public string? Note { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public double? EstimatedDeliveryQuantity { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public bool? IsWorkOrdered { get; set; }
    public string? ProductionOrdersCode { get; set; }
    public bool? IsEstimated { get; set; }
    public bool? IsBom { get; set; }
    public int? Status { get; set; }
    public int? EstimateStatus { get; set; }
    public string? Solution { get; set; }
    public string? Transport { get; set; }
    public double? Height { get; set; }
    public double? Package { get; set; }
    public double? Volume { get; set; }
    public double? Length { get; set; }
    public double? Weight { get; set; }
    public double? Width { get; set; }
    public string? CancelReason { get; set; }
    public DateTime? EstimatedDate { get; set; }
}

public class ProductionOrdersDetailEditPackageCommand : ProductionOrdersDetailCommand
{
    public ProductionOrdersDetailEditPackageCommand(
        Guid Id,
        string? Solution,
        string? Transport,
        double? Height,
        double? Package,
        double? Volume,
        double? Length,
        double? Weight,
        double? Width
        )
    {
        this.Id = Id;
        this.Solution = Solution;
        this.Transport = Transport;
        this.Height = Height;
        this.Package = Package;
        this.Volume = Volume;
        this.Length = Length;
        this.Weight = Weight;
        this.Width = Width;
    }
    public bool IsValid(IProductionOrdersDetailRepository _context)
    {
        ValidationResult = new ProductionOrdersDetailEditPackageCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ProductionOrdersDetailCancelCommand : ProductionOrdersDetailCommand
{
    public ProductionOrdersDetailCancelCommand(
        Guid Id,
        int? Status,
        string? CancelReason
        )
    {
        this.Id = Id;
        this.Status = Status;
        this.CancelReason = CancelReason;
    }
    public bool IsValidPro(IProductionOrdersDetailRepository _context)
    {
        ValidationResult = new ProductionOrdersDetailCancelCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ProductionOrdersDetailCompleteCommand : ProductionOrdersDetailCommand
{
    public ProductionOrdersDetailCompleteCommand(
        Guid Id,
        int? Status
        )
    {
        this.Id = Id;
        this.Status = Status;
    }
    public bool IsValidPro(IProductionOrdersDetailRepository _context)
    {
        ValidationResult = new ProductionOrdersDetailCompleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
