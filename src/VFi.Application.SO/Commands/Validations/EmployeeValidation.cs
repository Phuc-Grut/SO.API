using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class EmployeeValidation<T> : AbstractValidator<T> where T : EmployeeCommand

{
    protected readonly IEmployeeRepository _context;
    private Guid Id;

    public EmployeeValidation(IEmployeeRepository context)
    {
        _context = context;
    }
    public EmployeeValidation(IEmployeeRepository context, Guid id)
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

    private bool IsAddUniqueAccount(Guid accountId)
    {
        var model = _context.GetByAccountId(accountId).Result;

        if (model == null)
        {
            return true;
        }

        return false;
    }

    protected void ValidateAddUniqueAccount()
    {
        RuleFor(x => x.AccountId).Must(IsAddUniqueAccount).WithMessage("Account has created an employee");
    }

    private bool IsEditUniqueAccount(Guid accountId)
    {
        var model = _context.GetByAccountId(accountId).Result;

        if (model == null || model.Id == Id)
        {
            return true;
        }

        return false;
    }

    protected void ValidateEditUniqueAccount()
    {
        RuleFor(x => x.AccountId).Must(IsEditUniqueAccount).WithMessage("Account has created an employee");
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }

}
public class EmployeeAddCommandValidation : EmployeeValidation<EmployeeAddCommand>
{
    public EmployeeAddCommandValidation(IEmployeeRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
        ValidateAddUniqueAccount();
    }
}
public class EmployeeEditCommandValidation : EmployeeValidation<EmployeeEditCommand>
{
    public EmployeeEditCommandValidation(IEmployeeRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditUniqueAccount();
    }
}

public class EmployeeDeleteCommandValidation : EmployeeValidation<EmployeeDeleteCommand>
{
    public EmployeeDeleteCommandValidation(IEmployeeRepository context) : base(context)
    {
        ValidateId();
    }
}
