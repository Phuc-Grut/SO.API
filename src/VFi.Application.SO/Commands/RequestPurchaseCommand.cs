using Microsoft.AspNetCore.Http;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class RequestPurchaseCommand : Command
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public Guid? RequestBy { get; set; }
    public string? RequestByName { get; set; }
    public string? RequestByEmail { get; set; }
    public DateTime? RequestDate { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencyName { get; set; }
    public string? Calculation { get; set; }
    public decimal? ExchangeRate { get; set; }
    public string? Proposal { get; set; }
    public string? Note { get; set; }
    public DateTime? ApproveDate { get; set; }
    public Guid? ApproveBy { get; set; }
    public string? ApproveByName { get; set; }
    public string? ApproveComment { get; set; }
    public int? StatusPurchase { get; set; }
    public double? QuantityRequest { get; set; }
    public double? QuantityApproved { get; set; }
    public int? Status { get; set; }
    public string? ModuleCode { get; set; }
    public string? PurchaseRequestCode { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public string? File { get; set; }
    public List<RequestPurchaseProductDto>? Detail { get; set; }
    public List<ListId>? Delete { get; set; }
}

public class AddRequestPurchaseCommand : RequestPurchaseCommand
{
    public AddRequestPurchaseCommand(
        Guid id,
        string code,
        Guid? requestBy,
        string? requestByName,
        string? requestByEmail,
        DateTime? requestDate,
        string? currencyCode,
        string? currencyName,
        string? calculation,
        decimal? exchangeRate,
        string? proposal,
        string? note,
        DateTime? approveDate,
        Guid? approveBy,
        string? approveByName,
        string? approveComment,
        int? status,
        double? quantityRequest,
        double? quantityApproved,
        Guid? orderId,
        string? orderCode,
        string? file,
        List<RequestPurchaseProductDto>? detail)
    {
        Id = id;
        Code = code;
        RequestBy = requestBy;
        RequestByName = requestByName;
        RequestByEmail = requestByEmail;
        RequestDate = requestDate;
        CurrencyCode = currencyCode;
        CurrencyName = currencyName;
        Calculation = calculation;
        ExchangeRate = exchangeRate;
        Proposal = proposal;
        Note = note;
        ApproveDate = approveDate;
        ApproveBy = approveBy;
        ApproveByName = approveByName;
        ApproveComment = approveComment;
        Status = status;
        QuantityRequest = quantityRequest;
        QuantityApproved = quantityApproved;
        OrderId = orderId;
        OrderCode = orderCode;
        File = file;
        Detail = detail;
    }

    public bool IsValid(IRequestPurchaseRepository _context)
    {
        ValidationResult = new AddRequestPurchaseValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class EditRequestPurchaseCommand : RequestPurchaseCommand
{
    public EditRequestPurchaseCommand(
        Guid id,
        string code,
        Guid? requestBy,
        string? requestByName,
        string? requestByEmail,
        DateTime? requestDate,
        string? currencyCode,
        string? currencyName,
        string? calculation,
        decimal? exchangeRate,
        string? proposal,
        string? note,
        DateTime? approveDate,
        Guid? approveBy,
        string? approveByName,
        string? approveComment,
        int? status,
        double? quantityRequest,
        double? quantityApproved,
        Guid? orderId,
        string? orderCode,
        string? file,
        List<RequestPurchaseProductDto>? detail,
        List<ListId>? delete)
    {
        Id = id;
        Code = code;
        RequestBy = requestBy;
        RequestByName = requestByName;
        RequestByEmail = requestByEmail;
        RequestDate = requestDate;
        CurrencyCode = currencyCode;
        CurrencyName = currencyName;
        Calculation = calculation;
        ExchangeRate = exchangeRate;
        Proposal = proposal;
        Note = note;
        ApproveDate = approveDate;
        ApproveBy = approveBy;
        ApproveByName = approveByName;
        ApproveComment = approveComment;
        Status = status;
        QuantityRequest = quantityRequest;
        QuantityApproved = quantityApproved;
        OrderId = orderId;
        OrderCode = orderCode;
        File = file;
        Detail = detail;
        Delete = delete;
    }

    public bool IsValid(IRequestPurchaseRepository _context)
    {
        ValidationResult = new EditRequestPurchaseValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class DeleteRequestPurchaseCommand : RequestPurchaseCommand
{
    public DeleteRequestPurchaseCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IRequestPurchaseRepository _context)
    {
        ValidationResult = new DeteleRequestPurchaseValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class DeleteOrderRequestPurchaseCommand : RequestPurchaseCommand
{
    public DeleteOrderRequestPurchaseCommand(Guid requestPurchaseId, Guid? orderId)
    {
        Id = requestPurchaseId;
        OrderId = orderId;
    }
    public async Task<bool> IsValidAsync(IRequestPurchaseRepository _context)
    {
        ValidationResult = await new DeleteOrderRequestPurchaseValidation(_context, Id).ValidateAsync(this);
        return ValidationResult.IsValid;
    }
}

public class RequestPurchaseProcessCommand : Command
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public string? ApproveComment { get; set; }
    public int? POStatus { get; set; }
    public RequestPurchaseProcessCommand(
         Guid id,
         int? status,
         string? approveComment,
         int? poStatus
      )
    {
        Id = id;
        Status = status;
        ApproveComment = approveComment;
        POStatus = poStatus;
    }
}

public class RequestPurchaseDuplicateCommand : RequestPurchaseCommand
{
    public Guid RequestPurchaseId { get; set; }
    public RequestPurchaseDuplicateCommand(
        Guid id,
        Guid requestPurchaseId,
        string code)
    {
        Id = id;
        Code = code;
        RequestPurchaseId = requestPurchaseId;
    }
    public bool IsValid(IRequestPurchaseRepository _context)
    {
        ValidationResult = new RequestPurchasetDuplicateCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class RequestPurchasePurchaseCommand : Command
{
    public Guid Id { get; set; }
    public int? POStatus { get; set; }
    public RequestPurchasePurchaseCommand(
        Guid id,
        int? poStatus
    )
    {
        Id = id;
        POStatus = poStatus;
    }
}

public class POPurchaseProductCommand : Command
{
    public List<POPurchaseProductDto>? ListUpdate { get; set; }
    public POPurchaseProductCommand(
        List<POPurchaseProductDto>? listUpdate
    )
    {
        ListUpdate = listUpdate;
    }
}

public class UpdatePurchaseQtyCommand : Command
{
    public int? NotCancel { get; set; }
    public Guid Id { get; set; }
    public string? PurchaseRequestCode { get; set; }
    public UpdatePurchaseQtyCommand(
        int? notCancel,
        Guid id,
        string? purchaseRequestCode
    )
    {
        NotCancel = notCancel;
        Id = id;
        PurchaseRequestCode = purchaseRequestCode;
    }
}

public class ValidateExcelExportRequestPurchase
{
    public IFormFile File { get; set; } = null!;
    public string SheetId { get; set; } = null!;
    public int HeaderRow { get; set; }
    public int? ProductCode { get; set; }
    public int? ProductName { get; set; }
    public int? UnitCode { get; set; }
    public int? UnitName { get; set; }
    public int? QuantityRequest { get; set; }
    public int? UnitPrice { get; set; }
    public int? DeliveryDate { get; set; }
    public int? PriorityLevel { get; set; }
    public int? Note { get; set; }
}
public class RequestPurchaseAddOrdersCommand : RequestPurchaseCommand
{
    public Guid Id { get; set; }
    public List<Guid> OrderIds { get; set; }
    public RequestPurchaseAddOrdersCommand(Guid id, List<Guid> orderIds)
    {
        Id = id;
        OrderIds = orderIds;
    }
    public RequestPurchaseAddOrdersCommand() { }

    public async Task<bool> IsValidAsync(IRequestPurchaseRepository _context,
        IRequestPurchaseProductRepository _requestPurchaseProductRepository,
        IOrderRepository _orderRepository)
    {
        ValidationResult = await new RequestPurchaseAddOrdersCommandValidation(_context,
            _requestPurchaseProductRepository, _orderRepository, Id).ValidateAsync(this);
        return ValidationResult.IsValid;
    }
}
