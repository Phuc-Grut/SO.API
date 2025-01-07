using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class ShippingMethodRequest : FilterQuery
{
    public int? Status { get; set; }
}
public class AddShippingMethodRequest
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
}
public class EditShippingMethodRequest : AddShippingMethodRequest
{
    public string Id { get; set; }
}
