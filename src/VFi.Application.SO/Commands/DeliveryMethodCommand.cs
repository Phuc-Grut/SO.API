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

public class DeliveryMethodCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public class DeliveryMethodAddCommand : DeliveryMethodCommand
{
    public DeliveryMethodAddCommand(
        Guid id,
        string? code,
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
    public bool IsValid(IDeliveryMethodRepository _context)
    {
        ValidationResult = new DeliveryMethodAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class DeliveryMethodEditCommand : DeliveryMethodCommand
{
    public DeliveryMethodEditCommand(
       Guid id,
        string? code,
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
    public bool IsValid(IDeliveryMethodRepository _context)
    {
        ValidationResult = new DeliveryMethodEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class DeliveryMethodDeleteCommand : DeliveryMethodCommand
{
    public DeliveryMethodDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IDeliveryMethodRepository _context)
    {
        ValidationResult = new DeliveryMethodDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
