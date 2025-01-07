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

public class CommodityGroupCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string Note { get; set; }
    public int? DisplayOrder { get; set; }
    public int? Status { get; set; }
}

public class CommodityGroupAddCommand : CommodityGroupCommand
{
    public CommodityGroupAddCommand(
        Guid id,
        string? code,
        string? name,
        string? description, string note, int? displayOrder,
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
    public bool IsValid(ICommodityGroupRepository _context)
    {
        ValidationResult = new CommodityGroupAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CommodityGroupEditCommand : CommodityGroupCommand
{
    public CommodityGroupEditCommand(
       Guid id,
        string? code,
        string? name,
        string? description, string note, int? displayOrder,
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
    public bool IsValid(ICommodityGroupRepository _context)
    {
        ValidationResult = new CommodityGroupEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CommodityGroupDeleteCommand : CommodityGroupCommand
{
    public CommodityGroupDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ICommodityGroupRepository _context)
    {
        ValidationResult = new CommodityGroupDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class CommodityGroupSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public CommodityGroupSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
