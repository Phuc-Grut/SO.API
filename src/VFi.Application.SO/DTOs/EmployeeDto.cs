using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class EmployeeDto
{
    public Guid Id { get; set; }
    public bool? IsCustomer { get; set; }
    public string? Code { get; set; }
    public Guid? AccountId { get; set; }
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
    /// <summary>
    /// Giới tính. 0-Nam, 1-Nữ, 2-Khác
    /// </summary>
    public int? Gender { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public string? TaxCode { get; set; }
    public string? GroupEmployee { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public bool? IsLeader { get; set; }
    public List<GroupEmployeeMappingDto>? Groups { get; set; }
    public List<CustomerAddressDto>? ListAddress { get; set; }
    public List<CustomerBankDto>? ListBank { get; set; }
    public List<CustomerContactDto>? ListContact { get; set; }
}
public class EmployeeListBoxDto
{
    public Guid Value { get; set; }
    public string? Label { get; set; }
    public string? Key { get; set; }
    public Guid? AccountId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public List<GroupEmployeeDto>? ListGroup { get; set; }
}
