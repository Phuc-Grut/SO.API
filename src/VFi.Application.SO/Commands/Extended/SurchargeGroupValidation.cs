using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class SurchargeGroupValidation<T> : AbstractValidator<T> where T : SurchargeGroupCommand

{
    protected readonly ISurchargeGroupRepository _context;
    private Guid Id;

    public SurchargeGroupValidation(ISurchargeGroupRepository context)
    {
        _context = context;
    }
    public SurchargeGroupValidation(ISurchargeGroupRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code already exists");
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
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code already exists");
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
}
public class SurchargeGroupAddCommandValidation : SurchargeGroupValidation<SurchargeGroupAddCommand>
{
    public SurchargeGroupAddCommandValidation(ISurchargeGroupRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();

    }
}
public class SurchargeGroupEditCommandValidation : SurchargeGroupValidation<SurchargeGroupEditCommand>
{
    public SurchargeGroupEditCommandValidation(ISurchargeGroupRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class SurchargeGroupDeleteCommandValidation : SurchargeGroupValidation<SurchargeGroupDeleteCommand>
{
    public SurchargeGroupDeleteCommandValidation(ISurchargeGroupRepository context) : base(context)
    {
        ValidateId();
    }
}
