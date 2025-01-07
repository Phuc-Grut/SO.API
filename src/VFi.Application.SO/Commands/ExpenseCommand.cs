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

public class ExpenseCommand : Command
{

    public Guid Id { get; set; }
    public string Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public int? DisplayOrder { get; set; }

}

public class ExpenseAddCommand : ExpenseCommand
{
    public ExpenseAddCommand(
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
    public bool IsValid(IExpenseRepository _context)
    {
        ValidationResult = new ExpenseAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ExpenseEditCommand : ExpenseCommand
{
    public ExpenseEditCommand(
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
    public bool IsValid(IExpenseRepository _context)
    {
        ValidationResult = new ExpenseEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ExpenseDeleteCommand : ExpenseCommand
{
    public ExpenseDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IExpenseRepository _context)
    {
        ValidationResult = new ExpenseDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ExpenseSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public ExpenseSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
