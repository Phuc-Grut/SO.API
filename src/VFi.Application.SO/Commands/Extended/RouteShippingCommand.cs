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

public class RouteShippingCommand : Command
{

    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public string? FromPost { get; set; }
    public string? ToPost { get; set; }
    public int? Status { get; set; }
}

public class RouteShippingAddCommand : RouteShippingCommand
{
    public RouteShippingAddCommand(
        Guid id,
        string code,
        string? name,
        string? description,
        string? note,
        string? formPost,
        string? toPost,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Note = note;
        FromPost = formPost;
        ToPost = toPost;
        Status = status;
    }
    public bool IsValid(IRouteShippingRepository _context)
    {
        ValidationResult = new RouteShippingAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class RouteShippingEditCommand : RouteShippingCommand
{
    public RouteShippingEditCommand(
         Guid id,
        string code,
        string? name,
        string? description,
        string? note,
        string? formPost,
        string? toPost,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Note = note;
        FromPost = formPost;
        ToPost = toPost;
        Status = status;
    }
    public bool IsValid(IRouteShippingRepository _context)
    {
        ValidationResult = new RouteShippingEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class RouteShippingDeleteCommand : RouteShippingCommand
{
    public RouteShippingDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IRouteShippingRepository _context)
    {
        ValidationResult = new RouteShippingDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class RouteShippingSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public RouteShippingSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
