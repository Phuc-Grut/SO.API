using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class SurchargeGroupQueryAll : IQuery<IEnumerable<SurchargeGroupDto>>
{
    public SurchargeGroupQueryAll()
    {
    }
}

public class SurchargeGroupQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public SurchargeGroupQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class SurchargeGroupQueryCheckCode : IQuery<bool>
{

    public SurchargeGroupQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class SurchargeGroupQueryById : IQuery<SurchargeGroupDto>
{
    public SurchargeGroupQueryById()
    {
    }

    public SurchargeGroupQueryById(Guid surchargeGroupId)
    {
        SurchargeGroupId = surchargeGroupId;
    }

    public Guid SurchargeGroupId { get; set; }
}
public class SurchargeGroupPagingQuery : FopQuery, IQuery<PagedResult<List<SurchargeGroupDto>>>
{
    public SurchargeGroupPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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


public class SurchargeGroupQueryHandler : IQueryHandler<SurchargeGroupQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<SurchargeGroupQueryAll, IEnumerable<SurchargeGroupDto>>,
                                         IQueryHandler<SurchargeGroupQueryCheckCode, bool>,
                                         IQueryHandler<SurchargeGroupQueryById, SurchargeGroupDto>,
                                         IQueryHandler<SurchargeGroupPagingQuery, PagedResult<List<SurchargeGroupDto>>>
{
    private readonly ISurchargeGroupRepository _repository;
    public SurchargeGroupQueryHandler(ISurchargeGroupRepository repository)
    {
        _repository = repository;
    }
    public async Task<bool> Handle(SurchargeGroupQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<SurchargeGroupDto> Handle(SurchargeGroupQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.SurchargeGroupId);
        var result = new SurchargeGroupDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Note = item.Note,
            DisplayOrder = item.DisplayOrder,
            Status = item.Status,
            CreatedByName = item.CreatedByName,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedByName = item.UpdatedByName,
            UpdatedBy = item.UpdatedBy
        };
        return result;
    }

    public async Task<PagedResult<List<SurchargeGroupDto>>> Handle(SurchargeGroupPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<SurchargeGroupDto>>();
        var filter = new Dictionary<string, object>();
        var fopRequest = FopExpressionBuilder<SurchargeGroup>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, fopRequest);
        var data = items.Select(item => new SurchargeGroupDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            Note = item.Note,
            DisplayOrder = item.DisplayOrder,
            CreatedByName = item.CreatedByName,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedByName = item.UpdatedByName,
            UpdatedBy = item.UpdatedBy
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<SurchargeGroupDto>> Handle(SurchargeGroupQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new SurchargeGroupDto()
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

    public async Task<IEnumerable<ComboBoxDto>> Handle(SurchargeGroupQueryComboBox request, CancellationToken cancellationToken)
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
