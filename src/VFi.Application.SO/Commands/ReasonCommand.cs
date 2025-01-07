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

public class ReasonCommand : Command
{

    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? Status { get; set; }
}

public class ReasonAddCommand : ReasonCommand
{
    public ReasonAddCommand(
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
    public bool IsValid(IReasonRepository _context)
    {
        ValidationResult = new ReasonAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ReasonEditCommand : ReasonCommand
{
    public ReasonEditCommand(
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
    public bool IsValid(IReasonRepository _context)
    {
        ValidationResult = new ReasonEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ReasonDeleteCommand : ReasonCommand
{
    public ReasonDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IReasonRepository _context)
    {
        ValidationResult = new ReasonDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
