using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Events.OrderCross;
using VFi.Domain.SO.Events;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<OrderController> _logger;
    private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

    public EventController(IMediatorHandler mediator, IContextUser context,
    ILogger<OrderController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
        _environment = environment;
    }

    [HttpGet("purchase-notify")]
    public async Task<IActionResult> PurchaseNotify(CancellationToken cancellationToken)
    {
        var ev = new PurchaseNotifyEvent();

        ev.Date = DateTime.Now.ToString("d");
        _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
        return Ok();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("fetch-order-tracking")]
    public async Task<IActionResult> OrderFetchTracking(OrderFetchOrderTrackingEventRequest request, CancellationToken cancellationToken)
    {
        var ev = new OrderFetchTrackingEvent()
        {
            MaxDays = request.MaxDays,
            MaxItems = request.MaxItems,
            MinDays = request.MinDays,
            OrderType = request.OrderType,
            AuthorizationToken = request.AuthorizationToken,
        };
        if (_context.IsAutenticated())
        {
            ev.Data_Zone = _context.Data_Zone;
            ev.Data = _context.Data;
            ev.Tenant = _context.Tenant;
            ev.CreatedBy = _context.GetUserId();
            ev.CreatedName = _context.UserName;
        }

        await _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
        return Ok();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("fetch-order-domestic-delivery")]
    public async Task<IActionResult> OrderFetchDomesticDelivery(OrderFetchDomesticDeliveryEventRequest request, CancellationToken cancellationToken)
    {
        var ev = new OrderFetchDomesticDeliveryEvent()
        {
            MaxDays = request.MaxDays,
            MaxItems = request.MaxItems,
            MinDays = request.MinDays,
        };
        if (_context.IsAutenticated())
        {
            ev.Data_Zone = _context.Data_Zone;
            ev.Data = _context.Data;
            ev.Tenant = _context.Tenant;
            ev.CreatedBy = _context.GetUserId();
            ev.CreatedName = _context.UserName;
        }

        await _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
        return Ok();
    }
}
