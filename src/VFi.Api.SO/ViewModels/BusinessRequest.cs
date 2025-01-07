using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class BusinessRequest : FilterQuery
{
    public int? Status { get; set; }
}

public class AddBusinessRequest
{
    public string Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? DisplayOrder { get; set; }
    public int? Status { get; set; }
}
public class EditBusinessRequest : AddBusinessRequest
{
    public string Id { get; set; }
}
