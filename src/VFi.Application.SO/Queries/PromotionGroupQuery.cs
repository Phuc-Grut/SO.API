using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class PromotionGroupQueryAll : IQuery<IEnumerable<PromotionGroupDto>>
{
    public PromotionGroupQueryAll()
    {
    }
}

public class PromotionGroupQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public PromotionGroupQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class PromotionGroupQueryCheckCode : IQuery<bool>
{

    public PromotionGroupQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class PromotionGroupQueryById : IQuery<PromotionGroupDto>
{
    public PromotionGroupQueryById()
    {
    }

    public PromotionGroupQueryById(Guid promotionGroupId)
    {
        PromotionGroupId = promotionGroupId;
    }

    public Guid PromotionGroupId { get; set; }
}
public class PromotionGroupPagingQuery : FopQuery, IQuery<PagedResult<List<PromotionGroupDto>>>
{
    public PromotionGroupPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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

public class PromotionGroupQueryHandler : IQueryHandler<PromotionGroupQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<PromotionGroupQueryAll, IEnumerable<PromotionGroupDto>>,
                                         IQueryHandler<PromotionGroupQueryCheckCode, bool>,
                                         IQueryHandler<PromotionGroupQueryById, PromotionGroupDto>,
                                         IQueryHandler<PromotionGroupPagingQuery, PagedResult<List<PromotionGroupDto>>>
{
    private readonly IPromotionGroupRepository _repository;
    public PromotionGroupQueryHandler(IPromotionGroupRepository promotionGroupRespository)
    {
        _repository = promotionGroupRespository;
    }
    public async Task<bool> Handle(PromotionGroupQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<PromotionGroupDto> Handle(PromotionGroupQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.PromotionGroupId);
        var result = new PromotionGroupDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy
        };
        return result;
    }

    public async Task<PagedResult<List<PromotionGroupDto>>> Handle(PromotionGroupPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<PromotionGroupDto>>();

        var fopRequest = FopExpressionBuilder<PromotionGroup>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _repository.Filter(request.Keyword, request.Status, fopRequest);
        var data = datas.Select(item => new PromotionGroupDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<PromotionGroupDto>> Handle(PromotionGroupQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new PromotionGroupDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            UpdatedDate = item.UpdatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(PromotionGroupQueryComboBox request, CancellationToken cancellationToken)
    {

        var items = await _repository.GetListCbx(request.Status);
        var result = items.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
