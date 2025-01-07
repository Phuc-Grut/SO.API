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
public class ContractTypeController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ContractTypeController> _logger;

    public ContractTypeController(IMediatorHandler mediator, IContextUser context, ILogger<ContractTypeController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new ContractTypeQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new ContractTypeQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] ContractTypeRequest request)
    {
        var query = new ContractTypePagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddContractTypeRequest request)
    {
        var ContractTypeAddCommand = new ContractTypeAddCommand(
          Guid.NewGuid(),
          request.Code,
           request.Name,
           request.Description,
           request.Status
      );
        var result = await _mediator.SendCommand(ContractTypeAddCommand);
        return Ok(result);
    }

    [HttpPut("edit/{id}")]
    public async Task<IActionResult> Put(Guid Id, [FromBody] EditContractTypeRequest request)
    {
        ContractTypeDto dataContractType = await _mediator.Send(new ContractTypeQueryById(Id));

        if (dataContractType == null)
            return BadRequest(new ValidationResult("ContractType not exists"));

        var ContractTypeEditCommand = new ContractTypeEditCommand(
           Id,
          request.Code,
           request.Name,
           request.Description,
           request.Status
       );

        var result = await _mediator.SendCommand(ContractTypeEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var ContractTypeId = new Guid(id);
        if (await _mediator.Send(new ContractTypeQueryById(ContractTypeId)) == null)
            return BadRequest(new ValidationResult("ContractType not exists"));

        var result = await _mediator.SendCommand(new ContractTypeDeleteCommand(ContractTypeId));

        return Ok(result);
    }
}
