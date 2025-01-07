
using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class SalesDiscountValidation<T> : AbstractValidator<T> where T : SalesDiscountCommand

{
    protected readonly ISalesDiscountRepository _context;
    private Guid Id;

    public SalesDiscountValidation(ISalesDiscountRepository context)
    {
        _context = context;
    }
    public SalesDiscountValidation(ISalesDiscountRepository context, Guid id)
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
    private bool IsEditUnique(string code)
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
public class SalesDiscountAddCommandValidation : SalesDiscountValidation<SalesDiscountAddCommand>
{
    public SalesDiscountAddCommandValidation(ISalesDiscountRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class SalesDiscountEditCommandValidation : SalesDiscountValidation<SalesDiscountEditCommand>
{
    public SalesDiscountEditCommandValidation(ISalesDiscountRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class SalesDiscountDuplicateCommandValidation : SalesDiscountValidation<SalesDiscountDuplicateCommand>
{
    public SalesDiscountDuplicateCommandValidation(ISalesDiscountRepository context) : base(context)
    {
        ValidateAddCodeUnique();
        ValidateId();
    }
}

public class SalesDiscountDeleteCommandValidation : SalesDiscountValidation<SalesDiscountDeleteCommand>
{
    public SalesDiscountDeleteCommandValidation(ISalesDiscountRepository context) : base(context)
    {
        ValidateId();
    }
}

public class SalesDiscountUploadFileCommandValidation : SalesDiscountValidation<SalesDiscountUploadFileCommand>
{
    public SalesDiscountUploadFileCommandValidation(ISalesDiscountRepository context) : base(context)
    {
        ValidateId();
    }
}
