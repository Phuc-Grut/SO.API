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
public partial class ServiceAddController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ServiceAddController> _logger;

    public ServiceAddController(IMediatorHandler mediator, IContextUser context, ILogger<ServiceAddController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-listbox")]
    public async Task<IActionResult> Get([FromQuery] ListBoxServiceAddRequest request)
    {
        var result = await _mediator.Send(new ServiceAddQueryListBox(request.Keyword, request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PagingServiceAddRequest request)
    {
        var pageSize = request.Top;
        var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

        ServiceAddPagingQuery query = new ServiceAddPagingQuery(request.Keyword, request.Status, pageSize, pageIndex);

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddServiceAddRequest request)
    {
        var storeAddCommand = new ServiceAddAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.CalculationMethod,
          request.Price,
          request.PriceSyntax,
          request.MinPrice,
          request.MaxPrice,
          request.PayLater,
          request.Status,
          request.Tags,
          request.Currency,
          request.CurrencyName,
          request.DisplayOrder,
          _context.GetUserId(),
          DateTime.Now,
          _context.UserName
      );
        var result = await _mediator.SendCommand(storeAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditServiceAddRequest request)
    {
        var storeEditCommand = new ServiceAddEditCommand(
           new Guid(request.Id),
           request.Code,
           request.Name,
           request.Description,
           request.CalculationMethod,
           request.Price,
           request.PriceSyntax,
           request.MinPrice,
           request.MaxPrice,
           request.PayLater,
           request.Status,
           request.Tags,
           request.Currency,
           request.CurrencyName,
           request.DisplayOrder,
           _context.GetUserId(),
           DateTime.Now,
           _context.UserName
       );

        var result = await _mediator.SendCommand(storeEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var storeSortCommand = new ServiceAddSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

            var result = await _mediator.SendCommand(storeSortCommand);
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
        var result = await _mediator.SendCommand(new ServiceAddDeleteCommand(new Guid(id)));

        return Ok(result);
    }
}
