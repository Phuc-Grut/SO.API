using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class ProductionOrdersDetailValidation<T> : AbstractValidator<T> where T : ProductionOrdersDetailCommand

{
    protected readonly IProductionOrdersDetailRepository _context;
    private Guid Id;

    public ProductionOrdersDetailValidation(IProductionOrdersDetailRepository context)
    {
        _context = context;
    }
    public ProductionOrdersDetailValidation(IProductionOrdersDetailRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }

}
public class ProductionOrdersDetailEditPackageCommandValidation : ProductionOrdersDetailValidation<ProductionOrdersDetailEditPackageCommand>
{
    public ProductionOrdersDetailEditPackageCommandValidation(IProductionOrdersDetailRepository context) : base(context)
    {
        ValidateId();
    }
}
public class ProductionOrdersDetailCancelCommandValidation : ProductionOrdersDetailValidation<ProductionOrdersDetailCancelCommand>
{
    public ProductionOrdersDetailCancelCommandValidation(IProductionOrdersDetailRepository context) : base(context)
    {
        ValidateId();
    }
}
public class ProductionOrdersDetailCompleteCommandValidation : ProductionOrdersDetailValidation<ProductionOrdersDetailCompleteCommand>
{
    public ProductionOrdersDetailCompleteCommandValidation(IProductionOrdersDetailRepository context) : base(context)
    {
        ValidateId();
    }
}
