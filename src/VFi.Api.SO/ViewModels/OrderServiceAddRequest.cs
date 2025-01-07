using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddOrderServiceAddRequest
{
    public string? Id { get; set; }
    public Guid? OrderId { get; set; }
    public Guid? QuotationId { get; set; }
    public Guid? ServiceAddId { get; set; }
    public string? ServiceAddName { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public string? Calculation { get; set; }
    public int? Status { get; set; }
    public string? Note { get; set; }
    public decimal? ExchangeRate { get; set; }
    public int? DisplayOrder { get; set; }
}
public class EditOrderServiceAddRequest : AddOrderProductRequest
{
    public Guid Id { get; set; }
}

public class DeleteOrderServiceAddRequest
{
    public Guid? Id { get; set; }
}
