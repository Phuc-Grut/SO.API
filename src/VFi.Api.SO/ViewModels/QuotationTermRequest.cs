using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class QuotationTermRequest : FilterQuery
{
    public int? Status { get; set; }
}

public class AddQuotationTermRequest
{
    public string Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? DisplayOrder { get; set; }
    public int? Status { get; set; }
}
public class EditQuotationTermRequest : AddQuotationTermRequest
{
    public string Id { get; set; }
}
