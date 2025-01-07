using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class StorePriceListValidation<T> : AbstractValidator<T> where T : StorePriceListCommand

{
    protected readonly IStorePriceListRepository _context;
    private Guid Id;

    public StorePriceListValidation(IStorePriceListRepository context)
    {
        _context = context;
    }
    public StorePriceListValidation(IStorePriceListRepository context, Guid id)
    {
        _context = context;
        Id = id;
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


    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }


}
public class StorePriceListAddCommandValidation : StorePriceListValidation<StorePriceListAddCommand>
{
    public StorePriceListAddCommandValidation(IStorePriceListRepository context) : base(context)
    {
        ValidateId();
    }
}
public class StorePriceListEditCommandValidation : StorePriceListValidation<StorePriceListEditCommand>
{
    public StorePriceListEditCommandValidation(IStorePriceListRepository context, Guid id) : base(context, id)
    {
        ValidateId();
    }
}

public class StorePriceListDeleteCommandValidation : StorePriceListValidation<StorePriceListDeleteCommand>
{
    public StorePriceListDeleteCommandValidation(IStorePriceListRepository context) : base(context)
    {
        ValidateId();
    }
}
