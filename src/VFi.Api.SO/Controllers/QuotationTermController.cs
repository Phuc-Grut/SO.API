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
public class QuotationTermController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<QuotationTermController> _logger;

    public QuotationTermController(IMediatorHandler mediator, IContextUser context, ILogger<QuotationTermController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new QuotationTermQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] QuotationTermRequest request)
    {
        var query = new QuotationTermPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddQuotationTermRequest request)
    {
        var QuotationTermAddCommand = new QuotationTermAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.DisplayOrder,
          request.Status
      );
        var result = await _mediator.SendCommand(QuotationTermAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditQuotationTermRequest request)
    {
        var QuotationTermId = new Guid(request.Id);
        QuotationTermDto dataQuotationTerm = await _mediator.Send(new QuotationTermQueryById(QuotationTermId));

        if (dataQuotationTerm == null)
            return BadRequest(new ValidationResult("QuotationTerm not exists"));

        var QuotationTermEditCommand = new QuotationTermEditCommand(
           QuotationTermId,
           request.Code,
           request.Name,
           request.Description,
           request.DisplayOrder,
           request.Status
       );

        var result = await _mediator.SendCommand(QuotationTermEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new QuotationTermSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        var QuotationTermId = new Guid(id);
        if (await _mediator.Send(new QuotationTermQueryById(QuotationTermId)) == null)
            return BadRequest(new ValidationResult("QuotationTerm not exists"));

        var result = await _mediator.SendCommand(new QuotationTermDeleteCommand(QuotationTermId));

        return Ok(result);
    }
}
