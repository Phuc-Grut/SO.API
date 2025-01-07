using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class PurchaseGroupValidation<T> : AbstractValidator<T> where T : PurchaseGroupCommand

{
    protected readonly IPurchaseGroupRepository _context;
    private Guid Id;

    public PurchaseGroupValidation(IPurchaseGroupRepository context)
    {
        _context = context;
    }
    public PurchaseGroupValidation(IPurchaseGroupRepository context, Guid id)
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
public class PurchaseGroupAddCommandValidation : PurchaseGroupValidation<PurchaseGroupAddCommand>
{
    public PurchaseGroupAddCommandValidation(IPurchaseGroupRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();

    }
}
public class PurchaseGroupEditCommandValidation : PurchaseGroupValidation<PurchaseGroupEditCommand>
{
    public PurchaseGroupEditCommandValidation(IPurchaseGroupRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class PurchaseGroupDeleteCommandValidation : PurchaseGroupValidation<PurchaseGroupDeleteCommand>
{
    public PurchaseGroupDeleteCommandValidation(IPurchaseGroupRepository context) : base(context)
    {
        ValidateId();
    }
}
