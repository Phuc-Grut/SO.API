using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class SurchargeGroupRequest : FilterQuery
{
    public int? Status { get; set; }
}

public class AddSurchargeGroupRequest
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public string? Note { get; set; }
    public int? DisplayOrder { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
}
public class EditSurchargeGroupRequest : AddSurchargeGroupRequest
{
    public Guid Id { get; set; }
}
