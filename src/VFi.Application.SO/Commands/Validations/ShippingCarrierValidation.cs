using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class ShippingCarrierValidation<T> : AbstractValidator<T> where T : ShippingCarrierCommand

{
    protected readonly IShippingCarrierRepository _context;
    private Guid Id;

    public ShippingCarrierValidation(IShippingCarrierRepository context)
    {
        _context = context;
    }
    public ShippingCarrierValidation(IShippingCarrierRepository context, Guid id)
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
public class ShippingCarrierAddCommandValidation : ShippingCarrierValidation<ShippingCarrierAddCommand>
{
    public ShippingCarrierAddCommandValidation(IShippingCarrierRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class ShippingCarrierEditCommandValidation : ShippingCarrierValidation<ShippingCarrierEditCommand>
{
    public ShippingCarrierEditCommandValidation(IShippingCarrierRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class ShippingCarrierDeleteCommandValidation : ShippingCarrierValidation<ShippingCarrierDeleteCommand>
{
    public ShippingCarrierDeleteCommandValidation(IShippingCarrierRepository context) : base(context)
    {
        ValidateId();
    }
}
