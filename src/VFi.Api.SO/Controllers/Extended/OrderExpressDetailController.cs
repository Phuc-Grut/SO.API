using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers.Extended;

[Route("api/[controller]")]
[ApiController]
public class OrderExpressDetailController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<OrderExpressDetailController> _logger;

    public OrderExpressDetailController(IMediatorHandler mediator, ILogger<OrderExpressDetailController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new OrderExpressDetailQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] OrderExpressDetailPagingRequest request)
    {
        var query = new OrderExpressDetailPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddOrderExpressDetailRequest request)
    {
        var cmd = new OrderExpressDetailAddCommand()
        {
            Id = Guid.NewGuid()
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditOrderExpressDetailRequest request)
    {
        var item = await _mediator.Send(new OrderExpressDetailQueryById(request.Id));

        if (item == null)
            return BadRequest(new ValidationResult("OrderExpressDetail not exists"));

        var cmd = new OrderExpressDetailEditCommand()
        {
            Id = request.Id
        };
        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (await _mediator.Send(new OrderExpressDetailQueryById(id)) == null)
            return BadRequest(new ValidationResult("OrderExpressDetail not exists"));

        var result = await _mediator.SendCommand(new OrderExpressDetailDeleteCommand(id));

        return Ok(result);
    }
}
