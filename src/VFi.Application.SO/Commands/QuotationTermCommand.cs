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

public class QuotationTermCommand : Command
{

    public Guid Id { get; set; }
    public string Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public int? DisplayOrder { get; set; }
}

public class QuotationTermAddCommand : QuotationTermCommand
{
    public QuotationTermAddCommand(
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
    public bool IsValid(IQuotationTermRepository _context)
    {
        ValidationResult = new QuotationTermAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class QuotationTermEditCommand : QuotationTermCommand
{
    public QuotationTermEditCommand(
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
    public bool IsValid(IQuotationTermRepository _context)
    {
        ValidationResult = new QuotationTermEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class QuotationTermDeleteCommand : QuotationTermCommand
{
    public QuotationTermDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IQuotationTermRepository _context)
    {
        ValidationResult = new QuotationTermDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class QuotationTermSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public QuotationTermSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
