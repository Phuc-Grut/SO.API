using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class PaymentMethodValidation<T> : AbstractValidator<T> where T : PaymentMethodCommand

{
    protected readonly IPaymentMethodRepository _context;
    private Guid Id;

    public PaymentMethodValidation(IPaymentMethodRepository context)
    {
        _context = context;
    }
    public PaymentMethodValidation(IPaymentMethodRepository context, Guid id)
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
public class PaymentMethodAddCommandValidation : PaymentMethodValidation<PaymentMethodAddCommand>
{
    public PaymentMethodAddCommandValidation(IPaymentMethodRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();

    }
}
public class PaymentMethodEditCommandValidation : PaymentMethodValidation<PaymentMethodEditCommand>
{
    public PaymentMethodEditCommandValidation(IPaymentMethodRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class PaymentMethodDeleteCommandValidation : PaymentMethodValidation<PaymentMethodDeleteCommand>
{
    public PaymentMethodDeleteCommandValidation(IPaymentMethodRepository context) : base(context)
    {
        ValidateId();
    }
}
