using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class WalletValidation<T> : AbstractValidator<T> where T : WalletCommand

{
    protected readonly IWalletRepository _context;
    private Guid Id;

    public WalletValidation(IWalletRepository context)
    {
        _context = context;
    }
    public WalletValidation(IWalletRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }



    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
}
public class WalletAddCommandValidation : WalletValidation<WalletAddCommand>
{
    public WalletAddCommandValidation(IWalletRepository context) : base(context)
    {
        ValidateId();

    }
}
public class WalletEditCommandValidation : WalletValidation<WalletEditCommand>
{
    public WalletEditCommandValidation(IWalletRepository context, Guid id) : base(context, id)
    {
        ValidateId();
    }
}

public class WalletDeleteCommandValidation : WalletValidation<WalletDeleteCommand>
{
    public WalletDeleteCommandValidation(IWalletRepository context) : base(context)
    {
        ValidateId();
    }
}

public class DepositWalletValidation : AbstractValidator<DepositWalletCommand>
{
    protected readonly IWalletRepository _context;
    public DepositWalletValidation(IWalletRepository context)
    {
        _context = context;
        // ValidateId();
    }
}
public class DepositWalletFromBankValidation : AbstractValidator<DepositWalletFromBankCommand>
{
    protected readonly IWalletRepository _context;
    public DepositWalletFromBankValidation(IWalletRepository context)
    {
        _context = context;
        // ValidateId();
    }
}
public class WithdrawWalletValidation : AbstractValidator<WithdrawWalletCommand>
{
    protected readonly IWalletRepository _context;
    public WithdrawWalletValidation(IWalletRepository context)
    {
        _context = context;
        // ValidateId();
    }
}
public class PayWalletValidation : AbstractValidator<PayWalletCommand>
{
    protected readonly IWalletRepository _context;
    public PayWalletValidation(IWalletRepository context)
    {
        _context = context;
        // ValidateId();
    }
}
public class RefundPayWalletValidation : AbstractValidator<RefundPayWalletCommand>
{
    protected readonly IWalletRepository _context;
    public RefundPayWalletValidation(IWalletRepository context)
    {
        _context = context;
        // ValidateId();
    }
}
public class PayOrderWalletValidation : AbstractValidator<PayOrderWalletCommand>
{
    protected readonly IWalletRepository _context;
    public PayOrderWalletValidation(IWalletRepository context)
    {
        _context = context;
        // ValidateId();
    }
}
public class RefundPayOrderWalletValidation : AbstractValidator<RefundPayOrderWalletCommand>
{
    protected readonly IWalletRepository _context;
    public RefundPayOrderWalletValidation(IWalletRepository context)
    {
        _context = context;
        // ValidateId();
    }
}

public class HoldWalletValidation : AbstractValidator<HoldWalletCommand>
{
    protected readonly IWalletRepository _context;
    public HoldWalletValidation(IWalletRepository context)
    {
        _context = context;
        // ValidateId();
    }
}
public class HoldBidWalletValidation : AbstractValidator<HoldBidWalletCommand>
{
    protected readonly IWalletRepository _context;
    public HoldBidWalletValidation(IWalletRepository context)
    {
        _context = context;
        // ValidateId();
    }
}
public class RefundHoldWalletValidation : AbstractValidator<RefundHoldWalletCommand>
{
    protected readonly IWalletRepository _context;
    public RefundHoldWalletValidation(IWalletRepository context)
    {
        _context = context;
        // ValidateId();
    }
}
public class RefundHoldBidWalletValidation : AbstractValidator<RefundHoldBidWalletCommand>
{
    protected readonly IWalletRepository _context;
    public RefundHoldBidWalletValidation(IWalletRepository context)
    {
        _context = context;
        // ValidateId();
    }
}
