namespace VFi.Api.SO.ViewModels;

public class ContractTypeRequest : FilterQuery
{
    public int? Status { get; set; }
}
public class AddContractTypeRequest
{
    public string Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
}
public class EditContractTypeRequest : AddContractTypeRequest
{
}
