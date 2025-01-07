using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class ContractTypeValidation<T> : AbstractValidator<T> where T : ContractTypeCommand

{
    protected readonly IContractTypeRepository _context;
    private Guid Id;

    public ContractTypeValidation(IContractTypeRepository context)
    {
        _context = context;
    }
    public ContractTypeValidation(IContractTypeRepository context, Guid id)
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
public class ContractTypeAddCommandValidation : ContractTypeValidation<ContractTypeAddCommand>
{
    public ContractTypeAddCommandValidation(IContractTypeRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class ContractTypeEditCommandValidation : ContractTypeValidation<ContractTypeEditCommand>
{
    public ContractTypeEditCommandValidation(IContractTypeRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class ContractTypeDeleteCommandValidation : ContractTypeValidation<ContractTypeDeleteCommand>
{
    public ContractTypeDeleteCommandValidation(IContractTypeRepository context) : base(context)
    {
        ValidateId();
    }
}
