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

public class OrderExpressDetailCommand : Command
{

    public Guid Id { get; set; }
    public Guid OrderExpressId { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public string Origin { get; set; }
    public string UnitName { get; set; }
    public int? Quantity { get; set; }
    /// <summary>
    /// Đơn giá
    /// </summary>
    public decimal? UnitPrice { get; set; }
    public int? DisplayOrder { get; set; }
    public string Note { get; set; }
    public string CommodityGroup { get; set; }
    public string SurchargeGroup { get; set; }
    public decimal? Surcharge { get; set; }
}

public class OrderExpressDetailAddCommand : OrderExpressDetailCommand
{
    public OrderExpressDetailAddCommand()
    {
    }

    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedByName { get; set; }
    public bool IsValid(IOrderExpressDetailRepository _context)
    {
        ValidationResult = new OrderExpressDetailAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class OrderExpressDetailEditCommand : OrderExpressDetailCommand
{
    public OrderExpressDetailEditCommand()
    {
    }

    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string UpdatedByName { get; set; }
    public bool IsValid(IOrderExpressDetailRepository _context)
    {
        ValidationResult = new OrderExpressDetailEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class OrderExpressDetailDeleteCommand : OrderExpressDetailCommand
{
    public OrderExpressDetailDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IOrderExpressDetailRepository _context)
    {
        ValidationResult = new OrderExpressDetailDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
