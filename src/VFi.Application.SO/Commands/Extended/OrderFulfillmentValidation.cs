using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class OrderFulfillmentValidation<T> : AbstractValidator<T> where T : OrderFulfillmentCommand

{
    protected readonly IOrderFulfillmentRepository _context;
    private Guid Id;

    public OrderFulfillmentValidation(IOrderFulfillmentRepository context)
    {
        _context = context;
    }
    public OrderFulfillmentValidation(IOrderFulfillmentRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code already exists");
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
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code already exists");
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
}
public class OrderFulfillmentAddCommandValidation : OrderFulfillmentValidation<OrderFulfillmentAddCommand>
{
    public OrderFulfillmentAddCommandValidation(IOrderFulfillmentRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();

    }
}
public class OrderFulfillmentEditCommandValidation : OrderFulfillmentValidation<OrderFulfillmentEditCommand>
{
    public OrderFulfillmentEditCommandValidation(IOrderFulfillmentRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class OrderFulfillmentDeleteCommandValidation : OrderFulfillmentValidation<OrderFulfillmentDeleteCommand>
{
    public OrderFulfillmentDeleteCommandValidation(IOrderFulfillmentRepository context) : base(context)
    {
        ValidateId();
    }
}
