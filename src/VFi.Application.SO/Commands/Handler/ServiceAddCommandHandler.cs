using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class ServiceAddCommandHandler : CommandHandler, IRequestHandler<ServiceAddAddCommand, ValidationResult>,
                                                    IRequestHandler<ServiceAddDeleteCommand, ValidationResult>,
                                                    IRequestHandler<ServiceAddEditCommand, ValidationResult>,
                                                    IRequestHandler<ServiceAddSortCommand, ValidationResult>
{
    private readonly IServiceAddRepository _repository;

    public ServiceAddCommandHandler(IServiceAddRepository serviceAddRepository)
    {
        _repository = serviceAddRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(ServiceAddAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var serviceAdd = new ServiceAdd
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            CalculationMethod = request.CalculationMethod,
            Price = request.Price,
            PriceSyntax = request.PriceSyntax,
            MinPrice = request.MinPrice,
            MaxPrice = request.MaxPrice,
            PayLater = request.PayLater,
            Status = request.Status,
            Tags = request.Tags,
            Currency = request.Currency,
            CurrencyName = request.CurrencyName,
            DisplayOrder = request.DisplayOrder,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
            CreatedByName = request.CreatedByName
        };

        _repository.Add(serviceAdd);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ServiceAddDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var serviceAdd = new ServiceAdd
        {
            Id = request.Id
        };

        _repository.Remove(serviceAdd);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ServiceAddEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var data = await _repository.GetById(request.Id);

        data.Code = request.Code;
        data.Name = request.Name;
        data.Description = request.Description;
        data.CalculationMethod = request.CalculationMethod;
        data.Price = request.Price;
        data.PriceSyntax = request.PriceSyntax;
        data.MinPrice = request.MinPrice;
        data.MaxPrice = request.MaxPrice;
        data.PayLater = request.PayLater;
        data.Status = request.Status;
        data.Tags = request.Tags;
        data.Currency = request.Currency;
        data.CurrencyName = request.CurrencyName;
        data.DisplayOrder = request.DisplayOrder;
        data.UpdatedBy = request.UpdatedBy;
        data.UpdatedDate = request.UpdatedDate;
        data.UpdatedByName = request.UpdatedByName;

        _repository.Update(data);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ServiceAddSortCommand request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetAll();

        List<ServiceAdd> serviceAdds = new List<ServiceAdd>();

        foreach (var sort in request.SortList)
        {
            ServiceAdd serviceAdd = data.FirstOrDefault(c => c.Id == sort.Id);
            if (serviceAdd != null)
            {
                serviceAdd.DisplayOrder = sort.SortOrder;
                serviceAdds.Add(serviceAdd);
            }
        }
        _repository.Update(serviceAdds);
        return await Commit(_repository.UnitOfWork);
    }
}
