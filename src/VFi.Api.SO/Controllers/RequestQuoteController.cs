using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequestQuoteController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<RequestQuoteController> _logger;

    public RequestQuoteController(IMediatorHandler mediator, IContextUser context, ILogger<RequestQuoteController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new RequestQuoteQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] RequestQuoteListBoxRequest request)
    {
        var result = await _mediator.Send(new RequestQuoteQueryComboBox(request.ToBaseQuery()));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] RequestQuotePagingRequest request)
    {
        if (_context.QueryMyData())
        {
            request.EmployeeId = _context.GetUserId().ToString();
        }
        var query = new RequestQuotePagingQuery(
              request.Keyword ?? "",
              request.Status,
              request.EmployeeId,
              request.Filter ?? "",
              request.Order ?? "",
              request.PageNumber,
              request.PageSize
          );

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddRequestQuoteRequest request)
    {
        var Code = request.Code;
        if (request.IsAuto == true)
        {
            Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, 1));
        }
        else
        {
            var useCodeCommand = new UseCodeCommand(
                request.ModuleCode,
                Code
                );
            _mediator.SendCommand(useCodeCommand);
        }
        var cmd = new RequestQuoteAddCommand(
          Guid.NewGuid(),
          Code,
          request.Name,
          request.Description,
          request.RequestDate,
          request.DueDate,
          String.IsNullOrEmpty(request.StoreId) ? null : new Guid(request.StoreId),
          request.StoreCode,
          request.StoreName,
          String.IsNullOrEmpty(request.CustomerId) ? null : new Guid(request.CustomerId),
          request.CustomerCode,
          request.CustomerName,
          request.Phone,
          request.Email,
          request.Address,
          String.IsNullOrEmpty(request.EmployeeId) ? null : new Guid(request.EmployeeId),
          request.EmployeeName,
          request.Note,
          request.Status,
          request.ChannelId,
          request.ChannelCode,
          request.ChannelName
      );
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditRequestQuoteRequest request)
    {
        var cmd = new RequestQuoteEditCommand(
           new Guid(request.Id),
           request.Code,
           request.Name,
           request.Description,
           request.RequestDate,
           request.DueDate,
           String.IsNullOrEmpty(request.StoreId) ? null : new Guid(request.StoreId),
           request.StoreCode,
           request.StoreName,
           String.IsNullOrEmpty(request.CustomerId) ? null : new Guid(request.CustomerId),
           request.CustomerCode,
           request.CustomerName,
           request.Phone,
           request.Email,
           request.Address,
           String.IsNullOrEmpty(request.EmployeeId) ? null : new Guid(request.EmployeeId),
           request.EmployeeName,
           request.Note,
           request.Status,
           request.ChannelId,
           request.ChannelCode,
           request.ChannelName
       );

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var RequestQuoteId = new Guid(id);
        if (await _mediator.Send(new RequestQuoteQueryById(RequestQuoteId)) == null)
            return BadRequest(new ValidationResult("RequestQuote not exists"));

        var result = await _mediator.SendCommand(new RequestQuoteDeleteCommand(RequestQuoteId));

        return Ok(result);
    }

    [HttpPost("update-status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusRequestQuoteRequest request)
    {
        var data = new UpdateStatusRequestQuoteCommand(
            request.Id,
            request.Status
            );

        var result = await _mediator.SendCommand(data);

        return Ok(result);
    }
}
