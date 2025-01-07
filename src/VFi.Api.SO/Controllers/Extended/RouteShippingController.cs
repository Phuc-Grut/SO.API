using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers.Extended;

[Route("api/[controller]")]
[ApiController]
public class RouteShippingController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<RouteShippingController> _logger;

    public RouteShippingController(IMediatorHandler mediator, IContextUser context, ILogger<RouteShippingController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxStatusRequest request)
    {
        var result = await _mediator.Send(new RouteShippingQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] RouteShippingRequest request)
    {
        var query = new RouteShippingPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddRouteShippingRequest request)
    {
        var PostOfficeAddCommand = new RouteShippingAddCommand(
          Guid.NewGuid(),
           request.Code,
           request.Name,
           request.Description,
           request.Note,
           request.FromPost,
           request.ToPost,
           request.Status
      );
        var result = await _mediator.SendCommand(PostOfficeAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditRouteShippingRequest request)
    {
        var RouteShippingEditCommand = new RouteShippingEditCommand(
           request.Id,
           request.Code,
           request.Name,
           request.Description,
           request.Note,
           request.FromPost,
           request.ToPost,
           request.Status
       );

        var result = await _mediator.SendCommand(RouteShippingEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new RouteShippingSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        var RouteShippingId = new Guid(id);
        if (await _mediator.Send(new RouteShippingQueryById(RouteShippingId)) == null)
            return BadRequest(new ValidationResult("RouteShipping not exists"));

        var result = await _mediator.SendCommand(new RouteShippingDeleteCommand(RouteShippingId));

        return Ok(result);
    }
}
