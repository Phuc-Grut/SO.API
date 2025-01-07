using VFi.Application.SO.Commands.Validations;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;
public class ExportWarehouseProductCommand : Command
{
    public Guid Id { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public Guid? OrderProductId { get; set; }
    public Guid? ExportWarehouseId { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? ProductImage { get; set; }
    public string? WarehouseCode { get; set; }
    public string? WarehouseName { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public double QuantityRequest { get; set; }
    public double QuantityExported { get; set; }
    public string? Note { get; set; }
    public Guid? CustomerId { get; set; }
    public decimal? ExportWarehouseStatus { get; set; }
    public decimal? Status { get; set; }
}

public class ExportWarehouseProductDeleteCommand : Command
{
    public Guid Id { get; set; }
    public ExportWarehouseProductDeleteCommand(Guid id)
    {
        Id = id;
    }

    public bool IsValid(IExportWarehouseProductRepository _context)
    {
        ValidationResult = new ExportWarehouseProductDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}