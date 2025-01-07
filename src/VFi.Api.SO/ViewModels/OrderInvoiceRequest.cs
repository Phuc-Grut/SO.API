using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class OrderInvoiceRequest
{
    public Guid? Id { get; set; }
    public string? Serial { get; set; }
    public string? Symbol { get; set; }
    public string? Number { get; set; }
    public decimal? Value { get; set; }
    public DateTime? Date { get; set; }
    public string? Note { get; set; }
    public int? DisplayOrder { get; set; }
}
