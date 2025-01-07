using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveryMethodController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<DeliveryMethodController> _logger;

    public DeliveryMethodController(IMediatorHandler mediator, IContextUser context, ILogger<DeliveryMethodController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new DeliveryMethodQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new DeliveryMethodQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] DeliveryMethodRequest request)
    {
        var query = new DeliveryMethodPagingQuery(request.Status, request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddDeliveryMethodRequest request)
    {
        var DeliveryMethodAddCommand = new DeliveryMethodAddCommand(
          Guid.NewGuid(),
          request.Code,
           request.Name,
           request.Description,
           request.Status
      );
        var result = await _mediator.SendCommand(DeliveryMethodAddCommand);
        return Ok(result);
    }

    [HttpPut("edit/{id}")]
    public async Task<IActionResult> Put(Guid Id, [FromBody] EditDeliveryMethodRequest request)
    {
        DeliveryMethodDto dataDeliveryMethod = await _mediator.Send(new DeliveryMethodQueryById(Id));

        if (dataDeliveryMethod == null)
            return BadRequest(new ValidationResult("DeliveryMethod not exists"));

        var DeliveryMethodEditCommand = new DeliveryMethodEditCommand(
           Id,
          request.Code,
           request.Name,
           request.Description,
           request.Status
       );

        var result = await _mediator.SendCommand(DeliveryMethodEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var DeliveryMethodId = new Guid(id);
        if (await _mediator.Send(new DeliveryMethodQueryById(DeliveryMethodId)) == null)
            return BadRequest(new ValidationResult("DeliveryMethod not exists"));

        var result = await _mediator.SendCommand(new DeliveryMethodDeleteCommand(DeliveryMethodId));

        return Ok(result);
    }
}
