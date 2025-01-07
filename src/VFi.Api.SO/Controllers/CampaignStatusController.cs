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
public class CampaignStatusController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<CampaignStatusController> _logger;

    public CampaignStatusController(IMediatorHandler mediator, IContextUser context, ILogger<CampaignStatusController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Lấy thông tin
    /// </summary>
    /// <param name="id">Thông tin</param>
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new CampaignStatusQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new CampaignStatusQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] CampaignStatusRequest request)
    {
        if (request.PageNumber == 0)
        {
            request.PageNumber = 1;
        }
        if (request.PageSize == 0)
        {
            request.PageSize = int.MaxValue;
        }
        var query = new CampaignStatusPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize, request.CampaignId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddCampaignStatusRequest request)
    {
        var CampaignStatusAddCommand = new CampaignStatusAddCommand(
          Guid.NewGuid(),
          request.CampaignId,
          request.Name,
          request.Color,
          request.Description,
          request.IsDefault,
          request.IsClose,
          request.Status,
          request.DisplayOrder
      );
        var result = await _mediator.SendCommand(CampaignStatusAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditCampaignStatusRequest request)
    {
        var CampaignStatusEditCommand = new CampaignStatusEditCommand(
           (Guid)request.Id,
           request.CampaignId,
           request.Name,
           request.Color,
           request.Description,
           request.IsDefault,
           request.IsClose,
           request.Status,
           request.DisplayOrder
       );
        var result = await _mediator.SendCommand(CampaignStatusEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new CampaignStatusSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

            var result = await _mediator.SendCommand(sortCommand);
            return Ok(result);
        }
        else
        {
            return BadRequest("Please input list sort");
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var CampaignStatusId = new Guid(id);
        if (await _mediator.Send(new CampaignStatusQueryById(CampaignStatusId)) == null)
            return BadRequest(new ValidationResult("CampaignStatus not exists"));

        var result = await _mediator.SendCommand(new CampaignStatusDeleteCommand(CampaignStatusId));

        return Ok(result);
    }
}
