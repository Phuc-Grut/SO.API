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

public class PaymentMethodCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
}

public class PaymentMethodAddCommand : PaymentMethodCommand
{
    public PaymentMethodAddCommand(
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
    public bool IsValid(IPaymentMethodRepository _context)
    {
        ValidationResult = new PaymentMethodAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PaymentMethodEditCommand : PaymentMethodCommand
{
    public PaymentMethodEditCommand(
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
    public bool IsValid(IPaymentMethodRepository _context)
    {
        ValidationResult = new PaymentMethodEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PaymentMethodDeleteCommand : PaymentMethodCommand
{
    public PaymentMethodDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IPaymentMethodRepository _context)
    {
        ValidationResult = new PaymentMethodDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
