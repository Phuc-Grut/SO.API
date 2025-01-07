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
public class BankController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<BankController> _logger;

    public BankController(IMediatorHandler mediator, IContextUser context, ILogger<BankController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new BankQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] BankRequest request)
    {
        var query = new BankPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddBankRequest request)
    {
        var BankAddCommand = new BankAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Qrbin,
          request.ShortName,
          request.Name,
          request.EnglishName,
          request.Address,
          request.DisplayOrder,
          request.Status,
          request.Note,
          request.Image
      );
        var result = await _mediator.SendCommand(BankAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditBankRequest request)
    {
        var BankId = new Guid(request.Id);
        var BankEditCommand = new BankEditCommand(
           BankId,
           request.Code,
           request.Qrbin,
           request.ShortName,
           request.Name,
           request.EnglishName,
           request.Address,
           request.DisplayOrder,
           request.Status,
           request.Note,
           request.Image
       );
        var result = await _mediator.SendCommand(BankEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new BankSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        var BankId = new Guid(id);
        if (await _mediator.Send(new BankQueryById(BankId)) == null)
            return BadRequest(new ValidationResult("Bank not exists"));

        var result = await _mediator.SendCommand(new BankDeleteCommand(BankId));

        return Ok(result);
    }
}
