using FluentValidation;
using Microsoft.AspNetCore.Http;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class ExportWarehouseCommand : Command
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Description { get; set; }
    public Guid? WarehouseId { get; set; }
    public string? WarehouseCode { get; set; }
    public string? WarehouseName { get; set; }
    public int? DeliveryStatus { get; set; }
    public string? DeliveryName { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryCountry { get; set; }
    public string? DeliveryProvince { get; set; }
    public string? DeliveryDistrict { get; set; }
    public string? DeliveryWard { get; set; }
    public string? DeliveryNote { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public Guid? DeliveryMethodId { get; set; }
    public string? DeliveryMethodCode { get; set; }
    public string? DeliveryMethodName { get; set; }
    public Guid? ShippingMethodId { get; set; }
    public string? ShippingMethodCode { get; set; }
    public string? ShippingMethodName { get; set; }
    public int? Status { get; set; }
    public string? Note { get; set; }
    public Guid? RequestBy { get; set; }
    public string? RequestByName { get; set; }
    public string? RequestByEmail { get; set; }
    public DateTime? RequestDate { get; set; }
    public string? File { get; set; }
    public List<ExportWarehouseProductDto>? Detail { get; set; }
    public List<ListId>? Delete { get; set; }
}

public class ExportWarehouseAddCommand : ExportWarehouseCommand
{
    public Guid? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }

    public ExportWarehouseAddCommand(
        Guid id,
        string? code,
        Guid? orderId,
        string? orderCode,
        Guid? customerId,
        string? customerCode,
        string? customerName,
        string? description,
        Guid? warehouseId,
        string? warehouseCode,
        string? warehouseName,
        int? deliveryStatus,
        string? deliveryName,
        string? deliveryAddress,
        string? deliveryCountry,
        string? deliveryProvince,
        string? deliveryDistrict,
        string? deliveryWard,
        string? deliveryNote,
        DateTime? estimatedDeliveryDate,
        Guid? deliveryMethodId,
        string? deliveryMethodCode,
        string? deliveryMethodName,
        Guid? shippingMethodId,
        string? shippingMethodCode,
        string? shippingMethodName,
        int? status,
        string? note,
        Guid? requestBy,
        string? requestByName,
        DateTime? requestDate,
        Guid? createdBy,
        string? createdByName,
        string? file,
        List<ExportWarehouseProductDto>? detail
    )
    {
        Id = id;
        Code = code;
        OrderId = orderId;
        OrderCode = orderCode;
        CustomerId = customerId;
        CustomerCode = customerCode;
        CustomerName = customerName;
        Description = description;
        WarehouseId = warehouseId;
        WarehouseCode = warehouseCode;
        WarehouseName = warehouseName;
        DeliveryStatus = deliveryStatus;
        DeliveryName = deliveryName;
        DeliveryAddress = deliveryAddress;
        DeliveryCountry = deliveryCountry;
        DeliveryProvince = deliveryProvince;
        DeliveryDistrict = deliveryDistrict;
        DeliveryWard = deliveryWard;
        DeliveryNote = deliveryNote;
        EstimatedDeliveryDate = estimatedDeliveryDate;
        DeliveryMethodId = deliveryMethodId;
        DeliveryMethodCode = deliveryMethodCode;
        DeliveryMethodName = deliveryMethodName;
        ShippingMethodId = shippingMethodId;
        ShippingMethodCode = shippingMethodCode;
        ShippingMethodName = shippingMethodName;
        Status = status;
        Note = note;
        RequestBy = requestBy;
        RequestByName = requestByName;
        RequestDate = requestDate;
        CreatedBy = createdBy;
        CreatedByName = createdByName;
        File = file;
        Detail = detail;
    }

    public bool IsValid(IExportWarehouseRepository _context)
    {
        ValidationResult = new ExportWarehouseAddCommandValidation(_context, this).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ExportWarehouseEditCommand : ExportWarehouseCommand
{
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }

    public ExportWarehouseEditCommand(
        Guid id,
        string? code,
        Guid? orderId,
        string? orderCode,
        Guid? customerId,
        string? customerCode,
        string? customerName,
        string? description,
        Guid? warehouseId,
        string? warehouseCode,
        string? warehouseName,
        int? deliveryStatus,
        string? deliveryName,
        string? deliveryAddress,
        string? deliveryCountry,
        string? deliveryProvince,
        string? deliveryDistrict,
        string? deliveryWard,
        string? deliveryNote,
        DateTime? estimatedDeliveryDate,
        Guid? deliveryMethodId,
        string? deliveryMethodCode,
        string? deliveryMethodName,
        Guid? shippingMethodId,
        string? shippingMethodCode,
        string? shippingMethodName,
        int? status,
        string? note,
        Guid? requestBy,
        string? requestByName,
        DateTime? requestDate,
        Guid? updatedBy,
        string? updatedByName,
        string? file,
        List<ExportWarehouseProductDto>? detail,
        List<ListId>? delete
    )
    {
        Id = id;
        Code = code;
        OrderId = orderId;
        OrderCode = orderCode;
        CustomerId = customerId;
        CustomerCode = customerCode;
        CustomerName = customerName;
        Description = description;
        WarehouseId = warehouseId;
        WarehouseCode = warehouseCode;
        WarehouseName = warehouseName;
        DeliveryStatus = deliveryStatus;
        DeliveryName = deliveryName;
        DeliveryAddress = deliveryAddress;
        DeliveryCountry = deliveryCountry;
        DeliveryProvince = deliveryProvince;
        DeliveryDistrict = deliveryDistrict;
        DeliveryWard = deliveryWard;
        DeliveryNote = deliveryNote;
        EstimatedDeliveryDate = estimatedDeliveryDate;
        DeliveryMethodId = deliveryMethodId;
        DeliveryMethodCode = deliveryMethodCode;
        DeliveryMethodName = deliveryMethodName;
        ShippingMethodId = shippingMethodId;
        ShippingMethodCode = shippingMethodCode;
        ShippingMethodName = shippingMethodName;
        Status = status;
        Note = note;
        RequestBy = requestBy;
        RequestByName = requestByName;
        RequestDate = requestDate;
        UpdatedBy = updatedBy;
        UpdatedByName = updatedByName;
        File = file;
        Detail = detail;
        Delete = delete;
    }

    public bool IsValid(IExportWarehouseRepository _context)
    {
        ValidationResult = new ExportWarehouseEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ExportWarehouseDeleteCommand : ExportWarehouseCommand
{
    public ExportWarehouseDeleteCommand(Guid id)
    {
        Id = id;
    }

    public bool IsValid(IExportWarehouseRepository _context)
    {
        ValidationResult = new ExportWarehouseDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ApprovalExportWarehouseCommand : Command
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public int? Status { get; set; }
    public string? ApproveComment { get; set; }
    public int? Type { get; set; }
    public string? WarehouseCode { get; set; }
    public string? Sonumber { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? DeliveryName { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryCountry { get; set; }
    public string? DeliveryProvince { get; set; }
    public string? DeliveryDistrict { get; set; }
    public string? DeliveryWard { get; set; }
    public string? DeliveryNote { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public string? ShippingMethodCode { get; set; }
    public string? DeliveryMethodCode { get; set; }
    public int? WMSStatus { get; set; }
    public string? Note { get; set; }
    public string? RequestByName { get; set; }
    public Guid? RequestBy { get; set; }
    public List<ProductItemListAddExt>? Details { get; set; }

    public ApprovalExportWarehouseCommand(
        Guid id,
        int? status,
        string? approveComment,
        int? type,
        string? warehouseCode,
        string? sonumber,
        string? customerCode,
        string? customerName,
        string? deliveryName,
        string? deliveryAddress,
        string? deliveryCountry,
        string? deliveryProvince,
        string? deliveryDistrict,
        string? deliveryWard,
        string? deliveryNote,
        DateTime? estimatedDeliveryDate,
        string? shippingMethodCode,
        string? deliveryMethodCode,
        int? wmsStatus,
        string? note,
        string? requestByName,
        Guid? requestBy,
        List<ProductItemListAddExt>? details
    )
    {
        Id = id;
        Status = status;
        ApproveComment = approveComment;
        Type = type;
        WarehouseCode = warehouseCode;
        Sonumber = sonumber;
        CustomerCode = customerCode;
        CustomerName = customerName;
        DeliveryName = deliveryName;
        DeliveryAddress = deliveryAddress;
        DeliveryCountry = deliveryCountry;
        DeliveryProvince = deliveryProvince;
        DeliveryDistrict = deliveryDistrict;
        DeliveryWard = deliveryWard;
        DeliveryNote = deliveryNote;
        EstimatedDeliveryDate = estimatedDeliveryDate;
        ShippingMethodCode = shippingMethodCode;
        DeliveryMethodCode = deliveryMethodCode;
        WMSStatus = wmsStatus;
        Note = note;
        RequestByName = requestByName;
        RequestBy = requestBy;
        Details = details;
    }
}

public class ExportWarehouseDuplicateCommand : ExportWarehouseCommand
{
    public ExportWarehouseDuplicateCommand(
        Guid id,
        string code)
    {
        Id = id;
        Code = code;
    }

    public bool IsValid(IExportWarehouseRepository _context)
    {
        ValidationResult = new ExportWarehousetDuplicateCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ExportWarehouseAddOrderIdsCommand : ExportWarehouseCommand
{
    public Guid Id { get; set; }
    public List<Guid>? OrderIds { get; set; }

    public ExportWarehouseAddOrderIdsCommand(Guid _Id, List<Guid> _orderIds)
    {
        Id = _Id;
        OrderIds = _orderIds;

    }

    public ExportWarehouseAddOrderIdsCommand()
    {
    }

    public async Task<bool> IsValidAsync(IExportWarehouseRepository _exportWarehouseRepository, IExportWarehouseProductRepository _exportWarehouseProductRepository,IOrderRepository _orderRepository)
    {
        ValidationResult = await new ExportWarehouseAddOrderIdsCommandValidation(_exportWarehouseRepository, _exportWarehouseProductRepository, _orderRepository, Id).ValidateAsync(this);
        return ValidationResult.IsValid;
    }

}


public class ValidateExcelExportWarehouset
{
    public IFormFile File { get; set; } = null!;
    public string SheetId { get; set; } = null!;
    public int HeaderRow { get; set; }
    public int? ProductCode { get; set; }
    public int? ProductName { get; set; }
    public int? UnitCode { get; set; }
    public int? UnitName { get; set; }
    public int? UnitPrice { get; set; }
    public int? DeliveryDate { get; set; }
    public int? PriorityLevel { get; set; }
    public int? QuantityRequest { get; set; }
    public int? Note { get; set; }
}

public class UpdateServiceFeesCommand : Command
{
    public Guid Id { get; set; }
    public Guid ServiceAddId { get; set; }
    public Guid? ServiceAddCurrencyId { get; set; }
    public string ServiceAddCurrencyCode { get; set; }
    public string? ServiceAddName { get; set; }
    public decimal PriceTotal { get; set; }

    public UpdateServiceFeesCommand(
        Guid id,
        Guid serviceAddId,
        Guid? serviceAddCurrencyId,
        string serviceAddCurrencyCode,
        string? serviceAddName,
        decimal priceTotal
    )
    {
        Id = id;
        ServiceAddId = serviceAddId;
        ServiceAddCurrencyId = serviceAddCurrencyId;
        ServiceAddCurrencyCode = serviceAddCurrencyCode;
        ServiceAddName = serviceAddName;
        PriceTotal = priceTotal;
    }
    
    public bool IsValid(IExportWarehouseRepository context)
    {
        ValidationResult = new ExportWarehouseUpdateServiceFeesCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}