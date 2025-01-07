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

public class PostOfficeCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? ShortName { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public string? Address1 { get; set; }
    public string? PostCode { get; set; }
    public string? Phone { get; set; }
    public string? SyntaxSender { get; set; }
    public string? Note { get; set; }
    public int? Status { get; set; }
}

public class PostOfficeAddCommand : PostOfficeCommand
{
    public PostOfficeAddCommand(
        Guid id,
        string code,
        string? name,
        string? shortName,
        string? cuntry,
        string? address,
        string? address1,
        string? postCode,
        string? phone,
        string? syntaxSender,
        string? note,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        ShortName = shortName;
        Country = cuntry;
        Address = address;
        Address1 = address1;
        PostCode = postCode;
        Phone = phone;
        SyntaxSender = syntaxSender;
        Note = note;
        Status = status;
    }

    public bool IsValid(IPostOfficeRepository _context)
    {
        ValidationResult = new PostOfficeAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PostOfficeEditCommand : PostOfficeCommand
{
    public PostOfficeEditCommand(
        Guid id,
        string code,
        string? name,
        string? shortName,
        string? cuntry,
        string? address,
        string? address1,
        string? postCode,
        string? phone,
        string? syntaxSender,
        string? note,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        ShortName = shortName;
        Country = cuntry;
        Address = address;
        Address1 = address1;
        PostCode = postCode;
        Phone = phone;
        SyntaxSender = syntaxSender;
        Note = note;
        Status = status;
    }

    public bool IsValid(IPostOfficeRepository _context)
    {
        ValidationResult = new PostOfficeEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PostOfficeDeleteCommand : PostOfficeCommand
{
    public PostOfficeDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IPostOfficeRepository _context)
    {
        ValidationResult = new PostOfficeDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class PostOfficeSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public PostOfficeSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
