using System.Reflection;
using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class BankValidation<T> : AbstractValidator<T> where T : BankCommand

{
    protected readonly IBankRepository _context;
    private Guid Id;

    public BankValidation(IBankRepository context)
    {
        _context = context;
    }
    public BankValidation(IBankRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
        ;
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
        ;
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

    private bool IsUsing(Guid id)
    {
        return _context.CheckUsing(id);
    }
    protected void ValidateDeleteUsing()
    {
        RuleFor(x => x.Id).Must(IsUsing).WithMessage("Bank has been used");
    }

}
public class BankAddCommandValidation : BankValidation<BankAddCommand>
{
    public BankAddCommandValidation(IBankRepository context) : base(context)
    {
        ValidateId();
        ValidateCode();
        ValidateAddCodeUnique();
    }
}
public class BankEditCommandValidation : BankValidation<BankEditCommand>
{
    public BankEditCommandValidation(IBankRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateCode();
        ValidateEditCodeUnique();
    }
}

public class BankDeleteCommandValidation : BankValidation<BankDeleteCommand>
{
    public BankDeleteCommandValidation(IBankRepository context) : base(context)
    {
        ValidateId();
        ValidateDeleteUsing();
    }
}
