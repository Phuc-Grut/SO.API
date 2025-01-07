using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class PurchaseGroupQueryAll : IQuery<IEnumerable<PurchaseGroupDto>>
{
    public PurchaseGroupQueryAll()
    {
    }
}

public class PurchaseGroupQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public PurchaseGroupQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class PurchaseGroupQueryCheckCode : IQuery<bool>
{

    public PurchaseGroupQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class PurchaseGroupQueryById : IQuery<PurchaseGroupDto>
{
    public PurchaseGroupQueryById()
    {
    }

    public PurchaseGroupQueryById(Guid purchaseGroupId)
    {
        PurchaseGroupId = purchaseGroupId;
    }

    public Guid PurchaseGroupId { get; set; }
}
public class PurchaseGroupPagingQuery : FopQuery, IQuery<PagedResult<List<PurchaseGroupDto>>>
{
    public PurchaseGroupPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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


public class PurchaseGroupQueryHandler : IQueryHandler<PurchaseGroupQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<PurchaseGroupQueryAll, IEnumerable<PurchaseGroupDto>>,
                                         IQueryHandler<PurchaseGroupQueryCheckCode, bool>,
                                         IQueryHandler<PurchaseGroupQueryById, PurchaseGroupDto>,
                                         IQueryHandler<PurchaseGroupPagingQuery, PagedResult<List<PurchaseGroupDto>>>
{
    private readonly IPurchaseGroupRepository _repository;
    public PurchaseGroupQueryHandler(IPurchaseGroupRepository repository)
    {
        _repository = repository;
    }
    public async Task<bool> Handle(PurchaseGroupQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<PurchaseGroupDto> Handle(PurchaseGroupQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.PurchaseGroupId);
        var result = new PurchaseGroupDto()
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

    public async Task<PagedResult<List<PurchaseGroupDto>>> Handle(PurchaseGroupPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<PurchaseGroupDto>>();
        var filter = new Dictionary<string, object>();
        var fopRequest = FopExpressionBuilder<PurchaseGroup>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, fopRequest);
        var data = items.Select(item => new PurchaseGroupDto()
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

    public async Task<IEnumerable<PurchaseGroupDto>> Handle(PurchaseGroupQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new PurchaseGroupDto()
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

    public async Task<IEnumerable<ComboBoxDto>> Handle(PurchaseGroupQueryComboBox request, CancellationToken cancellationToken)
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
