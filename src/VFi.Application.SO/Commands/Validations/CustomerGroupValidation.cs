using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class CustomerGroupValidation<T> : AbstractValidator<T> where T : CustomerGroupCommand

{
    protected readonly ICustomerGroupRepository _context;
    private Guid Id;

    public CustomerGroupValidation(ICustomerGroupRepository context)
    {
        _context = context;
    }
    public CustomerGroupValidation(ICustomerGroupRepository context, Guid id)
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
    protected void ValidateDeleteBeingUsed()
    {
        RuleFor(x => x.Id).Must(id => _context.IsNotBeingUsed(id).Result).WithMessage("In use, cannot be deleted");
    }
}
public class CustomerGroupAddCommandValidation : CustomerGroupValidation<CustomerGroupAddCommand>
{
    public CustomerGroupAddCommandValidation(ICustomerGroupRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class CustomerGroupEditCommandValidation : CustomerGroupValidation<CustomerGroupEditCommand>
{
    public CustomerGroupEditCommandValidation(ICustomerGroupRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class CustomerGroupDeleteCommandValidation : CustomerGroupValidation<CustomerGroupDeleteCommand>
{
    public CustomerGroupDeleteCommandValidation(ICustomerGroupRepository context) : base(context)
    {
        ValidateId();
        ValidateDeleteBeingUsed();
    }
}
