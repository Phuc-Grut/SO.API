using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using VFi.Application.SO.Commands.Validations;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class WalletTransactionCommand : Command
{

    public Guid Id { get; set; }
    public string Code { get; set; }
    public decimal Amount { get; set; }
    public Guid AccountId { get; set; }
    public Guid? WalletId { get; set; }
    /// <summary>
    /// DEPOSIT, PAY_ORDER, PAY, REFUND_FROM_CHARGE_ORDER
    /// </summary>
    public string Type { get; set; }
    public string Method { get; set; }
    public int Status { get; set; }
    public long? ParentId { get; set; }
    public DateTime? ApplyDate { get; set; }
    public string RawData { get; set; }
    public Guid RefId { get; set; }
    public string RefType { get; set; }
    public string RefCode { get; set; }
    public DateTime? RefDate { get; set; }
    public decimal? Balance { get; set; }
    public int? RefundStatus { get; set; }
    public decimal RefundAmount { get; set; }

}

public class WalletTransactionAddCommand : WalletTransactionCommand
{
    public WalletTransactionAddCommand()
    {
    }

    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedByName { get; set; }
    public bool IsValid(IWalletTransactionRepository _context)
    {
        ValidationResult = new WalletTransactionAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class WalletTransactionEditCommand : WalletTransactionCommand
{
    public WalletTransactionEditCommand()
    {
    }

    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string UpdatedByName { get; set; }
    public bool IsValid(IWalletTransactionRepository _context)
    {
        ValidationResult = new WalletTransactionEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class WalletTransactionDeleteCommand : WalletTransactionCommand
{
    public WalletTransactionDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IWalletTransactionRepository _context)
    {
        ValidationResult = new WalletTransactionDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
