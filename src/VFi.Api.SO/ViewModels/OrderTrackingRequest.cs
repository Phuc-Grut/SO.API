using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddOrderTrackingRequest
{
    public Guid? Id { get; set; }
    public Guid? OrderId { get; set; }
    public string? Name { get; set; }
    public int? Status { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public DateTime? TrackingDate { get; set; }
}
public class EditOrderTrackingRequest : AddOrderTrackingRequest
{
}

public class DeleteOrderTrackingRequest
{
    public Guid? Id { get; set; }
}
public class OrderTrackingListBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$orderId")]
    public Guid? OrderId { get; set; }
    public OrderTrackingQueryParams ToBaseQuery() => new OrderTrackingQueryParams
    {
        Status = Status,
        OrderId = OrderId
    };
}
