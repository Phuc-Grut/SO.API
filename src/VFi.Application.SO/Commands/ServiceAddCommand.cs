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

public class ServiceAddCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int CalculationMethod { get; set; }
    public decimal? Price { get; set; }
    public string? PriceSyntax { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? PayLater { get; set; }
    public int Status { get; set; }
    public string? Tags { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public int DisplayOrder { get; set; }

    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
}

public class ServiceAddAddCommand : ServiceAddCommand
{
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public ServiceAddAddCommand(
        Guid id,
        string? code,
        string name,
        string? description,
        int calculationMethod,
        decimal? price,
        string? priceSyntax,
        decimal? minPrice,
        decimal? maxPrice,
        bool? payLater,
        int status,
        string? tags,
        string? currency,
        string? currencyName,
        int displayOrder,
        Guid? createdBy,
        DateTime createdDate,
        string? createdByName)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        CalculationMethod = calculationMethod;
        Price = price;
        PriceSyntax = priceSyntax;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        PayLater = payLater;
        Status = status;
        Tags = tags;
        Currency = currency;
        CurrencyName = currencyName;
        DisplayOrder = displayOrder;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
        CreatedByName = createdByName;
    }
    public bool IsValid(IServiceAddRepository _context)
    {
        ValidationResult = new ServiceAddAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ServiceAddEditCommand : ServiceAddCommand
{
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public ServiceAddEditCommand(
        Guid id,
        string? code,
        string name,
        string? description,
        int calculationMethod,
        decimal? price,
        string? priceSyntax,
        decimal? minPrice,
        decimal? maxPrice,
        bool? payLater,
        int status,
        string? tags,
        string? currency,
        string? currencyName,
        int displayOrder,
        Guid? updatedBy,
        DateTime? updatedDate,
        string? updatedByName)
    {

        Id = id;
        Code = code;
        Name = name;
        Description = description;
        CalculationMethod = calculationMethod;
        Price = price;
        PriceSyntax = priceSyntax;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        PayLater = payLater;
        Status = status;
        Tags = tags;
        Currency = currency;
        CurrencyName = currencyName;
        DisplayOrder = displayOrder;
        UpdatedBy = updatedBy;
        UpdatedDate = updatedDate;
        UpdatedByName = updatedByName;
    }
    public bool IsValid(IServiceAddRepository _context)
    {
        ValidationResult = new ServiceAddEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ServiceAddSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public ServiceAddSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}

public class ServiceAddDeleteCommand : ServiceAddCommand
{
    public ServiceAddDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IServiceAddRepository _context)
    {
        ValidationResult = new ServiceAddDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
