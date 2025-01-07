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
public class OrderCostController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<OrderCostController> _logger;

    public OrderCostController(IMediatorHandler mediator, IContextUser context, ILogger<OrderCostController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new OrderCostQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new OrderCostQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
    {
        var query = new OrderCostPagingQuery(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddOrderCostRequest request)
    {
        var OrderCostAddCommand = new OrderCostAddCommand(
          Guid.NewGuid(),
          request.QuotationId,
          !String.IsNullOrEmpty(request.ExpenseId) ? new Guid(request.ExpenseId) : null,
         request.Type,
         request.Rate,
         request.Amount,
         request.Status,
         request.DisplayOrder,
          _context.GetUserId(),
          DateTime.Now,
         request.UpdatedBy,
         request.UpdatedDate

      );
        var result = await _mediator.SendCommand(OrderCostAddCommand);
        return Ok(result);
    }

    [HttpPut("edit/{id}")]
    public async Task<IActionResult> Put(Guid Id, [FromBody] EditOrderCostRequest request)
    {
        OrderCostDto dataOrderCost = await _mediator.Send(new OrderCostQueryById(Id));

        if (dataOrderCost == null)
            return BadRequest(new ValidationResult("OrderCost not exists"));

        var OrderCostEditCommand = new OrderCostEditCommand(
           Id,
          request.QuotationId,
          !String.IsNullOrEmpty(request.ExpenseId) ? new Guid(request.ExpenseId) : null,
         request.Type,
         request.Rate,
         request.Amount,
         request.Status,
         request.DisplayOrder,
          _context.GetUserId(),
          DateTime.Now,
         request.UpdatedBy,
         request.UpdatedDate
       );

        var result = await _mediator.SendCommand(OrderCostEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var OrderCostId = new Guid(id);
        if (await _mediator.Send(new OrderCostQueryById(OrderCostId)) == null)
            return BadRequest(new ValidationResult("OrderCost not exists"));

        var result = await _mediator.SendCommand(new OrderCostDeleteCommand(OrderCostId));

        return Ok(result);
    }
}
