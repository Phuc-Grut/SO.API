using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class PriceListSurchargeQueryAll : IQuery<IEnumerable<PriceListSurchargeDto>>
{
    public PriceListSurchargeQueryAll()
    {
    }
}

public class PriceListSurchargeQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public PriceListSurchargeQueryComboBox(int? status, Guid? routerShippingId)
    {
        Status = status;
        RouterShippingId = routerShippingId;
    }
    public int? Status { get; set; }
    public Guid? RouterShippingId { get; set; }
}
public class PriceListSurchargeQueryCheckCode : IQuery<bool>
{

    public PriceListSurchargeQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class PriceListSurchargeQueryById : IQuery<PriceListSurchargeDto>
{
    public PriceListSurchargeQueryById()
    {
    }

    public PriceListSurchargeQueryById(Guid priceListSurchargeId)
    {
        PriceListSurchargeId = priceListSurchargeId;
    }

    public Guid PriceListSurchargeId { get; set; }
}
public class PriceListSurchargePagingQuery : FopQuery, IQuery<PagedResult<List<PriceListSurchargeDto>>>
{
    public PriceListSurchargePagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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


public class PriceListSurchargeQueryHandler : IQueryHandler<PriceListSurchargeQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<PriceListSurchargeQueryAll, IEnumerable<PriceListSurchargeDto>>,
                                         IQueryHandler<PriceListSurchargeQueryById, PriceListSurchargeDto>,
                                         IQueryHandler<PriceListSurchargePagingQuery, PagedResult<List<PriceListSurchargeDto>>>
{
    private readonly IPriceListSurchargeRepository _repository;
    public PriceListSurchargeQueryHandler(IPriceListSurchargeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PriceListSurchargeDto> Handle(PriceListSurchargeQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.PriceListSurchargeId);
        var result = new PriceListSurchargeDto()
        {
            Id = item.Id,
            Note = item.Note,
            RouterShippingId = item.RouterShippingId,
            RouterShipping = item.RouterShipping,
            SurchargeGroupId = item.SurchargeGroupId,
            SurchargeGroup = item.SurchargeGroup,
            Price = item.Price,
            Currency = item.Currency,
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

    public async Task<PagedResult<List<PriceListSurchargeDto>>> Handle(PriceListSurchargePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<PriceListSurchargeDto>>();
        var filter = new Dictionary<string, object>();
        var fopRequest = FopExpressionBuilder<PriceListSurcharge>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, fopRequest);
        var data = items.Select(item => new PriceListSurchargeDto()
        {
            Id = item.Id,
            Note = item.Note,
            RouterShippingId = item.RouterShippingId,
            RouterShipping = item.RouterShipping,
            SurchargeGroupId = item.SurchargeGroupId,
            SurchargeGroup = item.SurchargeGroup,
            Price = item.Price,
            Currency = item.Currency,
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

    public async Task<IEnumerable<PriceListSurchargeDto>> Handle(PriceListSurchargeQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new PriceListSurchargeDto()
        {
            Id = item.Id,
            Note = item.Note,
            RouterShippingId = item.RouterShippingId,
            RouterShipping = item.RouterShipping,
            SurchargeGroupId = item.SurchargeGroupId,
            SurchargeGroup = item.SurchargeGroup,
            Price = item.Price,
            Currency = item.Currency,
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

    public async Task<IEnumerable<ComboBoxDto>> Handle(PriceListSurchargeQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
        {
            filter.Add("status", request.Status);
        }
        if (request.RouterShippingId != null)
        {
            filter.Add("routerShippingId", request.RouterShippingId);
        }
        var items = await _repository.GetListBox(filter);
        var result = items.Select(x => new ComboBoxDto()
        {
            Key = x.RouterShipping,
            Value = x.Id,
            Label = x.SurchargeGroup
        });
        return result;
    }
}
