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

public class GroupEmployeeCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
}

public class GroupEmployeeAddCommand : GroupEmployeeCommand
{
    public GroupEmployeeAddCommand(
        Guid id,
        string? code,
        string? name,
        string? description,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
    }
    public bool IsValid(IGroupEmployeeRepository _context)
    {
        ValidationResult = new GroupEmployeeAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class GroupEmployeeEditCommand : GroupEmployeeCommand
{
    public GroupEmployeeEditCommand(
       Guid id,
        string? code,
        string? name,
        string? description,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
    }
    public bool IsValid(IGroupEmployeeRepository _context)
    {
        ValidationResult = new GroupEmployeeEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class GroupEmployeeDeleteCommand : GroupEmployeeCommand
{
    public GroupEmployeeDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IGroupEmployeeRepository _context)
    {
        ValidationResult = new GroupEmployeeDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
