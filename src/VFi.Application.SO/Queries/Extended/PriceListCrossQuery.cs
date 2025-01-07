using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class PriceListCrossQueryAll : IQuery<IEnumerable<PriceListCrossDto>>
{
    public PriceListCrossQueryAll()
    {
    }
}

public class PriceListCrossQueryComboBox : IQuery<IEnumerable<PriceListCrossListboxDto>>
{
    public PriceListCrossQueryComboBox(string keyword, int? status, Guid? routerShippingId)
    {
        Status = status;
        RouterShippingId = routerShippingId;
        Keyword = keyword;
    }
    public string Keyword { get; set; }
    public int? Status { get; set; }
    public Guid? RouterShippingId { get; set; }
}
public class PriceListCrossQueryCheckCode : IQuery<bool>
{

    public PriceListCrossQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class PriceListCrossQueryById : IQuery<PriceListCrossDto>
{
    public PriceListCrossQueryById()
    {
    }

    public PriceListCrossQueryById(Guid priceListCrossId)
    {
        PriceListCrossId = priceListCrossId;
    }

    public Guid PriceListCrossId { get; set; }
}

public class PriceListCrossQueryByAccount : FopQuery, IQuery<PagedResult<List<PriceListCrossDetailDto>>>
{
    public PriceListCrossQueryByAccount(Guid id, Guid? routerShippingId, string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        AccountId = id;
        RouterShippingId = routerShippingId;
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public string Keyword { get; set; }
    public Guid AccountId { get; set; }
    public Guid? RouterShippingId { get; set; }
}
public class PriceListCrossPagingQuery : FopQuery, IQuery<PagedResult<List<PriceListCrossDto>>>
{
    public PriceListCrossPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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


public class PriceListCrossQueryHandler : IQueryHandler<PriceListCrossQueryComboBox, IEnumerable<PriceListCrossListboxDto>>,
                                          IQueryHandler<PriceListCrossQueryAll, IEnumerable<PriceListCrossDto>>,
                                          IQueryHandler<PriceListCrossQueryCheckCode, bool>,
                                          IQueryHandler<PriceListCrossQueryById, PriceListCrossDto>,
                                          IQueryHandler<PriceListCrossQueryByAccount, PagedResult<List<PriceListCrossDetailDto>>>,
                                          IQueryHandler<PriceListCrossPagingQuery, PagedResult<List<PriceListCrossDto>>>
{
    private readonly IPriceListCrossRepository _repository;
    private readonly IPriceListCrossDetailRepository _priceListCrossDetailRepository;
    private readonly ICustomerRepository _customerRepository;
    public PriceListCrossQueryHandler(IPriceListCrossRepository repository, IPriceListCrossDetailRepository priceListCrossDetailRepository, ICustomerRepository customerRepository)
    {
        _repository = repository;
        _customerRepository = customerRepository;
        _priceListCrossDetailRepository = priceListCrossDetailRepository;
    }
    public async Task<PagedResult<List<PriceListCrossDetailDto>>> Handle(PriceListCrossQueryByAccount request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<PriceListCrossDetailDto>>();
        var customer = await _customerRepository.GetByAccountId(request.AccountId);

        bool flag = true;
        Guid? priceListCrossId = null;
        if (customer is not null && customer.CustomerPriceListCross.Count > 0)
        {
            var priceListCross = customer.CustomerPriceListCross.FirstOrDefault(x => x.RouterShippingId.Equals(request.RouterShippingId));
            if (priceListCross is null)
            {
                flag = false;
            }
            else
            {
                priceListCrossId = priceListCross.PriceListCrossId;
            }
        }
        else
        {
            flag = false;
        }

        if (!flag)
        {
            var priceListCross = await _repository.GetDefaultByRouterShipping(request.RouterShippingId ?? Guid.NewGuid());
            if (priceListCross is null)
            {
                return response;
            }
            else
            {
                priceListCrossId = priceListCross.Id;
            }
        }

        var fopRequest = FopExpressionBuilder<PriceListCross>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        filter.Add("priceListCrossId", priceListCrossId);
        var (datas, count) = await _priceListCrossDetailRepository.Filter(request.Keyword, filter, fopRequest);

        var result = datas.Select(x => new PriceListCrossDetailDto()
        {
            Id = x.Id,
            PriceListCrossId = x.PriceListCrossId,
            PriceListCross = x.PriceListCross,
            Note = x.Note,
            CommodityGroupId = x.CommodityGroupId,
            CommodityGroupCode = x.CommodityGroupCode,
            CommodityGroupName = x.CommodityGroupName,
            AirFreight = x.AirFreight,
            SeaFreight = x.SeaFreight,
            Currency = x.Currency,
            Status = x.Status,
            CreatedBy = x.CreatedBy,
            CreatedDate = x.CreatedDate,
            UpdatedBy = x.UpdatedBy,
            UpdatedDate = x.UpdatedDate,
            CreatedByName = x.CreatedByName,
            UpdatedByName = x.UpdatedByName
        }).ToList();

        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
    public async Task<bool> Handle(PriceListCrossQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<PriceListCrossDto> Handle(PriceListCrossQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.PriceListCrossId);
        var result = new PriceListCrossDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Default = item.Default,
            RouterShipping = item.RouterShipping,
            RouterShippingId = item.RouterShippingId,
            Details = item.PriceListCrossDetail.OrderBy(x => x.DisplayOrder).Select(x => new PriceListCrossDetailDto()
            {
                Id = x.Id,
                PriceListCrossId = x.PriceListCrossId,
                PriceListCross = x.PriceListCross,
                Note = x.Note,
                CommodityGroupId = x.CommodityGroupId,
                CommodityGroupCode = x.CommodityGroupCode,
                CommodityGroupName = x.CommodityGroupName,
                AirFreight = x.AirFreight,
                SeaFreight = x.SeaFreight,
                Currency = x.Currency,
                Status = x.Status,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                CreatedByName = x.CreatedByName,
                UpdatedByName = x.UpdatedByName

            }).ToList(),
            UpdatedByName = item.UpdatedByName,
            DisplayOrder = item.DisplayOrder,
            Status = item.Status,
            CreatedByName = item.CreatedByName,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy
        };
        return result;
    }

    public async Task<PagedResult<List<PriceListCrossDto>>> Handle(PriceListCrossPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<PriceListCrossDto>>();
        var filter = new Dictionary<string, object>();
        var fopRequest = FopExpressionBuilder<PriceListCross>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, fopRequest);
        var data = items.Select(item => new PriceListCrossDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            Default = item.Default,
            DisplayOrder = item.DisplayOrder,
            RouterShipping = item.RouterShipping,
            RouterShippingId = item.RouterShippingId,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedByName = item.UpdatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<PriceListCrossDto>> Handle(PriceListCrossQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new PriceListCrossDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Default = item.Default,
            DisplayOrder = item.DisplayOrder,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            UpdatedDate = item.UpdatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy
        });
        return result;
    }

    public async Task<IEnumerable<PriceListCrossListboxDto>> Handle(PriceListCrossQueryComboBox request, CancellationToken cancellationToken)
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
        var items = await _repository.GetListBox(request.Keyword, filter);
        var result = items.Select(x => new PriceListCrossListboxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            RouterShippingId = x.RouterShippingId,
            RouterShipping = x.RouterShipping,
            Status = x.Status
        });
        return result;
    }
}
