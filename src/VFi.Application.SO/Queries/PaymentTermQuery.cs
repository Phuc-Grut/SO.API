using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;


public class PaymentTermQueryAll : IQuery<IEnumerable<PaymentTermDto>>
{
    public PaymentTermQueryAll()
    {
    }
}

public class PaymentTermQueryById : IQuery<PaymentTermDto>
{
    public PaymentTermQueryById()
    {
    }

    public PaymentTermQueryById(Guid id)
    {
        PaymentTermId = id;
    }

    public Guid PaymentTermId { get; set; }
}

public class PaymentTermPagingQuery : FopQuery, IQuery<PagedResult<List<PaymentTermDto>>>
{
    public PaymentTermPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Status = status;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public string? Keyword { get; set; }
    public int? Status { get; set; }
    public string Filter { get; set; }
    public string Order { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

}

public class PaymentTermQuery : IQueryHandler<PaymentTermQueryAll, IEnumerable<PaymentTermDto>>,
                                        IQueryHandler<PaymentTermQueryById, PaymentTermDto>,
                                        IQueryHandler<PaymentTermPagingQuery, PagedResult<List<PaymentTermDto>>>
{
    private readonly IPaymentTermRepository _repository;
    public PaymentTermQuery(IPaymentTermRepository respository)
    {
        _repository = respository;
    }

    public async Task<PaymentTermDto> Handle(PaymentTermQueryById request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetById(request.PaymentTermId);
        var result = new PaymentTermDto()
        {
            Id = data.Id,
            Code = data.Code,
            Name = data.Name,
            Description = data.Description,
            Day = data.Day,
            Percent = data.Percent,
            Type = data.Percent is not null ? 0 : data.Value is not null ? 1 : data.Percent is null && data.Value is null ? 2 : null,
            Value = data.Value,
            Status = data.Status,
            CreatedDate = data.CreatedDate,
            UpdatedDate = data.UpdatedDate,
            CreatedBy = data.CreatedBy,
            UpdatedBy = data.UpdatedBy,
            CreatedByName = data.CreatedByName,
            UpdatedByName = data.UpdatedByName,
        };
        return result;
    }

    public async Task<PagedResult<List<PaymentTermDto>>> Handle(PaymentTermPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<PaymentTermDto>>();

        var fopRequest = FopExpressionBuilder<PaymentTerm>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _repository.Filter(request.Keyword, request.Status, fopRequest);
        var data = datas.Select(x => new PaymentTermDto()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            Description = x.Description,
            Day = x.Day,
            Percent = x.Percent,
            Value = x.Value,
            Status = x.Status,
            CreatedDate = x.CreatedDate,
            UpdatedDate = x.UpdatedDate,
            CreatedBy = x.CreatedBy,
            UpdatedBy = x.UpdatedBy,
            CreatedByName = x.CreatedByName,
            UpdatedByName = x.UpdatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<PaymentTermDto>> Handle(PaymentTermQueryAll request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetAll();
        var result = data.Select(x => new PaymentTermDto()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            Description = x.Description,
            Day = x.Day,
            Percent = x.Percent,
            Value = x.Value,
            Status = x.Status,
            CreatedDate = x.CreatedDate,
            UpdatedDate = x.UpdatedDate,
            CreatedBy = x.CreatedBy,
            UpdatedBy = x.UpdatedBy,
            CreatedByName = x.CreatedByName,
            UpdatedByName = x.UpdatedByName
        });
        return result;
    }
}
