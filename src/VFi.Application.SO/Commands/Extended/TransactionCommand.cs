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

public class TransactionCommand : Command
{

    public Guid Id { get; set; }
    public string Code { get; set; }
    public decimal Amount { get; set; }
    public string AccountId { get; set; }
    public string WalletId { get; set; }
    /// <summary>
    /// DEPOSIT, PAY_ORDER, PAY, REFUND_FROM_CHARGE_ORDER
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// 1,2,3
    /// </summary>
    public string ObjectRef { get; set; }
    /// <summary>
    /// 1,2,3
    /// </summary>
    public string AuthorizeRef { get; set; }
    public string TransactionRef { get; set; }
    public string Source { get; set; }
    public string MetaData { get; set; }
    public int Status { get; set; }
    public long? ParentId { get; set; }
    public DateTime? ApplyDate { get; set; }
    public string RawData { get; set; }
    public string Currency { get; set; }
    public string AuthorizationTransactionId { get; set; }
    public string AuthorizationTransactionCode { get; set; }
    public string AuthorizationTransactionResult { get; set; }
    public string CaptureTransactionId { get; set; }
    public string CaptureTransactionResult { get; set; }
    public string RefundTransactionId { get; set; }
    public string RefundTransactionResult { get; set; }
    public DateTime? TransactionDate { get; set; }
}

public class TransactionAddCommand : TransactionCommand
{
    public TransactionAddCommand()
    {
    }

    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedByName { get; set; }
    public bool IsValid(ITransactionRepository _context)
    {
        ValidationResult = new TransactionAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class TransactionEditCommand : TransactionCommand
{
    public TransactionEditCommand()
    {
    }

    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string UpdatedByName { get; set; }
    public bool IsValid(ITransactionRepository _context)
    {
        ValidationResult = new TransactionEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class TransactionDeleteCommand : TransactionCommand
{
    public TransactionDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ITransactionRepository _context)
    {
        ValidationResult = new TransactionDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
