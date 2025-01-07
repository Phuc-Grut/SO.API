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
public class CustomerGroupController : ControllerBase
{
    private readonly IMediatorHandler _mediator;

    public CustomerGroupController(
        IMediatorHandler mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new CustomerGroupQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] CustomerGroupRequest request)
    {
        var query = new CustomerGroupPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddCustomerGroupRequest request)
    {
        var CustomerGroupAddCommand = new CustomerGroupAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.DisplayOrder,
          request.Status
      );
        var result = await _mediator.SendCommand(CustomerGroupAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditCustomerGroupRequest request)
    {
        var CustomerGroupId = new Guid(request.Id);
        CustomerGroupDto dataCustomerGroup = await _mediator.Send(new CustomerGroupQueryById(CustomerGroupId));

        if (dataCustomerGroup == null)
            return BadRequest(new ValidationResult("CustomerGroup not exists"));

        var CustomerGroupEditCommand = new CustomerGroupEditCommand(
           CustomerGroupId,
           request.Code,
           request.Name,
           request.Description,
           request.DisplayOrder,
           request.Status
       );

        var result = await _mediator.SendCommand(CustomerGroupEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new CustomerGroupSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        var customerGroupId = new Guid(id);
        if (await _mediator.Send(new CustomerGroupQueryById(customerGroupId)) == null)
            return BadRequest(new ValidationResult("CustomerGroup not exists"));
        var result = await _mediator.SendCommand(new CustomerGroupDeleteCommand(customerGroupId));

        return Ok(result);
    }
}
