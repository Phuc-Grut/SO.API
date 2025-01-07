using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;


namespace VFi.Application.SO.Queries;

public class DeliveryProductPagingQuery : FopQuery, IQuery<PagedResult<List<DeliveryProductDto>>>
{
    public DeliveryProductPagingQuery(string? filter, string? order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class DeliveryProductGetByOrder : IQuery<List<DeliveryProductDto>>
{
    public DeliveryProductGetByOrder(string orderId)
    {
        OrderId = orderId;
    }
    public string OrderId { get; set; }
}
public class DeliveryProductQueryHandler : IQueryHandler<DeliveryProductPagingQuery, PagedResult<List<DeliveryProductDto>>>,
    IQueryHandler<DeliveryProductGetByOrder, List<DeliveryProductDto>>
{
    private readonly IDeliveryProductRepository _DeliveryProductRepository;
    public DeliveryProductQueryHandler(IDeliveryProductRepository DeliveryProductRepository)
    {
        _DeliveryProductRepository = DeliveryProductRepository;
    }

    public async Task<List<DeliveryProductDto>> Handle(DeliveryProductGetByOrder request, CancellationToken cancellationToken)
    {
        var datas = await _DeliveryProductRepository.GetListDetaiId(request.OrderId);
        var data = datas?.Select(item => new DeliveryProductDto()
        {
            Id = item.Id,
            Description = item.Description,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            QuantityExpected = item.QuantityExpected,
            OrderProductId = item.OrderProductId,
            UpdatedByName = item.UpdatedByName,
            CreatedByName = item.CreatedByName,
            DeliveryDate = item.DeliveryDate

        }).ToList();
        return data;
    }
    public async Task<PagedResult<List<DeliveryProductDto>>> Handle(DeliveryProductPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<DeliveryProductDto>>();

        var fopRequest = FopExpressionBuilder<DeliveryProduct>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _DeliveryProductRepository.Filter(fopRequest);

        var data = datas?.Select(item => new DeliveryProductDto()
        {
            Id = item.Id,
            Description = item.Description,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            QuantityExpected = item.QuantityExpected,
            OrderProductId = item.OrderProductId,
            UpdatedByName = item.UpdatedByName,
            CreatedByName = item.CreatedByName,
            DeliveryDate = item.DeliveryDate

        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
}
