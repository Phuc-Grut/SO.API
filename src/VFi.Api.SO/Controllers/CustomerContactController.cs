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
public class CustomerContactController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<CustomerContactController> _logger;

    public CustomerContactController(IMediatorHandler mediator, IContextUser context, ILogger<CustomerContactController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new CustomerContactQueryComboBox(request.Status, request.GroupId));
        return Ok(result);
    }

    [HttpGet("get-by-customerid")]
    public async Task<IActionResult> GetCustomerContact([FromQuery] CustomerContactGetByCustomerIdRequest request)
    {
        var result = await _mediator.Send(new CustomerContactQueryByCustomerId(request.Status, request.CustomerId));
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddCustomerContactRequest request)
    {
        var CustomerContactAddCommand = new CustomerContactAddCommand(
          Guid.NewGuid(),
          request.CustomerId,
          request.Name,
          request.Gender,
          request.Phone,
          request.Email,
          request.Facebook,
          request.Tags,
          request.Address,
          request.Status,
          request.SortOrder,
          _context.GetUserId(),
          _context.UserClaims.FullName
      );
        var result = await _mediator.SendCommand(CustomerContactAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditCustomerContactRequest request)
    {
        var CustomerContactId = new Guid(request.Id);

        var CustomerContactEditCommand = new CustomerContactEditCommand(
           CustomerContactId,
           request.CustomerId,
           request.Name,
           request.Gender,
           request.Phone,
           request.Email,
           request.Facebook,
           request.Tags,
           request.Address,
           request.Status,
           request.SortOrder,
           _context.GetUserId(),
          _context.UserClaims.FullName
       );

        var result = await _mediator.SendCommand(CustomerContactEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var CustomerContactId = new Guid(id);
        if (await _mediator.Send(new CustomerContactQueryById(CustomerContactId)) == null)
            return BadRequest(new ValidationResult("CustomerContact not exists"));

        var result = await _mediator.SendCommand(new CustomerContactDeleteCommand(CustomerContactId));

        return Ok(result);
    }
}
