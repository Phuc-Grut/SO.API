using System.Reflection;
using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class LeadActivityValidation<T> : AbstractValidator<T> where T : LeadActivityCommand

{
    protected readonly ILeadActivityRepository _context;
    private Guid Id;

    public LeadActivityValidation(ILeadActivityRepository context)
    {
        _context = context;
    }
    public LeadActivityValidation(ILeadActivityRepository context, Guid id)
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
public class LeadActivityAddCommandValidation : LeadActivityValidation<LeadActivityAddCommand>
{
    public LeadActivityAddCommandValidation(ILeadActivityRepository context) : base(context)
    {
        ValidateId();
    }
}
public class LeadActivityEditCommandValidation : LeadActivityValidation<LeadActivityEditCommand>
{
    public LeadActivityEditCommandValidation(ILeadActivityRepository context, Guid id) : base(context, id)
    {
        ValidateId();
    }
}

public class LeadActivityDeleteCommandValidation : LeadActivityValidation<LeadActivityDeleteCommand>
{
    public LeadActivityDeleteCommandValidation(ILeadActivityRepository context) : base(context)
    {
        ValidateId();
        ValidateDeleteUsing();
    }
}
