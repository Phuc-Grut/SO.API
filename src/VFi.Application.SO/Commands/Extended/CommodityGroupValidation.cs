using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class CommodityGroupValidation<T> : AbstractValidator<T> where T : CommodityGroupCommand

{
    protected readonly ICommodityGroupRepository _context;
    private Guid Id;

    public CommodityGroupValidation(ICommodityGroupRepository context)
    {
        _context = context;
    }
    public CommodityGroupValidation(ICommodityGroupRepository context, Guid id)
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
public class CommodityGroupAddCommandValidation : CommodityGroupValidation<CommodityGroupAddCommand>
{
    public CommodityGroupAddCommandValidation(ICommodityGroupRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();

    }
}
public class CommodityGroupEditCommandValidation : CommodityGroupValidation<CommodityGroupEditCommand>
{
    public CommodityGroupEditCommandValidation(ICommodityGroupRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class CommodityGroupDeleteCommandValidation : CommodityGroupValidation<CommodityGroupDeleteCommand>
{
    public CommodityGroupDeleteCommandValidation(ICommodityGroupRepository context) : base(context)
    {
        ValidateId();
    }
}
