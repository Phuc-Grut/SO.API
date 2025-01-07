using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class PostOfficeValidation<T> : AbstractValidator<T> where T : PostOfficeCommand

{
    protected readonly IPostOfficeRepository _context;
    private Guid Id;

    public PostOfficeValidation(IPostOfficeRepository context)
    {
        _context = context;
    }
    public PostOfficeValidation(IPostOfficeRepository context, Guid id)
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
public class PostOfficeAddCommandValidation : PostOfficeValidation<PostOfficeAddCommand>
{
    public PostOfficeAddCommandValidation(IPostOfficeRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();

    }
}
public class PostOfficeEditCommandValidation : PostOfficeValidation<PostOfficeEditCommand>
{
    public PostOfficeEditCommandValidation(IPostOfficeRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class PostOfficeDeleteCommandValidation : PostOfficeValidation<PostOfficeDeleteCommand>
{
    public PostOfficeDeleteCommandValidation(IPostOfficeRepository context) : base(context)
    {
        ValidateId();
    }
}
