using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class ExportProductDto
{
    public Guid? Id { get; set; }
    public Guid? ExportId { get; set; }
    public Guid? ExportWarehouseProductId { get; set; }
    public Guid? ProductId { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public string Origin { get; set; }
    public string UnitType { get; set; }
    public string UnitCode { get; set; }
    public string UnitName { get; set; }
    public decimal? StockQuantity { get; set; }
    public double? QuantityRequest { get; set; }
    public double? Quantity { get; set; }
    public string Note { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string CreatedByName { get; set; }
    public string UpdatedByName { get; set; }
}
public class DeleteExportProductDto
{
    public Guid Id { get; set; }
}
