using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class PriceListPurchaseDetailValidation<T> : AbstractValidator<T> where T : PriceListPurchaseDetailCommand

{
    protected readonly IPriceListPurchaseDetailRepository _context;
    private Guid Id;

    public PriceListPurchaseDetailValidation(IPriceListPurchaseDetailRepository context)
    {
        _context = context;
    }
    public PriceListPurchaseDetailValidation(IPriceListPurchaseDetailRepository context, Guid id)
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
public class PriceListPurchaseDetailAddCommandValidation : PriceListPurchaseDetailValidation<PriceListPurchaseDetailAddCommand>
{
    public PriceListPurchaseDetailAddCommandValidation(IPriceListPurchaseDetailRepository context) : base(context)
    {
        ValidateId();
        //ValidateAddCodeUnique();

    }
}
public class PriceListPurchaseDetailEditCommandValidation : PriceListPurchaseDetailValidation<PriceListPurchaseDetailEditCommand>
{
    public PriceListPurchaseDetailEditCommandValidation(IPriceListPurchaseDetailRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        //ValidateEditCodeUnique();
    }
}

public class PriceListPurchaseDetailDeleteCommandValidation : PriceListPurchaseDetailValidation<PriceListPurchaseDetailDeleteCommand>
{
    public PriceListPurchaseDetailDeleteCommandValidation(IPriceListPurchaseDetailRepository context) : base(context)
    {
        ValidateId();
    }
}
