using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using FluentValidation;
using MassTransit.Internals.GraphValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class PriceListCommand : Command
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public int? DisplayOrder { get; set; }
    public List<PriceListDetailDto>? PriceListDetail { get; set; }
}

public class AddPriceListCommand : PriceListCommand
{
    public AddPriceListCommand(
        Guid id,
        string? code,
        string? name,
        string? description,
        int? status,
        DateTime? startDate,
        DateTime? endDate,
        string? currency,
        string? currencyName,
        int? displayOrder,
        List<PriceListDetailDto>? priceListDetail)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        StartDate = startDate;
        EndDate = endDate;
        Currency = currency;
        CurrencyName = currencyName;
        DisplayOrder = displayOrder;
        PriceListDetail = priceListDetail;
    }

    public bool IsValid(IPriceListRepository _context)
    {
        ValidationResult = new AddPriceListValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class EditPriceListCommand : PriceListCommand
{
    public EditPriceListCommand(
        Guid id,
        string? code,
        string? name,
        string? description,
        int? status,
        DateTime? startDate,
        DateTime? endDate,
        string? currency,
        string? currencyName,
        int? displayOrder,
        List<PriceListDetailDto>? priceListDetail)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        StartDate = startDate;
        EndDate = endDate;
        Currency = currency;
        CurrencyName = currencyName;
        DisplayOrder = displayOrder;
        PriceListDetail = priceListDetail;
    }

    public bool IsValid(IPriceListRepository _context)
    {
        ValidationResult = new EditPriceListValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class PriceListSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public PriceListSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}

public class DeletePriceListCommand : PriceListCommand
{
    public DeletePriceListCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IPriceListRepository _context)
    {
        ValidationResult = new DetelePriceListValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ValidateExcelPriceList
{
    public IFormFile File { get; set; } = null!;
    public string SheetId { get; set; } = null!;
    public int HeaderRow { get; set; }
    public int? ProductCode { get; set; }
    public int? ProductName { get; set; }
    public int? Type { get; set; }
    public int? FixPrice { get; set; }
    public int? UnitName { get; set; }
    public int? QuantityMin { get; set; }

}
