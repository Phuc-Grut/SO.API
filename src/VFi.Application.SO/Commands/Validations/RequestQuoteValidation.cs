using FluentValidation;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class RequestQuoteValidation<T> : AbstractValidator<T> where T : RequestQuoteCommand

{
    protected readonly IRequestQuoteRepository _context;
    private Guid Id;

    public RequestQuoteValidation(IRequestQuoteRepository context)
    {
        _context = context;
    }
    public RequestQuoteValidation(IRequestQuoteRepository context, Guid id)
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
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    protected void ValidateCode()
    {
        RuleFor(c => c.Code)
            .NotEmpty().WithMessage("Please ensure you have entered the code")
            .Length(0, 50).WithMessage("The code must have between 0 and 50 characters");
    }
}
public class RequestQuoteAddCommandValidation : RequestQuoteValidation<RequestQuoteAddCommand>
{
    public RequestQuoteAddCommandValidation(IRequestQuoteRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
        ValidateCode();
    }
}
public class RequestQuoteEditCommandValidation : RequestQuoteValidation<RequestQuoteEditCommand>
{
    public RequestQuoteEditCommandValidation(IRequestQuoteRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
        ValidateCode();
    }
}

public class RequestQuoteDeleteCommandValidation : RequestQuoteValidation<RequestQuoteDeleteCommand>
{
    public RequestQuoteDeleteCommandValidation(IRequestQuoteRepository context) : base(context)
    {
        ValidateId();
    }
}
