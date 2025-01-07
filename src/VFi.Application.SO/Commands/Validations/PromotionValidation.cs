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


public abstract class PromotionValidation<T> : AbstractValidator<T> where T : PromotionCommand
{
    protected readonly IPromotionRepository _context;

    public PromotionValidation(IPromotionRepository context)
    {
        _context = context;
    }

    public PromotionValidation()
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

public class AddPromotionValidation : PromotionValidation<AddPromotionCommand>
{
    public AddPromotionValidation(IPromotionRepository context) : base(context)
    {
        ValidateId();
        ValidateCode();
        ValidateAddCodeUnique();
    }
}

public class EditPromotionValidation : PromotionValidation<EditPromotionCommand>
{
    public EditPromotionValidation()
    {
        ValidateId();
        ValidateCode();
    }
}

public class DetelePromotionValidation : PromotionValidation<DeletePromotionCommand>
{
    public DetelePromotionValidation()
    {
        ValidateId();
    }

}
