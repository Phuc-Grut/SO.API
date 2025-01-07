using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class ContractValidation<T> : AbstractValidator<T> where T : ContractCommand

{
    protected readonly IContractRepository _context;
    private Guid Id;

    public ContractValidation(IContractRepository context)
    {
        _context = context;
    }
    public ContractValidation(IContractRepository context, Guid id)
    {
        _context = context;
        Id = id;
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
public class ContractAddCommandValidation : ContractValidation<ContractAddCommand>
{
    public ContractAddCommandValidation(IContractRepository context) : base(context)
    {
        ValidateId();
        ValidateCode();
        ValidateAddCodeUnique();
    }
}
public class ContractEditCommandValidation : ContractValidation<ContractEditCommand>
{
    public ContractEditCommandValidation(IContractRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateCode();
    }
}

public class ContractDeleteCommandValidation : ContractValidation<ContractDeleteCommand>
{
    public ContractDeleteCommandValidation(IContractRepository context) : base(context)
    {
        ValidateId();
    }
}
