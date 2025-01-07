using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using VFi.Application.SO.Commands.Validations;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class PromotionGroupCommand : Command
{

    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? Status { get; set; }
}

public class PromotionGroupAddCommand : PromotionGroupCommand
{
    public PromotionGroupAddCommand(
        Guid id,
        string code,
        string name,
        string description,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
    }
    public bool IsValid(IPromotionGroupRepository _context)
    {
        ValidationResult = new PromotionGroupAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PromotionGroupEditCommand : PromotionGroupCommand
{
    public PromotionGroupEditCommand(
       Guid id,
        string code,
        string name,
        string description,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
    }
    public bool IsValid(IPromotionGroupRepository _context)
    {
        ValidationResult = new PromotionGroupEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PromotionGroupDeleteCommand : PromotionGroupCommand
{
    public PromotionGroupDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IPromotionGroupRepository _context)
    {
        ValidationResult = new PromotionGroupDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
