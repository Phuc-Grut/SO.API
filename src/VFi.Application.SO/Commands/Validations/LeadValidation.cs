using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class LeadValidation<T> : AbstractValidator<T> where T : LeadCommand

{
    protected readonly ILeadRepository _context;
    private Guid Id;

    public LeadValidation(ILeadRepository context)
    {
        _context = context;
    }
    public LeadValidation(ILeadRepository context, Guid id)
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
    protected void ValidateCode()
    {
        RuleFor(c => c.Code)
            .NotNull()
            .WithMessage("Code not null")
            .MaximumLength(50)
            .WithMessage("Code must not exceed 50 characters");
    }

}
public class LeadAddCommandValidation : LeadValidation<LeadAddCommand>
{
    public LeadAddCommandValidation(ILeadRepository context) : base(context)
    {
        ValidateId();
        ValidateCode();
        ValidateAddCodeUnique();
    }
}
public class LeadEditCommandValidation : LeadValidation<LeadEditCommand>
{
    public LeadEditCommandValidation(ILeadRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class ConvertLeadCommandValidation : LeadValidation<ConvertLeadCommand>
{
    public ConvertLeadCommandValidation(ILeadRepository context, Guid id) : base(context, id)
    {
        ValidateId();
    }
}

public class LeadDeleteCommandValidation : LeadValidation<LeadDeleteCommand>
{
    public LeadDeleteCommandValidation(ILeadRepository context) : base(context)
    {
        ValidateId();
    }
}
