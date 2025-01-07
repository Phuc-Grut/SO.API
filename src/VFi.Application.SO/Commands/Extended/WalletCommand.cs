using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using FluentValidation;
using VFi.Application.SO.Commands.Validations;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class WalletCommand : Command
{

    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string WalletCode { get; set; }
    public decimal? Cash { get; set; }
    public decimal? CashHold { get; set; }
    public int? Status { get; set; }
}

public class WalletAddCommand : WalletCommand
{
    public WalletAddCommand()
    {
    }

    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedByName { get; set; }
    public bool IsValid(IWalletRepository _context)
    {
        ValidationResult = new WalletAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class WalletEditCommand : WalletCommand
{
    public WalletEditCommand()
    {
    }

    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string UpdatedByName { get; set; }
    public bool IsValid(IWalletRepository _context)
    {
        ValidationResult = new WalletEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class WalletDeleteCommand : WalletCommand
{
    public WalletDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IWalletRepository _context)
    {
        ValidationResult = new WalletDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class DepositWalletCommand : Command
{
    public DepositWalletCommand()
    {
    }

    public Guid? AccountId { get; set; }
    public string CustomerCode { get; set; }
    public string WalletCode { get; set; }
    public decimal Amount { get; set; }
    public string PaymentCode { get; set; }
    public string PaymentNote { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Document { get; set; }
    public bool IsValid(IWalletRepository _context)
    {
        ValidationResult = new DepositWalletValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class DepositWalletFromBankCommand : Command
{
    public DepositWalletFromBankCommand()
    {
    }

    public Guid? AccountId { get; set; }
    public string CustomerCode { get; set; }
    public string WalletCode { get; set; }
    public decimal Amount { get; set; }
    public string PaymentCode { get; set; }
    public string PaymentNote { get; set; }
    public DateTime PaymentDate { get; set; }
    public string BankName { get; set; }
    public string BankAccount { get; set; }
    public string BankNumber { get; set; }
    public bool IsValid(IWalletRepository _context)
    {
        ValidationResult = new DepositWalletFromBankValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class WithdrawWalletCommand : Command
{
    public WithdrawWalletCommand()
    {
    }

    public Guid AccountId { get; set; }
    public string WalletCode { get; set; }

    public decimal Amount { get; set; }
    public string Method { get; set; }
    public string Note { get; set; }
    public string RawData { get; set; }
    public string TransactionCode { get; set; }
    public DateTime RefDate { get; set; }
    public Guid? RefId { get; set; }
    public string RefType { get; set; }
    public string Document { get; set; }
    public bool IsValid(IWalletRepository _context)
    {
        ValidationResult = new WithdrawWalletValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PayWalletCommand : Command
{
    public PayWalletCommand()
    {
    }

    public Guid AccountId { get; set; }
    public string WalletCode { get; set; }

    public decimal Amount { get; set; }
    public string Method { get; set; }
    public string Note { get; set; }
    public string RawData { get; set; }
    public string TransactionCode { get; set; }
    public DateTime? RefDate { get; set; }
    public Guid? RefId { get; set; }
    public string RefType { get; set; }
    public bool IsValid(IWalletRepository _context)
    {
        ValidationResult = new PayWalletValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class RefundPayWalletCommand : Command
{
    public RefundPayWalletCommand()
    {
    }

    public Guid AccountId { get; set; }
    public string WalletCode { get; set; }

    public decimal Amount { get; set; }
    public string Method { get; set; }
    public string Note { get; set; }
    public string RawData { get; set; }
    public string TransactionCode { get; set; }
    public DateTime? RefDate { get; set; }
    public Guid? RefId { get; set; }
    public string RefType { get; set; }
    public bool IsValid(IWalletRepository _context)
    {
        ValidationResult = new RefundPayWalletValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class PayOrderWalletCommand : Command
{
    public PayOrderWalletCommand()
    {
    }

    public Guid AccountId { get; set; }
    public string WalletCode { get; set; }

    public decimal Amount { get; set; }
    public string Method { get; set; }
    public string Note { get; set; }
    public string RawData { get; set; }
    public string TransactionCode { get; set; }

    public Guid OrderId { get; set; }
    public string OrderCode { get; set; }
    public bool IsValid(IWalletRepository _context)
    {
        ValidationResult = new PayOrderWalletValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class RefundPayOrderWalletCommand : Command
{
    public RefundPayOrderWalletCommand()
    {
    }

    public Guid AccountId { get; set; }
    public string WalletCode { get; set; }

    public decimal Amount { get; set; }
    public string Method { get; set; }
    public string Note { get; set; }
    public string RawData { get; set; }
    public string TransactionCode { get; set; }

    public Guid OrderId { get; set; }
    public string OrderCode { get; set; }
    public DateTime RefDate { get; set; }
    public Guid? RefId { get; set; }
    public string RefType { get; set; }
    public bool IsValid(IWalletRepository _context)
    {
        ValidationResult = new RefundPayOrderWalletValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class HoldWalletCommand : Command
{
    public HoldWalletCommand()
    {
    }

    public Guid AccountId { get; set; }
    public string WalletCode { get; set; }

    public decimal Amount { get; set; }
    public string Method { get; set; }
    public string Note { get; set; }
    public string RawData { get; set; }
    public string TransactionCode { get; set; }
    public DateTime RefDate { get; set; }
    public Guid? RefId { get; set; }
    public string RefType { get; set; }
    public bool IsValid(IWalletRepository _context)
    {
        ValidationResult = new HoldWalletValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class RefundHoldWalletCommand : Command
{
    public RefundHoldWalletCommand()
    {
    }

    public Guid AccountId { get; set; }
    public string WalletCode { get; set; }

    public decimal Amount { get; set; }
    public string Method { get; set; }
    public string Note { get; set; }
    public string RawData { get; set; }
    public string TransactionCode { get; set; }
    public DateTime? RefDate { get; set; }
    public Guid? RefId { get; set; }
    public string RefType { get; set; }
    public bool IsValid(IWalletRepository _context)
    {
        ValidationResult = new RefundHoldWalletValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class HoldBidWalletCommand : Command
{
    public HoldBidWalletCommand()
    {
    }

    public Guid AccountId { get; set; }
    public string WalletCode { get; set; }

    public decimal Amount { get; set; }
    public string Method { get; set; }
    public string Note { get; set; }
    public string TransactionCode { get; set; }
    public DateTime RefDate { get; set; }
    public Guid? RefId { get; set; }
    public string RefType { get; set; }
    public bool IsValid(IWalletRepository _context)
    {
        ValidationResult = new HoldBidWalletValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class RefundHoldBidWalletCommand : Command
{
    public RefundHoldBidWalletCommand()
    {
    }
    public string TransactionCode { get; set; }
    public Guid AccountId { get; set; }
    public Guid? RefId { get; set; }
    public string Note { get; set; }
    public bool IsValid(IWalletRepository _context)
    {
        ValidationResult = new RefundHoldBidWalletValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
