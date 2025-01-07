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
public class PromotionGroupController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<PromotionGroupController> _logger;

    public PromotionGroupController(IMediatorHandler mediator, IContextUser context, ILogger<PromotionGroupController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new PromotionGroupQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PromotionGroupRequest request)
    {
        var query = new PromotionGroupPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddPromotionGroupRequest request)
    {
        var PromotionGroupAddCommand = new PromotionGroupAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.Status
      );
        var result = await _mediator.SendCommand(PromotionGroupAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditPromotionGroupRequest request)
    {
        var PromotionGroupId = new Guid(request.Id);
        PromotionGroupDto dataPromotionGroup = await _mediator.Send(new PromotionGroupQueryById(PromotionGroupId));

        if (dataPromotionGroup == null)
            return BadRequest(new ValidationResult("PromotionGroup not exists"));

        var PromotionGroupEditCommand = new PromotionGroupEditCommand(
           PromotionGroupId,
           request.Code,
           request.Name,
           request.Description,
           request.Status
       );

        var result = await _mediator.SendCommand(PromotionGroupEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var PromotionGroupId = new Guid(id);
        if (await _mediator.Send(new PromotionGroupQueryById(PromotionGroupId)) == null)
            return BadRequest(new ValidationResult("PromotionGroup not exists"));

        var result = await _mediator.SendCommand(new PromotionGroupDeleteCommand(PromotionGroupId));

        return Ok(result);
    }
}
