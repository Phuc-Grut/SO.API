using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries.Extended;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public partial class WalletTransactionController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<WalletTransactionController> _logger;

    public WalletTransactionController(IMediatorHandler mediator, ILogger<WalletTransactionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new WalletTransactionQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] WalletTransactionRequest request)
    {
        var query = new WalletTransactionPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddWalletTransactionRequest request)
    {
        var cmd = new WalletTransactionAddCommand()
        {
            Id = Guid.NewGuid(),
            Code = request.Code
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditWalletTransactionRequest request)
    {
        var item = await _mediator.Send(new WalletTransactionQueryById(request.Id));

        if (item == null)
            return BadRequest(new ValidationResult("WalletTransaction not exists"));

        var cmd = new WalletTransactionEditCommand()
        {
            Id = request.Id,
            Code = request.Code
        };
        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (await _mediator.Send(new WalletTransactionQueryById(id)) == null)
            return BadRequest(new ValidationResult("WalletTransaction not exists"));

        var result = await _mediator.SendCommand(new WalletTransactionDeleteCommand(id));

        return Ok(result);
    }
}
