using System.Reflection;
using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class LeadProfileValidation<T> : AbstractValidator<T> where T : LeadProfileCommand

{
    protected readonly ILeadProfileRepository _context;
    private Guid Id;

    public LeadProfileValidation(ILeadProfileRepository context)
    {
        _context = context;
    }
    public LeadProfileValidation(ILeadProfileRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }

    private bool IsUsing(Guid id)
    {
        return _context.CheckUsing(id);
    }
    protected void ValidateDeleteUsing()
    {
        RuleFor(x => x.Id).Must(IsUsing).WithMessage("Item is being used");
    }

}
public class LeadProfileAddCommandValidation : LeadProfileValidation<LeadProfileAddCommand>
{
    public LeadProfileAddCommandValidation(ILeadProfileRepository context) : base(context)
    {
        ValidateId();
    }
}
public class LeadProfileEditCommandValidation : LeadProfileValidation<LeadProfileEditCommand>
{
    public LeadProfileEditCommandValidation(ILeadProfileRepository context, Guid id) : base(context, id)
    {
        ValidateId();
    }
}

public class LeadProfileDeleteCommandValidation : LeadProfileValidation<LeadProfileDeleteCommand>
{
    public LeadProfileDeleteCommandValidation(ILeadProfileRepository context) : base(context)
    {
        ValidateId();
        ValidateDeleteUsing();
    }
}
