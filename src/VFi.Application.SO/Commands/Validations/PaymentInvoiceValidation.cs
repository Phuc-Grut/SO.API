using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class PaymentInvoiceValidation<T> : AbstractValidator<T> where T : PaymentInvoiceCommand

{
    protected readonly IPaymentInvoiceRepository _context;
    private Guid Id;

    public PaymentInvoiceValidation(IPaymentInvoiceRepository context)
    {
        _context = context;
    }
    public PaymentInvoiceValidation(IPaymentInvoiceRepository context, Guid id)
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
public class PaymentInvoiceAddCommandValidation : PaymentInvoiceValidation<PaymentInvoiceAddCommand>
{
    public PaymentInvoiceAddCommandValidation(IPaymentInvoiceRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class PaymentInvoiceEditCommandValidation : PaymentInvoiceValidation<PaymentInvoiceEditCommand>
{
    public PaymentInvoiceEditCommandValidation(IPaymentInvoiceRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class PaymentInvoiceDeleteCommandValidation : PaymentInvoiceValidation<PaymentInvoiceDeleteCommand>
{
    public PaymentInvoiceDeleteCommandValidation(IPaymentInvoiceRepository context) : base(context)
    {
        ValidateId();
    }
}
