using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class OrderFulfillmentDetailValidation<T> : AbstractValidator<T> where T : OrderFulfillmentDetailCommand

{
    protected readonly IOrderFulfillmentDetailRepository _context;
    private Guid Id;

    public OrderFulfillmentDetailValidation(IOrderFulfillmentDetailRepository context)
    {
        _context = context;
    }
    public OrderFulfillmentDetailValidation(IOrderFulfillmentDetailRepository context, Guid id)
    {
        _context = context;
        Id = id;
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


    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
}
public class OrderFulfillmentDetailAddCommandValidation : OrderFulfillmentDetailValidation<OrderFulfillmentDetailAddCommand>
{
    public OrderFulfillmentDetailAddCommandValidation(IOrderFulfillmentDetailRepository context) : base(context)
    {
        ValidateId();

    }
}
public class OrderFulfillmentDetailEditCommandValidation : OrderFulfillmentDetailValidation<OrderFulfillmentDetailEditCommand>
{
    public OrderFulfillmentDetailEditCommandValidation(IOrderFulfillmentDetailRepository context, Guid id) : base(context, id)
    {
        ValidateId();
    }
}

public class OrderFulfillmentDetailDeleteCommandValidation : OrderFulfillmentDetailValidation<OrderFulfillmentDetailDeleteCommand>
{
    public OrderFulfillmentDetailDeleteCommandValidation(IOrderFulfillmentDetailRepository context) : base(context)
    {
        ValidateId();
    }
}
