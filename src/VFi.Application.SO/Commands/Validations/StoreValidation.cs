using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;

namespace VFi.Application.SO.Commands.Validations;

public abstract class StoreValidation<T> : AbstractValidator<T> where T : StoreCommand

{
    protected readonly IStoreRepository _context;
    private Guid Id;
    public StoreValidation(IStoreRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    public StoreValidation(IStoreRepository context)
    {
        _context = context;
    }
    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Store code already exists");
    }

    private bool IsAddUnique(string? code)
    {
        return !_context.CheckExist(code, null).Result;
    }
    protected void ValidateEditCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Store code already exists");
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

    protected void ValidateDisplayOrder()
    {
        RuleFor(c => c.DisplayOrder)
                  .NotNull().WithMessage("Please ensure you have entered the displayOrder");
    }

}
public class StoreAddCommandValidation : StoreValidation<StoreAddCommand>
{
    public StoreAddCommandValidation(IStoreRepository context) : base(context)
    {
        ValidateId();
        ValidateDisplayOrder();
        ValidateAddCodeUnique();
    }
}
public class StoreEditCommandValidation : StoreValidation<StoreEditCommand>
{
    public StoreEditCommandValidation(IStoreRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateIdExists();
        ValidateEditCodeUnique();
        ValidateDisplayOrder();
    }
}

public class StoreDeleteCommandValidation : StoreValidation<StoreDeleteCommand>
{
    public StoreDeleteCommandValidation(IStoreRepository context) : base(context)
    {
        ValidateIdExists();
        ValidateId();
    }
}
