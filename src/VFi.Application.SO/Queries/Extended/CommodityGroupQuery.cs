using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class CommodityGroupQueryAll : IQuery<IEnumerable<CommodityGroupDto>>
{
    public CommodityGroupQueryAll()
    {
    }
}

public class CommodityGroupQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public CommodityGroupQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class CommodityGroupQueryCheckCode : IQuery<bool>
{

    public CommodityGroupQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class CommodityGroupQueryById : IQuery<CommodityGroupDto>
{
    public CommodityGroupQueryById()
    {
    }

    public CommodityGroupQueryById(Guid commodityGroupId)
    {
        CommodityGroupId = commodityGroupId;
    }

    public Guid CommodityGroupId { get; set; }
}
public class CommodityGroupPagingQuery : FopQuery, IQuery<PagedResult<List<CommodityGroupDto>>>
{
    public CommodityGroupPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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


public class CommodityGroupQueryHandler : IQueryHandler<CommodityGroupQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<CommodityGroupQueryAll, IEnumerable<CommodityGroupDto>>,
                                         IQueryHandler<CommodityGroupQueryCheckCode, bool>,
                                         IQueryHandler<CommodityGroupQueryById, CommodityGroupDto>,
                                         IQueryHandler<CommodityGroupPagingQuery, PagedResult<List<CommodityGroupDto>>>
{
    private readonly ICommodityGroupRepository _repository;
    public CommodityGroupQueryHandler(ICommodityGroupRepository repository)
    {
        _repository = repository;
    }
    public async Task<bool> Handle(CommodityGroupQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<CommodityGroupDto> Handle(CommodityGroupQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.CommodityGroupId);
        var result = new CommodityGroupDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Note = item.Note,
            DisplayOrder = item.DisplayOrder,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy
        };
        return result;
    }

    public async Task<PagedResult<List<CommodityGroupDto>>> Handle(CommodityGroupPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<CommodityGroupDto>>();
        var filter = new Dictionary<string, object>();
        var fopRequest = FopExpressionBuilder<CommodityGroup>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, fopRequest);
        var data = items.Select(item => new CommodityGroupDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            Note = item.Note,
            DisplayOrder = item.DisplayOrder,
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

    public async Task<IEnumerable<CommodityGroupDto>> Handle(CommodityGroupQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new CommodityGroupDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Note = item.Note,
            DisplayOrder = item.DisplayOrder,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            UpdatedDate = item.UpdatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(CommodityGroupQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
        {
            filter.Add("status", request.Status);
        }
        var items = await _repository.GetListBox(filter);
        var result = items.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
