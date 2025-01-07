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

public class PriceListPurchaseDetailCommand : Command
{

    public Guid Id { get; set; }
    public Guid PriceListPurchaseId { get; set; }
    public string? PriceListPurchase { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public Guid? PurchaseGroupId { get; set; }
    public string? PurchaseGroup { get; set; }
    public int? BuyFee { get; set; }
    public decimal? BuyFeeMin { get; set; }
    public string? Currency { get; set; }
    public int? Status { get; set; }
}

public class PriceListPurchaseDetailAddCommand : PriceListPurchaseDetailCommand
{
    public PriceListPurchaseDetailAddCommand(
       Guid id,
        string code,
        string? name,
        string? description,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
    }
    public bool IsValid(IPriceListPurchaseDetailRepository _context)
    {
        ValidationResult = new PriceListPurchaseDetailAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PriceListPurchaseDetailEditCommand : PriceListPurchaseDetailCommand
{
    public PriceListPurchaseDetailEditCommand(Guid id,
        string code,
        string? name,
        string? description,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
    }

    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string UpdatedByName { get; set; }
    public bool IsValid(IPriceListPurchaseDetailRepository _context)
    {
        ValidationResult = new PriceListPurchaseDetailEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PriceListPurchaseDetailDeleteCommand : PriceListPurchaseDetailCommand
{
    public PriceListPurchaseDetailDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IPriceListPurchaseDetailRepository _context)
    {
        ValidationResult = new PriceListPurchaseDetailDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class PriceListPurchaseDetailSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public PriceListPurchaseDetailSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
