using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;

namespace VFi.Application.SO.Commands.Validations;

public abstract class ServiceAddValidation<T> : AbstractValidator<T> where T : ServiceAddCommand

{
    protected readonly IServiceAddRepository _context;
    private Guid Id;
    public ServiceAddValidation(IServiceAddRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    public ServiceAddValidation(IServiceAddRepository context)
    {
        _context = context;
    }
    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
    }

    private bool IsAddUnique(string? code)
    {
        return !_context.CheckExist(code, null).Result;
    }
    protected void ValidateEditCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
    }
    private bool IsEditUnique(string? code)
    {
        return !_context.CheckExist(code, Id).Result;
    }
    protected void ValidateIdExists()
    {
        RuleFor(x => x.Id).Must(IsExist).WithMessage("Id nots exists");
    }
    private bool IsExist(Guid id)
    {
        return _context.CheckExistById(id).Result;
    }
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the Id");
    }
    protected void ValidateCalculationMethod()
    {
        RuleFor(c => c.CalculationMethod)
            .NotEmpty().WithMessage("Please ensure you have entered the calculationMethod");
    }
    protected void ValidateName()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Please ensure you have entered the name")
            .Length(2, 400).WithMessage("The name must have between 2 and 400 characters");
    }
    protected void ValidateDisplayOrder()
    {
        RuleFor(c => c.DisplayOrder)
                  .NotNull().WithMessage("Please ensure you have entered the displayOrder");
    }

}
public class ServiceAddAddCommandValidation : ServiceAddValidation<ServiceAddAddCommand>
{
    public ServiceAddAddCommandValidation(IServiceAddRepository context) : base(context)
    {
        ValidateId();
        //ValidateCalculationMethod();
        ValidateDisplayOrder();
        ValidateName();
        ValidateAddCodeUnique();
    }
}
public class ServiceAddEditCommandValidation : ServiceAddValidation<ServiceAddEditCommand>
{
    public ServiceAddEditCommandValidation(IServiceAddRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        //ValidateCalculationMethod();
        ValidateIdExists();
        ValidateEditCodeUnique();
        ValidateDisplayOrder();
        ValidateName();
    }
}

public class ServiceAddDeleteCommandValidation : ServiceAddValidation<ServiceAddDeleteCommand>
{
    public ServiceAddDeleteCommandValidation(IServiceAddRepository context) : base(context)
    {
        ValidateIdExists();
        ValidateId();
    }
}
