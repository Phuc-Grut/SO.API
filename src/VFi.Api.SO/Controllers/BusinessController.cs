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
public class BusinessController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<BusinessController> _logger;

    public BusinessController(IMediatorHandler mediator, IContextUser context, ILogger<BusinessController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new BusinessQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] BusinessRequest request)
    {
        var query = new BusinessPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddBusinessRequest request)
    {
        var BusinessAddCommand = new BusinessAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.DisplayOrder,
          request.Status
      );
        var result = await _mediator.SendCommand(BusinessAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditBusinessRequest request)
    {
        var BusinessId = new Guid(request.Id);
        var BusinessEditCommand = new BusinessEditCommand(
           BusinessId,
           request.Code,
           request.Name,
           request.Description,
           request.DisplayOrder,
           request.Status
       );

        var result = await _mediator.SendCommand(BusinessEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new BusinessSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        var BusinessId = new Guid(id);
        if (await _mediator.Send(new BusinessQueryById(BusinessId)) == null)
            return BadRequest(new ValidationResult("Business not exists"));

        var result = await _mediator.SendCommand(new BusinessDeleteCommand(BusinessId));

        return Ok(result);
    }
}
