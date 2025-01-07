using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class ReasonQueryAll : IQuery<IEnumerable<ReasonDto>>
{
    public ReasonQueryAll()
    {
    }
}

public class ReasonQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public ReasonQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class ReasonQueryCheckCode : IQuery<bool>
{

    public ReasonQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class ReasonQueryById : IQuery<ReasonDto>
{
    public ReasonQueryById()
    {
    }

    public ReasonQueryById(Guid reasonId)
    {
        ReasonId = reasonId;
    }

    public Guid ReasonId { get; set; }
}
public class ReasonPagingQuery : FopQuery, IQuery<PagedResult<List<ReasonDto>>>
{
    public ReasonPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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

public class ReasonQueryHandler : IQueryHandler<ReasonQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<ReasonQueryAll, IEnumerable<ReasonDto>>,
                                         IQueryHandler<ReasonQueryCheckCode, bool>,
                                         IQueryHandler<ReasonQueryById, ReasonDto>,
                                         IQueryHandler<ReasonPagingQuery, PagedResult<List<ReasonDto>>>
{

    private readonly IReasonRepository _repository;
    public ReasonQueryHandler(IReasonRepository reasonRespository)
    {
        _repository = reasonRespository;
    }
    public async Task<bool> Handle(ReasonQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<ReasonDto> Handle(ReasonQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.ReasonId);
        var result = new ReasonDto()
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

    public async Task<PagedResult<List<ReasonDto>>> Handle(ReasonPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ReasonDto>>();

        var fopRequest = FopExpressionBuilder<Reason>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _repository.Filter(request.Keyword, request.Status, fopRequest);
        var data = datas.Select(item => new ReasonDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
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

    public async Task<IEnumerable<ReasonDto>> Handle(ReasonQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new ReasonDto()
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

    public async Task<IEnumerable<ComboBoxDto>> Handle(ReasonQueryComboBox request, CancellationToken cancellationToken)
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
