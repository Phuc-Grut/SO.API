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

public class OrderFulfillmentDetailCommand : Command
{

    public Guid Id { get; set; }
    public Guid OrderFulfillmentId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public string Origin { get; set; }
    public Guid? WarehouseId { get; set; }
    public string WarehouseCode { get; set; }
    public string WarehouseName { get; set; }
    /// <summary>
    /// Đơn giá
    /// </summary>
    public decimal? UnitPrice { get; set; }
    public string UnitName { get; set; }
    public int? Quantity { get; set; }
    public int? DisplayOrder { get; set; }
    public string Note { get; set; }
}

public class OrderFulfillmentDetailAddCommand : OrderFulfillmentDetailCommand
{
    public OrderFulfillmentDetailAddCommand()
    {
    }

    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedByName { get; set; }
    public bool IsValid(IOrderFulfillmentDetailRepository _context)
    {
        ValidationResult = new OrderFulfillmentDetailAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class OrderFulfillmentDetailEditCommand : OrderFulfillmentDetailCommand
{
    public OrderFulfillmentDetailEditCommand()
    {
    }

    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string UpdatedByName { get; set; }
    public bool IsValid(IOrderFulfillmentDetailRepository _context)
    {
        ValidationResult = new OrderFulfillmentDetailEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class OrderFulfillmentDetailDeleteCommand : OrderFulfillmentDetailCommand
{
    public OrderFulfillmentDetailDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IOrderFulfillmentDetailRepository _context)
    {
        ValidationResult = new OrderFulfillmentDetailDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
