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
public class ContractTermController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ContractTermController> _logger;

    public ContractTermController(IMediatorHandler mediator, IContextUser context, ILogger<ContractTermController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new ContractTermQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] ContractTermRequest request)
    {
        var query = new ContractTermPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddContractTermRequest request)
    {
        var ContractTermAddCommand = new ContractTermAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.Status
      );
        var result = await _mediator.SendCommand(ContractTermAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditContractTermRequest request)
    {
        var ContractTermId = new Guid(request.Id);
        ContractTermDto dataContractTerm = await _mediator.Send(new ContractTermQueryById(ContractTermId));

        if (dataContractTerm == null)
            return BadRequest(new ValidationResult("ContractTerm not exists"));

        var ContractTermEditCommand = new ContractTermEditCommand(
           ContractTermId,
           request.Code,
           request.Name,
           request.Description,
           request.Status
       );

        var result = await _mediator.SendCommand(ContractTermEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new ContractTermSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        var ContractTermId = new Guid(id);
        if (await _mediator.Send(new ContractTermQueryById(ContractTermId)) == null)
            return BadRequest(new ValidationResult("ContractTerm not exists"));

        var result = await _mediator.SendCommand(new ContractTermDeleteCommand(ContractTermId));

        return Ok(result);
    }
}
