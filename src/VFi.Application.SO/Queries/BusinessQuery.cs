using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class BusinessQueryAll : IQuery<IEnumerable<BusinessDto>>
{
    public BusinessQueryAll()
    {
    }
}

public class BusinessQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public BusinessQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class BusinessQueryCheckCode : IQuery<bool>
{

    public BusinessQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class BusinessQueryById : IQuery<BusinessDto>
{
    public BusinessQueryById()
    {
    }

    public BusinessQueryById(Guid businessId)
    {
        BusinessId = businessId;
    }

    public Guid BusinessId { get; set; }
}
public class BusinessPagingQuery : FopQuery, IQuery<PagedResult<List<BusinessDto>>>
{
    public BusinessPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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

public class BusinessQueryHandler : IQueryHandler<BusinessQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<BusinessQueryAll, IEnumerable<BusinessDto>>,
                                         IQueryHandler<BusinessQueryCheckCode, bool>,
                                         IQueryHandler<BusinessQueryById, BusinessDto>,
                                         IQueryHandler<BusinessPagingQuery, PagedResult<List<BusinessDto>>>
{
    private readonly IBusinessRepository _repository;
    public BusinessQueryHandler(IBusinessRepository businessRespository)
    {
        _repository = businessRespository;
    }
    public async Task<bool> Handle(BusinessQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<BusinessDto> Handle(BusinessQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.BusinessId);
        var result = new BusinessDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            DisplayOrder = item.DisplayOrder,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<BusinessDto>>> Handle(BusinessPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<BusinessDto>>();

        var fopRequest = FopExpressionBuilder<Business>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _repository.Filter(request.Keyword, request.Status, fopRequest);
        var data = datas.Select(item => new BusinessDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            DisplayOrder = item.DisplayOrder,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<BusinessDto>> Handle(BusinessQueryAll request, CancellationToken cancellationToken)
    {
        var Businesss = await _repository.GetAll();
        var result = Businesss.Select(Business => new BusinessDto()
        {
            Id = Business.Id,
            Code = Business.Code,
            Name = Business.Name,
            Description = Business.Description,
            DisplayOrder = Business.DisplayOrder,
            Status = Business.Status,
            CreatedDate = Business.CreatedDate,
            CreatedBy = Business.CreatedBy,
            UpdatedDate = Business.UpdatedDate,
            UpdatedBy = Business.UpdatedBy,
            CreatedByName = Business.CreatedByName,
            UpdatedByName = Business.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(BusinessQueryComboBox request, CancellationToken cancellationToken)
    {

        var Businesss = await _repository.GetListCbx(request.Status);
        var result = Businesss.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
