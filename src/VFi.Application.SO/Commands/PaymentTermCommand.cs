using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using MassTransit.Internals.GraphValidation;
using Microsoft.EntityFrameworkCore;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class PaymentTermCommand : Command
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? Day { get; set; }
    public decimal? Value { get; set; }
    public double? Percent { get; set; }
    public int? Status { get; set; }
}

public class AddPaymentTermCommand : PaymentTermCommand
{
    public AddPaymentTermCommand(
        Guid id,
        string code,
        string name,
        string description,
        int? day,
        decimal? value,
        double? percent,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Day = day;
        Value = value;
        Percent = percent;
        Status = status;
    }

    public bool IsValid(IPaymentTermRepository _context)
    {
        ValidationResult = new AddPaymentTermValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class EditPaymentTermCommand : PaymentTermCommand
{
    public EditPaymentTermCommand(
        Guid id,
        string code,
        string name,
        string description,
        int? day,
        decimal? value,
        double? percent,
        int? status)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Day = day;
        Value = value;
        Percent = percent;
        Status = status;
    }

    public bool IsValid(IPaymentTermRepository _context)
    {
        ValidationResult = new EditPaymentTermValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class DeletePaymentTermCommand : PaymentTermCommand
{
    public DeletePaymentTermCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IPaymentTermRepository _context)
    {
        ValidationResult = new DetelePaymentTermValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
