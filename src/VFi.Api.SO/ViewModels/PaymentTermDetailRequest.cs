using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddPaymentTermDetailRequest
{
    public Guid? Id { get; set; }
    public Guid? PaymentTermId { get; set; }
    public int Type { get; set; }
    public decimal? Value { get; set; }
    public decimal? Duration { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}
public class EditPaymentTermDetailRequest : AddPaymentTermDetailRequest
{
    public Guid Id { get; set; }
}

public class DeletePaymentTermDetailRequest
{
    public Guid? Id { get; set; }
}
