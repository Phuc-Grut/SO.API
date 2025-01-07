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

public class PriceListCrossCommand : Command
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? Status { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? Default { get; set; }
    public Guid? RouterShippingId { get; set; }
    public string RouterShipping { get; set; }
    public List<PriceListCrossDetailDto> Detail { get; set; } = new List<PriceListCrossDetailDto>();
}

public class PriceListCrossAddCommand : PriceListCrossCommand
{
    public PriceListCrossAddCommand(Guid id,
                                    string code,
                                    string name,
                                    string description,
                                    int? status,
                                    int? displayOrder,
                                    bool? @default,
                                    Guid? routerShippingId,
                                    string routerShipping,
                                    List<PriceListCrossDetailDto> priceListCrossDetail)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        DisplayOrder = displayOrder;
        Default = @default;
        RouterShippingId = routerShippingId;
        RouterShipping = routerShipping;
        Detail = priceListCrossDetail;
    }
    public bool IsValid(IPriceListCrossRepository _context)
    {
        ValidationResult = new PriceListCrossAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PriceListCrossEditCommand : PriceListCrossCommand
{
    public PriceListCrossEditCommand(Guid id,
                                     string code,
                                     string name,
                                     string description,
                                     int? status,
                                     int? displayOrder,
                                     bool? @default,
                                     Guid? routerShippingId,
                                     string routerShipping,
                                     List<PriceListCrossDetailDto> priceListCrossDetail)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        DisplayOrder = displayOrder;
        Default = @default;
        RouterShippingId = routerShippingId;
        RouterShipping = routerShipping;
        Detail = priceListCrossDetail;
    }
    public bool IsValid(IPriceListCrossRepository _context)
    {
        ValidationResult = new PriceListCrossEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PriceListCrossDeleteCommand : PriceListCrossCommand
{
    public PriceListCrossDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IPriceListCrossRepository _context)
    {
        ValidationResult = new PriceListCrossDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PriceListCrossSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public PriceListCrossSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
