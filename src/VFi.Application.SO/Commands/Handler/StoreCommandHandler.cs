using FluentValidation.Results;
using MassTransit.Internals;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class StoreCommandHandler : CommandHandler, IRequestHandler<StoreAddCommand, ValidationResult>,
                                                     IRequestHandler<StoreDeleteCommand, ValidationResult>,
                                                     IRequestHandler<StoreEditCommand, ValidationResult>,
                                                     IRequestHandler<StoreSortCommand, ValidationResult>,
                                                     IRequestHandler<SetupPriceListCommand, ValidationResult>

{
    private readonly IStoreRepository _repository;
    private readonly IStorePriceListRepository _storePriceListRepository;
    private readonly IContextUser _context;

    public StoreCommandHandler(IStoreRepository storeRepository, IContextUser contextUser, IStorePriceListRepository storePriceListRepository)
    {
        _repository = storeRepository;
        _storePriceListRepository = storePriceListRepository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(StoreAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var store = new Store
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Address = request.Address,
            Phone = request.Phone,
            DisplayOrder = request.DisplayOrder,
            CreatedBy = createdBy,
            CreatedByName = createName,
            CreatedDate = createdDate,
            Status = request.Status
        };

        _repository.Add(store);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(StoreDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var store = new Store
        {
            Id = request.Id
        };

        _repository.Remove(store);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(StoreEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.Code = request.Code;
        item.Name = request.Name;
        item.Description = request.Description;
        item.Address = request.Address;
        item.Phone = request.Phone;
        item.DisplayOrder = request.DisplayOrder;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;
        item.UpdatedDate = updatedDate;
        item.Status = request.Status;


        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(StoreSortCommand request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetAll();

        List<Store> stores = new List<Store>();

        foreach (var sort in request.SortList)
        {
            Store store = data.FirstOrDefault(c => c.Id == sort.Id);
            if (store != null)
            {
                store.DisplayOrder = sort.SortOrder;
                stores.Add(store);
            }
        }
        _repository.Update(stores);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(SetupPriceListCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var obj = await _repository.GetById(request.Id);

        List<StorePriceList> storePriceListUpdate = new List<StorePriceList>();
        List<StorePriceList> storePriceListDelete = new List<StorePriceList>();
        if (request.StorePriceList.Any())
        {
            foreach (var item in obj.StorePriceList)
            {
                var u = request.StorePriceList.Where(x => x.Id == item.Id).FirstOrDefault();
                if (u != null)
                {
                    item.PriceListId = u.PriceListId;
                    item.PriceListName = u.PriceListName;
                    item.Default = u.Default;
                    item.DisplayOrder = u.DisplayOrder;
                    storePriceListUpdate.Add(item);
                    request.StorePriceList.Remove(u);
                }
                else
                {
                    storePriceListDelete.Add(item);
                }
            }

            List<StorePriceList> storePriceListAdd = new List<StorePriceList>();
            for (int i = 0; i < request.StorePriceList.Count; i++)
            {
                var u = request.StorePriceList[i];
                var orderServiceAdd = obj.StorePriceList.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (orderServiceAdd is null)
                {
                    storePriceListAdd.Add(new StorePriceList()
                    {
                        Id = Guid.NewGuid(),
                        StoreId = request.Id,
                        PriceListId = u.PriceListId,
                        PriceListName = u.PriceListName,
                        Default = u.Default,
                        DisplayOrder = u.DisplayOrder
                    });
                    request.StorePriceList.Remove(request.StorePriceList[i]);
                    i--;
                }
            }

            _storePriceListRepository.Update(storePriceListUpdate);
            _storePriceListRepository.Add(storePriceListAdd);
            _storePriceListRepository.Remove(storePriceListDelete);
            var result = await Commit(_storePriceListRepository.UnitOfWork);
            return result;


        }
        else if (obj.StorePriceList.Any())
        {
            foreach (var item in obj.StorePriceList)
            {
                storePriceListDelete.Add(item);
            }
            _storePriceListRepository.Remove(storePriceListDelete);
            var result = await Commit(_storePriceListRepository.UnitOfWork);
            return result;
        }

        return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "No change") } };

    }

}
