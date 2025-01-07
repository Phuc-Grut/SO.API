using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class DeliveryProductValidation<T> : AbstractValidator<T> where T : DeliveryProductCommand

{
    protected readonly IDeliveryProductRepository _context;
    private Guid Id;

    public DeliveryProductValidation(IDeliveryProductRepository context)
    {
        _context = context;
    }
    public DeliveryProductValidation(IDeliveryProductRepository context, Guid id)
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
public class DeliveryProductAddCommandValidation : DeliveryProductValidation<DeliveryProductAddCommand>
{
    public DeliveryProductAddCommandValidation(IDeliveryProductRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class DeliveryProductEditCommandValidation : DeliveryProductValidation<DeliveryProductEditCommand>
{
    public DeliveryProductEditCommandValidation(IDeliveryProductRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class DeliveryProductDeleteCommandValidation : DeliveryProductValidation<DeliveryProductDeleteCommand>
{
    public DeliveryProductDeleteCommandValidation(IDeliveryProductRepository context) : base(context)
    {
        ValidateId();
    }
}
