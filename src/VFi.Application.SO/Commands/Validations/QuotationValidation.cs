using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;


public abstract class QuotationValidation<T> : AbstractValidator<T> where T : QuotationCommand
{
    protected readonly IQuotationRepository _context;

    public QuotationValidation(IQuotationRepository context)
    {
        _context = context;
    }

    public QuotationValidation()
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

    private bool IsAddUnique(string code)
    {
        var model = _context.GetByCode(code).Result;

        if (model is null)
        {
            return true;
        }

        return false;
    }


}

public class AddQuotationValidation : QuotationValidation<AddQuotationCommand>
{
    public AddQuotationValidation(IQuotationRepository context) : base(context)
    {
        ValidateId();
        ValidateCode();
        ValidateAddCodeUnique();
    }
}

public class EditQuotationValidation : QuotationValidation<EditQuotationCommand>
{
    public EditQuotationValidation()
    {
        ValidateId();
        ValidateCode();
    }
}

public class DeteleQuotationValidation : QuotationValidation<DeleteQuotationCommand>
{
    public DeteleQuotationValidation()
    {
        ValidateId();
    }

}
