using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class CustomerSourceValidation<T> : AbstractValidator<T> where T : CustomerSourceCommand

{
    protected readonly ICustomerSourceRepository _context;
    private Guid Id;

    public CustomerSourceValidation(ICustomerSourceRepository context)
    {
        _context = context;
    }
    public CustomerSourceValidation(ICustomerSourceRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
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
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }

}
public class CustomerSourceAddCommandValidation : CustomerSourceValidation<CustomerSourceAddCommand>
{
    public CustomerSourceAddCommandValidation(ICustomerSourceRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class CustomerSourceEditCommandValidation : CustomerSourceValidation<CustomerSourceEditCommand>
{
    public CustomerSourceEditCommandValidation(ICustomerSourceRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class CustomerSourceDeleteCommandValidation : CustomerSourceValidation<CustomerSourceDeleteCommand>
{
    public CustomerSourceDeleteCommandValidation(ICustomerSourceRepository context) : base(context)
    {
        ValidateId();
    }
}
