using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddExportProductRequest
{
    public Guid? Id { get; set; }
    public Guid? ExportId { get; set; }
    public Guid? ExportWarehouseProductId { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? ProductImage { get; set; }
    public string? Origin { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public double? Quantity { get; set; }
    public string? Note { get; set; }
    public int? DisplayOrder { get; set; }

}
public class EditExportProductRequest : AddExportProductRequest
{
    public Guid Id { get; set; }
}

public class DeleteRequest
{
    public Guid? Id { get; set; }
}
