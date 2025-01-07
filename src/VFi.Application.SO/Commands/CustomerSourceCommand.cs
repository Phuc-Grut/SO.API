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

public class CustomerSourceCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? DisplayOrder { get; set; }
    public int? Status { get; set; }
}

public class CustomerSourceAddCommand : CustomerSourceCommand
{
    public CustomerSourceAddCommand(
        Guid id,
        string? code,
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
    public bool IsValid(ICustomerSourceRepository _context)
    {
        ValidationResult = new CustomerSourceAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerSourceEditCommand : CustomerSourceCommand
{
    public CustomerSourceEditCommand(
       Guid id,
        string? code,
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
    public bool IsValid(ICustomerSourceRepository _context)
    {
        ValidationResult = new CustomerSourceEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerSourceDeleteCommand : CustomerSourceCommand
{
    public CustomerSourceDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ICustomerSourceRepository _context)
    {
        ValidationResult = new CustomerSourceDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class CustomerSourceSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public CustomerSourceSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
