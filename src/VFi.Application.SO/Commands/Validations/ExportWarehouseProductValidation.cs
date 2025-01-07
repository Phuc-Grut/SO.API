using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;
public abstract class ExportWarehouseProductValidation<T> : AbstractValidator<T> where T : ExportWarehouseProductCommand
{
    protected readonly IExportWarehouseProductRepository _exportWarehouseProductRepository;
    protected Guid Id;

    public ExportWarehouseProductValidation(IExportWarehouseProductRepository exportWarehouseProductRepository)
    {
        _exportWarehouseProductRepository = exportWarehouseProductRepository;
    }

    public ExportWarehouseProductValidation(IExportWarehouseProductRepository exportWarehouseProductRepository, Guid id)
    {
        _exportWarehouseProductRepository = exportWarehouseProductRepository;
        Id = id;
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
}

public class ExportWarehouseProductDeleteCommandValidation : AbstractValidator<ExportWarehouseProductDeleteCommand>
{
    protected readonly IExportWarehouseProductRepository _exportWarehouseProductRepository;
    public ExportWarehouseProductDeleteCommandValidation(IExportWarehouseProductRepository exportWarehouseProductRepository)
    {
        _exportWarehouseProductRepository = exportWarehouseProductRepository;
        ValidateId();
        ValidateExistExportWarehouseProduct();
    }
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
    protected void ValidateExistExportWarehouseProduct()
    {
        var abc = RuleFor(c => c.Id).Must(IsExistExportWarehouseProduct)
            .WithMessage("ExportWarehouseProduct is not exist");
    }

    protected bool IsExistExportWarehouseProduct(Guid id)
    {
        var ewProduct = _exportWarehouseProductRepository.GetById(id).Result;
        if(ewProduct != null)
        {
            return true;
        }
        return false;
    }
}