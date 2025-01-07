using FluentValidation;
using VFi.Application.SO.Commands.Extended;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class OrderExpressValidation<T> : AbstractValidator<T> where T : OrderExpressCommand

{
    protected readonly IOrderExpressRepository _context;
    private Guid Id;

    public OrderExpressValidation(IOrderExpressRepository context)
    {
        _context = context;
    }
    public OrderExpressValidation(IOrderExpressRepository context, Guid id)
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
public class OrderExpressAddCommandValidation : OrderExpressValidation<OrderExpressAddCommand>
{
    public OrderExpressAddCommandValidation(IOrderExpressRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();

    }
}
public class OrderExpressEditCommandValidation : OrderExpressValidation<OrderExpressEditCommand>
{
    public OrderExpressEditCommandValidation(IOrderExpressRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class OrderExpressDeleteCommandValidation : OrderExpressValidation<OrderExpressDeleteCommand>
{
    public OrderExpressDeleteCommandValidation(IOrderExpressRepository context) : base(context)
    {
        ValidateId();
    }
}

public class OrderExpressCreateByCustomerCommandValidation : OrderExpressValidation<OrderExpressCreateByCustomerCommand>
{
    public OrderExpressCreateByCustomerCommandValidation(IOrderExpressRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();

    }
}
