using System.Reflection;
using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class LeadCampaignValidation<T> : AbstractValidator<T> where T : LeadCampaignCommand

{
    protected readonly ILeadCampaignRepository _context;
    private Guid Id;

    public LeadCampaignValidation(ILeadCampaignRepository context)
    {
        _context = context;
    }
    public LeadCampaignValidation(ILeadCampaignRepository context, Guid id)
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
public class LeadCampaignAddCommandValidation : LeadCampaignValidation<LeadCampaignAddCommand>
{
    public LeadCampaignAddCommandValidation(ILeadCampaignRepository context) : base(context)
    {
        ValidateId();
    }
}
public class LeadCampaignEditCommandValidation : LeadCampaignValidation<LeadCampaignEditCommand>
{
    public LeadCampaignEditCommandValidation(ILeadCampaignRepository context, Guid id) : base(context, id)
    {
        ValidateId();
    }
}

public class LeadCampaignDeleteCommandValidation : LeadCampaignValidation<LeadCampaignDeleteCommand>
{
    public LeadCampaignDeleteCommandValidation(ILeadCampaignRepository context) : base(context)
    {
        ValidateId();
        ValidateDeleteUsing();
    }
}
