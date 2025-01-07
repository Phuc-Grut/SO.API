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

public class CustomerGroupCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? DisplayOrder { get; set; }
    public int? Status { get; set; }
}

public class CustomerGroupAddCommand : CustomerGroupCommand
{
    public CustomerGroupAddCommand(
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
    public bool IsValid(ICustomerGroupRepository _context)
    {
        ValidationResult = new CustomerGroupAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerGroupEditCommand : CustomerGroupCommand
{
    public CustomerGroupEditCommand(
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
    public bool IsValid(ICustomerGroupRepository _context)
    {
        ValidationResult = new CustomerGroupEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerGroupDeleteCommand : CustomerGroupCommand
{
    public CustomerGroupDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ICustomerGroupRepository _context)
    {
        ValidationResult = new CustomerGroupDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class CustomerGroupSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public CustomerGroupSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
