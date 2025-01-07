using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class PaymentTermRequest : FilterQuery
{
    public int? Status { get; set; }
}

public class AddPaymentTermRequest
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Day { get; set; }
    public decimal? Value { get; set; }
    public double? Percent { get; set; }
    public int? Status { get; set; }
}
public class EditPaymentTermRequest : AddPaymentTermRequest
{
    public Guid Id { get; set; }
}
