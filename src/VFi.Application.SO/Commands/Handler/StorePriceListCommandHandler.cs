using FluentValidation.Results;
using MediatR;
using VFi.Application.SO.Commands;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CRM.Commands;

internal class StorePriceListCommandHandler : CommandHandler,
                                    IRequestHandler<StorePriceListAddCommand, ValidationResult>,
                                    IRequestHandler<StorePriceListEditCommand, ValidationResult>,
                                    IRequestHandler<StorePriceListDeleteCommand, ValidationResult>
{
    private readonly IStorePriceListRepository _repository;

    public StorePriceListCommandHandler(IStorePriceListRepository storePriceListRepository)
    {
        _repository = storePriceListRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(StorePriceListAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new StorePriceList
        {
            Id = request.Id,
            StoreId = request.StoreId,
            PriceListId = request.PriceListId,
            PriceListName = request.PriceListName,
            Default = request.Default,
            CreatedDate = request.CreatedDate,
            UpdatedDate = request.UpdatedDate,
            CreatedBy = request.CreatedBy,
            UpdatedBy = request.UpdatedBy,
            DisplayOrder = request.DisplayOrder,
            CreatedByName = request.CreatedByName,
            UpdatedByName = request.UpdatedByName,
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(StorePriceListDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new StorePriceList
        {
            Id = request.Id,
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(StorePriceListEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = await _repository.GetById(request.Id);

        item.StoreId = request.StoreId;
        item.PriceListId = request.PriceListId;
        item.PriceListName = request.PriceListName;
        item.Default = request.Default;
        item.UpdatedDate = request.UpdatedDate;
        item.UpdatedBy = request.UpdatedBy;
        item.DisplayOrder = request.DisplayOrder;
        item.UpdatedByName = request.UpdatedByName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
