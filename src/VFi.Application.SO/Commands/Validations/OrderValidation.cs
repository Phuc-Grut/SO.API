using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;


public abstract class OrderValidation<T> : AbstractValidator<T> where T : OrderCommand
{
    protected readonly IOrderRepository _context;

    public OrderValidation(IOrderRepository context)
    {
        _context = context;
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Order id is null")
            .MustAsync((item, cancelToken) => _context.ExistId(item))
            .WithMessage("Order id not exits");
    }

    protected void ValidateGuid()
    {
        RuleFor(c => c.Id)
        .NotEqual(Guid.Empty)
        .WithMessage("Order id is null");
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
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
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

public class AddOrderValidation : OrderValidation<AddOrderCommand>
{
    public AddOrderValidation(IOrderRepository context) : base(context)
    {
        ValidateGuid();
        ValidateCode();
        ValidateAddCodeUnique();
    }
}

public class EditOrderValidation : OrderValidation<EditOrderCommand>
{
    public EditOrderValidation(IOrderRepository context) : base(context)
    {
        ValidateId();
        ValidateCode();
    }
}

public class DeteleOrderValidation : OrderValidation<DeleteOrderCommand>
{
    private bool IsUsing(Guid id)
    {
        return _context.CheckUsing(id);
    }
    protected void ValidateDeleteUsing()
    {
        RuleFor(x => x.Id).Must(IsUsing).WithMessage("The saleOrder is already used!");
    }
    public DeteleOrderValidation(IOrderRepository context) : base(context)
    {
        ValidateId();
        ValidateDeleteUsing();
    }
}

public class CreateOrderValidation : AbstractValidator<CreateOrderCommand>
{
    protected readonly IOrderRepository _context;

    public CreateOrderValidation(IOrderRepository context)
    {
        _context = context;
    }

    public CreateOrderValidation()
    {
    }


}
