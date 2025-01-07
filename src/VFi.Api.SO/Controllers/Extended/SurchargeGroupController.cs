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
public class SurchargeGroupController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<SurchargeGroupController> _logger;

    public SurchargeGroupController(IMediatorHandler mediator, ILogger<SurchargeGroupController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new SurchargeGroupQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] SurchargeGroupRequest request)
    {
        var query = new SurchargeGroupPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddSurchargeGroupRequest request)
    {
        var cmd = new SurchargeGroupAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.Note,
          request.DisplayOrder,
          request.Status
      );
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditSurchargeGroupRequest request)
    {
        var item = await _mediator.Send(new SurchargeGroupQueryById(request.Id));

        if (item == null)
            return BadRequest(new ValidationResult("SurchargeGroup not exists"));

        var cmd = new SurchargeGroupEditCommand(
           request.Id,
           request.Code,
           request.Name,
           request.Description, request.Note, request.DisplayOrder,
           request.Status
       );

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (await _mediator.Send(new SurchargeGroupQueryById(id)) == null)
            return BadRequest(new ValidationResult("SurchargeGroup not exists"));

        var result = await _mediator.SendCommand(new SurchargeGroupDeleteCommand(id));

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new SurchargeGroupSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

            var result = await _mediator.SendCommand(sortCommand);
            return Ok(result);
        }
        else
        {
            return BadRequest("Please input list sort");
        }
    }
}
