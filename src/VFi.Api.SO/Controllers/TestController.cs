using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Events;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<TestController> _logger;

    public TestController(IMediatorHandler mediator, IContextUser context, ILogger<TestController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("purchase-notify-all")]
    public async Task<IActionResult> PurchaseNotifyAll(CancellationToken cancellationToken)
    {
        var ev = new PurchaseNotifyAllEvent();
        ev.Data = _context.Data;
        ev.Data_Zone = _context.Data_Zone;
        ev.Tenant = _context.Tenant;
        _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
        return Ok();
    }
}
