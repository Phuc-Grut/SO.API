using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Enums;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;


public abstract class RequestPurchaseValidation<T> : AbstractValidator<T> where T : RequestPurchaseCommand
{
    protected readonly IRequestPurchaseRepository _context;
    protected readonly IOrderRepository _orderRepository;
    protected readonly IRequestPurchaseProductRepository _requestPurchaseProductRepository;
    protected Guid Id;


    public RequestPurchaseValidation(IRequestPurchaseRepository context)
    {
        _context = context;
    }

    public RequestPurchaseValidation(IRequestPurchaseRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }

    public RequestPurchaseValidation(IRequestPurchaseRepository context,
        IRequestPurchaseProductRepository requestPurchaseProductRepository,
        IOrderRepository orderRepository, Guid id)
    {
        _context = context;
        _orderRepository = orderRepository;
        _requestPurchaseProductRepository = requestPurchaseProductRepository;
        Id = id;
    }

    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
    }

    private bool IsAddUnique(string code)
    {
        var model = _context.GetByCode(code).Result;

        if (model == null)
        {
            return true;
        }

        return false;
    }
    private bool IsEditUnique(string? code)
    {
        var model = _context.GetByCode(code).Result;

        if (model == null || model.Id == Id)
        {
            return true;
        }

        return false;
    }
    protected void ValidateEditCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    protected void ValidateCode()
    {
        RuleFor(c => c.Code)
            .NotNull()
            .WithMessage("Code not null")
            .MaximumLength(50)
            .WithMessage("Code must not exceed 50 characters");
    }
}


public class AddRequestPurchaseValidation : RequestPurchaseValidation<AddRequestPurchaseCommand>
{
    public AddRequestPurchaseValidation(IRequestPurchaseRepository context) : base(context)
    {
        ValidateId();
        ValidateCode();
        ValidateAddCodeUnique();
    }
}

public class EditRequestPurchaseValidation : RequestPurchaseValidation<EditRequestPurchaseCommand>
{
    public EditRequestPurchaseValidation(IRequestPurchaseRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateCode();
        ValidateEditCodeUnique();
    }
}

public class DeteleRequestPurchaseValidation : RequestPurchaseValidation<DeleteRequestPurchaseCommand>
{
    public DeteleRequestPurchaseValidation(IRequestPurchaseRepository context) : base(context)
    {
        ValidateId();
    }
}

public class RequestPurchasetDuplicateCommandValidation : RequestPurchaseValidation<RequestPurchaseDuplicateCommand>
{
    public RequestPurchasetDuplicateCommandValidation(IRequestPurchaseRepository context) : base(context)
    {
        ValidateAddCodeUnique();
        ValidateId();
    }
}
public class DeleteOrderRequestPurchaseValidation : RequestPurchaseValidation<DeleteOrderRequestPurchaseCommand>
{
    public Guid RequestPurchaseId { get; set; }
    public DeleteOrderRequestPurchaseValidation(IRequestPurchaseRepository context, Guid requestPurchaseId)
        : base(context)
    {
        RequestPurchaseId = requestPurchaseId;
        ValidateRequestPurchaseId();
        ValidateOrderId();
    }


    private void ValidateRequestPurchaseId()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id cannot be empty")
            .WithErrorCode(ErrorCode.TRUNGMA)
            .MustAsync(async (x, cancellation) =>
             {
                 var requestPurchase = await _context.GetById(x);
                 return requestPurchase?.Id == x;
             })
            .WithMessage("Purchase request does not exist.")
            .WithErrorCode(ErrorCode.TRUNGMA);
    }

    protected void ValidateOrderId()
    {
        RuleFor(c => c.OrderId)
            .NotEmpty()
            .WithMessage("Order does not exist.")
            .MustAsync(async (orderId, cancellation) =>
            {
                var requestPurchase = await _context.GetRemoveOrderId(RequestPurchaseId, orderId);
                return requestPurchase is not null
                && requestPurchase.Status == (int)RequestPurchaseStatus.Pending
                && requestPurchase.RequestPurchaseProduct.Any(p => p.OrderId == orderId);
            })
            .WithMessage("Purchase request has already been approved or purchased.");
    }
}

public class RequestPurchaseAddOrdersCommandValidation : RequestPurchaseValidation<RequestPurchaseAddOrdersCommand>
{
    public RequestPurchaseAddOrdersCommandValidation(
        IRequestPurchaseRepository context,
        IRequestPurchaseProductRepository requestPurchaseProductRepository,
        IOrderRepository orderRepository,
        Guid id) : base(context, requestPurchaseProductRepository, orderRepository, id)
    {
        ValidateRequestPurchaseId();
        ValidateStatus(id);
        ValidateOrderIds();
        ValidateOrderStatuses();
    }
    private void ValidateRequestPurchaseId()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty).WithMessage("Id cannot be empty");
    }
    private void ValidateOrderIds()
    {
        RuleFor(x => x.OrderIds)
            .NotEmpty().WithMessage("Order Ids cannot be empty")
            .MustAsync(async (orderIds, cancellationToken) => await AreOrderIdsIsUnique(orderIds, Id, cancellationToken))
            .WithMessage("Some OrderIds already exist");
    }
    private async Task<bool> AreOrderIdsIsUnique(List<Guid> orderIds, Guid requestPurchaseId, CancellationToken cancellationToken)
    {
        var existingProducts = await _requestPurchaseProductRepository
            .Filter(new Dictionary<string, object> { { "requestPurchaseId", requestPurchaseId } });

        var existingOrderIds = existingProducts
            .Where(x => x.OrderId.HasValue)
            .Select(x => x.OrderId.Value)
            .ToList();
        return !orderIds.Any(orderId => existingOrderIds.Contains(orderId));
    }

    private void ValidateStatus(Guid Id)
    {
        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellationToken) => await IsStatusValid(id))
            .WithMessage("The status of the request Purchase must be waiting accept");
    }

    private async Task<bool> IsStatusValid(Guid id)
    {
        var requestPurchase = await _context.GetById(id);
        return requestPurchase != null && (requestPurchase.Status == 0);
    }
    private void ValidateOrderStatuses()
    {
        RuleFor(x => x.OrderIds)
            .MustAsync(async (orderIds, cancellationToken) => await AreOrderStatusesValid(orderIds))
            .WithMessage("All orders status must be PendingConfirm or WaitForPurchase .");
    }

    private async Task<bool> AreOrderStatusesValid(List<Guid> orderIds)
    {
        foreach (var orderId in orderIds)
        {
            var order = await _orderRepository.GetById(orderId);
            if (order != null && order.Status == (int)OrderStatus.PendingConfirm || order.Status == (int)OrderStatus.WaitForPurchase)
            {
                return true;
            }
        }
        return false;
    }
}
