using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Consul;
using VFi.Application.SO.Commands.Validations;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class CustomerContactCommand : Command
{

    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public string? Name { get; set; }
    public int? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Facebook { get; set; }
    public string? Tags { get; set; }
    public string? Address { get; set; }
    public int? Status { get; set; }
    public int? SortOrder { get; set; }
}

public class CustomerContactAddCommand : CustomerContactCommand
{
    public Guid? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public CustomerContactAddCommand(
        Guid id,
        Guid? customerId,
        string? name,
        int? gender,
        string? phone,
        string? email,
        string? facebook,
        string? tags,
        string? address,
        int? status,
        int? sortOrder,
        Guid? createdBy,
        string? createdByName)
    {
        Id = id;
        CustomerId = customerId;
        Name = name;
        Gender = gender;
        Phone = phone;
        Email = email;
        Facebook = facebook;
        Tags = tags;
        Address = address;
        Status = status;
        SortOrder = sortOrder;
        CreatedBy = createdBy;
        CreatedByName = createdByName;
    }
    public bool IsValid(ICustomerContactRepository _context)
    {
        ValidationResult = new CustomerContactAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerContactEditCommand : CustomerContactCommand
{
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }
    public CustomerContactEditCommand(
        Guid id,
        Guid? customerId,
        string? name,
        int? gender,
        string? phone,
        string? email,
        string? facebook,
        string? tags,
        string? address,
        int? status,
        int? sortOrder,
        Guid? updatedBy,
        string? updatedByName)
    {
        Id = id;
        CustomerId = customerId;
        Name = name;
        Gender = gender;
        Phone = phone;
        Email = email;
        Facebook = facebook;
        Tags = tags;
        Address = address;
        Status = status;
        SortOrder = sortOrder;
        UpdatedBy = updatedBy;
        UpdatedByName = updatedByName;
    }
    public bool IsValid(ICustomerContactRepository _context)
    {
        ValidationResult = new CustomerContactEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerContactDeleteCommand : CustomerContactCommand
{
    public CustomerContactDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ICustomerContactRepository _context)
    {
        ValidationResult = new CustomerContactDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
