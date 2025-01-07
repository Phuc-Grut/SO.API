using System.Reflection;
using FluentValidation;
using MassTransit.Internals;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class CustomerValidation<T> : AbstractValidator<T> where T : CustomerCommand

{
    protected readonly ICustomerRepository _context;
    private Guid Id;

    public CustomerValidation(ICustomerRepository context)
    {
        _context = context;
    }
    public CustomerValidation(ICustomerRepository context, Guid id)
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

    protected void ValidateAddTaxCodeUnique()
    {
        RuleFor(x => x.TaxCode).Must(IsAddTaxCodeUnique)
                            .WithMessage("TaxCode alrealy exists");
    }

    protected void ValidateAddEmailUnique()
    {
        RuleFor(x => x.Email).Must(IsAddEmailUnique)
                            .WithMessage("alrealy exists");
    }

    protected void ValidateAddPhoneUnique()
    {
        RuleFor(x => x.Phone).Must(IsAddPhoneUnique)
                            .WithMessage("alrealy exists");
    }

    protected void ValidateUpdateCodeUnique()
    {
        RuleFor(x => x.TaxCode)
            .Must((m, x) => IsUpdateUnique(m.Id, m.TaxCode))
            .WithMessage("TaxCode alrealy exists");
    }

    protected void ValidateUpdateEmailCodeUnique()
    {
        RuleFor(x => x.Email)
            .Must((m, x) => IsUpdateEmailUnique(m.Id, m.Email))
            .WithMessage("alrealy exists");
    }

    protected void ValidateUpdatePhoneCodeUnique()
    {
        RuleFor(x => x.Phone)
            .Must((m, x) => IsUpdatePhoneUnique(m.Id, m.Phone))
            .WithMessage("alrealy exists");
    }

    private bool IsAddTaxCodeUnique(string? taxCode)
    {
        return !_context.CheckTaxCode(taxCode, null).Result;
    }

    private bool IsAddEmailUnique(string? email)
    {
        return !_context.CheckEmailCode(email, null).Result;
    }

    private bool IsAddPhoneUnique(string? phone)
    {
        return !_context.CheckPhoneCode(phone, null).Result;
    }

    private bool IsUpdateUnique(Guid id, string? taxCode)
    {
        return !_context.CheckTaxCode(taxCode, id).Result;
    }

    private bool IsUpdateEmailUnique(Guid id, string? email)
    {
        return !_context.CheckEmailCode(email, id).Result;
    }

    private bool IsUpdatePhoneUnique(Guid id, string? phone)
    {
        return !_context.CheckPhoneCode(phone, id).Result;
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
    protected void ValidateName()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Please ensure you have entered the name")
            .Length(2, 250).WithMessage("The name must have between 2 and 250 characters");
    }
}
public class CustomerAddCommandValidation : CustomerValidation<CustomerAddCommand>
{
    public CustomerAddCommandValidation(ICustomerRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
        ValidateCode();
        ValidateName();
        ValidateAddEmailUnique();
        ValidateAddPhoneUnique();
    }
}
public class CustomerEditCommandValidation : CustomerValidation<CustomerEditCommand>
{
    public CustomerEditCommandValidation(ICustomerRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
        ValidateCode();
        ValidateName();
        ValidateUpdateCodeUnique();
        ValidateUpdateEmailCodeUnique();
        ValidateUpdatePhoneCodeUnique();
    }
}
public class CustomermenageAccountCommandValidation : CustomerValidation<CustomerUpdateAccountCommand>
{
    public CustomermenageAccountCommandValidation(ICustomerRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateEditCodeUnique();
        ValidateCode();
        ValidateName();
    }
}

public class CustomerDeleteCommandValidation : AbstractValidator<CustomerDeleteCommand>
{
    protected readonly ICustomerRepository _context;
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    private bool IsUsing(Guid id)
    {
        return _context.CheckUsing(id);
    }
    protected void ValidateDeleteUsing()
    {
        RuleFor(x => x.Id).Must(IsUsing).WithMessage("The customer is already used!");
    }
    public CustomerDeleteCommandValidation(ICustomerRepository context)
    {
        _context = context;
        ValidateId();
        ValidateDeleteUsing();
    }

}

public class CustomerImageEditCommandValidation : AbstractValidator<CustomerImageEditCommand>
{
    protected readonly ICustomerRepository _context;
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    public CustomerImageEditCommandValidation(ICustomerRepository context)
    {
        _context = context;
        ValidateId();
    }

}

public class CustomerUpdateFinanceValidation : AbstractValidator<CustomerUpdateFinanceCommand>
{
    protected readonly ICustomerRepository _context;
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    public CustomerUpdateFinanceValidation(ICustomerRepository context)
    {
        _context = context;
        ValidateId();
    }

}
