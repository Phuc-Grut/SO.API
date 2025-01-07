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
public partial class CustomerAddressController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<CustomerAddressController> _logger;

    public CustomerAddressController(IMediatorHandler mediator, IContextUser context, ILogger<CustomerAddressController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new CustomerAddressQueryComboBox(request.Status, request.GroupId));
        return Ok(result);
    }

    [HttpGet("get-by-customerid")]
    public async Task<IActionResult> GetCustomerAddress([FromQuery] CustomerAddressGetByCustomerIdRequest request)
    {
        var result = await _mediator.Send(new CustomerAddressQueryByCustomerId(request.Status, request.CustomerId));
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddCustomerAddressRequest request)
    {
        var CustomerAddressAddCommand = new CustomerAddressAddCommand(
          Guid.NewGuid(),
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
        ;
        var result = await _mediator.SendCommand(CustomerAddressAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditCustomerAddressRequest request)
    {
        var CustomerAddressId = new Guid(request.Id);

        var CustomerAddressEditCommand = new CustomerAddressEditCommand(
           CustomerAddressId,
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

        var result = await _mediator.SendCommand(CustomerAddressEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var CustomerAddressId = new Guid(id);
        if (await _mediator.Send(new CustomerAddressQueryById(CustomerAddressId)) == null)
            return BadRequest(new ValidationResult("CustomerAddress not exists"));

        var result = await _mediator.SendCommand(new CustomerAddressDeleteCommand(CustomerAddressId));

        return Ok(result);
    }
}
