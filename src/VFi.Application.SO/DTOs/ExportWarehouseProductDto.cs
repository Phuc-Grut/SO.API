using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class ExportWarehouseProductDto
{
    public Guid? Id { get; set; }
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
    public decimal? StockQuantity { get; set; }
    public double QuantityRequest { get; set; }
    public double QuantityExported { get; set; }
    public string? Note { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? CustomerId { get; set; }
    public decimal? ExportWarehouseStatus { get; set; }
    public decimal? Status { get; set; }
}
public class DeleteExportWarehouseProductDto
{
    public Guid Id { get; set; }
}
