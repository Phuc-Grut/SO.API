using Consul;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;


public class StoreQueryAll : IQuery<IEnumerable<StoreDto>>
{
    public StoreQueryAll()
    {
    }
}

public class StoreQueryListBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public StoreQueryListBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class StoreQueryCheckExist : IQuery<bool>
{

    public StoreQueryCheckExist(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}
public class StoreQueryById : IQuery<StoreDto>
{
    public StoreQueryById()
    {
    }

    public StoreQueryById(Guid storeId)
    {
        StoreId = storeId;
    }

    public Guid StoreId { get; set; }
}
public class StorePagingQuery : FopQuery, IQuery<PagedResult<List<StoreDto>>>
{
    public StorePagingQuery(int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
    }
    public int? Status { get; set; }

}

public class StoreQueryHandler : IQueryHandler<StoreQueryListBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<StoreQueryAll, IEnumerable<StoreDto>>,
                                         IQueryHandler<StoreQueryCheckExist, bool>,
                                         IQueryHandler<StoreQueryById, StoreDto>,
                                         IQueryHandler<StorePagingQuery, PagedResult<List<StoreDto>>>
{
    private readonly IStoreRepository _storeRepository;
    public StoreQueryHandler(IStoreRepository storeRespository)
    {
        _storeRepository = storeRespository;
    }
    public async Task<bool> Handle(StoreQueryCheckExist request, CancellationToken cancellationToken)
    {
        return await _storeRepository.CheckExistById(request.Id);
    }

    public async Task<StoreDto> Handle(StoreQueryById request, CancellationToken cancellationToken)
    {
        var store = await _storeRepository.GetById(request.StoreId);
        var result = new StoreDto()
        {
            Id = store.Id,
            Code = store.Code,
            Name = store.Name,
            Description = store.Description,
            Address = store.Address,
            Phone = store.Phone,
            DisplayOrder = store.DisplayOrder,
            CreatedBy = store.CreatedBy,
            CreatedByName = store.CreatedByName,
            CreatedDate = store.CreatedDate.Value,
            UpdatedBy = store.UpdatedBy,
            UpdatedByName = store.UpdatedByName,
            UpdatedDate = store.UpdatedDate,
            Status = store.Status
        };
        return result;
    }

    public async Task<PagedResult<List<StoreDto>>> Handle(StorePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<StoreDto>>();

        var fopRequest = FopExpressionBuilder<Store>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
        { filter.Add("status", request.Status.ToString()); }

        var (Stores, count) = await _storeRepository.Filter(filter, fopRequest);

        var data = Stores.Select(store => new StoreDto()
        {
            Id = store.Id,
            Code = store.Code,
            Name = store.Name,
            Description = store.Description,
            Address = store.Address,
            Phone = store.Phone,
            DisplayOrder = store.DisplayOrder,
            CreatedBy = store.CreatedBy,
            CreatedByName = store.CreatedByName,
            CreatedDate = store.CreatedDate.Value,
            UpdatedBy = store.UpdatedBy,
            UpdatedByName = store.UpdatedByName,
            UpdatedDate = store.UpdatedDate,
            Status = store.Status
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<StoreDto>> Handle(StoreQueryAll request, CancellationToken cancellationToken)
    {
        var stores = await _storeRepository.GetAll();
        var result = stores.Select(store => new StoreDto()
        {
            Id = store.Id,
            Code = store.Code,
            Name = store.Name,
            Description = store.Description,
            Address = store.Address,
            Phone = store.Phone,
            DisplayOrder = store.DisplayOrder,
            CreatedBy = store.CreatedBy,
            CreatedByName = store.CreatedByName,
            CreatedDate = store.CreatedDate.Value,
            UpdatedBy = store.UpdatedBy,
            UpdatedByName = store.UpdatedByName,
            UpdatedDate = store.UpdatedDate,
            Status = store.Status
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(StoreQueryListBox request, CancellationToken cancellationToken)
    {

        var stores = await _storeRepository.GetListListBox(request.Status);
        var result = stores.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
