using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class GroupEmployeeValidation<T> : AbstractValidator<T> where T : GroupEmployeeCommand

{
    protected readonly IGroupEmployeeRepository _context;
    private Guid Id;

    public GroupEmployeeValidation(IGroupEmployeeRepository context)
    {
        _context = context;
    }
    public GroupEmployeeValidation(IGroupEmployeeRepository context, Guid id)
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
public class GroupEmployeeAddCommandValidation : GroupEmployeeValidation<GroupEmployeeAddCommand>
{
    public GroupEmployeeAddCommandValidation(IGroupEmployeeRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class GroupEmployeeEditCommandValidation : GroupEmployeeValidation<GroupEmployeeEditCommand>
{
    public GroupEmployeeEditCommandValidation(IGroupEmployeeRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class GroupEmployeeDeleteCommandValidation : GroupEmployeeValidation<GroupEmployeeDeleteCommand>
{
    public GroupEmployeeDeleteCommandValidation(IGroupEmployeeRepository context) : base(context)
    {
        ValidateId();
    }
}
