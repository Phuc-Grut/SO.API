using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class ReasonRequest : FilterQuery
{
    public int? Status { get; set; }
}
public class AddReasonRequest
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? Status { get; set; }
}
public class EditReasonRequest : AddReasonRequest
{
    public string Id { get; set; }
}
