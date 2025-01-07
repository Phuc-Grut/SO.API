using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class ShippingCarrierRequest : FilterQuery
{
    public string? Country { get; set; }
    public int? Status { get; set; }
}
public class AddShippingCarrierRequest
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
}
public class EditShippingCarrierRequest : AddShippingCarrierRequest
{
    public string Id { get; set; }
}
