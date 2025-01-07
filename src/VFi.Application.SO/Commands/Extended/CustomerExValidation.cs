using System.Reflection;
using FluentValidation;
using MassTransit.Internals;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;


public class SignupNoPassCommandValidation<T> : AbstractValidator<T> where T : SignupNoPassCommand

{
    protected readonly IAccountRepository _context;
    public SignupNoPassCommandValidation(IAccountRepository context)
    {
        _context = context;
        ValidateEmailExists();
    }
    protected void ValidateEmailExists()
    {
        RuleFor(x => x.Email).Must(IsNotExist).WithErrorCode("183").WithMessage("Email already exists");
    }
    protected bool IsNotExist(string email)
    {
        return !_context.IsExistEmail(email);
    }


}
public class CustomerExEditCommandValidation : AbstractValidator<CustomerExEditCommand>
{
    protected readonly ICustomerRepository _context;
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    public CustomerExEditCommandValidation(ICustomerRepository context)
    {
        _context = context;
        ValidateId();
    }

}


public class CustomerExIdentityEditCommandValidation : AbstractValidator<CustomerExIdentityEditCommand>
{
    protected readonly ICustomerRepository _context;
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    public CustomerExIdentityEditCommandValidation(ICustomerRepository context)
    {
        _context = context;
        ValidateId();
    }

}

public class CustomerExActiveBidValidation : AbstractValidator<CustomerExActiveBidCommand>
{
    protected readonly ICustomerRepository _context;
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    public CustomerExActiveBidValidation(ICustomerRepository context)
    {
        _context = context;
        ValidateId();
    }

}
public class CustomerExActiveBidHoldValidation : AbstractValidator<CustomerExActiveBidHoldCommand>
{
    protected readonly ICustomerRepository _context;
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    public CustomerExActiveBidHoldValidation(ICustomerRepository context)
    {
        _context = context;
        ValidateId();
    }

}
public class CustomerExEditBidQuantityValidation : AbstractValidator<CustomerExEditBidQuantityCommand>
{
    protected readonly ICustomerRepository _context;
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    public CustomerExEditBidQuantityValidation(ICustomerRepository context)
    {
        _context = context;
        ValidateId();
    }

}
public class CustomerExDeactiveBidValidation : AbstractValidator<CustomerExDeactiveBidCommand>
{
    protected readonly ICustomerRepository _context;
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    public CustomerExDeactiveBidValidation(ICustomerRepository context)
    {
        _context = context;
        ValidateId();
    }

}
public class CustomerExDeactiveBidHoldValidation : AbstractValidator<CustomerExDeactiveBidHoldCommand>
{
    protected readonly ICustomerRepository _context;
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    public CustomerExDeactiveBidHoldValidation(ICustomerRepository context)
    {
        _context = context;
        ValidateId();
    }

}
public class CustomerExUpBidQuantityHoldValidation : AbstractValidator<CustomerExUpBidQuantityHoldCommand>
{
    protected readonly ICustomerRepository _context;
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    public CustomerExUpBidQuantityHoldValidation(ICustomerRepository context)
    {
        _context = context;
        ValidateId();
    }

}
