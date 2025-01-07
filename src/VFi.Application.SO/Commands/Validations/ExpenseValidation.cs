using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class ExpenseValidation<T> : AbstractValidator<T> where T : ExpenseCommand

{
    protected readonly IExpenseRepository _context;
    private Guid Id;

    public ExpenseValidation(IExpenseRepository context)
    {
        _context = context;
    }
    public ExpenseValidation(IExpenseRepository context, Guid id)
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
public class ExpenseAddCommandValidation : ExpenseValidation<ExpenseAddCommand>
{
    public ExpenseAddCommandValidation(IExpenseRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class ExpenseEditCommandValidation : ExpenseValidation<ExpenseEditCommand>
{
    public ExpenseEditCommandValidation(IExpenseRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class ExpenseDeleteCommandValidation : ExpenseValidation<ExpenseDeleteCommand>
{
    public ExpenseDeleteCommandValidation(IExpenseRepository context) : base(context)
    {
        ValidateId();
    }
}
