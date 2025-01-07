namespace VFi.Api.SO.ViewModels;

public class DeliveryMethodRequest : FilterQuery
{
    public int? Status { get; set; }
}
public class AddDeliveryMethodRequest
{
    public string Code { get; set; } = null!;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
}
public class EditDeliveryMethodRequest : AddDeliveryMethodRequest
{
}
