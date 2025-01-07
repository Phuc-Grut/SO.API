using Consul;
using MediatR;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using VFi.Application.SO.Commands.Validations;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class OrderCostCommand : Command
{
    public Guid Id { get; set; }
    public Guid QuotationId { get; set; }
    public Guid? ExpenseId { get; set; }
    public int? Type { get; set; }
    public double? Rate { get; set; }
    public decimal? Amount { get; set; }
    public int? Status { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
public class OrderCostAddCommand : OrderCostCommand
{
    public OrderCostAddCommand(
        Guid id,
        Guid quotationId,
        Guid? expenseId,
        int? type,
        double? rate,
        decimal? amount,
        int? status,
        int? displayOrder,
        Guid? createdBy,
        DateTime? createdDate,
        Guid? updatedBy,
        DateTime? updatedDate


        )
    {
        Id = id;
        QuotationId = quotationId;
        ExpenseId = expenseId;
        Type = type;
        Rate = rate;
        Amount = amount;
        Status = status;
        DisplayOrder = displayOrder;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
        UpdatedBy = updatedBy;
        UpdatedDate = updatedDate;


    }
    public bool IsValid(IOrderCostRepository _context)
    {
        ValidationResult = new OrderCostAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class OrderCostEditCommand : OrderCostCommand
{
    public OrderCostEditCommand(
        Guid id,
        Guid quotationId,
        Guid? expenseId,
        int? type,
        double? rate,
        decimal? amount,
        int? status,
        int? displayOrder,
        Guid? createdBy,
        DateTime? createdDate,
        Guid? updatedBy,
        DateTime? updatedDate
        )
    {
        Id = id;
        QuotationId = quotationId;
        ExpenseId = expenseId;
        Type = type;
        Rate = rate;
        Amount = amount;
        Status = status;
        DisplayOrder = displayOrder;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
        UpdatedBy = updatedBy;
        UpdatedDate = updatedDate;

    }
    public bool IsValid(IOrderCostRepository _context)
    {
        ValidationResult = new OrderCostEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class OrderCostDeleteCommand : OrderCostCommand
{
    public OrderCostDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IOrderCostRepository _context)
    {
        ValidationResult = new OrderCostDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
