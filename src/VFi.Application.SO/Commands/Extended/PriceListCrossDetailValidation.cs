using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class PriceListCrossDetailValidation<T> : AbstractValidator<T> where T : PriceListCrossDetailCommand

{
    protected readonly IPriceListCrossDetailRepository _context;
    private Guid Id;

    public PriceListCrossDetailValidation(IPriceListCrossDetailRepository context)
    {
        _context = context;
    }
    public PriceListCrossDetailValidation(IPriceListCrossDetailRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }


    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
}
public class PriceListCrossDetailAddCommandValidation : PriceListCrossDetailValidation<PriceListCrossDetailAddCommand>
{
    public PriceListCrossDetailAddCommandValidation(IPriceListCrossDetailRepository context) : base(context)
    {
        ValidateId();

    }
}
public class PriceListCrossDetailEditCommandValidation : PriceListCrossDetailValidation<PriceListCrossDetailEditCommand>
{
    public PriceListCrossDetailEditCommandValidation(IPriceListCrossDetailRepository context, Guid id) : base(context, id)
    {
        ValidateId();
    }
}

public class PriceListCrossDetailDeleteCommandValidation : PriceListCrossDetailValidation<PriceListCrossDetailDeleteCommand>
{
    public PriceListCrossDetailDeleteCommandValidation(IPriceListCrossDetailRepository context) : base(context)
    {
        ValidateId();
    }
}
