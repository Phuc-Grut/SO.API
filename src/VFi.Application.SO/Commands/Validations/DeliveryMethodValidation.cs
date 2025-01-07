using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class DeliveryMethodValidation<T> : AbstractValidator<T> where T : DeliveryMethodCommand

{
    protected readonly IDeliveryMethodRepository _context;
    private Guid Id;

    public DeliveryMethodValidation(IDeliveryMethodRepository context)
    {
        _context = context;
    }
    public DeliveryMethodValidation(IDeliveryMethodRepository context, Guid id)
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
public class DeliveryMethodAddCommandValidation : DeliveryMethodValidation<DeliveryMethodAddCommand>
{
    public DeliveryMethodAddCommandValidation(IDeliveryMethodRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class DeliveryMethodEditCommandValidation : DeliveryMethodValidation<DeliveryMethodEditCommand>
{
    public DeliveryMethodEditCommandValidation(IDeliveryMethodRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class DeliveryMethodDeleteCommandValidation : DeliveryMethodValidation<DeliveryMethodDeleteCommand>
{
    public DeliveryMethodDeleteCommandValidation(IDeliveryMethodRepository context) : base(context)
    {
        ValidateId();
    }
}
