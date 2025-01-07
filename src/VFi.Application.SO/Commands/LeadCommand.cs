using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class LeadCommand : Command
{
    public Guid Id { get; set; }

    public string? Source { get; set; }

    public string? Code { get; set; }

    public string? Image { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Country { get; set; }

    public string? Province { get; set; }

    public string? District { get; set; }

    public string? Ward { get; set; }

    public string? ZipCode { get; set; }

    public string? Address { get; set; }

    public string? Website { get; set; }

    public string? TaxCode { get; set; }

    public string? BusinessSector { get; set; }

    public string? Company { get; set; }

    public string? CompanyPhone { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanySize { get; set; }

    public decimal? Capital { get; set; }

    public DateTime? EstablishedDate { get; set; }

    public string? Tags { get; set; }

    public string? Note { get; set; }

    public int? Status { get; set; }

    public Guid? GroupId { get; set; }

    public string Group { get; set; }

    public Guid? EmployeeId { get; set; }

    public string? Employee { get; set; }

    public Guid? GroupEmployeeId { get; set; }

    public string GroupEmployee { get; set; }

    public int? Gender { get; set; }

    public int? Year { get; set; }

    public int? Month { get; set; }

    public int? Day { get; set; }

    public string? Facebook { get; set; }

    public string? Zalo { get; set; }

    public decimal? RevenueTarget { get; set; }

    public int? Revenue { get; set; }

    public string? Scale { get; set; }

    public int? Difficult { get; set; }

    public int? Point { get; set; }

    public int? Priority { get; set; }

    public string? Demand { get; set; }

    public string? DynamicData { get; set; }

    public int? Converted { get; set; }

    public string? CustomerCode { get; set; }

    public string? File { get; set; }
}

public class LeadAddCommand : LeadCommand
{

    public LeadAddCommand(
        Guid id,
        string? source,
        string? code,
        string? image,
        string? name,
        string? email,
        string? phone,
        string? country,
        string? province,
        string? district,
        string? ward,
        string? zipCode,
        string? address,
        string? website,
        string? taxCode,
        string? businessSector,
        string? company,
        string? companyPhone,
        string? companyName,
        string? companySize,
        decimal? capital,
        DateTime? establishedDate,
        string? tags,
        string? note,
        int? status,
        Guid? groupId,
        string group,
        Guid? employeeId,
        string employee,
        Guid? groupEmployeeId,
        string groupEmployee,
        int? gender,
        int? year,
        int? month,
        int? day,
        string? facebook,
        string? zalo,
        decimal? revenueTarget,
        int? revenue,
        string? scale,
        int? difficult,
        int? point,
        int? priority,
        string? demand,
        string? dynamicData,
        int? converted
        )
    {
        Id = id;
        Source = source;
        Code = code;
        Image = image;
        Name = name;
        Email = email;
        Phone = phone;
        Country = country;
        Province = province;
        District = district;
        Ward = ward;
        ZipCode = zipCode;
        Address = address;
        Website = website;
        TaxCode = taxCode;
        BusinessSector = businessSector;
        Company = company;
        CompanyPhone = companyPhone;
        CompanyName = companyName;
        CompanySize = companySize;
        Capital = capital;
        EstablishedDate = establishedDate;
        Tags = tags;
        Note = note;
        Status = status;
        GroupId = groupId;
        Group = group;
        EmployeeId = employeeId;
        Employee = employee;
        GroupEmployeeId = groupEmployeeId;
        GroupEmployee = groupEmployee;
        Gender = gender;
        Year = year;
        Month = month;
        Day = day;
        Facebook = facebook;
        Zalo = zalo;
        RevenueTarget = revenueTarget;
        Revenue = revenue;
        Scale = scale;
        Difficult = difficult;
        Point = point;
        Priority = priority;
        Demand = demand;
        DynamicData = dynamicData;
        Converted = converted;
    }
    public bool IsValid(ILeadRepository _context)
    {
        ValidationResult = new LeadAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class LeadEditCommand : LeadCommand
{

    public LeadEditCommand(
        Guid id,
        string? source,
        string? code,
        string? image,
        string? name,
        string? email,
        string? phone,
        string? country,
        string? province,
        string? district,
        string? ward,
        string? zipCode,
        string? address,
        string? website,
        string? taxCode,
        string? businessSector,
        string? company,
        string? companyPhone,
        string? companyName,
        string? companySize,
        decimal? capital,
        DateTime? establishedDate,
        string? tags,
        string? note,
        int? status,
        Guid? groupId,
        string group,
        Guid? employeeId,
        string employee,
        Guid? groupEmployeeId,
        string groupEmployee,
        int? gender,
        int? year,
        int? month,
        int? day,
        string? facebook,
        string? zalo,
        decimal? revenueTarget,
        int? revenue,
        string? scale,
        int? difficult,
        int? point,
        int? priority,
        string? demand,
        string? dynamicData,
        int? converted,
        string customerCode
        )
    {
        Id = id;
        Source = source;
        Code = code;
        Image = image;
        Name = name;
        Email = email;
        Phone = phone;
        Country = country;
        Province = province;
        District = district;
        Ward = ward;
        ZipCode = zipCode;
        Address = address;
        Website = website;
        TaxCode = taxCode;
        BusinessSector = businessSector;
        Company = company;
        CompanyPhone = companyPhone;
        CompanyName = companyName;
        CompanySize = companySize;
        Capital = capital;
        EstablishedDate = establishedDate;
        Tags = tags;
        Note = note;
        Status = status;
        GroupId = groupId;
        Group = group;
        EmployeeId = employeeId;
        Employee = employee;
        GroupEmployeeId = groupEmployeeId;
        GroupEmployee = groupEmployee;
        Gender = gender;
        Year = year;
        Month = month;
        Day = day;
        Facebook = facebook;
        Zalo = zalo;
        RevenueTarget = revenueTarget;
        Revenue = revenue;
        Scale = scale;
        Difficult = difficult;
        Point = point;
        Priority = priority;
        Demand = demand;
        DynamicData = dynamicData;
        Converted = converted;
        CustomerCode = customerCode;
    }
    public bool IsValid(ILeadRepository _context)
    {
        ValidationResult = new LeadEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ConvertLeadCommand : LeadCommand
{

    public ConvertLeadCommand(
        Guid id,
        string? source,
        string? code,
        string? image,
        string? name,
        string? email,
        string? phone,
        string? country,
        string? province,
        string? district,
        string? ward,
        string? zipCode,
        string? address,
        string? website,
        string? taxCode,
        string? businessSector,
        string? company,
        string? companyPhone,
        string? companyName,
        string? companySize,
        decimal? capital,
        DateTime? establishedDate,
        string? tags,
        string? note,
        int? status,
        Guid? groupId,
        string group,
        Guid? employeeId,
        string employee,
        Guid? groupEmployeeId,
        string groupEmployee,
        int? gender,
        int? year,
        int? month,
        int? day,
        string? facebook,
        string? zalo,
        decimal? revenueTarget,
        int? revenue,
        string? scale,
        int? difficult,
        int? point,
        int? priority,
        string? demand,
        string? dynamicData,
        int? converted,
        string customerCode
        )
    {
        Id = id;
        Source = source;
        Code = code;
        Image = image;
        Name = name;
        Email = email;
        Phone = phone;
        Country = country;
        Province = province;
        District = district;
        Ward = ward;
        ZipCode = zipCode;
        Address = address;
        Website = website;
        TaxCode = taxCode;
        BusinessSector = businessSector;
        Company = company;
        CompanyPhone = companyPhone;
        CompanyName = companyName;
        CompanySize = companySize;
        Capital = capital;
        EstablishedDate = establishedDate;
        Tags = tags;
        Note = note;
        Status = status;
        GroupId = groupId;
        Group = group;
        EmployeeId = employeeId;
        Employee = employee;
        GroupEmployeeId = groupEmployeeId;
        GroupEmployee = groupEmployee;
        Gender = gender;
        Year = year;
        Month = month;
        Day = day;
        Facebook = facebook;
        Zalo = zalo;
        RevenueTarget = revenueTarget;
        Revenue = revenue;
        Scale = scale;
        Difficult = difficult;
        Point = point;
        Priority = priority;
        Demand = demand;
        DynamicData = dynamicData;
        Converted = converted;
        CustomerCode = customerCode;
    }
    public bool IsValid(ILeadRepository _context)
    {
        ValidationResult = new ConvertLeadCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class LeadDeleteCommand : LeadCommand
{
    public LeadDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ILeadRepository _context)
    {
        ValidationResult = new LeadDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class LeadEmailNotifyCommand : Command
{
    public string SenderCode { get; set; }
    public string SenderName { get; set; }
    public string Subject { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string CC { get; set; }
    public string BCC { get; set; }
    public string Body { get; set; }
    public string TemplateCode { get; set; }

    public LeadEmailNotifyCommand(string senderCode,
        string senderName,
        string subject,
        string from,
        string to,
        string cC,
        string bCC,
        string body,
        string templateCode)
    {
        SenderCode = senderCode;
        SenderName = senderName;
        Subject = subject;
        From = from;
        To = to;
        CC = cC;
        BCC = bCC;
        Body = body;
        TemplateCode = templateCode;
    }
}
