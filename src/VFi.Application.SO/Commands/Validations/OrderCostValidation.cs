using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class OrderCostValidation<T> : AbstractValidator<T> where T : OrderCostCommand

{
    protected readonly IOrderCostRepository _context;
    private Guid Id;

    public OrderCostValidation(IOrderCostRepository context)
    {
        _context = context;
    }
    public OrderCostValidation(IOrderCostRepository context, Guid id)
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
public class OrderCostAddCommandValidation : OrderCostValidation<OrderCostAddCommand>
{
    public OrderCostAddCommandValidation(IOrderCostRepository context) : base(context)
    {
        ValidateId();
    }
}
public class OrderCostEditCommandValidation : OrderCostValidation<OrderCostEditCommand>
{
    public OrderCostEditCommandValidation(IOrderCostRepository context, Guid id) : base(context, id)
    {
        ValidateId();
    }
}

public class OrderCostDeleteCommandValidation : OrderCostValidation<OrderCostDeleteCommand>
{
    public OrderCostDeleteCommandValidation(IOrderCostRepository context) : base(context)
    {
        ValidateId();
    }
}
