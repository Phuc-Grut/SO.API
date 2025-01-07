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

public class ContractTermCommand : Command
{

    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? Status { get; set; }
}

public class ContractTermAddCommand : ContractTermCommand
{
    public ContractTermAddCommand(
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
    public bool IsValid(IContractTermRepository _context)
    {
        ValidationResult = new ContractTermAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ContractTermEditCommand : ContractTermCommand
{
    public ContractTermEditCommand(
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
    public bool IsValid(IContractTermRepository _context)
    {
        ValidationResult = new ContractTermEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ContractTermDeleteCommand : ContractTermCommand
{
    public ContractTermDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IContractTermRepository _context)
    {
        ValidationResult = new ContractTermDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ContractTermSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public ContractTermSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
