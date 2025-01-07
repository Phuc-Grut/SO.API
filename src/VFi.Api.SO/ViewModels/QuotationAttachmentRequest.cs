using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddQuotationAttachment
{
    public Guid Id { get; set; }
    public Guid QuotationId { get; set; }
    public string? Name { get; set; }
    public string? Path { get; set; }
    public string? AttachType { get; set; }
    public int? DisplayOrder { get; set; }
}


public class DeleteQuotationAttachment
{
    public Guid? Id { get; set; }
}
