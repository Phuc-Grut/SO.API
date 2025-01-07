using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class WalletTransactionValidation<T> : AbstractValidator<T> where T : WalletTransactionCommand

{
    protected readonly IWalletTransactionRepository _context;
    private Guid Id;

    public WalletTransactionValidation(IWalletTransactionRepository context)
    {
        _context = context;
    }
    public WalletTransactionValidation(IWalletTransactionRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code already exists");
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
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code already exists");
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
}
public class WalletTransactionAddCommandValidation : WalletTransactionValidation<WalletTransactionAddCommand>
{
    public WalletTransactionAddCommandValidation(IWalletTransactionRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();

    }
}
public class WalletTransactionEditCommandValidation : WalletTransactionValidation<WalletTransactionEditCommand>
{
    public WalletTransactionEditCommandValidation(IWalletTransactionRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class WalletTransactionDeleteCommandValidation : WalletTransactionValidation<WalletTransactionDeleteCommand>
{
    public WalletTransactionDeleteCommandValidation(IWalletTransactionRepository context) : base(context)
    {
        ValidateId();
    }
}
