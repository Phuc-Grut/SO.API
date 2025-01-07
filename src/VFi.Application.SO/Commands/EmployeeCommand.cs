using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using MediatR;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class EmployeeCommand : Command
{

    public Guid Id { get; set; }
    public bool? IsCustomer { get; set; }
    public Guid AccountId { get; set; }
    public string? AccountName { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
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
    public List<GroupEmployeeMappingDto>? Group { get; set; }
    public List<CustomerAddressDto>? ListAddress { get; set; }
    public List<CustomerBankDto>? ListBank { get; set; }
    public List<CustomerContactDto>? ListContact { get; set; }
}

public class EmployeeAddCommand : EmployeeCommand
{
    public EmployeeAddCommand(
        Guid id,
        bool? isCustomer,
        string? code,
        Guid accountId,
        string? accountName,
        string? email,
        string? name,
        string? image,
        string? phone,
        string? country,
        string? province,
        string? district,
        string? ward,
        string? address,
        int? gender,
        int? year,
        int? month,
        int? day,
        string? taxCode,
        string? groupEmployee,
        string? description,
        int? status,
        List<GroupEmployeeMappingDto>? group,
        List<CustomerAddressDto>? listAddress,
        List<CustomerBankDto>? listBank,
        List<CustomerContactDto>? listContact
        )
    {
        Id = id;
        IsCustomer = isCustomer;
        Code = code;
        AccountId = accountId;
        AccountName = accountName;
        Email = email;
        Name = name;
        Image = image;
        Phone = phone;
        Country = country;
        Province = province;
        District = district;
        Ward = ward;
        Address = address;
        Gender = gender;
        Year = year;
        Month = month;
        Day = day;
        TaxCode = taxCode;
        GroupEmployee = groupEmployee;
        Description = description;
        Status = status;
        Group = group;
        ListAddress = listAddress;
        ListBank = listBank;
        ListContact = listContact;
    }
    public bool IsValid(IEmployeeRepository _context)
    {
        ValidationResult = new EmployeeAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class EmployeeEditCommand : EmployeeCommand
{
    public List<DeleteGroupEmployeeMappingDto>? Delete { get; set; }
    public EmployeeEditCommand(
        Guid id,
        bool? isCustomer,
        string? code,
        Guid accountId,
        string? accountName,
        string? email,
        string? name,
        string? image,
        string? phone,
        string? country,
        string? province,
        string? district,
        string? ward,
        string? address,
        int? gender,
        int? year,
        int? month,
        int? day,
        string? taxCode,
        string? groupEmployee,
        string? description,
        int? status,
        List<GroupEmployeeMappingDto>? group,
        List<DeleteGroupEmployeeMappingDto>? delete,
        List<CustomerAddressDto>? listAddress,
        List<CustomerBankDto>? listBank,
        List<CustomerContactDto>? listContact
        )
    {
        Id = id;
        IsCustomer = isCustomer;
        Code = code;
        AccountId = accountId;
        AccountName = accountName;
        Email = email;
        Name = name;
        Image = image;
        Phone = phone;
        Country = country;
        Province = province;
        District = district;
        Ward = ward;
        Address = address;
        Gender = gender;
        Year = year;
        Month = month;
        Day = day;
        TaxCode = taxCode;
        GroupEmployee = groupEmployee;
        Description = description;
        Status = status;
        Group = group;
        Delete = delete;
        ListAddress = listAddress;
        ListBank = listBank;
        ListContact = listContact;
    }
    public bool IsValid(IEmployeeRepository _context)
    {
        ValidationResult = new EmployeeEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class EmployeeDeleteCommand : EmployeeCommand
{
    public EmployeeDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IEmployeeRepository _context)
    {
        ValidationResult = new EmployeeDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
