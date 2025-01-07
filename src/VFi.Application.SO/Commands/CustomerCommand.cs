using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class CustomerCommand : Command
{
    public Guid Id { get; set; }
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
    public Guid? AccountId { get; set; }
    public string? AccountEmail { get; set; }
    public bool? AccountEmailVerified { get; set; }
    public string? AccountUsername { get; set; }
    public DateTime? AccountCreatedDate { get; set; }
    public string? AccountPhone { get; set; }
    public bool? AccountPhoneVerified { get; set; }
    public string? Tenant { get; set; }
    public string? Representative { get; set; }
    public decimal? Revenue { get; set; }
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

    public List<CustomerAddressDto>? ListAddress { get; set; }
    public List<CustomerContactDto>? ListContact { get; set; }
    public List<CustomerBankDto>? ListBank { get; set; }
}

public class CustomerAddCommand : CustomerCommand
{
    public CustomerAddCommand()
    {
    }

    public CustomerAddCommand(
        Guid id,
        Guid? customerSourceId,
        string? image,
        int? type,
        string? code,
        string? alias,
        string? name,
        string? phone,
        string? email,
        string? fax,
        string? country,
        string? province,
        string? district,
        string? ward,
        string? zipCode,
        string? address,
        string? website,
        string? taxCode,
        string? businessSector,
        string? companyPhone,
        string? companyName,
        string? companySize,
        decimal? capital,
        DateTime? establishedDate,
        string? tags,
        string? note,
        int? status,
        Guid? employeeId,
        Guid? groupEmployeeId,
        bool? isVendor,
        bool? isAuto,
        int? gender,
        int? year,
        int? month,
        int? day,
        string? customerGroup,
        string? representative,
        decimal? revenue,
        string? idName,
        string? idNumber,
        DateTime? idDate,
        string? idIssuer,
        string? idImage1,
        string? idImage2,
        int? idStatus,
        string? cccdNumber,
        DateTime? dateRange,
        DateTime? birthday,
        string? issuedBy,
        Guid? leadId,
        List<CustomerAddressDto>? listAddress,
        List<CustomerContactDto>? listContact,
        List<CustomerBankDto>? listBank
        )
    {
        Id = id;
        CustomerSourceId = customerSourceId;
        Image = image;
        Type = type;
        Code = code;
        Alias = alias;
        Name = name;
        Phone = phone;
        Email = email;
        Fax = fax;
        Country = country;
        Province = province;
        District = district;
        Ward = ward;
        ZipCode = zipCode;
        Address = address;
        Website = website;
        TaxCode = taxCode;
        BusinessSector = businessSector;
        CompanyName = companyName;
        CompanyPhone = companyPhone;
        CompanySize = companySize;
        Capital = capital;
        EstablishedDate = establishedDate;
        Tags = tags;
        Note = note;
        Status = status;
        EmployeeId = employeeId;
        GroupEmployeeId = groupEmployeeId;
        IsVendor = isVendor;
        IsAuto = isAuto;
        Gender = gender;
        Year = year;
        Month = month;
        Day = day;
        CustomerGroup = customerGroup;
        Representative = representative;
        Revenue = revenue;
        IdName = idName;
        IdNumber = idNumber;
        IdDate = idDate;
        IdIssuer = idIssuer;
        IdImage1 = idImage1;
        IdImage2 = idImage2;
        IdStatus = idStatus;
        CccdNumber = cccdNumber;
        DateRange = dateRange;
        Birthday = birthday;
        IssuedBy = issuedBy;
        LeadId = leadId;
        ListAddress = listAddress;
        ListContact = listContact;
        ListBank = listBank;
    }
    public bool IsValid(ICustomerRepository _context)
    {
        ValidationResult = new CustomerAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerEditCommand : CustomerCommand
{
    public CustomerEditCommand(
        Guid id,
        Guid? customerSourceId,
        string? image,
        int? type,
        string? code,
        string? alias,
        string? name,
        string? phone,
        string? email,
        string? fax,
        string? country,
        string? province,
        string? district,
        string? ward,
        string? zipCode,
        string? address,
        string? website,
        string? taxCode,
        string? businessSector,
        string? companyPhone,
        string? companyName,
        string? companySize,
        decimal? capital,
        DateTime? establishedDate,
        string? tags,
        string? note,
        int? status,
        Guid? employeeId,
        Guid? groupEmployeeId,
        bool? isVendor,
        bool? isAuto,
        int? gender,
        int? year,
        int? month,
        int? day,
        Guid? currencyId,
        string? currency,
        string? currencyName,
        Guid? priceListId,
        string? priceListName,
        decimal? debtLimit,
        string? customerGroup,
        string? representative,
        decimal? revenue,
        string? idName,
        string? idNumber,
        DateTime? idDate,
        string? idIssuer,
        string? idImage1,
        string? idImage2,
        int? idStatus,
        string? cccdNumber,
        DateTime? dateRange,
        DateTime? birthday,
        string? issuedBy,
        Guid? accountId,
        string? accountEmail,
        bool? accountEmailVerified,
        string? accountUsername,
        DateTime? accountCreatedDate,
        string? accountPhone,
        bool? accountPhoneVerified,
        List<CustomerAddressDto>? listAddress,
        List<CustomerContactDto>? listContact,
        List<CustomerBankDto>? listBank
        )
    {
        Id = id;
        CustomerSourceId = customerSourceId;
        Image = image;
        Type = type;
        Code = code;
        Alias = alias;
        Name = name;
        Phone = phone;
        Email = email;
        Fax = fax;
        Country = country;
        Province = province;
        District = district;
        Ward = ward;
        ZipCode = zipCode;
        Address = address;
        Website = website;
        TaxCode = taxCode;
        BusinessSector = businessSector;
        CompanyName = companyName;
        CompanyPhone = companyPhone;
        CompanySize = companySize;
        Capital = capital;
        EstablishedDate = establishedDate;
        Tags = tags;
        Note = note;
        Status = status;
        EmployeeId = employeeId;
        GroupEmployeeId = groupEmployeeId;
        IsVendor = isVendor;
        IsAuto = isAuto;
        Gender = gender;
        Year = year;
        Month = month;
        Day = day;
        CurrencyId = currencyId;
        Currency = currency;
        CurrencyName = currencyName;
        PriceListId = priceListId;
        PriceListName = priceListName;
        DebtLimit = debtLimit;
        CustomerGroup = customerGroup;
        Representative = representative;
        Revenue = revenue;
        IdName = idName;
        IdNumber = idNumber;
        IdDate = idDate;
        IdIssuer = idIssuer;
        IdImage1 = idImage1;
        IdImage2 = idImage2;
        IdStatus = idStatus;
        CccdNumber = cccdNumber;
        DateRange = dateRange;
        Birthday = birthday;
        IssuedBy = issuedBy;
        AccountId = accountId;
        AccountEmail = accountEmail;
        AccountEmailVerified = accountEmailVerified;
        AccountUsername = accountUsername;
        AccountCreatedDate = accountCreatedDate;
        AccountPhone = accountPhone;
        AccountPhoneVerified = accountPhoneVerified;
        ListAddress = listAddress;
        ListContact = listContact;
        ListBank = listBank;
    }
    public bool IsValid(ICustomerRepository _context)
    {
        ValidationResult = new CustomerEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerDeleteCommand : Command
{
    public Guid Id { get; set; }
    public CustomerDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ICustomerRepository _context)
    {
        ValidationResult = new CustomerDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerUpdateAccountCommand : CustomerCommand
{
    public CustomerUpdateAccountCommand(
        Guid id,
        Guid? accountId,
        string? accountEmail,
        bool? accountEmailVerified,
        string? accountUsername,
        DateTime? accountCreatedDate,
        string? accountPhone,
        bool? accountPhoneVerified
        )
    {
        Id = id;
        AccountId = accountId;
        AccountEmail = accountEmail;
        AccountEmailVerified = accountEmailVerified;
        AccountUsername = accountUsername;
        AccountCreatedDate = accountCreatedDate;
        AccountPhone = accountPhone;
        AccountPhoneVerified = accountPhoneVerified;
    }
    public bool IsValid(ICustomerRepository _context)
    {
        ValidationResult = new CustomermenageAccountCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerImageEditCommand : Command
{
    public Guid Id { get; set; }
    public string? Image { get; set; }
    public CustomerImageEditCommand()
    {

    }
    public bool IsValid(ICustomerRepository _context)
    {
        ValidationResult = new CustomerImageEditCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}


public class CustomerUpdateFinanceCommand : Command
{
    public Guid Id { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public Guid? PriceListId { get; set; }
    public string? PriceListName { get; set; }
    public decimal? DebtLimit { get; set; }
    public CustomerUpdateFinanceCommand(
        Guid id,
        Guid? currencyId,
        string? currency,
        string? currencyName,
        Guid? priceListId,
        string? priceListName,
        decimal? debtLimit
        )
    {
        Id = id;
        CurrencyId = currencyId;
        Currency = currency;
        CurrencyName = currencyName;
        PriceListId = priceListId;
        PriceListName = priceListName;
        DebtLimit = debtLimit;
    }
    public bool IsValid(ICustomerRepository _context)
    {
        ValidationResult = new CustomerUpdateFinanceValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ValidateImportCustomer
{
    public IFormFile File { get; set; } = null!;
    public string SheetId { get; set; } = null!;
    public int HeaderRow { get; set; }
    public int? Code { get; set; }
    public int? Name { get; set; }
    public int? IsVendor { get; set; }
    public int? Type { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public int? Gender { get; set; }
    public int? Phone { get; set; }
    public int? Email { get; set; }
    public int? TaxCode { get; set; }
    public int? ZipCode { get; set; }
    public int? Fax { get; set; }
    public int? Website { get; set; }
    public int? BusinessSector { get; set; }
    public int? CompanySize { get; set; }
    public int? Capital { get; set; }
    public int? Country { get; set; }
    public int? Province { get; set; }
    public int? District { get; set; }
    public int? Ward { get; set; }
    public int? Address { get; set; }
    public int? IdNumber { get; set; }
    public int? IdDate { get; set; }
    public int? IdIssuer { get; set; }
    public int? EmployeeName { get; set; }
    public int? CustomerGroup { get; set; }
    public int? SalesGroup { get; set; }
    public int? CustomerSource { get; set; }
    public int? GroupEmployeeName { get; set; }
    public int? Note { get; set; }
}

public class ImportExcelCustomerCommand : Command
{
    public ImportExcelCustomerCommand(IEnumerable<CustomerImportDto> customerImportDtos)
    {
        this.customerImportDtos = customerImportDtos;
    }

    public IEnumerable<CustomerImportDto> customerImportDtos { get; set; }

}
