using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class ContractTermValidation<T> : AbstractValidator<T> where T : ContractTermCommand

{
    protected readonly IContractTermRepository _context;
    private Guid Id;

    public ContractTermValidation(IContractTermRepository context)
    {
        _context = context;
    }
    public ContractTermValidation(IContractTermRepository context, Guid id)
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

}
public class ContractTermAddCommandValidation : ContractTermValidation<ContractTermAddCommand>
{
    public ContractTermAddCommandValidation(IContractTermRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class ContractTermEditCommandValidation : ContractTermValidation<ContractTermEditCommand>
{
    public ContractTermEditCommandValidation(IContractTermRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class ContractTermDeleteCommandValidation : ContractTermValidation<ContractTermDeleteCommand>
{
    public ContractTermDeleteCommandValidation(IContractTermRepository context) : base(context)
    {
        ValidateId();
    }
}
