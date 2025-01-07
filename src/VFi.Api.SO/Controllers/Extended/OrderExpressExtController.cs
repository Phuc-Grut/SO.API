using System.ComponentModel;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.Commands.Extended;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.Application.SO.Queries.Extended;

namespace VFi.Api.SO.Controllers;

public partial class OrderExpressController : ControllerBase
{
    //[HttpPost("ext-create")]
    //public async Task<IActionResult> CreateOrder([FromBody] CreateOrderExtRequest request)
    //{
    //    var orderId = Guid.NewGuid();
    //    var createdBy = _context.GetUserId();
    //    var code = await _mediator.Send(new GetCodeQuery(_codeSyntax.SO_Order, 1));
    //    var cmd = new CreateOrderCommand();
    //    cmd.Id = orderId;
    //    cmd.Code = code;
    //    if (request.AccountId.HasValue)
    //    {
    //        var queryCustomerId = new CustomerIdQueryByAccountId() { AccountId = request.AccountId.Value };
    //        cmd.CustomerId = await _mediator.Send(queryCustomerId);
    //    }
    //    if (request.CustomerId.HasValue)
    //    {
    //        cmd.CustomerId = request.CustomerId.Value;
    //    }

    //    cmd.StoreCode = request.StoreCode;
    //    cmd.ChannelCode = request.ChannelCode;
    //    cmd.CurrencyCode = request.CurrencyCode;
    //    cmd.ExchangeRate = request.ExchangeRate;
    //    cmd.PaymentTermCode = request.PaymentMethodCode;
    //    cmd.PaymentMethodCode = request.PaymentMethodCode;
    //    cmd.ShippingMethodCode = request.ShippingMethodCode;
    //    cmd.BuyFee = request.BuyFee;
    //    cmd.RouterShipping = request.RouterShipping;
    //    cmd.Products = ToDataTable(request.Products);
    //    cmd.Image = request.Image;
    //    cmd.Description = request.Description;
    //    cmd.Note = request.Note;
    //    cmd.CreatedBy = createdBy;
    //    cmd.CreatedByName = _context.FullName;
    //    var result = await _mediator.SendCommand(cmd);
    //    return Ok(result);
    //}

    [HttpPost("create-2")]
    public async Task<IActionResult> Add([FromBody] AddOrderExpressByCustomerRequest request)
    {
        var cmd = new OrderExpressAddByCustomerCommand(Guid.NewGuid(),
                                                       request.Code,
                                                       null,
                                                       request.StoreCode,
                                                       request.CurrencyCode,
                                                       request.ShippingMethodCode,
                                                       request.RouterShipping,
                                                       request.TrackingCode,
                                                       request.TrackingCarrier,
                                                       request.Weight,
                                                       request.Width,
                                                       request.Height,
                                                       request.Length,
                                                       request.DeliveryCountry,
                                                       request.DeliveryProvince,
                                                       request.DeliveryDistrict,
                                                       request.DeliveryWard,
                                                       request.DeliveryAddress,
                                                       request.DeliveryName,
                                                       request.DeliveryPhone,
                                                       request.DeliveryNote,
                                                       ToDataTable(request.Products),
                                                       ToDataTable(request.ServiceAdd),
                                                       request.Image,
                                                       request.Images,
                                                       request.Description,
                                                       request.Note);

        Guid? customerId = null;
        if (request.AccountId.HasValue)
        {
            var queryCustomerId = new CustomerIdQueryByAccountId() { AccountId = request.AccountId.Value };
            customerId = await _mediator.Send(queryCustomerId);
        }
        if (customerId.HasValue)
        {
            cmd.CustomerId = customerId.Value;
        }
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    protected DataTable ToDataTable<IType>(List<IType> data)
    {
        PropertyDescriptorCollection props =
            TypeDescriptor.GetProperties(typeof(IType));
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
        var query = new OrderExpressCountByCustomerId(customerId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("paging-by-account")]
    public async Task<IActionResult> PagingByAccount([FromQuery] OrderExpressByAccountPagingRequest request)
    {
        var query = new OrderExpressPagingByAccountQuery(request.Keyword ?? "",
                                                         request.AccountId,
                                                         request.Filter ?? "",
                                                         request.Status,
                                                         request.Order ?? "",
                                                         request.PageNumber,
                                                         request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("create-payment-invoice")]
    public async Task<IActionResult> CreateOrderPaymentInvoice([FromBody] CreateOrderExpressPaymentInvoiceRequest request, CancellationToken cancellationToken)
    {
        var createdBy = _contextUser.GetUserId();
        var createdByName = _contextUser.FullName;
        var cmd = new CreateInvoicePayOrderExpressCommand();
        cmd.Id = request.Id;
        cmd.TotalPay = request.TotalPay;
        cmd.AccountId = request.AccountId;
        cmd.CreatedBy = createdBy;
        cmd.CreatedByName = createdByName;
        var result = await _mediator.SendCommand(cmd, cancellationToken);
        return Ok(result);
    }
}
