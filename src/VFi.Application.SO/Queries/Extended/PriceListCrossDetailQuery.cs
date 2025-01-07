using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class PriceListCrossDetailQueryAll : IQuery<IEnumerable<PriceListCrossDetailDto>>
{
    public PriceListCrossDetailQueryAll()
    {
    }
}

public class PriceListCrossDetailQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public PriceListCrossDetailQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class PriceListCrossDetailQueryById : IQuery<PriceListCrossDetailDto>
{
    public PriceListCrossDetailQueryById()
    {
    }


    public PriceListCrossDetailQueryById(Guid PriceListCrossDetailId)
    {
        PriceListCrossDetailId = PriceListCrossDetailId;
    }

    public Guid PriceListCrossDetailId { get; set; }
}
public class PriceListCrossDetailPagingQuery : FopQuery, IQuery<PagedResult<List<PriceListCrossDetailDto>>>
{
    public PriceListCrossDetailPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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


public class PriceListCrossDetailQueryHandler : IQueryHandler<PriceListCrossDetailQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<PriceListCrossDetailQueryAll, IEnumerable<PriceListCrossDetailDto>>,
                                         IQueryHandler<PriceListCrossDetailQueryById, PriceListCrossDetailDto>,
                                         IQueryHandler<PriceListCrossDetailPagingQuery, PagedResult<List<PriceListCrossDetailDto>>>
{
    private readonly IPriceListCrossDetailRepository _repository;
    public PriceListCrossDetailQueryHandler(IPriceListCrossDetailRepository repository)
    {
        _repository = repository;
    }


    public async Task<PriceListCrossDetailDto> Handle(PriceListCrossDetailQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.PriceListCrossDetailId);
        var result = new PriceListCrossDetailDto()
        {
            Id = item.Id,
            PriceListCrossId = item.PriceListCrossId,
            PriceListCross = item.PriceListCross,
            Note = item.Note,
            CommodityGroupId = item.CommodityGroupId,
            CommodityGroupCode = item.CommodityGroupCode,
            CommodityGroupName = item.CommodityGroupName,
            AirFreight = item.AirFreight,
            SeaFreight = item.SeaFreight,
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

    public async Task<PagedResult<List<PriceListCrossDetailDto>>> Handle(PriceListCrossDetailPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<PriceListCrossDetailDto>>();
        var filter = new Dictionary<string, object>();
        var fopRequest = FopExpressionBuilder<PriceListCrossDetail>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, fopRequest);
        var data = items.Select(item => new PriceListCrossDetailDto()
        {
            Id = item.Id,
            PriceListCrossId = item.PriceListCrossId,
            PriceListCross = item.PriceListCross,
            Note = item.Note,
            CommodityGroupId = item.CommodityGroupId,
            CommodityGroupCode = item.CommodityGroupCode,
            CommodityGroupName = item.CommodityGroupName,
            AirFreight = item.AirFreight,
            SeaFreight = item.SeaFreight,
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

    public async Task<IEnumerable<PriceListCrossDetailDto>> Handle(PriceListCrossDetailQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new PriceListCrossDetailDto()
        {
            Id = item.Id,
            PriceListCrossId = item.PriceListCrossId,
            PriceListCross = item.PriceListCross,
            Note = item.Note,
            CommodityGroupId = item.CommodityGroupId,
            CommodityGroupCode = item.CommodityGroupCode,
            CommodityGroupName = item.CommodityGroupName,
            AirFreight = item.AirFreight,
            SeaFreight = item.SeaFreight,
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

    public async Task<IEnumerable<ComboBoxDto>> Handle(PriceListCrossDetailQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        filter.Add("status", request.Status);
        var items = await _repository.GetListBox(filter);
        var result = items.Select(x => new ComboBoxDto()
        {
            Value = x.Id
        });
        return result;
    }
}
