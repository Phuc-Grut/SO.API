using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers.Extended;

[Route("api/[controller]")]
[ApiController]
public class PostOfficeController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<PostOfficeController> _logger;

    public PostOfficeController(IMediatorHandler mediator, IContextUser context, ILogger<PostOfficeController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> GetListBox([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new PostOfficeQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _mediator.Send(new PostOfficeQueryById(id));
        return Ok(result);
    }

    [HttpGet("get-my-address/{code}")]
    public async Task<IActionResult> GetMyAddress(string code)
    {
        var query = new MyPostOfficeQueryByCode(code);
        query.AccountId = _context.UserId;
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("get-by-code/{code}")]
    public async Task<IActionResult> Get(string code)
    {
        var result = await _mediator.Send(new PostOfficeQueryByCode(code));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PostOfficeRequest request)
    {
        var query = new PostOfficePagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddPostOfficeRequest request)
    {
        var PostOfficeAddCommand = new PostOfficeAddCommand(
          Guid.NewGuid(),
           request.Code,
           request.Name,
           request.ShortName,
           request.Country,
           request.Address,
           request.Address1,
           request.PostCode,
           request.Phone,
           request.SyntaxSender,
           request.Note,
           request.Status
      );
        var result = await _mediator.SendCommand(PostOfficeAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditPostOfficeRequest request)
    {
        var PostOfficeEditCommand = new PostOfficeEditCommand(
           request.Id,
           request.Code,
           request.Name,
           request.ShortName,
           request.Country,
           request.Address,
           request.Address1,
           request.PostCode,
           request.Phone,
           request.SyntaxSender,
           request.Note,
           request.Status
       );

        var result = await _mediator.SendCommand(PostOfficeEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new PostOfficeSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        var PostOfficeId = new Guid(id);
        if (await _mediator.Send(new PostOfficeQueryById(PostOfficeId)) == null)
            return BadRequest(new ValidationResult("PostOffice not exists"));

        var result = await _mediator.SendCommand(new PostOfficeDeleteCommand(PostOfficeId));

        return Ok(result);
    }
}
