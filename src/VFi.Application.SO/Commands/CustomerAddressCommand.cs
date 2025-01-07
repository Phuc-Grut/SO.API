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

public class CustomerAddressCommand : Command
{

    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool? ShippingDefault { get; set; }
    public bool? BillingDefault { get; set; }
    public int? Status { get; set; }
    public int? SortOrder { get; set; }
}

public class CustomerAddressAddCommand : CustomerAddressCommand
{
    public Guid? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public CustomerAddressAddCommand(
        Guid id,
        Guid? customerId,
        string? name,
        string? country,
        string? province,
        string? district,
        string? ward,
        string? address,
        string? phone,
        string? email,
        bool? shippingDefault,
        bool? billingDefault,
        int? status,
        int? sortOrder,
        Guid? createdBy,
        string? createdByName
        )
    {
        Id = id;
        CustomerId = customerId;
        Name = name;
        Country = country;
        Province = province;
        District = district;
        Ward = ward;
        Address = address;
        Phone = phone;
        Email = email;
        ShippingDefault = shippingDefault;
        BillingDefault = billingDefault;
        Status = status;
        SortOrder = sortOrder;
        CreatedBy = createdBy;
        CreatedByName = createdByName;
    }
    public bool IsValid(ICustomerAddressRepository _context)
    {
        ValidationResult = new CustomerAddressAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerAddressEditCommand : CustomerAddressCommand
{
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }
    public CustomerAddressEditCommand(
        Guid id,
        Guid? customerId,
        string? name,
        string? country,
        string? province,
        string? district,
        string? ward,
        string? address,
        string? phone,
        string? email,
        bool? shippingDefault,
        bool? billingDefault,
        int? status,
        int? sortOrder,
        Guid? updatedBy,
        string? updatedByName)
    {
        Id = id;
        CustomerId = customerId;
        Name = name;
        Country = country;
        Province = province;
        District = district;
        Ward = ward;
        Address = address;
        Phone = phone;
        Email = email;
        ShippingDefault = shippingDefault;
        BillingDefault = billingDefault;
        Status = status;
        SortOrder = sortOrder;
        UpdatedBy = updatedBy;
        UpdatedByName = updatedByName;
    }
    public bool IsValid(ICustomerAddressRepository _context)
    {
        ValidationResult = new CustomerAddressEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerAddressDeleteCommand : CustomerAddressCommand
{
    public CustomerAddressDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ICustomerAddressRepository _context)
    {
        ValidationResult = new CustomerAddressDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
