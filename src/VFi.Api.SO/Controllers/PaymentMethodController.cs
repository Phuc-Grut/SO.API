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
public class PaymentMethodController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<PaymentMethodController> _logger;

    public PaymentMethodController(IMediatorHandler mediator, ILogger<PaymentMethodController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new PaymentMethodQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PaymentMethodRequest request)
    {
        var query = new PaymentMethodPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddPaymentMethodRequest request)
    {
        var PaymentMethodAddCommand = new PaymentMethodAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.Status
      );
        var result = await _mediator.SendCommand(PaymentMethodAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditPaymentMethodRequest request)
    {
        var PaymentMethodId = new Guid(request.Id);
        PaymentMethodDto dataPaymentMethod = await _mediator.Send(new PaymentMethodQueryById(PaymentMethodId));

        if (dataPaymentMethod == null)
            return BadRequest(new ValidationResult("PaymentMethod not exists"));

        var PaymentMethodEditCommand = new PaymentMethodEditCommand(
           PaymentMethodId,
           request.Code,
           request.Name,
           request.Description,
           request.Status
       );

        var result = await _mediator.SendCommand(PaymentMethodEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var PaymentMethodId = new Guid(id);
        if (await _mediator.Send(new PaymentMethodQueryById(PaymentMethodId)) == null)
            return BadRequest(new ValidationResult("PaymentMethod not exists"));

        var result = await _mediator.SendCommand(new PaymentMethodDeleteCommand(PaymentMethodId));

        return Ok(result);
    }
}
