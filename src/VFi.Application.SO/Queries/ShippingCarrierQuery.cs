using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class ShippingCarrierQueryAll : IQuery<IEnumerable<ShippingCarrierDto>>
{
    public ShippingCarrierQueryAll()
    {
    }
}

public class ShippingCarrierQueryComboBox : IQuery<IEnumerable<ShippingCarrierComboBoxDto>>
{
    public ShippingCarrierQueryComboBox(int? status)
    {
        Status = status;
    }

    public int? Status { get; set; }
    public string? Country { get; set; }
}
public class ShippingCarrierQueryCheckCode : IQuery<bool>
{

    public ShippingCarrierQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class ShippingCarrierQueryById : IQuery<ShippingCarrierDto>
{
    public ShippingCarrierQueryById()
    {
    }

    public ShippingCarrierQueryById(Guid shippingMethodId)
    {
        ShippingCarrierId = shippingMethodId;
    }

    public Guid ShippingCarrierId { get; set; }
}
public class ShippingCarrierPagingFilterQuery : FopQuery, IQuery<PagedResult<List<ShippingCarrierDto>>>
{
    public ShippingCarrierPagingFilterQuery(int? status, string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
    }
    public string Keyword { get; set; }
    public string? Country { get; set; }
    public string Filter { get; set; }
    public string Order { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int? Status { get; set; }
}


public class ShippingCarrierQueryHandler : IQueryHandler<ShippingCarrierQueryComboBox, IEnumerable<ShippingCarrierComboBoxDto>>,
                                         IQueryHandler<ShippingCarrierQueryAll, IEnumerable<ShippingCarrierDto>>,
                                         IQueryHandler<ShippingCarrierQueryCheckCode, bool>,
                                         IQueryHandler<ShippingCarrierQueryById, ShippingCarrierDto>,
                                         IQueryHandler<ShippingCarrierPagingFilterQuery, PagedResult<List<ShippingCarrierDto>>>
{
    private readonly IShippingCarrierRepository _ShippingCarrierRepository;
    public ShippingCarrierQueryHandler(IShippingCarrierRepository ShippingCarrierRespository)
    {
        _ShippingCarrierRepository = ShippingCarrierRespository;
    }
    public async Task<bool> Handle(ShippingCarrierQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _ShippingCarrierRepository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<ShippingCarrierDto> Handle(ShippingCarrierQueryById request, CancellationToken cancellationToken)
    {
        var x = await _ShippingCarrierRepository.GetById(request.ShippingCarrierId);
        var result = new ShippingCarrierDto()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            Country = x.Country,
            Description = x.Description,
            Status = x.Status,
            CreatedDate = x.CreatedDate,
            CreatedBy = x.CreatedBy,
            CreatedByName = x.CreatedByName,
            UpdatedDate = x.UpdatedDate,
            UpdatedByName = x.UpdatedByName,
            UpdatedBy = x.UpdatedBy
        };
        return result;
    }

    public async Task<PagedResult<List<ShippingCarrierDto>>> Handle(ShippingCarrierPagingFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ShippingCarrierDto>>();
        var fopRequest = FopExpressionBuilder<ShippingCarrier>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
        {
            filter.Add("status", request.Status.ToString());
        }
        if (request.Country != null)
        {
            filter.Add("country", request.Country.ToString());
        }
        var (datas, count) = await _ShippingCarrierRepository.Filter(request.Keyword, filter, fopRequest);
        var result = datas.Select(x =>
        {
            return new ShippingCarrierDto()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Country = x.Country,
                Description = x.Description,
                Status = x.Status,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                CreatedByName = x.CreatedByName,
                UpdatedDate = x.UpdatedDate,
                UpdatedByName = x.UpdatedByName,
                UpdatedBy = x.UpdatedBy
            };
        }
        ).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<ShippingCarrierDto>> Handle(ShippingCarrierQueryAll request, CancellationToken cancellationToken)
    {
        var ShippingCarriers = await _ShippingCarrierRepository.GetAll();
        var result = ShippingCarriers.Select(x => new ShippingCarrierDto()
        {
            Id = x.Id,
            Code = x.Code,
            Country = x.Country,
            Name = x.Name,
            Description = x.Description,
            Status = x.Status,
            CreatedDate = x.CreatedDate,
            CreatedBy = x.CreatedBy,
            CreatedByName = x.CreatedByName,
            UpdatedDate = x.UpdatedDate,
            UpdatedByName = x.UpdatedByName,
            UpdatedBy = x.UpdatedBy
        });
        return result;
    }

    public async Task<IEnumerable<ShippingCarrierComboBoxDto>> Handle(ShippingCarrierQueryComboBox request, CancellationToken cancellationToken)
    {

        var shippingCarriers = await _ShippingCarrierRepository.GetListCbx(request.Status, request.Country);
        var result = shippingCarriers.Select(x => new ShippingCarrierComboBoxDto()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            Description = x.Description
        });
        return result;
    }
}
