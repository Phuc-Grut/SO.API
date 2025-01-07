using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class ShippingMethodValidation<T> : AbstractValidator<T> where T : ShippingMethodCommand

{
    protected readonly IShippingMethodRepository _context;
    private Guid Id;

    public ShippingMethodValidation(IShippingMethodRepository context)
    {
        _context = context;
    }
    public ShippingMethodValidation(IShippingMethodRepository context, Guid id)
    {
        _context = context;
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
}
public class ShippingMethodAddCommandValidation : ShippingMethodValidation<ShippingMethodAddCommand>
{
    public ShippingMethodAddCommandValidation(IShippingMethodRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class ShippingMethodEditCommandValidation : ShippingMethodValidation<ShippingMethodEditCommand>
{
    public ShippingMethodEditCommandValidation(IShippingMethodRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class ShippingMethodDeleteCommandValidation : ShippingMethodValidation<ShippingMethodDeleteCommand>
{
    public ShippingMethodDeleteCommandValidation(IShippingMethodRepository context) : base(context)
    {
        ValidateId();
    }
}
