using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpenseController : ControllerBase
{
    private readonly IMediatorHandler _mediator;

    public ExpenseController(IMediatorHandler mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new ExpenseQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] ExpenseRequest request)
    {
        var query = new ExpensePagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddExpenseRequest request)
    {
        var ExpenseAddCommand = new ExpenseAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.DisplayOrder,
          request.Status
      );
        var result = await _mediator.SendCommand(ExpenseAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditExpenseRequest request)
    {
        var ExpenseId = new Guid(request.Id);
        ExpenseDto dataExpense = await _mediator.Send(new ExpenseQueryById(ExpenseId));

        if (dataExpense == null)
            return BadRequest(new ValidationResult("Expense not exists"));

        var ExpenseEditCommand = new ExpenseEditCommand(
           ExpenseId,
           request.Code,
           request.Name,
           request.Description,
           request.DisplayOrder,
           request.Status
       );

        var result = await _mediator.SendCommand(ExpenseEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new ExpenseSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        var ExpenseId = new Guid(id);
        if (await _mediator.Send(new ExpenseQueryById(ExpenseId)) == null)
            return BadRequest(new ValidationResult("Expense not exists"));

        var result = await _mediator.SendCommand(new ExpenseDeleteCommand(ExpenseId));

        return Ok(result);
    }
}
