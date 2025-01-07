using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddQuotationDetailRequest
{
    //public Guid? Id { get; set; }
    public Guid QuotationId { get; set; }
    public string? ReferenceId { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? UnitId { get; set; }
    public string? OriginId { get; set; }
    public string? RootUnitId { get; set; }
    public double? Quantity { get; set; }
    public double? ExchangeQuantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public double? DiscountRate { get; set; }
    public decimal? DiscountValue { get; set; }
    public decimal? AmountNoVat { get; set; }
    public double? Vatrate { get; set; }
    public decimal? Vatvalue { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public int? SortOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
public class EditQuotationDetailRequest : AddQuotationDetailRequest
{
    public Guid? Id { get; set; }
}

public class DeleteQuotationDetailRequest
{
    public Guid? Id { get; set; }
}
