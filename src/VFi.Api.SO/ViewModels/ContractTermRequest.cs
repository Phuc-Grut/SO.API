using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class ContractTermRequest : FilterQuery
{
    public int? Status { get; set; }
}
public class AddContractTermRequest
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
}
public class EditContractTermRequest : AddContractTermRequest
{
    public string Id { get; set; }
}
