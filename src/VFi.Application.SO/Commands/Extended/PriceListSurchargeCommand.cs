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

public class PriceListSurchargeCommand : Command
{
    public Guid Id { get; set; }
    public Guid RouterShippingId { get; set; }
    public string RouterShipping { get; set; } = null!;
    public Guid? SurchargeGroupId { get; set; }
    public string SurchargeGroup { get; set; } = null!;
    public decimal? Price { get; set; }
    public string Currency { get; set; } = null!;
    public string? Note { get; set; }
    public int? Status { get; set; }
}
public class PriceListSurchargeAddCommand : PriceListSurchargeCommand
{
    public List<PriceListSurchargeDto> Details { get; set; } = new List<PriceListSurchargeDto>();
    public PriceListSurchargeAddCommand(Guid routerShippingId, string routerShipping, List<PriceListSurchargeDto> details)
    {
        RouterShippingId = routerShippingId;
        RouterShipping = routerShipping;
        Details = details;
    }

    public bool IsValid(IPriceListSurchargeRepository _context)
    {
        ValidationResult = new PriceListSurchargeAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PriceListSurchargeEditCommand : PriceListSurchargeCommand
{
    public PriceListSurchargeEditCommand(Guid id, Guid routerShippingId, string routerShipping, Guid? surchargeGroupId, string surchargeGroup, decimal? price, string currency, string? note, int? status)
    {
        Id = id;
        RouterShippingId = routerShippingId;
        RouterShipping = routerShipping;
        SurchargeGroupId = surchargeGroupId;
        SurchargeGroup = surchargeGroup;
        Price = price;
        Currency = currency;
        Note = note;
        Status = status;
    }
    public bool IsValid(IPriceListSurchargeRepository _context)
    {
        ValidationResult = new PriceListSurchargeEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PriceListSurchargeDeleteCommand : PriceListSurchargeCommand
{
    public PriceListSurchargeDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IPriceListSurchargeRepository _context)
    {
        ValidationResult = new PriceListSurchargeDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class PriceListSurchargeSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public PriceListSurchargeSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
