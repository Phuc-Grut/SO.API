using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExcelController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ExcelController> _logger;

    public ExcelController(IMediatorHandler mediator, IContextUser context, ILogger<ExcelController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpPost("get-info-sheet")]
    public async Task<IActionResult> GetInforSheet([FromForm] ExcelLoadInfoSheetRequest request)
    {
        var data = new ExcelQueryGetInfoSheet(request.File);
        var result = await _mediator.Send(data);
        return Ok(result);
    }

    [HttpPost("get-info-column")]
    public async Task<IActionResult> GetInforColumn([FromForm] ExcelLoadInfoColumnRequest request)
    {
        var data = new ExcelQueryGetInfoColumn(request.File, request.SheetId, request.HeaderRow);
        var result = await _mediator.Send(data);
        return Ok(result);
    }
}
