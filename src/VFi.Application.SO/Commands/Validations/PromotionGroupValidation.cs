using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class PromotionGroupValidation<T> : AbstractValidator<T> where T : PromotionGroupCommand

{
    protected readonly IPromotionGroupRepository _context;
    private Guid Id;

    public PromotionGroupValidation(IPromotionGroupRepository context)
    {
        _context = context;
    }
    public PromotionGroupValidation(IPromotionGroupRepository context, Guid id)
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
public class PromotionGroupAddCommandValidation : PromotionGroupValidation<PromotionGroupAddCommand>
{
    public PromotionGroupAddCommandValidation(IPromotionGroupRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class PromotionGroupEditCommandValidation : PromotionGroupValidation<PromotionGroupEditCommand>
{
    public PromotionGroupEditCommandValidation(IPromotionGroupRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class PromotionGroupDeleteCommandValidation : PromotionGroupValidation<PromotionGroupDeleteCommand>
{
    public PromotionGroupDeleteCommandValidation(IPromotionGroupRepository context) : base(context)
    {
        ValidateId();
    }
}
