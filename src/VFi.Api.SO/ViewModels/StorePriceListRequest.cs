namespace VFi.Api.SO.ViewModels;

public class AddStorePriceListRequest
{
    public Guid? Id { get; set; }
    public Guid? StoreId { get; set; }
    public Guid? PriceListId { get; set; }
    public string? PriceListName { get; set; }
    public bool? Default { get; set; }
    public int? DisplayOrder { get; set; }
}
public class EditStorePriceListRequest : AddStorePriceListRequest
{
}
