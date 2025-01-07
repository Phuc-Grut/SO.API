using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class ShippingMethodQueryAll : IQuery<IEnumerable<ShippingMethodDto>>
{
    public ShippingMethodQueryAll()
    {
    }
}

public class ShippingMethodQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public ShippingMethodQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class ShippingMethodQueryCheckCode : IQuery<bool>
{

    public ShippingMethodQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class ShippingMethodQueryById : IQuery<ShippingMethodDto>
{
    public ShippingMethodQueryById()
    {
    }

    public ShippingMethodQueryById(Guid shippingMethodId)
    {
        ShippingMethodId = shippingMethodId;
    }

    public Guid ShippingMethodId { get; set; }
}
public class ShippingMethodPagingFilterQuery : FopQuery, IQuery<PagedResult<List<ShippingMethodDto>>>
{
    public ShippingMethodPagingFilterQuery(int? status, string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
    }
    public string Keyword { get; set; }
    public string Filter { get; set; }
    public string Order { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int? Status { get; set; }
}


public class ShippingMethodQueryHandler : IQueryHandler<ShippingMethodQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<ShippingMethodQueryAll, IEnumerable<ShippingMethodDto>>,
                                         IQueryHandler<ShippingMethodQueryCheckCode, bool>,
                                         IQueryHandler<ShippingMethodQueryById, ShippingMethodDto>,
                                         IQueryHandler<ShippingMethodPagingFilterQuery, PagedResult<List<ShippingMethodDto>>>
{
    private readonly IShippingMethodRepository _ShippingMethodRepository;
    public ShippingMethodQueryHandler(IShippingMethodRepository ShippingMethodRespository)
    {
        _ShippingMethodRepository = ShippingMethodRespository;
    }
    public async Task<bool> Handle(ShippingMethodQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _ShippingMethodRepository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<ShippingMethodDto> Handle(ShippingMethodQueryById request, CancellationToken cancellationToken)
    {
        var ShippingMethod = await _ShippingMethodRepository.GetById(request.ShippingMethodId);
        var result = new ShippingMethodDto()
        {
            Id = ShippingMethod.Id,
            Code = ShippingMethod.Code,
            Name = ShippingMethod.Name,
            Description = ShippingMethod.Description,
            Status = ShippingMethod.Status,
            CreatedDate = ShippingMethod.CreatedDate,
            CreatedBy = ShippingMethod.CreatedBy,
            CreatedByName = ShippingMethod.CreatedByName,
            UpdatedDate = ShippingMethod.UpdatedDate,
            UpdatedByName = ShippingMethod.UpdatedByName,
            UpdatedBy = ShippingMethod.UpdatedBy
        };
        return result;
    }

    public async Task<PagedResult<List<ShippingMethodDto>>> Handle(ShippingMethodPagingFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ShippingMethodDto>>();
        var fopRequest = FopExpressionBuilder<ShippingMethod>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
        {
            filter.Add("status", request.Status.ToString());
        }
        var (datas, count) = await _ShippingMethodRepository.Filter(request.Keyword, filter, fopRequest);
        var result = datas.Select(ShippingMethod =>
        {
            return new ShippingMethodDto()
            {
                Id = ShippingMethod.Id,
                Code = ShippingMethod.Code,
                Name = ShippingMethod.Name,
                Description = ShippingMethod.Description,
                Status = ShippingMethod.Status,
                CreatedDate = ShippingMethod.CreatedDate,
                CreatedBy = ShippingMethod.CreatedBy,
                CreatedByName = ShippingMethod.CreatedByName,
                UpdatedDate = ShippingMethod.UpdatedDate,
                UpdatedByName = ShippingMethod.UpdatedByName,
                UpdatedBy = ShippingMethod.UpdatedBy
            };
        }
        ).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<ShippingMethodDto>> Handle(ShippingMethodQueryAll request, CancellationToken cancellationToken)
    {
        var ShippingMethods = await _ShippingMethodRepository.GetAll();
        var result = ShippingMethods.Select(ShippingMethod => new ShippingMethodDto()
        {
            Id = ShippingMethod.Id,
            Code = ShippingMethod.Code,
            Name = ShippingMethod.Name,
            Description = ShippingMethod.Description,
            Status = ShippingMethod.Status,
            CreatedDate = ShippingMethod.CreatedDate,
            CreatedBy = ShippingMethod.CreatedBy,
            CreatedByName = ShippingMethod.CreatedByName,
            UpdatedDate = ShippingMethod.UpdatedDate,
            UpdatedByName = ShippingMethod.UpdatedByName,
            UpdatedBy = ShippingMethod.UpdatedBy
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(ShippingMethodQueryComboBox request, CancellationToken cancellationToken)
    {

        var ShippingMethods = await _ShippingMethodRepository.GetListCbx(request.Status);
        var result = ShippingMethods.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
