using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers.Extended;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<TransactionController> _logger;

    public TransactionController(IMediatorHandler mediator, ILogger<TransactionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new TransactionQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] TransactionRequest request)
    {
        var query = new TransactionPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddTransactionRequest request)
    {
        var cmd = new TransactionAddCommand()
        {
            Id = Guid.NewGuid(),
            Code = request.Code
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditTransactionRequest request)
    {
        var item = await _mediator.Send(new TransactionQueryById(request.Id));

        if (item == null)
            return BadRequest(new ValidationResult("Transaction not exists"));

        var cmd = new TransactionEditCommand()
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
        if (await _mediator.Send(new TransactionQueryById(id)) == null)
            return BadRequest(new ValidationResult("Transaction not exists"));

        var result = await _mediator.SendCommand(new TransactionDeleteCommand(id));

        return Ok(result);
    }
}
