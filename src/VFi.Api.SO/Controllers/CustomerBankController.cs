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
public partial class CustomerBankController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<CustomerBankController> _logger;

    public CustomerBankController(IMediatorHandler mediator, IContextUser context, ILogger<CustomerBankController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new CustomerBankQueryComboBox(request.Status, request.GroupId));
        return Ok(result);
    }

    [HttpGet("get-by-customerid")]
    public async Task<IActionResult> GetCustomerBank([FromQuery] CustomerBankGetByCustomerIdRequest request)
    {
        var result = await _mediator.Send(new CustomerBankQueryByCustomerId(request.Status, request.CustomerId));
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddCustomerBankRequest request)
    {
        var CustomerBankAddCommand = new CustomerBankAddCommand(
          Guid.NewGuid(),
          request.CustomerId.Value,
          request.Name,
          request.BankCode,
          request.BankName,
          request.BankBranch,
          request.AccountName,
          request.AccountNumber,
          request.Default,
          request.Status,
          request.SortOrder,
          _context.GetUserId(),
          _context.UserClaims.FullName
      );
        var result = await _mediator.SendCommand(CustomerBankAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditCustomerBankRequest request)
    {
        var cmd = new CustomerBankEditCommand(
           (Guid)request.Id,
           request.CustomerId.Value,
           request.Name,
           request.BankCode,
           request.BankName,
           request.BankBranch,
           request.AccountName,
           request.AccountNumber,
           request.Default,
           request.Status,
           request.SortOrder,
           _context.GetUserId(),
          _context.UserClaims.FullName
       );

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (await _mediator.Send(new CustomerBankQueryById(id)) == null)
            return BadRequest(new ValidationResult("CustomerBank not exists"));

        var result = await _mediator.SendCommand(new CustomerBankDeleteCommand(id));

        return Ok(result);
    }
}
