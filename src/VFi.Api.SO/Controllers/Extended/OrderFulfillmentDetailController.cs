using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers.Extended;

[Route("api/[controller]")]
[ApiController]
public class OrderFulfillmentDetailController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<OrderFulfillmentDetailController> _logger;

    public OrderFulfillmentDetailController(IMediatorHandler mediator, ILogger<OrderFulfillmentDetailController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new OrderFulfillmentDetailQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] OrderFulfillmentDetailRequest request)
    {
        var query = new OrderFulfillmentDetailPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddOrderFulfillmentDetailRequest request)
    {
        var cmd = new OrderFulfillmentDetailAddCommand()
        {
            Id = Guid.NewGuid()
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditOrderFulfillmentDetailRequest request)
    {
        var item = await _mediator.Send(new OrderFulfillmentDetailQueryById(request.Id));

        if (item == null)
            return BadRequest(new ValidationResult("OrderFulfillmentDetail not exists"));

        var cmd = new OrderFulfillmentDetailEditCommand()
        {
            Id = request.Id
        };
        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (await _mediator.Send(new OrderFulfillmentDetailQueryById(id)) == null)
            return BadRequest(new ValidationResult("OrderFulfillmentDetail not exists"));

        var result = await _mediator.SendCommand(new OrderFulfillmentDetailDeleteCommand(id));

        return Ok(result);
    }
}
