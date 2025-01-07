using System.Reflection;
using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class CampaignStatusValidation<T> : AbstractValidator<T> where T : CampaignStatusCommand

{
    protected readonly ICampaignStatusRepository _context;
    private Guid Id;

    public CampaignStatusValidation(ICampaignStatusRepository context)
    {
        _context = context;
    }
    public CampaignStatusValidation(ICampaignStatusRepository context, Guid id)
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
        RuleFor(x => x.Id).Must(IsUsing).WithMessage("CampaignStatus has been used");
    }

}
public class CampaignStatusAddCommandValidation : CampaignStatusValidation<CampaignStatusAddCommand>
{
    public CampaignStatusAddCommandValidation(ICampaignStatusRepository context) : base(context)
    {
        ValidateId();
    }
}
public class CampaignStatusEditCommandValidation : CampaignStatusValidation<CampaignStatusEditCommand>
{
    public CampaignStatusEditCommandValidation(ICampaignStatusRepository context, Guid id) : base(context, id)
    {
        ValidateId();
    }
}

public class CampaignStatusDeleteCommandValidation : CampaignStatusValidation<CampaignStatusDeleteCommand>
{
    public CampaignStatusDeleteCommandValidation(ICampaignStatusRepository context) : base(context)
    {
        ValidateId();
        ValidateDeleteUsing();
    }
}
