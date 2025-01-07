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
public class LeadProfileController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<LeadProfileController> _logger;

    public LeadProfileController(IMediatorHandler mediator, IContextUser context, ILogger<LeadProfileController> logger)
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
        var rs = await _mediator.Send(new LeadProfileQueryById(id));
        return Ok(rs);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] LeadProfileRequest request)
    {
        var query = new LeadProfilePagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddLeadProfileRequest request)
    {
        var LeadProfileAddCommand = new LeadProfileAddCommand(
          Guid.NewGuid(),
          request.LeadId,
          request.Key,
          request.Value,
          request.Description
      );
        var result = await _mediator.SendCommand(LeadProfileAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditLeadProfileRequest request)
    {
        LeadProfileDto data = await _mediator.Send(new LeadProfileQueryById(request.Id));
        if (data == null)
            return BadRequest(new ValidationResult("Code not exists"));

        var LeadProfileEditCommand = new LeadProfileEditCommand(
           request.Id,
           request.LeadId,
           request.Key,
           request.Value,
           request.Description
       );
        var result = await _mediator.SendCommand(LeadProfileEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new LeadProfileSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        var LeadProfileId = new Guid(id);
        if (await _mediator.Send(new LeadProfileQueryById(LeadProfileId)) == null)
            return BadRequest(new ValidationResult("Code not exists"));

        var result = await _mediator.SendCommand(new LeadProfileDeleteCommand(LeadProfileId));

        return Ok(result);
    }
}
