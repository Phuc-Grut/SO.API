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
public class CustomerSourceController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<CustomerSourceController> _logger;

    public CustomerSourceController(IMediatorHandler mediator, ILogger<CustomerSourceController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new CustomerSourceQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] CustomerSourceRequest request)
    {
        var query = new CustomerSourcePagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddCustomerSourceRequest request)
    {
        var CustomerSourceAddCommand = new CustomerSourceAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.DisplayOrder,
          request.Status
      );
        var result = await _mediator.SendCommand(CustomerSourceAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditCustomerSourceRequest request)
    {
        var CustomerSourceId = new Guid(request.Id);
        CustomerSourceDto dataCustomerSource = await _mediator.Send(new CustomerSourceQueryById(CustomerSourceId));

        if (dataCustomerSource == null)
            return BadRequest(new ValidationResult("CustomerSource not exists"));

        var CustomerSourceEditCommand = new CustomerSourceEditCommand(
           CustomerSourceId,
           request.Code,
           request.Name,
           request.Description,
           request.DisplayOrder,
           request.Status
       );

        var result = await _mediator.SendCommand(CustomerSourceEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new CustomerSourceSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        var CustomerSourceId = new Guid(id);
        if (await _mediator.Send(new CustomerSourceQueryById(CustomerSourceId)) == null)
            return BadRequest(new ValidationResult("CustomerSource not exists"));

        var result = await _mediator.SendCommand(new CustomerSourceDeleteCommand(CustomerSourceId));

        return Ok(result);
    }
}
