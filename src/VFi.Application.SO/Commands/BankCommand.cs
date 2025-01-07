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

public class BankCommand : Command
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string? Qrbin { get; set; }
    public string? ShortName { get; set; }
    public string? Name { get; set; }
    public string? EnglishName { get; set; }
    public string? Address { get; set; }
    public int DisplayOrder { get; set; }
    public int Status { get; set; }
    public string? Note { get; set; }
    public string? Image { get; set; }
}

public class BankAddCommand : BankCommand
{
    public BankAddCommand(
        Guid id,
        string code,
        string? qrbin,
        string? shortName,
        string? name,
        string? englishName,
        string? address,
        int displayOrder,
        int status,
        string note,
        string image)
    {
        Id = id;
        Code = code;
        Qrbin = qrbin;
        ShortName = shortName;
        Name = name;
        EnglishName = englishName;
        Address = address;
        DisplayOrder = displayOrder;
        Status = status;
        Note = note;
        Image = image;
    }
    public bool IsValid(IBankRepository _context)
    {
        ValidationResult = new BankAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class BankEditCommand : BankCommand
{

    public BankEditCommand(
        Guid id,
        string code,
        string? qrbin,
        string? shortName,
        string? name,
        string? englishName,
        string? address,
        int displayOrder,
        int status,
        string note,
        string image)
    {
        Id = id;
        Code = code;
        Qrbin = qrbin;
        ShortName = shortName;
        Name = name;
        EnglishName = englishName;
        Address = address;
        DisplayOrder = displayOrder;
        Status = status;
        Note = note;
        Image = image;
    }
    public bool IsValid(IBankRepository _context)
    {
        ValidationResult = new BankEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class BankDeleteCommand : BankCommand
{
    public BankDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IBankRepository _context)
    {
        ValidationResult = new BankDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class BankSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public BankSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
