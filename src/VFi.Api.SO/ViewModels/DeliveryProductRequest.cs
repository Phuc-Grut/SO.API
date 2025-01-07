using VFi.Application.SO.DTOs;

namespace VFi.Api.SO.ViewModels;

public class DeliveryProductRequest : FilterQuery
{
}
public class AddDeliveryProductRequest
{
    public Guid OrderProductId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public double? QuantityExpected { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int? DisplayOrder { get; set; }
}
public class EditDeliveryProductRequest : AddDeliveryProductRequest
{
    public Guid Id { get; set; }
}
public class AddRangeDeliveryProductRequest
{
    public List<DeliveryProductDto>? List { get; set; }
    public string ListGuidDeliveryProduct { get; set; } = null!;
}
