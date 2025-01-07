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
public class GroupEmployeeController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<GroupEmployeeController> _logger;

    public GroupEmployeeController(IMediatorHandler mediator, IContextUser context, ILogger<GroupEmployeeController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new GroupEmployeeQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpPost("get-by-list-id")]
    public async Task<IActionResult> Get([FromBody] ListIdRequest request)
    {
        var result = await _mediator.Send(new GroupEmployeeQueryByListId(request.ListId));
        return Ok(result);
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new GroupEmployeeQueryById(id));
        return Ok(rs);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] GroupEmployeeRequest request)
    {
        var query = new GroupEmployeePagingQuery(request.Status, request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddGroupEmployeeRequest request)
    {
        var GroupEmployeeAddCommand = new GroupEmployeeAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.Status
      );
        var result = await _mediator.SendCommand(GroupEmployeeAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditGroupEmployeeRequest request)
    {
        var GroupEmployeeId = request.Id;
        GroupEmployeeDto dataGroupEmployee = await _mediator.Send(new GroupEmployeeQueryById(GroupEmployeeId));

        if (dataGroupEmployee == null)
            return BadRequest(new ValidationResult("GroupEmployee not exists"));

        var GroupEmployeeEditCommand = new GroupEmployeeEditCommand(
           GroupEmployeeId,
           request.Code,
           request.Name,
           request.Description,
           request.Status
       );

        var result = await _mediator.SendCommand(GroupEmployeeEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var GroupEmployeeId = new Guid(id);
        if (await _mediator.Send(new GroupEmployeeQueryById(GroupEmployeeId)) == null)
            return BadRequest(new ValidationResult("GroupEmployee not exists"));

        var result = await _mediator.SendCommand(new GroupEmployeeDeleteCommand(GroupEmployeeId));

        return Ok(result);
    }
}
