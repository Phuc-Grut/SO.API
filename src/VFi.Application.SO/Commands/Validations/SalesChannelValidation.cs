using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class SalesChannelValidation<T> : AbstractValidator<T> where T : SalesChannelCommand

{
    protected readonly ISalesChannelRepository _context;
    private Guid Id;

    public SalesChannelValidation(ISalesChannelRepository context)
    {
        _context = context;
    }
    public SalesChannelValidation(ISalesChannelRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("The sales channel code already exists");
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
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("The sales channel code already exists");
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }


}
public class SalesChannelAddCommandValidation : SalesChannelValidation<SalesChannelAddCommand>
{
    public SalesChannelAddCommandValidation(ISalesChannelRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
    }
}
public class SalesChannelEditCommandValidation : SalesChannelValidation<SalesChannelEditCommand>
{
    public SalesChannelEditCommandValidation(ISalesChannelRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
    }
}

public class SalesChannelDeleteCommandValidation : SalesChannelValidation<SalesChannelDeleteCommand>
{
    public SalesChannelDeleteCommandValidation(ISalesChannelRepository context) : base(context)
    {
        ValidateId();
    }
}
