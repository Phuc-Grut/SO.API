using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShippingMethodController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ShippingMethodController> _logger;

    public ShippingMethodController(IMediatorHandler mediator, ILogger<ShippingMethodController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new ShippingMethodQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] ShippingMethodRequest request)
    {
        var query = new ShippingMethodPagingFilterQuery(request.Status, request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddShippingMethodRequest request)
    {
        var ShippingMethodAddCommand = new ShippingMethodAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.Status
      );
        var result = await _mediator.SendCommand(ShippingMethodAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditShippingMethodRequest request)
    {
        var ShippingMethodId = new Guid(request.Id);
        ShippingMethodDto dataShippingMethod = await _mediator.Send(new ShippingMethodQueryById(ShippingMethodId));

        if (dataShippingMethod == null)
            return BadRequest(new ValidationResult("ShippingMethod not exists"));

        var ShippingMethodEditCommand = new ShippingMethodEditCommand(
           ShippingMethodId,
           request.Code,
           request.Name,
           request.Description,
           request.Status
       );

        var result = await _mediator.SendCommand(ShippingMethodEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var ShippingMethodId = new Guid(id);
        if (await _mediator.Send(new ShippingMethodQueryById(ShippingMethodId)) == null)
            return BadRequest(new ValidationResult("ShippingMethod not exists"));

        var result = await _mediator.SendCommand(new ShippingMethodDeleteCommand(ShippingMethodId));

        return Ok(result);
    }
}
