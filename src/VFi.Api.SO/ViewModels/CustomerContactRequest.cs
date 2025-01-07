using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Domain;

namespace VFi.Api.SO.ViewModels;

public class CustomerContactGetByCustomerIdRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$customerId")]
    public Guid? CustomerId { get; set; }
}
public class AddCustomerContactRequest
{
    public Guid? CustomerId { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? Name { get; set; }
    public int? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Facebook { get; set; }
    public string? JobTitle { get; set; }
    public string? Tags { get; set; }
    public string? Address { get; set; }
    public int? Status { get; set; }
    public int? SortOrder { get; set; }
    public string? Note { get; set; }
}
public class EditCustomerContactRequest : AddCustomerContactRequest
{
    public string? Id { get; set; }
}
