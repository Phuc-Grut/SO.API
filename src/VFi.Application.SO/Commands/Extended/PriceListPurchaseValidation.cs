using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class PriceListPurchaseValidation<T> : AbstractValidator<T> where T : PriceListPurchaseCommand

{
    protected readonly IPriceListPurchaseRepository _context;
    private Guid Id;

    public PriceListPurchaseValidation(IPriceListPurchaseRepository context)
    {
        _context = context;
    }
    public PriceListPurchaseValidation(IPriceListPurchaseRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    //protected void ValidateAddCodeUnique()
    //{
    //    RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code already exists");
    //}

    //private bool IsAddUnique(string code)
    //{
    //    var model = _context.GetByCode(code).Result;

    //    if (model == null)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
    //private bool IsEditUnique(string? code)
    //{
    //    var model = _context.GetByCode(code).Result;

    //    if (model == null || model.Id == Id)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
    //protected void ValidateEditCodeUnique()
    //{
    //    RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code already exists");
    //}

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
}
public class PriceListPurchaseAddCommandValidation : PriceListPurchaseValidation<PriceListPurchaseAddCommand>
{
    public PriceListPurchaseAddCommandValidation(IPriceListPurchaseRepository context) : base(context)
    {
        ValidateId();
        //ValidateAddCodeUnique();

    }
}
public class PriceListPurchaseEditCommandValidation : PriceListPurchaseValidation<PriceListPurchaseEditCommand>
{
    public PriceListPurchaseEditCommandValidation(IPriceListPurchaseRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        //ValidateEditCodeUnique();
    }
}

public class PriceListPurchaseDeleteCommandValidation : PriceListPurchaseValidation<PriceListPurchaseDeleteCommand>
{
    public PriceListPurchaseDeleteCommandValidation(IPriceListPurchaseRepository context) : base(context)
    {
        ValidateId();
    }
}
