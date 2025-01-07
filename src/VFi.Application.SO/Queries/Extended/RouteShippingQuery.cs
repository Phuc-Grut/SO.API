using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class RouteShippingQueryAll : IQuery<IEnumerable<RouteShippingDto>>
{
    public RouteShippingQueryAll()
    {
    }
}

public class RouteShippingQueryComboBox : IQuery<IEnumerable<RouteShippingListboxDto>>
{
    public RouteShippingQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class RouteShippingQueryCheckCode : IQuery<bool>
{

    public RouteShippingQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class RouteShippingQueryById : IQuery<RouteShippingDto>
{
    public RouteShippingQueryById()
    {
    }

    public RouteShippingQueryById(Guid routeShippingId)
    {
        RouteShippingId = routeShippingId;
    }

    public Guid RouteShippingId { get; set; }
}
public class RouteShippingPagingQuery : FopQuery, IQuery<PagedResult<List<RouteShippingDto>>>
{
    public RouteShippingPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        Keyword = keyword;
    }
    public string Filter { get; set; }
    public string Order { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int? Status { get; set; }
    public string? Keyword { get; set; }

}


public class RouteShippingQueryHandler : IQueryHandler<RouteShippingQueryComboBox, IEnumerable<RouteShippingListboxDto>>,
                                         IQueryHandler<RouteShippingQueryAll, IEnumerable<RouteShippingDto>>,
                                         IQueryHandler<RouteShippingQueryCheckCode, bool>,
                                         IQueryHandler<RouteShippingQueryById, RouteShippingDto>,
                                         IQueryHandler<RouteShippingPagingQuery, PagedResult<List<RouteShippingDto>>>
{
    private readonly IRouteShippingRepository _repository;
    public RouteShippingQueryHandler(IRouteShippingRepository repository)
    {
        _repository = repository;
    }
    public async Task<bool> Handle(RouteShippingQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<RouteShippingDto> Handle(RouteShippingQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.RouteShippingId);
        var result = new RouteShippingDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Note = item.Note,
            FromPost = item.FromPost,
            ToPost = item.ToPost,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<RouteShippingDto>>> Handle(RouteShippingPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<RouteShippingDto>>();
        var filter = new Dictionary<string, object>();
        var fopRequest = FopExpressionBuilder<RouteShipping>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, request.Status, fopRequest);
        var data = items.Select(item => new RouteShippingDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Note = item.Note,
            FromPost = item.FromPost,
            ToPost = item.ToPost,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<RouteShippingDto>> Handle(RouteShippingQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new RouteShippingDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Note = item.Note,
            FromPost = item.FromPost,
            ToPost = item.ToPost,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<RouteShippingListboxDto>> Handle(RouteShippingQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        filter.Add("status", request.Status);
        var items = await _repository.GetListBox(filter);
        var result = items.Select(x => new RouteShippingListboxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            FromPost = x.FromPost,
            ToPost = x.ToPost,
        });
        return result;
    }
}
