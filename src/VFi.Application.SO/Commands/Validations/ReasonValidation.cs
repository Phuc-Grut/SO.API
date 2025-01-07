using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class ReasonValidation<T> : AbstractValidator<T> where T : ReasonCommand

{
    protected readonly IReasonRepository _context;
    private Guid Id;

    public ReasonValidation(IReasonRepository context)
    {
        _context = context;
    }
    public ReasonValidation(IReasonRepository context, Guid id)
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
public class ReasonAddCommandValidation : ReasonValidation<ReasonAddCommand>
{
    public ReasonAddCommandValidation(IReasonRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class ReasonEditCommandValidation : ReasonValidation<ReasonEditCommand>
{
    public ReasonEditCommandValidation(IReasonRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class ReasonDeleteCommandValidation : ReasonValidation<ReasonDeleteCommand>
{
    public ReasonDeleteCommandValidation(IReasonRepository context) : base(context)
    {
        ValidateId();
    }
}
