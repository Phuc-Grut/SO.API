﻿using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class TransactionValidation<T> : AbstractValidator<T> where T : TransactionCommand

{
    protected readonly ITransactionRepository _context;
    private Guid Id;

    public TransactionValidation(ITransactionRepository context)
    {
        _context = context;
    }
    public TransactionValidation(ITransactionRepository context, Guid id)
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
public class TransactionAddCommandValidation : TransactionValidation<TransactionAddCommand>
{
    public TransactionAddCommandValidation(ITransactionRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();

    }
}
public class TransactionEditCommandValidation : TransactionValidation<TransactionEditCommand>
{
    public TransactionEditCommandValidation(ITransactionRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class TransactionDeleteCommandValidation : TransactionValidation<TransactionDeleteCommand>
{
    public TransactionDeleteCommandValidation(ITransactionRepository context) : base(context)
    {
        ValidateId();
    }
}
