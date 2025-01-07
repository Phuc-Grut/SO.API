using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class CustomerGroupQueryAll : IQuery<IEnumerable<CustomerGroupDto>>
{
    public CustomerGroupQueryAll()
    {
    }
}

public class CustomerGroupQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public CustomerGroupQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class CustomerGroupQueryCheckCode : IQuery<bool>
{

    public CustomerGroupQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class CustomerGroupQueryById : IQuery<CustomerGroupDto>
{
    public CustomerGroupQueryById()
    {
    }

    public CustomerGroupQueryById(Guid customerGroupId)
    {
        CustomerGroupId = customerGroupId;
    }

    public Guid CustomerGroupId { get; set; }
}
public class CustomerGroupPagingQuery : FopQuery, IQuery<PagedResult<List<CustomerGroupDto>>>
{
    public CustomerGroupPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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

public class CustomerGroupQueryHandler : IQueryHandler<CustomerGroupQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<CustomerGroupQueryAll, IEnumerable<CustomerGroupDto>>,
                                         IQueryHandler<CustomerGroupQueryCheckCode, bool>,
                                         IQueryHandler<CustomerGroupQueryById, CustomerGroupDto>,
                                         IQueryHandler<CustomerGroupPagingQuery, PagedResult<List<CustomerGroupDto>>>
{
    private readonly ICustomerGroupRepository _repository;
    public CustomerGroupQueryHandler(ICustomerGroupRepository customerGroupRespository)
    {
        _repository = customerGroupRespository;
    }
    public async Task<bool> Handle(CustomerGroupQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<CustomerGroupDto> Handle(CustomerGroupQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.CustomerGroupId);
        var result = new CustomerGroupDto()
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
        };
        return result;
    }

    public async Task<PagedResult<List<CustomerGroupDto>>> Handle(CustomerGroupPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<CustomerGroupDto>>();

        var fopRequest = FopExpressionBuilder<CustomerGroup>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _repository.Filter(request.Keyword, request.Status, fopRequest);

        var data = datas.Select(item => new CustomerGroupDto()
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

    public async Task<IEnumerable<CustomerGroupDto>> Handle(CustomerGroupQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new CustomerGroupDto()
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
            CreatedByName = item.CreatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(CustomerGroupQueryComboBox request, CancellationToken cancellationToken)
    {

        var CustomerGroups = await _repository.GetListCbx(request.Status);
        var result = CustomerGroups.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
