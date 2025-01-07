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


public abstract class ReturnOrderValidation<T> : AbstractValidator<T> where T : ReturnOrderCommand
{
    protected readonly IReturnOrderRepository _context;
    private Guid Id;

    public ReturnOrderValidation(IReturnOrderRepository context)
    {
        _context = context;
    }

    public ReturnOrderValidation(IReturnOrderRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }

    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
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
    private bool IsEditUnique(string code)
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
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }


}

public class AddReturnOrderValidation : ReturnOrderValidation<AddReturnOrderCommand>
{
    public AddReturnOrderValidation(IReturnOrderRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}

public class ReturnOrderDuplicateCommandValidation : ReturnOrderValidation<ReturnOrderDuplicateCommand>
{
    public ReturnOrderDuplicateCommandValidation(IReturnOrderRepository context) : base(context)
    {
        ValidateAddCodeUnique();
        ValidateId();
    }
}

public class EditReturnOrderValidation : ReturnOrderValidation<EditReturnOrderCommand>
{
    public EditReturnOrderValidation(IReturnOrderRepository context, Guid id) : base(context, id)
    {
        ValidateId();
    }
}

public class DeteleReturnOrderValidation : ReturnOrderValidation<DeleteReturnOrderCommand>
{
    public DeteleReturnOrderValidation(IReturnOrderRepository context) : base(context)
    {
        ValidateId();
    }

}
