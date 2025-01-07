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

public class StoreCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public int? DisplayOrder { get; set; }
    public int? Status { get; set; }
}

public class StoreAddCommand : StoreCommand
{

    public StoreAddCommand(
        Guid id,
        string code,
        string name,
        string? description,
        string? address,
        string? phone,
        int? displayOrder,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Address = address;
        Phone = phone;
        DisplayOrder = displayOrder;
        Status = status;
    }
    public bool IsValid(IStoreRepository _context)
    {
        ValidationResult = new StoreAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class StoreEditCommand : StoreCommand
{
    public StoreEditCommand(
         Guid id,
        string code,
        string name,
        string? description,
        string? address,
        string? phone,
        int? displayOrder,
        int? status)
    {

        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Address = address;
        Phone = phone;
        DisplayOrder = displayOrder;
        Status = status;
    }
    public bool IsValid(IStoreRepository _context)
    {
        ValidationResult = new StoreEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class StoreSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public StoreSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}

public class StoreDeleteCommand : StoreCommand
{
    public StoreDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IStoreRepository _context)
    {
        ValidationResult = new StoreDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class SetupPriceListCommand : Command
{
    public Guid Id { get; set; }
    public List<StorePriceListDto>? StorePriceList { get; set; }
    public SetupPriceListCommand(
          Guid id,
        List<StorePriceListDto>? storePriceList)
    {
        Id = id;
        StorePriceList = storePriceList;
    }
}
