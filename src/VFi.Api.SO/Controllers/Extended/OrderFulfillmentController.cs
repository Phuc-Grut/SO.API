using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers.Extended;

[Route("api/[controller]")]
[ApiController]
public class OrderFulfillmentController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<OrderFulfillmentController> _logger;

    public OrderFulfillmentController(IMediatorHandler mediator, ILogger<OrderFulfillmentController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new OrderFulfillmentQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] OrderFulfillmentRequest request)
    {
        var query = new OrderFulfillmentPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddOrderFulfillmentRequest request)
    {
        var cmd = new OrderFulfillmentAddCommand()
        {
            Id = Guid.NewGuid(),
            Code = request.Code
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditOrderFulfillmentRequest request)
    {
        var item = await _mediator.Send(new OrderFulfillmentQueryById(request.Id));

        if (item == null)
            return BadRequest(new ValidationResult("OrderFulfillment not exists"));

        var cmd = new OrderFulfillmentEditCommand()
        {
            Id = request.Id,
            Code = request.Code
        };
        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (await _mediator.Send(new OrderFulfillmentQueryById(id)) == null)
            return BadRequest(new ValidationResult("OrderFulfillment not exists"));

        var result = await _mediator.SendCommand(new OrderFulfillmentDeleteCommand(id));

        return Ok(result);
    }
}
