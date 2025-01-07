using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class DeliveryProductCommand : Command
{
    public Guid Id { get; set; }
    public Guid OrderProductId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public double? QuantityExpected { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int? DisplayOrder { get; set; }

}

public class DeliveryProductAddCommand : DeliveryProductCommand
{
    public DeliveryProductAddCommand(Guid id,
                                  Guid orderProductId,
                                  string? code,
                                  string? name,
                                  string? description,
                                  int? status,
                                  double? quantityExpected,
                                  DateTime? deliveryDate,
                                  int? displayOrder)
    {
        Id = id;
        OrderProductId = orderProductId;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        QuantityExpected = quantityExpected;
        DeliveryDate = deliveryDate;
        DisplayOrder = displayOrder;
    }
    public bool IsValid(IDeliveryProductRepository _context)
    {
        ValidationResult = new DeliveryProductAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class DeliveryProductEditCommand : DeliveryProductCommand
{
    public DeliveryProductEditCommand(Guid id,
                                  Guid orderProductId,
                                  string? code,
                                  string? name,
                                  string? description,
                                  int? status,
                                  double? quantityExpected,
                                  DateTime? deliveryDate,
                                  int? displayOrder)
    {
        Id = id;
        OrderProductId = orderProductId;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        QuantityExpected = quantityExpected;
        DeliveryDate = deliveryDate;
        DisplayOrder = displayOrder;
    }
    public bool IsValid(IDeliveryProductRepository _context)
    {
        ValidationResult = new DeliveryProductEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class DeliveryProductAddRangeCommand : Command
{
    public DeliveryProductAddRangeCommand(
        string listGuidDeliveryProduct,
        List<DeliveryProductDto>? list
        )
    {
        List = list;
        ListGuidDeliveryProduct = listGuidDeliveryProduct;
    }
    public List<DeliveryProductDto>? List { get; set; }
    public string ListGuidDeliveryProduct { get; set; }
}

public class DeliveryProductDeleteCommand : DeliveryProductCommand
{
    public DeliveryProductDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IDeliveryProductRepository _context)
    {
        ValidationResult = new DeliveryProductDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
