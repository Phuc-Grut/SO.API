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
public class CampaignController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<CampaignController> _logger;

    public CampaignController(IMediatorHandler mediator, IContextUser context, ILogger<CampaignController> logger)
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
        var rs = await _mediator.Send(new CampaignQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new CampaignQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] CampaignRequest request)
    {
        if (request.PageNumber == 0)
        {
            request.PageNumber = 1;
        }
        if (request.PageSize == 0)
        {
            request.PageSize = int.MaxValue;
        }
        if (_context.QueryMyData())
        {
            request.EmployeeId = _context.GetUserId().ToString();
        }
        var query = new CampaignPagingQuery(
            request.Keyword,
            request.Status,
            request.EmployeeId,
            request.Filter,
            request.Order,
            request.PageNumber,
            request.PageSize
            );
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddCampaignRequest request)
    {
        var Code = request.Code;
        if (request.IsAuto == true)
        {
            Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, 1));
        }
        else
        {
            var useCodeCommand = new UseCodeCommand(
                request.ModuleCode,
                Code
                );
            _mediator.SendCommand(useCodeCommand);
        }
        var Details = request.Details?.Select(x => new CampaignStatusDto()
        {
            Id = Guid.NewGuid(),
            CampaignId = x.CampaignId,
            Name = x.Name,
            Color = x.Color,
            TextColor = x.TextColor,
            IsDefault = x.IsDefault,
            IsClose = x.IsClose,
            Description = x.Description,
            Status = x.Status,
            DisplayOrder = x.DisplayOrder
        }).ToList();
        var CampaignAddCommand = new CampaignAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.StartDate,
          request.EndDate,
          request.Leader,
          request.LeaderName,
          request.Member.Count() > 0 ? string.Join(",", request.Member) : null,
          request.Status,
          Details
      );
        var result = await _mediator.SendCommand(CampaignAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditCampaignRequest request)
    {
        CampaignDto data = await _mediator.Send(new CampaignQueryById(request.Id));
        if (data == null)
            return BadRequest(new ValidationResult("Code not exists"));

        var Details = request.Details?.Select(x => new CampaignStatusDto()
        {
            Id = (Guid)(x.Id == null ? Guid.NewGuid() : x.Id),
            CampaignId = x.CampaignId,
            Name = x.Name,
            Color = x.Color,
            TextColor = x.TextColor,
            IsDefault = x.IsDefault,
            IsClose = x.IsClose,
            Description = x.Description,
            Status = x.Status,
            DisplayOrder = x.DisplayOrder
        }).ToList();
        var Delete = data.Details?.Where(x => request.Details.Where(f => f.Id != null).Select(f => f.Id).Contains(x.Id) == false)?.Select(x => new ListId()
        {
            Id = x.Id
        }).ToList();
        var CampaignEditCommand = new CampaignEditCommand(
           request.Id,
           request.Code,
           request.Name,
           request.Description,
           request.StartDate,
           request.EndDate,
           request.Leader,
           request.LeaderName,
           request.Member.Count() > 0 ? string.Join(",", request.Member) : null,
           request.Status,
           Details,
           Delete
       );
        var result = await _mediator.SendCommand(CampaignEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new CampaignSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        var CampaignId = new Guid(id);
        if (await _mediator.Send(new CampaignQueryById(CampaignId)) == null)
            return BadRequest(new ValidationResult("Code not exists"));

        var result = await _mediator.SendCommand(new CampaignDeleteCommand(CampaignId));

        return Ok(result);
    }
}
