using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class GroupEmployeeRequest : FilterQuery
{
    public int? Status { get; set; }
}
public class AddGroupEmployeeRequest
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
}
public class EditGroupEmployeeRequest : AddGroupEmployeeRequest
{
    public Guid Id { get; set; }
}
