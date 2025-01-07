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

public class PurchaseGroupCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public int? DisplayOrder { get; set; }
    public int? Status { get; set; }
}

public class PurchaseGroupAddCommand : PurchaseGroupCommand
{
    public PurchaseGroupAddCommand(
        Guid id,
        string? code,
        string? name,
        string? description,
        string? note,
        int? displayOrder,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Note = note;
        DisplayOrder = displayOrder;
        Status = status;
    }
    public bool IsValid(IPurchaseGroupRepository _context)
    {
        ValidationResult = new PurchaseGroupAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PurchaseGroupEditCommand : PurchaseGroupCommand
{
    public PurchaseGroupEditCommand(
       Guid id,
        string? code,
        string? name,
        string? description, string? note, int? displayOrder,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Note = note;
        DisplayOrder = displayOrder;
        Status = status;
    }
    public bool IsValid(IPurchaseGroupRepository _context)
    {
        ValidationResult = new PurchaseGroupEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PurchaseGroupDeleteCommand : PurchaseGroupCommand
{
    public PurchaseGroupDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IPurchaseGroupRepository _context)
    {
        ValidationResult = new PurchaseGroupDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class PurchaseGroupSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public PurchaseGroupSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
