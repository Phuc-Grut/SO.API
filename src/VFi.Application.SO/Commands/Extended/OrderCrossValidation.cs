using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Extended;

public abstract class OrderCrossValidation<T> : AbstractValidator<T> where T : OrderCrossCommand
{
    protected readonly IOrderRepository _context;

    protected OrderCrossValidation(IOrderRepository context)
    {
        _context = context;
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }

    protected void ValidateCode()
    {
        RuleFor(c => c.Code)
            .NotNull()
            .WithMessage("Code not null")
            .MaximumLength(50)
            .WithMessage("Code must not exceed 50 characters");
    }

    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code already exists");
    }

    private bool IsAddUnique(string code)
    {
        var model = _context.GetByCode(code).Result;

        if (model is null)
        {
            return true;
        }

        return false;
    }

}

public class AddOrderCrossValidation : OrderCrossValidation<AddOrderCrossCommand>
{
    public AddOrderCrossValidation(IOrderRepository context) : base(context)
    {
        ValidateId();
        ValidateCode();
        ValidateAddCodeUnique();
    }
}

public class CreateOrderCrossValidation : AbstractValidator<CreateOrderCrossCommand>
{
    public CreateOrderCrossValidation()
    {
    }

    protected void ValidateOrderType()
    {
        RuleFor(c => c.OrderType)
            .Must(x => string.IsNullOrEmpty(x) || new[] { "AUC", "CROSS", "TM", "SX", "K", "BARGAIN" }.Contains(x))
            .WithMessage("Order type invalid");
    }
}
public class CreateInvoicePayOrderCrossValidation : AbstractValidator<CreateInvoicePayOrderCrossCommand>
{
    public CreateInvoicePayOrderCrossValidation()
    {
    }
}
