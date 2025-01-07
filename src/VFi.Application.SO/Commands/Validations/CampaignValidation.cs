using System.Reflection;
using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class CampaignValidation<T> : AbstractValidator<T> where T : CampaignCommand

{
    protected readonly ICampaignRepository _context;
    private Guid Id;

    public CampaignValidation(ICampaignRepository context)
    {
        _context = context;
    }
    public CampaignValidation(ICampaignRepository context, Guid id)
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
    protected void ValidateCode()
    {
        RuleFor(c => c.Code)
            .NotNull()
            .WithMessage("Code not null")
            .MaximumLength(50)
            .WithMessage("Code must not exceed 50 characters");
    }

    private bool IsUsing(Guid id)
    {
        return _context.CheckUsing(id);
    }
    protected void ValidateDeleteUsing()
    {
        RuleFor(x => x.Id).Must(IsUsing).WithMessage("Campaign has been used");
    }

}
public class CampaignAddCommandValidation : CampaignValidation<CampaignAddCommand>
{
    public CampaignAddCommandValidation(ICampaignRepository context) : base(context)
    {
        ValidateId();
        ValidateCode();
        ValidateAddCodeUnique();
    }
}
public class CampaignEditCommandValidation : CampaignValidation<CampaignEditCommand>
{
    public CampaignEditCommandValidation(ICampaignRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateCode();
        ValidateEditCodeUnique();
    }
}

public class CampaignDeleteCommandValidation : CampaignValidation<CampaignDeleteCommand>
{
    public CampaignDeleteCommandValidation(ICampaignRepository context) : base(context)
    {
        ValidateId();
        ValidateDeleteUsing();
    }
}
