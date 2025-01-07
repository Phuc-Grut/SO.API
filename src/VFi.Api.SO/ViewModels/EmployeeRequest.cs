using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class EmployeeRequest
{
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
    public bool? IsCustomer { get; set; }
    public string? Code { get; set; }
    public Guid AccountId { get; set; }
    public string? AccountName { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public string? Phone { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? Address { get; set; }
    public int? Gender { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public string? TaxCode { get; set; }
    public string? GroupEmployee { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }

}

public class EmployeePagingRequest : FilterQuery
{
    public Guid? GroupId { get; set; }
}
public class AddEmployeeRequest : EmployeeRequest
{
    public List<AddGroupEmployeeMappingRequest>? Groups { get; set; }
    public List<AddCustomerAddressRequest>? ListAddress { get; set; }
    public List<AddCustomerBankRequest>? ListBank { get; set; }
    public List<AddCustomerContactRequest>? ListContact { get; set; }
}
public class EditEmployeeRequest : EmployeeRequest
{
    public Guid Id { get; set; }
    public List<EditCustomerAddressRequest>? ListAddress { get; set; }
    public List<EditCustomerBankRequest>? ListBank { get; set; }
    public List<EditCustomerContactRequest>? ListContact { get; set; }
    public List<EditGroupEmployeeMappingRequest>? Groups { get; set; }
    public List<DeleteGroupEmployeeMappingRequest>? Deletes { get; set; }
}
