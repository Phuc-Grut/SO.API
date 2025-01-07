using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddGroupEmployeeMappingRequest
{
    public Guid? Id { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? GroupEmployeeId { get; set; }
    public bool? IsLeader { get; set; }
}
public class EditGroupEmployeeMappingRequest : AddGroupEmployeeMappingRequest
{
    public string Id { get; set; }
}

public class DeleteGroupEmployeeMappingRequest
{
    public Guid? Id { get; set; }
}
