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
public class ShippingCarrierController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ShippingCarrierController> _logger;

    public ShippingCarrierController(IMediatorHandler mediator, ILogger<ShippingCarrierController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var query = new ShippingCarrierQueryComboBox(request.Status);
        query.Country = request.Country;
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] ShippingCarrierRequest request)
    {
        var query = new ShippingCarrierPagingFilterQuery(request.Status, request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
        query.Country = request.Country;
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddShippingCarrierRequest request)
    {
        var cmd = new ShippingCarrierAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.Status
      );
        cmd.Country = request.Country;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditShippingCarrierRequest request)
    {
        var ShippingCarrierId = new Guid(request.Id);
        ShippingCarrierDto dataShippingCarrier = await _mediator.Send(new ShippingCarrierQueryById(ShippingCarrierId));

        if (dataShippingCarrier == null)
            return BadRequest(new ValidationResult("ShippingCarrier not exists"));

        var cmd = new ShippingCarrierEditCommand(
           ShippingCarrierId,
           request.Code,
           request.Name,
           request.Description,
           request.Status
       );
        cmd.Country = request.Country;
        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var ShippingCarrierId = new Guid(id);
        if (await _mediator.Send(new ShippingCarrierQueryById(ShippingCarrierId)) == null)
            return BadRequest(new ValidationResult("ShippingCarrier not exists"));

        var result = await _mediator.SendCommand(new ShippingCarrierDeleteCommand(ShippingCarrierId));

        return Ok(result);
    }
}
