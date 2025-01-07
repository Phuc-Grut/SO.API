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

public class BusinessCommand : Command
{

    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? DisplayOrder { get; set; }
    public int? Status { get; set; }
}

public class BusinessAddCommand : BusinessCommand
{

    public BusinessAddCommand(
        Guid id,
        string code,
        string? name,
        string? description,
        int? displayOrder,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        DisplayOrder = displayOrder;
        Status = status;
    }
    public bool IsValid(IBusinessRepository _context)
    {
        ValidationResult = new BusinessAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class BusinessEditCommand : BusinessCommand
{

    public BusinessEditCommand(
        Guid id,
        string code,
        string? name,
        string? description,
        int? displayOrder,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        DisplayOrder = displayOrder;
        Status = status;
    }
    public bool IsValid(IBusinessRepository _context)
    {
        ValidationResult = new BusinessEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class BusinessDeleteCommand : BusinessCommand
{
    public BusinessDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IBusinessRepository _context)
    {
        ValidationResult = new BusinessDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class BusinessSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public BusinessSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
