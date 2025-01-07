using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class BusinessValidation<T> : AbstractValidator<T> where T : BusinessCommand

{
    protected readonly IBusinessRepository _context;
    private Guid Id;

    public BusinessValidation(IBusinessRepository context)
    {
        _context = context;
    }
    public BusinessValidation(IBusinessRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
        ;
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
        ;
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }


}
public class BusinessAddCommandValidation : BusinessValidation<BusinessAddCommand>
{
    public BusinessAddCommandValidation(IBusinessRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class BusinessEditCommandValidation : BusinessValidation<BusinessEditCommand>
{
    public BusinessEditCommandValidation(IBusinessRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class BusinessDeleteCommandValidation : BusinessValidation<BusinessDeleteCommand>
{
    public BusinessDeleteCommandValidation(IBusinessRepository context) : base(context)
    {
        ValidateId();
    }
}
