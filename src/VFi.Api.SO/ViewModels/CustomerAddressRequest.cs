using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Domain;

namespace VFi.Api.SO.ViewModels;

public class CustomerAddressGetByCustomerIdRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$customerId")]
    public Guid? CustomerId { get; set; }
}
public class AddCustomerAddressRequest
{
    public Guid? CustomerId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? AccountId { get; set; }
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool? ShippingDefault { get; set; }
    public bool? BillingDefault { get; set; }
    public string? Note { get; set; }
    public int? Status { get; set; }
    public int? SortOrder { get; set; }
}
public class EditCustomerAddressRequest : AddCustomerAddressRequest
{
    public string? Id { get; set; }
}
