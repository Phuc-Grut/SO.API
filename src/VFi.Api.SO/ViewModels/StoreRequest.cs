using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class StoreRequest : FilterQuery
{
    public int? Status { get; set; }
}
public class AddStoreRequest
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public int DisplayOrder { get; set; }
    public int? Status { get; set; }
}
public class EditStoreRequest : AddStoreRequest
{
    public string Id { get; set; }
}
public class SetupPriceListRequest
{
    public Guid Id { get; set; }
    public List<AddStorePriceListRequest>? StorePriceList { get; set; }
}
