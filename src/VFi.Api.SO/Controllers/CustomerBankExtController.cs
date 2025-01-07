using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;

namespace VFi.Api.SO.Controllers;

public partial class CustomerBankController : ControllerBase
{
    [HttpGet("get-by-accountid")]
    public async Task<IActionResult> GetBankByAccountId(Guid accountid, int? status)
    {
        try
        {
            var result = await _mediator.Send(new CustomerBankQueryByAccountId(status, accountid));
            return Ok(result);
        }
        catch (Exception)
        {
            return Ok();
        }
    }

    [HttpGet("get-by-accountlogin")]
    public async Task<IActionResult> GetBankByAccount(int? status)
    {
        try
        {
            var accountid = _context.GetUserId();
            var result = await _mediator.Send(new CustomerBankQueryByAccountId(status, accountid));
            return Ok(result);
        }
        catch (Exception)
        {
            return Ok();
        }
    }

    [HttpPost("add-by-account")]
    public async Task<IActionResult> AddByAccount([FromBody] AddCustomerBankRequest request)
    {
        try
        {
            var accountid = request.AccountId.Value;
            var customer = await _mediator.Send(new CustomerQueryByAccountId(accountid));
            var id = Guid.NewGuid();
            var cmd = new CustomerBankAddCommand(
              id,
              customer.Id,
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
        catch (Exception ex)
        {
            return Ok(new FluentValidation.Results.ValidationResult()
            {
                Errors = new List<ValidationFailure>() {
                    new ValidationFailure("", ex.Message) }
            });
        }
    }

    [HttpPost("edit-by-account")]
    public async Task<IActionResult> EditByAccountId([FromBody] EditCustomerBankRequest request)
    {
        var accountid = request.AccountId.Value;
        var customer = await _mediator.Send(new CustomerQueryByAccountId(accountid));
        if (!customer.Id.Equals(request.CustomerId))
            return BadRequest(new System.ComponentModel.DataAnnotations.ValidationResult("Bank not exists"));

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

    [HttpDelete("delete-by-account/{id}")]
    public async Task<IActionResult> DeleteByAccount(Guid id, Guid accountid)
    {
        var customerBank = await _mediator.Send(new CustomerBankQueryById(id));
        if (customerBank == null)
            return BadRequest(new System.ComponentModel.DataAnnotations.ValidationResult("Bank not exists"));
        var customer = await _mediator.Send(new CustomerQueryByAccountId(accountid));
        if (!customer.Id.Equals(customerBank.CustomerId))
            return BadRequest(new System.ComponentModel.DataAnnotations.ValidationResult("Bank not exists"));

        var result = await _mediator.SendCommand(new CustomerBankDeleteCommand(id));

        return Ok(result);
    }
}
