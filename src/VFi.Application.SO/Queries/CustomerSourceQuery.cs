using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class CustomerSourceQueryAll : IQuery<IEnumerable<CustomerSourceDto>>
{
    public CustomerSourceQueryAll()
    {
    }
}

public class CustomerSourceQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public CustomerSourceQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class CustomerSourceQueryCheckCode : IQuery<bool>
{

    public CustomerSourceQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class CustomerSourceQueryById : IQuery<CustomerSourceDto>
{
    public CustomerSourceQueryById()
    {
    }

    public CustomerSourceQueryById(Guid customerSourceId)
    {
        CustomerSourceId = customerSourceId;
    }

    public Guid CustomerSourceId { get; set; }
}
public class CustomerSourcePagingQuery : FopQuery, IQuery<PagedResult<List<CustomerSourceDto>>>
{
    public CustomerSourcePagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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

public class CustomerSourceQueryHandler : IQueryHandler<CustomerSourceQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<CustomerSourceQueryAll, IEnumerable<CustomerSourceDto>>,
                                         IQueryHandler<CustomerSourceQueryCheckCode, bool>,
                                         IQueryHandler<CustomerSourceQueryById, CustomerSourceDto>,
                                         IQueryHandler<CustomerSourcePagingQuery, PagedResult<List<CustomerSourceDto>>>
{
    private readonly ICustomerSourceRepository _repository;
    public CustomerSourceQueryHandler(ICustomerSourceRepository customerSourceRespository)
    {
        _repository = customerSourceRespository;
    }
    public async Task<bool> Handle(CustomerSourceQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<CustomerSourceDto> Handle(CustomerSourceQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.CustomerSourceId);
        var result = new CustomerSourceDto()
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
            UpdatedByName = item.UpdatedByName,
            CreatedByName = item.UpdatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<CustomerSourceDto>>> Handle(CustomerSourcePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<CustomerSourceDto>>();

        var fopRequest = FopExpressionBuilder<CustomerSource>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _repository.Filter(request.Keyword, request.Status, fopRequest);
        var data = datas.Select(item => new CustomerSourceDto()
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
            UpdatedByName = item.UpdatedByName,
            CreatedByName = item.CreatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<CustomerSourceDto>> Handle(CustomerSourceQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new CustomerSourceDto()
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
            UpdatedByName = item.UpdatedByName,
            CreatedByName = item.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(CustomerSourceQueryComboBox request, CancellationToken cancellationToken)
    {

        var CustomerSources = await _repository.GetListCbx(request.Status);
        var result = CustomerSources.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
