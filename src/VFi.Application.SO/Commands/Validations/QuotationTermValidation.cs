using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class QuotationTermValidation<T> : AbstractValidator<T> where T : QuotationTermCommand

{
    protected readonly IQuotationTermRepository _context;
    private Guid Id;

    public QuotationTermValidation(IQuotationTermRepository context)
    {
        _context = context;
    }
    public QuotationTermValidation(IQuotationTermRepository context, Guid id)
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
public class QuotationTermAddCommandValidation : QuotationTermValidation<QuotationTermAddCommand>
{
    public QuotationTermAddCommandValidation(IQuotationTermRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class QuotationTermEditCommandValidation : QuotationTermValidation<QuotationTermEditCommand>
{
    public QuotationTermEditCommandValidation(IQuotationTermRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class QuotationTermDeleteCommandValidation : QuotationTermValidation<QuotationTermDeleteCommand>
{
    public QuotationTermDeleteCommandValidation(IQuotationTermRepository context) : base(context)
    {
        ValidateId();
    }
}
