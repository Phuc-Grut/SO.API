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

public class SurchargeGroupCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public int? DisplayOrder { get; set; }
    public int? Status { get; set; }
}

public class SurchargeGroupAddCommand : SurchargeGroupCommand
{
    public SurchargeGroupAddCommand(
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
    public bool IsValid(ISurchargeGroupRepository _context)
    {
        ValidationResult = new SurchargeGroupAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class SurchargeGroupEditCommand : SurchargeGroupCommand
{
    public SurchargeGroupEditCommand(
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
    public bool IsValid(ISurchargeGroupRepository _context)
    {
        ValidationResult = new SurchargeGroupEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class SurchargeGroupDeleteCommand : SurchargeGroupCommand
{
    public SurchargeGroupDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ISurchargeGroupRepository _context)
    {
        ValidationResult = new SurchargeGroupDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class SurchargeGroupSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public SurchargeGroupSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
