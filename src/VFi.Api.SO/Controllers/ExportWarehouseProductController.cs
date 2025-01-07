using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExportWarehouseProductController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ExportWarehouseController> _logger;

    public ExportWarehouseProductController(IMediatorHandler mediator, IContextUser context, ILogger<ExportWarehouseController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] ExportWarehouseRequest request)
    {
        var query = new ExportWarehouseProductPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var ExportWarehouseProductId = new Guid(id);
        var result = await _mediator.SendCommand(new ExportWarehouseProductDeleteCommand(ExportWarehouseProductId));

        return Ok(result);
    }
}
