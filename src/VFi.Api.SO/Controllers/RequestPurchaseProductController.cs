using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequestPurchaseProductController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ExportWarehouseController> _logger;

    public RequestPurchaseProductController(IMediatorHandler mediator, IContextUser context, ILogger<ExportWarehouseController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
    {
        var query = new RequestPurchaseProductPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
