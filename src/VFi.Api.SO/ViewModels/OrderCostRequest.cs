using VFi.Api.SO.ViewModels;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddOrderCostRequest
{
    //public Guid? Id { get; set; }
    public Guid QuotationId { get; set; }
    public string? ExpenseId { get; set; }
    public int? Type { get; set; }
    public double? Rate { get; set; }
    public decimal? Amount { get; set; }
    public int? Status { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
public class EditOrderCostRequest : AddOrderCostRequest
{
public Guid? Id { get; set; }
}

public class DeleteOrderCostRequest
{
// public Guid? Id { get; set; }
}


