using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;


public class OrderTrackingQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public OrderTrackingQueryComboBox(OrderTrackingQueryParams queryParams)
    {
        QueryParams = queryParams;
    }
    public OrderTrackingQueryParams QueryParams { get; set; }
}
public class OrderTrackingQueryCheckCode : IQuery<bool>
{

    public OrderTrackingQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class OrderTrackingQueryById : IQuery<OrderTrackingDto>
{
    public OrderTrackingQueryById()
    {
    }

    public OrderTrackingQueryById(Guid orderTrackingId)
    {
        OrderTrackingId = orderTrackingId;
    }

    public Guid OrderTrackingId { get; set; }
}

public class OrderTrackingQueryHandler : IQueryHandler<OrderTrackingQueryComboBox, IEnumerable<ComboBoxDto>>,
                                     IQueryHandler<OrderTrackingQueryById, OrderTrackingDto>
{
private readonly IOrderTrackingRepository _respository;
public OrderTrackingQueryHandler(IOrderTrackingRepository respository)
{
    _respository = respository;
}

public async Task<OrderTrackingDto> Handle(OrderTrackingQueryById request, CancellationToken cancellationToken)
{
    var item = await _respository.GetById(request.OrderTrackingId);
    var result = new OrderTrackingDto()
    {
        Id = item.Id,
        OrderId = item.OrderId,
        Name = item.Name,
        Status = item.Status,
        Description = item.Description,
        Image = item.Image,
        TrackingDate = item.TrackingDate,
        CreatedBy = item.CreatedBy,
        CreatedDate = item.CreatedDate,
        UpdatedBy = item.UpdatedBy,
        UpdatedDate = item.UpdatedDate,
        CreatedByName = item.CreatedByName,
        UpdatedByName = item.UpdatedByName
    };
    return result;
}

public async Task<IEnumerable<ComboBoxDto>> Handle(OrderTrackingQueryComboBox request, CancellationToken cancellationToken)
{
    var filter = new Dictionary<string, object>();
    if (request.QueryParams.Status != null)
    {
        filter.Add("status", request.QueryParams.Status);
    }
    if (request.QueryParams.OrderId != null)
    {
        filter.Add("orderId", request.QueryParams.OrderId);
    }
    var data = await _respository.Filter(filter);
    var result = data.OrderByDescending(x => x.TrackingDate).Select(x => new ComboBoxDto()
    {
        Value = x.Id,
        Label = x.Name
    });
    return result;
}
}
