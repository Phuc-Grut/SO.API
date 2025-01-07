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
public class PriceListPurchaseController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<PriceListPurchaseController> _logger;

    public PriceListPurchaseController(IMediatorHandler mediator, IContextUser context, ILogger<PriceListPurchaseController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] PriceListPurchaseListBoxRequest request)
    {
        var result = await _mediator.Send(new PriceListPurchaseQueryComboBox(request.ToBaseQuery()));
        return Ok(result);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new PriceListPurchaseQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-by-account")]
    public async Task<IActionResult> GetByAccount([FromQuery] PriceListPurchaseAccountPagingRequest request)
    {
        var rs = await _mediator.Send(new PriceListPurchaseQueryByAccount(request.AccountId,
                                                                          request.Keyword ?? "",
                                                                          request.Filter ?? "",
                                                                          request.Order ?? "",
                                                                          request.PageNumber,
                                                                          request.PageSize));
        return Ok(rs);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PriceListPurchasePagingRequest request)
    {
        var query = new PriceListPurchasePagingQuery(
              request.Keyword ?? "",
              request.Status,
              request.Filter ?? "",
              request.Order ?? "",
              request.PageNumber,
              request.PageSize
          );
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Post([FromBody] AddPriceListPurchaseRequest request)
    {
        var detail = request.Details.Select(u => new PriceListPurchaseDetailDto()
        {
            BuyFeeMin = u.BuyFeeMin,
            PurchaseGroupId = u.PurchaseGroupId,
            PurchaseGroupCode = u.PurchaseGroupCode,
            PurchaseGroupName = u.PurchaseGroupName,
            BuyFee = u.BuyFee,
            Currency = u.Currency,
            BuyFeeFix = u.BuyFeeFix,
            Note = u.Note,
            Status = u.Status,
            DisplayOrder = u.DisplayOrder,
        }).ToList();

        var data = new PriceListPurchaseAddCommand(Guid.NewGuid(),
                                                   request.Code,
                                                   request.Name,
                                                   request.Description,
                                                   request.Status,
                                                   request.DisplayOrder,
                                                   detail);

        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    [HttpPut("edit/{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] EditPriceListPurchaseRequest request)
    {
        var useBy = _context.GetUserId();
        var useDate = DateTime.Now;

        var detail = request.Details.Select(u => new PriceListPurchaseDetailDto()
        {
            Id = u.Id ?? Guid.NewGuid(),
            BuyFeeMin = u.BuyFeeMin,
            PurchaseGroupId = u.PurchaseGroupId,
            PurchaseGroupCode = u.PurchaseGroupCode,
            PurchaseGroupName = u.PurchaseGroupName,
            BuyFee = u.BuyFee,
            Currency = u.Currency,
            BuyFeeFix = u.BuyFeeFix,
            DisplayOrder = u.DisplayOrder,
            Note = u.Note,
            Status = u.Status,
        }).ToList();
        var data = new PriceListPurchaseEditCommand(id,
                                                   request.Code,
                                                   request.Name,
                                                   request.Description,
                                                   request.Status,
                                                   request.DisplayOrder,
                                                   detail);

        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new PriceListPurchaseSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        var PriceListPurchase = await _mediator.Send(new PriceListPurchaseQueryById(id));

        if (PriceListPurchase == null)
        {
            return BadRequest(new ValidationResult("PriceListPurchase not exists"));
        }

        var data = new PriceListPurchaseDeleteCommand(id);

        var result = await _mediator.SendCommand(data);

        return Ok(result);
    }
}
