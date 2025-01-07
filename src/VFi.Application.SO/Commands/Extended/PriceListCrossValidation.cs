using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class PriceListCrossValidation<T> : AbstractValidator<T> where T : PriceListCrossCommand

{
    protected readonly IPriceListCrossRepository _context;
    private Guid Id;

    public PriceListCrossValidation(IPriceListCrossRepository context)
    {
        _context = context;
    }
    public PriceListCrossValidation(IPriceListCrossRepository context, Guid id)
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
public class PriceListCrossAddCommandValidation : PriceListCrossValidation<PriceListCrossAddCommand>
{
    public PriceListCrossAddCommandValidation(IPriceListCrossRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();

    }
}
public class PriceListCrossEditCommandValidation : PriceListCrossValidation<PriceListCrossEditCommand>
{
    public PriceListCrossEditCommandValidation(IPriceListCrossRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class PriceListCrossDeleteCommandValidation : PriceListCrossValidation<PriceListCrossDeleteCommand>
{
    public PriceListCrossDeleteCommandValidation(IPriceListCrossRepository context) : base(context)
    {
        ValidateId();
    }
}
