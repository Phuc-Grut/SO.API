using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class RouteShippingValidation<T> : AbstractValidator<T> where T : RouteShippingCommand

{
    protected readonly IRouteShippingRepository _context;
    private Guid Id;

    public RouteShippingValidation(IRouteShippingRepository context)
    {
        _context = context;
    }
    public RouteShippingValidation(IRouteShippingRepository context, Guid id)
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
public class RouteShippingAddCommandValidation : RouteShippingValidation<RouteShippingAddCommand>
{
    public RouteShippingAddCommandValidation(IRouteShippingRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();

    }
}
public class RouteShippingEditCommandValidation : RouteShippingValidation<RouteShippingEditCommand>
{
    public RouteShippingEditCommandValidation(IRouteShippingRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class RouteShippingDeleteCommandValidation : RouteShippingValidation<RouteShippingDeleteCommand>
{
    public RouteShippingDeleteCommandValidation(IRouteShippingRepository context) : base(context)
    {
        ValidateId();
    }
}
