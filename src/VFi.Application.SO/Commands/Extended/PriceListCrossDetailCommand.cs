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

public class PriceListCrossDetailCommand : Command
{

    public Guid Id { get; set; }
    public Guid PriceListCrossId { get; set; }
    public string PriceListCross { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Note { get; set; }
    public int? Status { get; set; }
    public Guid RouterShippingId { get; set; }
    public string RouterShipping { get; set; }
    public Guid? CommodityGroupId { get; set; }
    public string CommodityGroup { get; set; }
    public decimal? AirFreight { get; set; }
    public decimal? SeaFreight { get; set; }
    public string Currency { get; set; }
}

public class PriceListCrossDetailAddCommand : PriceListCrossDetailCommand
{
    public PriceListCrossDetailAddCommand()
    {
    }

    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedByName { get; set; }
    public bool IsValid(IPriceListCrossDetailRepository _context)
    {
        ValidationResult = new PriceListCrossDetailAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PriceListCrossDetailEditCommand : PriceListCrossDetailCommand
{
    public PriceListCrossDetailEditCommand()
    {
    }

    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string UpdatedByName { get; set; }
    public bool IsValid(IPriceListCrossDetailRepository _context)
    {
        ValidationResult = new PriceListCrossDetailEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PriceListCrossDetailDeleteCommand : PriceListCrossDetailCommand
{
    public PriceListCrossDetailDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IPriceListCrossDetailRepository _context)
    {
        ValidationResult = new PriceListCrossDetailDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
