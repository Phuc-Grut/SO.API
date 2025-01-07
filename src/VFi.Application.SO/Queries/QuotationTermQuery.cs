using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class QuotationTermQueryAll : IQuery<IEnumerable<QuotationTermDto>>
{
    public QuotationTermQueryAll()
    {
    }
}

public class QuotationTermQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public QuotationTermQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class QuotationTermQueryCheckCode : IQuery<bool>
{

    public QuotationTermQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class QuotationTermQueryById : IQuery<QuotationTermDto>
{
    public QuotationTermQueryById()
    {
    }

    public QuotationTermQueryById(Guid quotationTermId)
    {
        QuotationTermId = quotationTermId;
    }

    public Guid QuotationTermId { get; set; }
}
public class QuotationTermPagingQuery : FopQuery, IQuery<PagedResult<List<QuotationTermDto>>>
{
    public QuotationTermPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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

public class QuotationTermQueryHandler : IQueryHandler<QuotationTermQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<QuotationTermQueryAll, IEnumerable<QuotationTermDto>>,
                                         IQueryHandler<QuotationTermQueryCheckCode, bool>,
                                         IQueryHandler<QuotationTermQueryById, QuotationTermDto>,
                                         IQueryHandler<QuotationTermPagingQuery, PagedResult<List<QuotationTermDto>>>
{
    private readonly IQuotationTermRepository _repository;
    public QuotationTermQueryHandler(IQuotationTermRepository quotationTermRespository)
    {
        _repository = quotationTermRespository;
    }
    public async Task<bool> Handle(QuotationTermQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<QuotationTermDto> Handle(QuotationTermQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.QuotationTermId);
        var result = new QuotationTermDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            DisplayOrder = item.DisplayOrder,
            UpdatedByName = item.UpdatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<QuotationTermDto>>> Handle(QuotationTermPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<QuotationTermDto>>();

        var fopRequest = FopExpressionBuilder<QuotationTerm>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _repository.Filter(request.Keyword, request.Status, fopRequest);
        var data = datas.Select(item => new QuotationTermDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            DisplayOrder = item.DisplayOrder,
            UpdatedByName = item.UpdatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<QuotationTermDto>> Handle(QuotationTermQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new QuotationTermDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            UpdatedDate = item.UpdatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            DisplayOrder = item.DisplayOrder,
            UpdatedByName = item.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(QuotationTermQueryComboBox request, CancellationToken cancellationToken)
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
