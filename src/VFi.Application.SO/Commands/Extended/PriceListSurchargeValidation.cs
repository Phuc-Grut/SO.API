using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class PriceListSurchargeValidation<T> : AbstractValidator<T> where T : PriceListSurchargeCommand

{
    protected readonly IPriceListSurchargeRepository _context;
    private Guid Id;

    public PriceListSurchargeValidation(IPriceListSurchargeRepository context)
    {
        _context = context;
    }
    public PriceListSurchargeValidation(IPriceListSurchargeRepository context, Guid id)
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
public class PriceListSurchargeAddCommandValidation : PriceListSurchargeValidation<PriceListSurchargeAddCommand>
{
    public PriceListSurchargeAddCommandValidation(IPriceListSurchargeRepository context) : base(context)
    {
    }
}
public class PriceListSurchargeEditCommandValidation : PriceListSurchargeValidation<PriceListSurchargeEditCommand>
{
    public PriceListSurchargeEditCommandValidation(IPriceListSurchargeRepository context, Guid id) : base(context, id)
    {
        ValidateId();
    }
}

public class PriceListSurchargeDeleteCommandValidation : PriceListSurchargeValidation<PriceListSurchargeDeleteCommand>
{
    public PriceListSurchargeDeleteCommandValidation(IPriceListSurchargeRepository context) : base(context)
    {
        ValidateId();
    }
}
