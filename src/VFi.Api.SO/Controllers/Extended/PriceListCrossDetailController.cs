using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers.Extended;

[Route("api/[controller]")]
[ApiController]
public class PriceListCrossDetailController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<PriceListCrossDetailController> _logger;

    public PriceListCrossDetailController(IMediatorHandler mediator, ILogger<PriceListCrossDetailController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new PriceListCrossDetailQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PriceListCrossDetailRequest request)
    {
        var query = new PriceListCrossDetailPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (await _mediator.Send(new PriceListCrossDetailQueryById(id)) == null)
            return BadRequest(new ValidationResult("PriceListCrossDetail not exists"));

        var result = await _mediator.SendCommand(new PriceListCrossDetailDeleteCommand(id));

        return Ok(result);
    }
}
