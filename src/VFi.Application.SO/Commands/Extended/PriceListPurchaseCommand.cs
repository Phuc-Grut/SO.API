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

public class PriceListPurchaseCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? Default { get; set; }
    public List<PriceListPurchaseDetailDto> Detail { get; set; } = new List<PriceListPurchaseDetailDto>();
}

public class PriceListPurchaseAddCommand : PriceListPurchaseCommand
{
    public PriceListPurchaseAddCommand(Guid id, string? code, string? name, string? description, int? status, int? displayOrder, List<PriceListPurchaseDetailDto> detail)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        DisplayOrder = displayOrder;
        Detail = detail;
    }

    public bool IsValid(IPriceListPurchaseRepository _context)
    {
        ValidationResult = new PriceListPurchaseAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PriceListPurchaseEditCommand : PriceListPurchaseCommand
{
    public PriceListPurchaseEditCommand(Guid id, string? code, string? name, string? description, int? status, int? displayOrder, List<PriceListPurchaseDetailDto> detail)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        DisplayOrder = displayOrder;
        Detail = detail;
    }
    public bool IsValid(IPriceListPurchaseRepository _context)
    {
        ValidationResult = new PriceListPurchaseEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PriceListPurchaseDeleteCommand : PriceListPurchaseCommand
{
    public PriceListPurchaseDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IPriceListPurchaseRepository _context)
    {
        ValidationResult = new PriceListPurchaseDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class PriceListPurchaseSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public PriceListPurchaseSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
