using System.Data;
using FluentValidation.Results;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands.Extended;

public class OrderCrossCommand : Command
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public DateTime? OrderDate { get; set; }

    public Guid? CustomerId { get; set; }
    public int? Status { get; set; }
    public string? Currency { get; set; }
    public string? Note { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public string ChannelCode { get; set; } = null!;
    public List<OrderProductDto>? Detail { get; set; }
    public List<OrderServiceAddDto>? OrderServiceAdd { get; set; }
    public List<PaymentInvoiceDto>? PaymentInvoice { get; set; }

    public string PurchaseGroup { get; set; } = null!;

}

public class AddOrderCrossCommand : OrderCrossCommand
{
    public Guid? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public bool? IsAutoPayment { get; set; }
    public AddOrderCrossCommand(
        Guid id,
        string code,
        DateTime? orderDate,
        Guid? customerId,
        int? status,
        string? currency,
        string? note,
        Guid? accountId,
        string? accountName,
        List<OrderProductDto>? detail,
        List<OrderServiceAddDto>? orderServiceAdd,
        List<PaymentInvoiceDto>? paymentInvoice,
        Guid? createdBy,
        string? createdByName,
        bool? isAutoPayment,
        string purchaseGroup,
        string channelCode)
    {
        Id = id;
        Code = code;
        OrderDate = orderDate;
        CustomerId = customerId;
        Status = status;
        Currency = currency;
        Note = note;
        AccountId = accountId;
        AccountName = accountName;
        Detail = detail;
        OrderServiceAdd = orderServiceAdd;
        PaymentInvoice = paymentInvoice;
        CreatedBy = createdBy;
        CreatedByName = createdByName;
        IsAutoPayment = isAutoPayment;
        PurchaseGroup = purchaseGroup;
        ChannelCode = channelCode;
    }



    public bool IsValid(IOrderRepository _context)
    {
        ValidationResult = new AddOrderCrossValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CreateOrderCrossCommand : CommandResult
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public Guid CustomerId { get; set; }
    public string PurchaseGroup { get; set; }
    public string StoreCode { get; set; }
    public string ChannelCode { get; set; }
    public string CurrencyCode { get; set; }
    public decimal? ExchangeRate { get; set; }
    public string PaymentTermCode { get; set; }
    public string PaymentMethodCode { get; set; }
    public string ShippingMethodCode { get; set; }
    public int? BuyFee { get; set; }
    public decimal? DomesticShipping { get; set; }
    public string RouterShipping { get; set; }
    public string? DeliveryCountry { get; set; }
    public string? DeliveryProvince { get; set; }
    public string? DeliveryDistrict { get; set; }
    public string? DeliveryWard { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryName { get; set; }
    public string? DeliveryPhone { get; set; }
    public string? DeliveryNote { get; set; }
    public decimal? TotalPay { get; set; }
    public DataTable Products { get; set; }
    public DataTable ServiceAdd { get; set; }
    public string Image { get; set; }
    public string Images { get; set; }
    public string Description { get; set; }
    public string Note { get; set; }
    public Guid? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public bool PayNow { get; set; } = false;
    public string? OrderType { get; set; }

    public bool IsValid()
    {
        ValidationResult = new NetDevPack.Domain.ValidationResult(new CreateOrderCrossValidation().Validate(this));
        return ValidationResult.IsValid;
    }
}


public class CreateInvoicePayOrderCrossCommand : Command
{
    public Guid Id { get; set; }
    public decimal TotalPay { get; set; }
    public Guid AccountId { get; set; }
    public Guid? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }

    public bool IsValid()
    {
        ValidationResult = new CreateInvoicePayOrderCrossValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class OrderCrossUpdateAddressCommand : Command
{
    public Guid Id { get; set; }
    public string? DeliveryCountry { get; set; }
    public string? DeliveryProvince { get; set; }
    public string? DeliveryDistrict { get; set; }
    public string? DeliveryWard { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryName { get; set; }
    public string? DeliveryPhone { get; set; }
    public string? DeliveryNote { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }

    public bool IsValid()
    {
        return ValidationResult.IsValid;
    }
}

public class OrderCrossUpdateBidUsernameCommand : Command
{
    public Guid Id { get; set; }
    public string? BidUsername { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }

    public bool IsValid()
    {
        return ValidationResult.IsValid;
    }
}


public class OrderCrossAddServiceCommand : Command
{
    public Guid Id { get; set; }
    public DataTable ServiceAdd { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }

    public bool IsValid()
    {
        return ValidationResult.IsValid;
    }
}

public class OrderCancelCommand : Command
{
    public Guid Id { get; set; }
    public bool IsPayFee { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }
}


public class OrderConfirmDeliveredCommand : Command
{
    public Guid AccountId { get; set; }
    public IList<string> OrderCode { get; set; }

    public bool IsValid()
    {
        return ValidationResult.IsValid;
    }
}

public class UpdateTrackingOrderCommand : Command
{
    public Guid Id { get; set; }
    public int? DomesticStatus { get; set; }
    public string? DomesticTracking { get; set; }
    public string? DomesticCarrier { get; set; }

    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }

}


public class UpdateShippmentRoutingOrderCommand : Command
{
    public Guid Id { get; set; }
    public string? ShipmentRouting { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }

}


public class UpdateInternalNoteCommand : Command
{
    public Guid Id { get; set; }
    public string? InternalNote { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }

}
