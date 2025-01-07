using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Domain;

namespace VFi.Api.SO.ViewModels;

public class CustomerBankGetByCustomerIdRequest
{
    [FromQuery(Name = "status")]
    public int? Status { get; set; }
    [FromQuery(Name = "customerId")]
    public Guid? CustomerId { get; set; }
}
public class AddCustomerBankRequest
{
    public Guid? CustomerId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? AccountId { get; set; }
    public string? Name { get; set; }
    public string? BankCode { get; set; }
    public string? BankName { get; set; }
    public string? BankBranch { get; set; }
    public string? AccountName { get; set; }
    public string? AccountNumber { get; set; }
    public bool? Default { get; set; }
    public int? Status { get; set; }
    public int? SortOrder { get; set; }
}
public class EditCustomerBankRequest : AddCustomerBankRequest
{
    public Guid? Id { get; set; }
}
