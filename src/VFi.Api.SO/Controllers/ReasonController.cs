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
public class ReasonController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ReasonController> _logger;

    public ReasonController(IMediatorHandler mediator, ILogger<ReasonController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new ReasonQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] ReasonRequest request)
    {
        var query = new ReasonPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddReasonRequest request)
    {
        var ReasonAddCommand = new ReasonAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.Status
      );
        var result = await _mediator.SendCommand(ReasonAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditReasonRequest request)
    {
        var ReasonId = new Guid(request.Id);
        ReasonDto dataReason = await _mediator.Send(new ReasonQueryById(ReasonId));

        if (dataReason == null)
            return BadRequest(new ValidationResult("Reason not exists"));

        var ReasonEditCommand = new ReasonEditCommand(
           ReasonId,
           request.Code,
           request.Name,
           request.Description,
           request.Status
       );

        var result = await _mediator.SendCommand(ReasonEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var ReasonId = new Guid(id);
        if (await _mediator.Send(new ReasonQueryById(ReasonId)) == null)
            return BadRequest(new ValidationResult("Reason not exists"));

        var result = await _mediator.SendCommand(new ReasonDeleteCommand(ReasonId));

        return Ok(result);
    }
}
