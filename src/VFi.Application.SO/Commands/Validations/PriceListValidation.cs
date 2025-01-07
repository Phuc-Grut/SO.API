using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;


public abstract class PriceListValidation<T> : AbstractValidator<T> where T : PriceListCommand
{
    protected readonly IPriceListRepository _context;
    private Guid Id;

    public PriceListValidation(IPriceListRepository context)
    {
        _context = context;
    }
    public PriceListValidation(IPriceListRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }

    public PriceListValidation()
    {
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
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
    protected void ValidateEditCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
    }
    protected void ValidateDeleteUsing()
    {
        RuleFor(x => x.Id).Must(IsUsing).WithMessage("Item is being used");
    }

    private bool IsAddUnique(string code)
    {
        return _context.CheckByCode(code, null);
    }
    private bool IsEditUnique(string? code)
    {
        return _context.CheckByCode(code, Id);
    }

    private bool IsUsing(Guid id)
    {
        return _context.CheckUsing(id);
    }

}

public class AddPriceListValidation : PriceListValidation<AddPriceListCommand>
{
    public AddPriceListValidation(IPriceListRepository context) : base(context)
    {
        ValidateId();
        ValidateCode();
        ValidateAddCodeUnique();
    }
}

public class EditPriceListValidation : PriceListValidation<EditPriceListCommand>
{
    public EditPriceListValidation(IPriceListRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateCode();
        ValidateEditCodeUnique();
    }
}

public class DetelePriceListValidation : PriceListValidation<DeletePriceListCommand>
{
    public DetelePriceListValidation(IPriceListRepository context) : base(context)
    {
        ValidateId();
        ValidateDeleteUsing();
    }

}
