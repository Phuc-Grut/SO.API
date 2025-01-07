using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers.Extended;

[Route("api/[controller]")]
[ApiController]
public class PurchaseGroupController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<PurchaseGroupController> _logger;

    public PurchaseGroupController(IMediatorHandler mediator, ILogger<PurchaseGroupController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new PurchaseGroupQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PurchaseGroupRequest request)
    {
        var query = new PurchaseGroupPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddPurchaseGroupRequest request)
    {
        var cmd = new PurchaseGroupAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description, request.Note, request.DisplayOrder,
          request.Status
      );
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditPurchaseGroupRequest request)
    {
        var item = await _mediator.Send(new PurchaseGroupQueryById(request.Id));

        if (item == null)
            return BadRequest(new ValidationResult("PurchaseGroup not exists"));

        var cmd = new PurchaseGroupEditCommand(
           request.Id,
           request.Code,
           request.Name,
           request.Description, request.Note, request.DisplayOrder,
           request.Status
       );

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new PurchaseGroupSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

            var result = await _mediator.SendCommand(sortCommand);
            return Ok(result);
        }
        else
        {
            return BadRequest("Please input list sort");
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (await _mediator.Send(new PurchaseGroupQueryById(id)) == null)
            return BadRequest(new ValidationResult("PurchaseGroup not exists"));

        var result = await _mediator.SendCommand(new PurchaseGroupDeleteCommand(id));

        return Ok(result);
    }
}
