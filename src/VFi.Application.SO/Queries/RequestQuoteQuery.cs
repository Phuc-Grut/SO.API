using MassTransit.Transports;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class RequestQuoteQueryComboBox : IQuery<IEnumerable<RequestQuoteListBoxDto>>
{
    public RequestQuoteQueryComboBox(RequestQuoteQueryParams queryParams)
    {
        QueryParams = queryParams;
    }
    public RequestQuoteQueryParams QueryParams { get; set; }
}
public class RequestQuoteQueryCheckCode : IQuery<bool>
{

    public RequestQuoteQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class RequestQuoteQueryById : IQuery<RequestQuoteDto>
{
    public RequestQuoteQueryById()
    {
    }

    public RequestQuoteQueryById(Guid requestQuoteId)
    {
        RequestQuoteId = requestQuoteId;
    }

    public Guid RequestQuoteId { get; set; }
}
public class RequestQuotePagingQuery : FopQuery, IQuery<PagedResult<List<RequestQuoteDto>>>
{
    public RequestQuotePagingQuery(string? keyword, int? status, string? employeeId, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        EmployeeId = employeeId;
    }
    public string? EmployeeId { get; set; }
}

public class RequestQuoteQueryHandler : IQueryHandler<RequestQuoteQueryComboBox, IEnumerable<RequestQuoteListBoxDto>>,
                                         IQueryHandler<RequestQuoteQueryCheckCode, bool>,
                                         IQueryHandler<RequestQuoteQueryById, RequestQuoteDto>,
                                         IQueryHandler<RequestQuotePagingQuery, PagedResult<List<RequestQuoteDto>>>
{
    private readonly IRequestQuoteRepository _repository;
    public RequestQuoteQueryHandler(IRequestQuoteRepository requestQuoteRespository)
    {
        _repository = requestQuoteRespository;
    }
    public async Task<bool> Handle(RequestQuoteQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<RequestQuoteDto> Handle(RequestQuoteQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.RequestQuoteId);

        var result = new RequestQuoteDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            RequestDate = item.RequestDate,
            DueDate = item.DueDate,
            StoreId = item.StoreId,
            StoreCode = item.StoreCode,
            StoreName = item.StoreName,
            CustomerId = item.CustomerId,
            CustomerCode = item.CustomerCode,
            CustomerName = item.CustomerName,
            Phone = item.Phone,
            Email = item.Email,
            Address = item.Address,
            EmployeeId = item.EmployeeId,
            EmployeeName = item.EmployeeName,
            Status = item.Status,
            Note = item.Note,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            ChannelId = item.ChannelId,
            ChannelCode = item.ChannelCode,
            ChannelName = item.ChannelName,
            QuotationCode = item.Quotation.FirstOrDefault()?.Code,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<RequestQuoteDto>>> Handle(RequestQuotePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<RequestQuoteDto>>();
        var fopRequest = FopExpressionBuilder<RequestQuote>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        var filterListBox = new Dictionary<string, object>();
        if (request.Status != null)
            filterListBox.Add("status", request.Status.ToString());
        if (!string.IsNullOrEmpty(request.EmployeeId))
            filterListBox.Add("employeeId", request.EmployeeId);
        var (datas, count) = await _repository.Filter(request.Keyword, filterListBox, fopRequest);

        var data = datas.Select(item => new RequestQuoteDto()
        {
            Id = item.Id,
            Code = item.Code,
            RequestDate = item.RequestDate,
            DueDate = item.DueDate,
            CustomerId = item.CustomerId,
            CustomerCode = item.CustomerCode,
            CustomerName = item.CustomerName,
            Phone = item.Phone,
            Email = item.Email,
            EmployeeId = item.EmployeeId,
            EmployeeName = item.EmployeeName,
            Status = item.Status,
            StoreName = item.StoreName,
            ChannelName = item.ChannelName,
            Note = item.Note,
            CreatedByName = item.CreatedByName,
            CreatedDate = item.CreatedDate
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<RequestQuoteListBoxDto>> Handle(RequestQuoteQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        if (request.QueryParams.Status != null)
        {
            filter.Add("status", request.QueryParams.Status);
        }
        if (request.QueryParams.CustomerId != null)
        {
            filter.Add("customerId", request.QueryParams.CustomerId);
        }
        var RequestQuotes = await _repository.GetListCbx(filter);
        var result = RequestQuotes.Select(x => new RequestQuoteListBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            CustomerId = x.CustomerId,
            CustomerCode = x.CustomerCode,
            CustomerName = x.CustomerName,
            Phone = x.Phone,
            Email = x.Email,
            Address = x.Address,
            StoreId = x.StoreId,
            StoreCode = x.StoreCode,
            StoreName = x.StoreName,
            ChannelId = x.ChannelId,
            ChannelCode = x.ChannelCode,
            ChannelName = x.ChannelName
        });
        return result;
    }
}
