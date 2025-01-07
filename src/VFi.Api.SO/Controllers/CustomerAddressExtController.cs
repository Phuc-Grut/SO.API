using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;

namespace VFi.Api.SO.Controllers;

public partial class CustomerAddressController : ControllerBase
{
    [HttpGet("get-by-accountid")]
    public async Task<IActionResult> GetAddressByAccountId(Guid accountid, int? status)
    {
        try
        {
            var result = await _mediator.Send(new CustomerAddressQueryByAccountId(status, accountid));
            return Ok(result);
        }
        catch (Exception)
        {
            return Ok();
        }
    }

    [HttpGet("get-by-accountlogin")]
    public async Task<IActionResult> GetAddressByAccount(int? status)
    {
        try
        {
            var accountid = _context.GetUserId();
            var result = await _mediator.Send(new CustomerAddressQueryByAccountId(status, accountid));
            return Ok(result);
        }
        catch (Exception)
        {
            return Ok();
        }
    }

    [HttpPost("add-by-account")]
    public async Task<IActionResult> AddByAccount([FromBody] AddCustomerAddressRequest request)
    {
        try
        {
            var accountid = request.AccountId.Value;
            var customer = await _mediator.Send(new CustomerQueryByAccountId(accountid));
            var id = Guid.NewGuid();
            var cmd = new CustomerAddressAddCommand(
              id,
              customer.Id,
              request.Name,
              request.Country,
              request.Province,
              request.District,
              request.Ward,
              request.Address,
              request.Phone,
              request.Email,
              request.ShippingDefault,
              request.BillingDefault,
              request.Status,
              request.SortOrder,
              _context.GetUserId(),
              _context.UserClaims.FullName
          );
            ;
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
    public async Task<IActionResult> EditByAccountId([FromBody] EditCustomerAddressRequest request)
    {
        var accountid = request.AccountId.Value;
        var customer = await _mediator.Send(new CustomerQueryByAccountId(accountid));
        if (!customer.Id.Equals(request.CustomerId))
        {
            return Ok(new FluentValidation.Results.ValidationResult()
            {
                Errors = new List<ValidationFailure>() {
                    new ValidationFailure("AccountId","Customer info not exists") }
            });
        }
        var customerAddressId = new Guid(request.Id);

        var cmd = new CustomerAddressEditCommand(
           customerAddressId,
           request.CustomerId,
           request.Name,
           request.Country,
           request.Province,
           request.District,
           request.Ward,
           request.Address,
           request.Phone,
           request.Email,
           request.ShippingDefault,
           request.BillingDefault,
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
        var customerAddress = await _mediator.Send(new CustomerAddressQueryById(id));
        if (customerAddress == null)
        {
            return Ok(new FluentValidation.Results.ValidationResult()
            {
                Errors = new List<ValidationFailure>() {
                    new ValidationFailure("Id","Customer address not exists") }
            });
        }
        var customer = await _mediator.Send(new CustomerQueryByAccountId(accountid));
        if (!customer.Id.Equals(customerAddress.CustomerId))
        {
            return Ok(new FluentValidation.Results.ValidationResult()
            {
                Errors = new List<ValidationFailure>() {
                    new ValidationFailure("AccountId","Customer info not exists") }
            });
        }

        var result = await _mediator.SendCommand(new CustomerAddressDeleteCommand(id));

        return Ok(result);
    }
}
