namespace VFi.Api.SO.ViewModels;

public class SalesChannelRequest : FilterQuery
{
    public int? Status { get; set; }
}

public class AddSalesChannelRequest
{
    public string Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public bool? IsDefault { get; set; }
    public int DisplayOrder { get; set; }
}
public class EditSalesChannelRequest : AddSalesChannelRequest
{
}
