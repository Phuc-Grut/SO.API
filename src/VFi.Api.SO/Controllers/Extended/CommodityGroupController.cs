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
public class CommodityGroupController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<CommodityGroupController> _logger;

    public CommodityGroupController(IMediatorHandler mediator, ILogger<CommodityGroupController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new CommodityGroupQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] CommodityGroupRequest request)
    {
        var query = new CommodityGroupPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddCommodityGroupRequest request)
    {
        var cmd = new CommodityGroupAddCommand(
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
    public async Task<IActionResult> Put([FromBody] EditCommodityGroupRequest request)
    {
        var item = await _mediator.Send(new CommodityGroupQueryById(request.Id));

        if (item == null)
            return BadRequest(new ValidationResult("CommodityGroup not exists"));

        var cmd = new CommodityGroupEditCommand(
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
        if (await _mediator.Send(new CommodityGroupQueryById(id)) == null)
            return BadRequest(new ValidationResult("CommodityGroup not exists"));

        var result = await _mediator.SendCommand(new CommodityGroupDeleteCommand(id));

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new CommodityGroupSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

            var result = await _mediator.SendCommand(sortCommand);
            return Ok(result);
        }
        else
        {
            return BadRequest("Please input list sort");
        }
    }
}
