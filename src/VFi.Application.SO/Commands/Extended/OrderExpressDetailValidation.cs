using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class OrderExpressDetailValidation<T> : AbstractValidator<T> where T : OrderExpressDetailCommand

{
    protected readonly IOrderExpressDetailRepository _context;
    private Guid Id;

    public OrderExpressDetailValidation(IOrderExpressDetailRepository context)
    {
        _context = context;
    }
    public OrderExpressDetailValidation(IOrderExpressDetailRepository context, Guid id)
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
public class OrderExpressDetailAddCommandValidation : OrderExpressDetailValidation<OrderExpressDetailAddCommand>
{
    public OrderExpressDetailAddCommandValidation(IOrderExpressDetailRepository context) : base(context)
    {
        ValidateId();


    }
}
public class OrderExpressDetailEditCommandValidation : OrderExpressDetailValidation<OrderExpressDetailEditCommand>
{
    public OrderExpressDetailEditCommandValidation(IOrderExpressDetailRepository context, Guid id) : base(context, id)
    {
        ValidateId();
    }
}

public class OrderExpressDetailDeleteCommandValidation : OrderExpressDetailValidation<OrderExpressDetailDeleteCommand>
{
    public OrderExpressDetailDeleteCommandValidation(IOrderExpressDetailRepository context) : base(context)
    {
        ValidateId();
    }
}
