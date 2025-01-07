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

public class OrderFulfillmentCommand : Command
{

    public Guid Id { get; set; }

    public int? OrderType { get; set; }
    public string Code { get; set; }
    public DateTime? OrderDate { get; set; }
    public Guid? CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerCode { get; set; }
    public Guid? StoreId { get; set; }
    public string StoreCode { get; set; }
    public string StoreName { get; set; }
    public Guid? ContractId { get; set; }
    public string ContractName { get; set; }
    public Guid? ChannelId { get; set; }
    public string ChannelName { get; set; }
    public string ShipperName { get; set; }
    public string ShipperPhone { get; set; }
    public string ShipperZipCode { get; set; }
    public string ShipperAddress { get; set; }
    public string ShipperCountry { get; set; }
    public string ShipperProvince { get; set; }
    public string ShipperDistrict { get; set; }
    public string ShipperWard { get; set; }
    public string ShipperNote { get; set; }
    public int? PickupStatus { get; set; }
    public string PickupName { get; set; }
    public string PickupPhone { get; set; }
    public string PickupZipCode { get; set; }
    public string PickupAddress { get; set; }
    public string PickupCountry { get; set; }
    public string PickupProvince { get; set; }
    public string PickupDistrict { get; set; }
    public string PickupWard { get; set; }
    public string PickupNote { get; set; }
    public string AccountName { get; set; }
    public string Note { get; set; }
    public Guid? GroupEmployeeId { get; set; }
    public string GroupEmployeeName { get; set; }
    public Guid? AccountId { get; set; }
    public int? Status { get; set; }
}

public class OrderFulfillmentAddCommand : OrderFulfillmentCommand
{
    public OrderFulfillmentAddCommand()
    {
    }

    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedByName { get; set; }
    public bool IsValid(IOrderFulfillmentRepository _context)
    {
        ValidationResult = new OrderFulfillmentAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class OrderFulfillmentEditCommand : OrderFulfillmentCommand
{
    public OrderFulfillmentEditCommand()
    {
    }

    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string UpdatedByName { get; set; }
    public bool IsValid(IOrderFulfillmentRepository _context)
    {
        ValidationResult = new OrderFulfillmentEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class OrderFulfillmentDeleteCommand : OrderFulfillmentCommand
{
    public OrderFulfillmentDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IOrderFulfillmentRepository _context)
    {
        ValidationResult = new OrderFulfillmentDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
