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
public class PriceListCrossController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<PriceListCrossController> _logger;

    public PriceListCrossController(IMediatorHandler mediator, ILogger<PriceListCrossController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] PriceListCrossListBoxRequest request)
    {
        var result = await _mediator.Send(new PriceListCrossQueryComboBox(request.KeyWord ?? "", request.Status, request.RouterShippingId));
        return Ok(result);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _mediator.Send(new PriceListCrossQueryById(id));
        return Ok(result);
    }

    [HttpGet("get-by-account")]
    public async Task<IActionResult> GetByAccount([FromQuery] PriceListCrossByAccountFilterQueryRequest request)
    {
        var rs = await _mediator.Send(new PriceListCrossQueryByAccount(request.AccountId,
                                                                       request.RouterShippingId,
                                                                       request.Keyword ?? "",
                                                                       request.Filter ?? "",
                                                                       request.Order ?? "",
                                                                       request.PageNumber,
                                                                       request.PageSize));
        return Ok(rs);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PriceListCrossFilterQueryRequest request)
    {
        var query = new PriceListCrossPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] PriceListCrossRequest request)
    {
        var detail = request.Details.Select(x => new PriceListCrossDetailDto()
        {
            PriceListCrossId = x.PriceListCrossId,
            PriceListCross = x.PriceListCross,
            CommodityGroupId = x.CommodityGroupId,
            CommodityGroupCode = x.CommodityGroupCode,
            CommodityGroupName = x.CommodityGroupName,
            AirFreight = x.AirFreight,
            SeaFreight = x.SeaFreight,
            DisplayOrder = x.DisplayOrder,
            Currency = x.Currency,
            Note = x.Note,
            Status = x.Status,
        }).ToList();
        var cmd = new PriceListCrossAddCommand(Guid.NewGuid(),
                                               request.Code,
                                               request.Name,
                                               request.Description,
                                               request.Status,
                                               request.DisplayOrder,
                                               request.Default,
                                               request.RouterShippingId,
                                               request.RouterShipping,
                                               detail);
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPut("edit/{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] PriceListCrossRequest request)
    {
        var detail = request.Details.Select(x => new PriceListCrossDetailDto()
        {
            Id = x.Id ?? Guid.NewGuid(),
            PriceListCrossId = x.PriceListCrossId,
            PriceListCross = x.PriceListCross,
            CommodityGroupId = x.CommodityGroupId,
            CommodityGroupCode = x.CommodityGroupCode,
            CommodityGroupName = x.CommodityGroupName,
            AirFreight = x.AirFreight,
            SeaFreight = x.SeaFreight,
            Currency = x.Currency,
            DisplayOrder = x.DisplayOrder,
            Note = x.Note,
            Status = x.Status,
        }).ToList();
        var cmd = new PriceListCrossEditCommand(id,
                                                request.Code,
                                                request.Name,
                                                request.Description,
                                                request.Status,
                                                request.DisplayOrder,
                                                request.Default,
                                                request.RouterShippingId,
                                                request.RouterShipping,
                                                detail);
        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new PriceListCrossSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        if (await _mediator.Send(new PriceListCrossQueryById(id)) == null)
            return BadRequest(new ValidationResult("PriceListCross not exists"));

        var result = await _mediator.SendCommand(new PriceListCrossDeleteCommand(id));

        return Ok(result);
    }
}
