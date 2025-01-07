using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeadActivityController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<LeadActivityController> _logger;

    public LeadActivityController(IMediatorHandler mediator, IContextUser context, ILogger<LeadActivityController> logger)
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
        var rs = await _mediator.Send(new LeadActivityQueryById(id));
        return Ok(rs);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] LeadActivityRequest request)
    {
        if (request.PageNumber == 0)
        {
            request.PageNumber = 1;
        }
        if (request.PageSize == 0)
        {
            request.PageSize = int.MaxValue;
        }
        var query = new LeadActivityPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddLeadActivityRequest request)
    {
        var LeadActivityAddCommand = new LeadActivityAddCommand(
          Guid.NewGuid(),
          request.LeadId,
          request.CampaignId,
          request.Campaign,
          request.Type,
          request.Name,
          request.Body,
          request.ActualDate,
          JsonConvert.SerializeObject(request.Attachment),
          request.Status
      );
        var result = await _mediator.SendCommand(LeadActivityAddCommand);

        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditLeadActivityRequest request)
    {
        LeadActivityDto data = await _mediator.Send(new LeadActivityQueryById(request.Id));
        if (data == null)
            return BadRequest(new ValidationResult("Code not exists"));

        var LeadActivityEditCommand = new LeadActivityEditCommand(
           request.Id,
           request.LeadId,
           request.CampaignId,
           request.Campaign,
           request.Type,
           request.Name,
           request.Body,
           request.ActualDate,
           JsonConvert.SerializeObject(request.Attachment),
           request.Status
       );
        var result = await _mediator.SendCommand(LeadActivityEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new LeadActivitySortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        var LeadActivityId = new Guid(id);
        if (await _mediator.Send(new LeadActivityQueryById(LeadActivityId)) == null)
            return BadRequest(new ValidationResult("Code not exists"));

        var result = await _mediator.SendCommand(new LeadActivityDeleteCommand(LeadActivityId));

        return Ok(result);
    }
}
