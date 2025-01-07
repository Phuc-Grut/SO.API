using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class CustomerContactValidation<T> : AbstractValidator<T> where T : CustomerContactCommand

{
    protected readonly ICustomerContactRepository _context;
    private Guid Id;

    public CustomerContactValidation(ICustomerContactRepository context)
    {
        _context = context;
    }
    public CustomerContactValidation(ICustomerContactRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    //protected void ValidateCode()
    //{
    //    RuleFor(c => c.Code)
    //        .NotEmpty().WithMessage("Please ensure you have entered the code")
    //        .Length(0, 50).WithMessage("The code must have between 0 and 50 characters");
    //}
    protected void ValidateName()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Please ensure you have entered the name")
            .Length(2, 250).WithMessage("The name must have between 2 and 250 characters");
    }
}
public class CustomerContactAddCommandValidation : CustomerContactValidation<CustomerContactAddCommand>
{
    public CustomerContactAddCommandValidation(ICustomerContactRepository context) : base(context)
    {
        ValidateId();
        //ValidateAddCodeUnique();
        //ValidateCode();
        ValidateName();
    }
}
public class CustomerContactEditCommandValidation : CustomerContactValidation<CustomerContactEditCommand>
{
    public CustomerContactEditCommandValidation(ICustomerContactRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        //ValidateEditCodeUnique();
        //ValidateCode();
        ValidateName();
    }
}

public class CustomerContactDeleteCommandValidation : CustomerContactValidation<CustomerContactDeleteCommand>
{
    public CustomerContactDeleteCommandValidation(ICustomerContactRepository context) : base(context)
    {
        ValidateId();
    }
}
