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
public class SalesChannelController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<SalesChannelController> _logger;

    public SalesChannelController(IMediatorHandler mediator, ILogger<SalesChannelController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new SalesChannelQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new SalesChannelQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] SalesChannelRequest request)
    {
        var query = new SalesChannelPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddSalesChannelRequest request)
    {
        var SalesChannelAddCommand = new SalesChannelAddCommand(
          Guid.NewGuid(),
          request.Code,
           request.Name,
           request.Description,
           request.Status,
           request.IsDefault,
           request.DisplayOrder

      );
        var result = await _mediator.SendCommand(SalesChannelAddCommand);
        return Ok(result);
    }

    [HttpPut("edit/{id}")]
    public async Task<IActionResult> Put(Guid Id, [FromBody] EditSalesChannelRequest request)
    {
        SalesChannelDto dataSalesChannel = await _mediator.Send(new SalesChannelQueryById(Id));

        if (dataSalesChannel == null)
            return BadRequest(new ValidationResult("SalesChannel not exists"));

        var SalesChannelEditCommand = new SalesChannelEditCommand(
           Id,
          request.Code,
           request.Name,
           request.Description,
           request.Status,
           request.IsDefault,
           request.DisplayOrder
       );

        var result = await _mediator.SendCommand(SalesChannelEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var SalesChannelId = new Guid(id);
        if (await _mediator.Send(new SalesChannelQueryById(SalesChannelId)) == null)
            return BadRequest(new ValidationResult("SalesChannel not exists"));

        var result = await _mediator.SendCommand(new SalesChannelDeleteCommand(SalesChannelId));

        return Ok(result);
    }
}
