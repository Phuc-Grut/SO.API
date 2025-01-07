using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers.Extended;

[Route("api/[controller]")]
[ApiController]
public class PriceListSurchargeController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<PriceListSurchargeController> _logger;

    public PriceListSurchargeController(IMediatorHandler mediator, ILogger<PriceListSurchargeController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _mediator.Send(new PriceListSurchargeQueryById(id));
        return Ok(result);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] PriceListSurchargeListBoxRequest request)
    {
        var result = await _mediator.Send(new PriceListSurchargeQueryComboBox(request.Status, request.RouterShippingId));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PriceListSurchargeFilterQueryRequest request)
    {
        var query = new PriceListSurchargePagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddPriceListSurchargeRequest request)
    {
        var details = request.Details.Select(x => new PriceListSurchargeDto()
        {
            SurchargeGroupId = x.SurchargeGroupId,
            SurchargeGroup = x.SurchargeGroup,
            Currency = x.Currency,
            Price = x.Price,
            Note = x.Note,
            Status = x.Status
        }).ToList();
        var cmd = new PriceListSurchargeAddCommand(request.RouterShippingId, request.RouterShipping, details);
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPut("edit/{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] EditPriceListSurchargeRequest request)
    {
        var cmd = new PriceListSurchargeEditCommand(id,
                                                    request.RouterShippingId,
                                                    request.RouterShipping,
                                                    request.SurchargeGroupId,
                                                    request.SurchargeGroup,
                                                    request.Price,
                                                    request.Currency,
                                                    request.Note,
                                                    request.Status);
        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.SendCommand(new PriceListSurchargeDeleteCommand(id));

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new PriceListSurchargeSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

            var result = await _mediator.SendCommand(sortCommand);
            return Ok(result);
        }
        else
        {
            return BadRequest("Please input list sort");
        }
    }
}
