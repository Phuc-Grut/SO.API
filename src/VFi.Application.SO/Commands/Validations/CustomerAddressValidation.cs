using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class CustomerAddressValidation<T> : AbstractValidator<T> where T : CustomerAddressCommand

{
    protected readonly ICustomerAddressRepository _context;
    private Guid Id;

    public CustomerAddressValidation(ICustomerAddressRepository context)
    {
        _context = context;
    }
    public CustomerAddressValidation(ICustomerAddressRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    //protected void ValidateAddCodeUnique()
    //{
    //    RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
    //}

    //private bool IsAddUnique(string code)
    //{
    //    var model = _context.GetByCode(code).Result;

    //    if (model == null)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
    //private bool IsEditUnique(string? code)
    //{
    //    var model = _context.GetByCode(code).Result;

    //    if (model == null || model.Id == Id)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
    //protected void ValidateEditCodeUnique()
    //{
    //    RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
    //}

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
        RuleFor(c => c.Address)
            .NotEmpty().WithMessage("Please ensure you have entered the address")
            .Length(2, 250).WithMessage("The address must have between 2 and 250 characters");
    }
}
public class CustomerAddressAddCommandValidation : CustomerAddressValidation<CustomerAddressAddCommand>
{
    public CustomerAddressAddCommandValidation(ICustomerAddressRepository context) : base(context)
    {
        ValidateId();
        //ValidateAddCodeUnique();
        //ValidateCode();
        ValidateName();
    }
}
public class CustomerAddressEditCommandValidation : CustomerAddressValidation<CustomerAddressEditCommand>
{
    public CustomerAddressEditCommandValidation(ICustomerAddressRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        //ValidateEditCodeUnique();
        //ValidateCode();
        ValidateName();
    }
}

public class CustomerAddressDeleteCommandValidation : CustomerAddressValidation<CustomerAddressDeleteCommand>
{
    public CustomerAddressDeleteCommandValidation(ICustomerAddressRepository context) : base(context)
    {
        ValidateId();
    }
}
