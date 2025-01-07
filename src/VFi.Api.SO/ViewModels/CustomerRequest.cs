using System.Diagnostics.Contracts;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Domain;

namespace VFi.Api.SO.ViewModels;

public class AddCustomerRequest
{
    public Guid? CustomerSourceId { get; set; }
    public string? Image { get; set; }
    public int? Type { get; set; }
    public string? Code { get; set; }
    public string? Alias { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Fax { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? ZipCode { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public string? TaxCode { get; set; }
    public string? BusinessSector { get; set; }
    public string? CompanyPhone { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanySize { get; set; }
    public decimal? Capital { get; set; }
    public DateTime? EstablishedDate { get; set; }
    public string? Tags { get; set; }
    public string? Note { get; set; }
    public int? Status { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? GroupEmployeeId { get; set; }
    public bool? IsVendor { get; set; }
    public bool? IsAuto { get; set; }
    public int? Gender { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public Guid? PriceListId { get; set; }
    public string? PriceListName { get; set; }
    public decimal? DebtLimit { get; set; }
    public string? CustomerGroup { get; set; }
    public string? Representative { get; set; }
    public decimal? Revenue { get; set; }
    public string? ModuleCode { get; set; }
    public string? IdName { get; set; }
    public string? IdNumber { get; set; }
    public DateTime? IdDate { get; set; }
    public string? IdIssuer { get; set; }
    public string? IdImage1 { get; set; }
    public string? IdImage2 { get; set; }
    public int? IdStatus { get; set; }
    public string? CccdNumber { get; set; }
    public DateTime? DateRange { get; set; }
    public DateTime? Birthday { get; set; }
    public string? IssuedBy { get; set; }
    public Guid? LeadId { get; set; }
    public List<EditCustomerAddressRequest>? ListAddress { get; set; }
    public List<EditCustomerContactRequest>? ListContact { get; set; }
    public List<EditCustomerBankRequest>? ListBank { get; set; }
    //extend 
    public Guid? AccountId { get; set; }
    public string? AccountEmail { get; set; }
    public bool? AccountEmailVerified { get; set; }
    public string? AccountUsername { get; set; }
    public DateTime? AccountCreatedDate { get; set; }
    public string? AccountPhone { get; set; }
    public bool? AccountPhoneVerified { get; set; }
    public string? Tenant { get; set; }
}
public class EditCustomerRequest : AddCustomerRequest
{
    public string Id { get; set; }
}

public class CustomerPagingRequest : FilterQuery
{
    [FromQuery(Name = "$type")]
    public int? Type { get; set; }
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$customerGroupId")]
    public string? CustomerGroupId { get; set; }
    [FromQuery(Name = "$employeeId")]
    public string? EmployeeId { get; set; }
    [FromQuery(Name = "$groupEmployeeId")]
    public string? GroupEmployeeId { get; set; }
    public CustomerParams ToBaseQuery() => new CustomerParams
    {
        Type = Type,
        Status = Status,
        CustomerGroupId = CustomerGroupId,
        EmployeeId = EmployeeId,
        GroupEmployeeId = GroupEmployeeId
    };
}

public class CustomerGetByCodeRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$customerCode")]
    public string? CustomerCode { get; set; }
}
public class UpdateFinanceCustomerRequest
{
    public Guid Id { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public Guid? PriceListId { get; set; }
    public string? PriceListName { get; set; }
    public decimal? DebtLimit { get; set; }
}
public class UpdateAccountCustomerRequest
{
    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountEmail { get; set; }
    public bool? AccountEmailVerified { get; set; }
    public string? AccountUsername { get; set; }
    public DateTime? AccountCreatedDate { get; set; }
    public string? AccountPhone { get; set; }
    public bool? AccountPhoneVerified { get; set; }
}
public class ImportCustomerRequest
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int? IsVendor { get; set; }
    public int? Type { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public int? Gender { get; set; }

    public string? Phone { get; set; }
    public string? Email { get; set; }

    public string? TaxCode { get; set; }
    public string? ZipCode { get; set; }
    public string? Fax { get; set; }
    public string? Website { get; set; }
    public string? BusinessSector { get; set; }
    public string? BusinessSectorId { get; set; }
    public string? CompanySize { get; set; }
    public decimal? Capital { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? Address { get; set; }
    public string? IdNumber { get; set; }
    public DateTime? IdDate { get; set; }
    public string? IdIssuer { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? CustomerGroup { get; set; }
    public string? CustomerGroupId { get; set; }
    public Guid? SalesGroupId { get; set; }
    public string? SalesGroup { get; set; }
    public string? CustomerSource { get; set; }
    public string? GroupEmployeeName { get; set; }
    public Guid? GroupEmployeeId { get; set; }
    public Guid? CustomerSourceId { get; set; }
    public string? Note { get; set; }
    public int? Status { get; set; }
    public List<CustomerGroupMapping>? CustomerGroupMapping { get; set; }
}
