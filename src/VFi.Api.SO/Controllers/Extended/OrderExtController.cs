using System.ComponentModel;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;

namespace VFi.Api.SO.Controllers;

public partial class OrderController : ControllerBase
{
    [HttpPost("ext-create")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderExtRequest request)
    {
        var orderId = Guid.NewGuid();
        var createdBy = _context.GetUserId();
        var code = await _mediator.Send(new GetCodeQuery(_codeSyntax.SO_Order, 1));
        var cmd = new CreateOrderCommand();
        cmd.Id = orderId;
        cmd.Code = code;
        if (request.AccountId.HasValue)
        {
            var queryCustomerId = new CustomerIdQueryByAccountId() { AccountId = request.AccountId.Value };
            cmd.CustomerId = await _mediator.Send(queryCustomerId);
        }
        if (request.CustomerId.HasValue)
        {
            cmd.CustomerId = request.CustomerId.Value;
        }

        cmd.StoreCode = request.StoreCode;
        cmd.ChannelCode = request.ChannelCode;
        cmd.CurrencyCode = request.CurrencyCode;
        cmd.ExchangeRate = request.ExchangeRate;
        cmd.PaymentTermCode = request.PaymentMethodCode;
        cmd.PaymentMethodCode = request.PaymentMethodCode;
        cmd.ShippingMethodCode = request.ShippingMethodCode;
        cmd.BuyFee = request.BuyFee;
        cmd.RouterShipping = request.RouterShipping;
        cmd.Products = ToDataTable(request.Products);
        cmd.Image = request.Image;
        cmd.Description = request.Description;
        cmd.Note = request.Note;
        cmd.CreatedBy = createdBy;
        cmd.CreatedByName = _context.FullName;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    protected DataTable ToDataTable<ProductRequest>(List<ProductRequest> data)
    {
        PropertyDescriptorCollection props =
            TypeDescriptor.GetProperties(typeof(ProductRequest));
        DataTable table = new DataTable();
        for (int i = 0; i < props.Count; i++)
        {
            PropertyDescriptor prop = props[i];
            table.Columns.Add(prop.Name, prop.PropertyType);
        }
        object[] values = new object[props.Count];
        foreach (var item in data)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = props[i].GetValue(item);
            }
            table.Rows.Add(values);
        }
        return table;
    }

    [HttpGet("counter-by-accountid")]
    public async Task<IActionResult> Counter(Guid accountId)
    {
        var queryCustomerId = new CustomerIdQueryByAccountId() { AccountId = accountId };
        var customerId = await _mediator.Send(queryCustomerId);
        var query = new OrderCountByCustomerId(customerId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("top-by-accountid")]
    public async Task<IActionResult> TopByAccountId(Guid accountId)
    {
        var queryCustomerId = new CustomerIdQueryByAccountId() { AccountId = accountId };
        var customerId = await _mediator.Send(queryCustomerId);
        var query = new TopOrderByCustomerIdQuery(customerId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
