using System.ComponentModel;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VFi.Api.SO.ViewModels.Extended;
using VFi.Application.SO.Commands.Extended;
using VFi.Application.SO.Commands.Extended.OrderCross;
using VFi.Application.SO.DTOs.Extended;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Configuration;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using VFi.NetDevPack.Queries;

namespace VFi.Api.SO.Controllers.Extended;

[Route("api/[controller]")]
[ApiController]
public class OrderCrossController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger _logger;
    private readonly CodeSyntaxConfig _codeSyntax;

    public OrderCrossController(
        IMediatorHandler mediator,
        IContextUser context,
        ILogger<OrderCrossController> logger,
        CodeSyntaxConfig codeSyntax)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
        _codeSyntax = codeSyntax;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCrossOrder([FromBody] CreateOrderCrossRequest request, CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();
        var createdBy = _context.GetUserId();
        var createdByName = _context.FullName;
        //var code = await _mediator.Send(new GetCodeQuery(_codeSyntax.SO_Order, 1), cancellationToken);
        var cmd = new CreateOrderCrossCommand();
        cmd.Id = orderId;
        //cmd.Code = code;

        if (request.AccountId.HasValue)
        {
            var queryCustomerId = new CustomerIdQueryByAccountId() { AccountId = request.AccountId.Value };
            cmd.CustomerId = await _mediator.Send(queryCustomerId, cancellationToken);
        }
        if (request.CustomerId.HasValue)
        {
            cmd.CustomerId = request.CustomerId.Value;
        }
        cmd.PurchaseGroup = request.PurchaseGroup;
        cmd.OrderType = request.OrderType;
        cmd.StoreCode = request.StoreCode;
        cmd.ChannelCode = request.ChannelCode;
        cmd.CurrencyCode = request.CurrencyCode;
        cmd.ExchangeRate = request.ExchangeRate;
        cmd.PaymentTermCode = request.PaymentMethodCode;
        cmd.PaymentMethodCode = request.PaymentMethodCode;
        cmd.ShippingMethodCode = request.ShippingMethodCode;
        cmd.BuyFee = request.BuyFee;
        cmd.RouterShipping = request.RouterShipping;
        cmd.DomesticShipping = request.DomesticShipping;
        cmd.DeliveryCountry = request.DeliveryCountry;
        cmd.DeliveryProvince = request.DeliveryProvince;
        cmd.DeliveryDistrict = request.DeliveryDistrict;
        cmd.DeliveryWard = request.DeliveryWard;
        cmd.DeliveryAddress = request.DeliveryAddress;
        cmd.DeliveryName = request.DeliveryName;
        cmd.DeliveryPhone = request.DeliveryPhone;
        cmd.DeliveryNote = request.DeliveryNote;

        cmd.TotalPay = request.TotalPay;
        cmd.PayNow = request.PayNow;

        cmd.ServiceAdd = ToDataTable(request.ServiceAdd);

        cmd.Products = ToDataTable(request.Products);

        cmd.Image = request.Image;
        cmd.Images = request.Images;
        cmd.Description = request.Description;
        cmd.Note = request.Note;
        cmd.CreatedBy = createdBy;
        cmd.CreatedByName = _context.FullName;
        var result = await _mediator.SendCommandResult(cmd, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// User lấy danh sách đơn hàng theo phân trang
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(PagedResult<List<OrderCrossDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Get orders by user")]
    [HttpGet("paging")]
    public async Task<PagedResult<List<OrderCrossDto>>> Paging([FromQuery] OrderCrossPagingQuery request)
    {
        var result = await _mediator.Send(request);
        return result;
    }

    /// <summary>
    /// User lấy đơn hàng theo code
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(OrderCrossDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Get order by user and order code")]
    [HttpGet("code")]
    public async Task<OrderCrossDto> GetByCode(Guid accountId, string code)
    {
        var query = new OrderCrossByCodeQuery(accountId, code);
        var result = await _mediator.Send(query);
        return result;
    }

    /// <summary>
    /// Lấy thông tin đơn hàng theo danh sách code
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="orderCode"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(IList<OrderInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Get order by user and order code")]
    [HttpPost("info-by-code")]
    public async Task<IList<OrderInfoDto>?> GetByCode([FromBody] OrderInfoRequest request)
    {
        var query = new OrderInfoQuery(request.AccountId, request.OrderCode);
        var result = await _mediator.Send(query);
        return result;
    }

    [HttpPost("create-payment-invoice")]
    public async Task<IActionResult> CreateOrderPaymentInvoice([FromBody] CreateOrderPaymentInvoiceRequest request, CancellationToken cancellationToken)
    {
        var createdBy = _context.GetUserId();
        var createdByName = _context.FullName;
        var cmd = new CreateInvoicePayOrderCrossCommand();
        cmd.Id = request.Id;
        cmd.TotalPay = request.TotalPay;
        cmd.AccountId = request.AccountId;
        cmd.CreatedBy = createdBy;
        cmd.CreatedByName = createdByName;
        var result = await _mediator.SendCommand(cmd, cancellationToken);
        return Ok(result);
    }

    [HttpPut("update-address")]
    public async Task<IActionResult> OrderUpdateAddress([FromBody] OrderUpdateAddressRequest request, CancellationToken cancellationToken)
    {
        var updatedBy = _context.GetUserId();
        var updatedByName = _context.FullName;

        var cmd = new OrderCrossUpdateAddressCommand();
        cmd.Id = request.Id;
        cmd.DeliveryCountry = request.DeliveryCountry;
        cmd.DeliveryProvince = request.DeliveryProvince;
        cmd.DeliveryDistrict = request.DeliveryDistrict;
        cmd.DeliveryWard = request.DeliveryWard;
        cmd.DeliveryAddress = request.DeliveryAddress;
        cmd.DeliveryName = request.DeliveryName;
        cmd.DeliveryPhone = request.DeliveryPhone;
        cmd.DeliveryNote = request.DeliveryNote;
        cmd.UpdatedBy = updatedBy;
        cmd.UpdatedByName = updatedByName;

        var result = await _mediator.SendCommand(cmd, cancellationToken);
        return Ok(result);
    }

    [HttpPut("update-bid-username")]
    public async Task<IActionResult> OrderUpdateBidUsername([FromBody] OrderUpdateBidUsernameRequest request, CancellationToken cancellationToken)
    {
        var updatedBy = _context.GetUserId();
        var updatedByName = _context.FullName;

        var cmd = new OrderCrossUpdateBidUsernameCommand();
        cmd.Id = request.Id;
        cmd.BidUsername = request.BidUsername;
        cmd.UpdatedBy = updatedBy;
        cmd.UpdatedByName = updatedByName;

        var result = await _mediator.SendCommand(cmd, cancellationToken);
        return Ok(result);
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> OrderCancel([FromBody] OrderCancelRequest request, CancellationToken cancellationToken)
    {
        var updatedBy = _context.GetUserId();
        var updatedByName = _context.FullName;

        var cmd = new OrderCancelCommand();
        cmd.Id = request.Id;
        cmd.IsPayFee = request.IsPayFee;
        cmd.UpdatedBy = updatedBy;
        cmd.UpdatedByName = updatedByName;

        var result = await _mediator.SendCommand(cmd, cancellationToken);
        return Ok(result);
    }

    [HttpPost("services")]
    public async Task<IActionResult> AddService([FromBody] OrderCrossAddServiceRequest request, CancellationToken cancellationToken)
    {
        var updatedBy = _context.GetUserId();
        var updatedByName = _context.FullName;

        var cmd = new OrderCrossAddServiceCommand();
        cmd.Id = request.Id;
        cmd.ServiceAdd = ToDataTable(request.ServiceAdd);
        cmd.UpdatedBy = updatedBy;
        cmd.UpdatedByName = updatedByName;

        var result = await _mediator.SendCommand(cmd, cancellationToken);
        return Ok(result);
    }

    [HttpPut("update-tracking")]
    public async Task<IActionResult> UpdateTrackingOrder([FromBody] UpdateTrackingOrderRequest request, CancellationToken cancellationToken)
    {
        var updatedBy = _context.GetUserId();
        var updatedByName = _context.FullName;

        var cmd = new UpdateTrackingOrderCommand();
        cmd.Id = request.Id;
        cmd.DomesticTracking = request.DomesticTracking;
        cmd.DomesticCarrier = request.DomesticCarrier;
        cmd.DomesticStatus = request.DomesticStatus;
        cmd.UpdatedBy = updatedBy;
        cmd.UpdatedByName = updatedByName;

        var result = await _mediator.SendCommand(cmd, cancellationToken);
        return Ok(result);
    }

    [HttpPut("update-shipment-routing")]
    public async Task<IActionResult> UpdateShipmentRouting([FromBody] UpdateShipmentRoutingOrderRequest request, CancellationToken cancellationToken)
    {
        var updatedBy = _context.GetUserId();
        var updatedByName = _context.FullName;

        var cmd = new UpdateShippmentRoutingOrderCommand();
        cmd.Id = request.Id;
        cmd.ShipmentRouting = request.ShipmentRouting;
        cmd.UpdatedBy = updatedBy;
        cmd.UpdatedByName = updatedByName;

        var result = await _mediator.SendCommand(cmd, cancellationToken);
        return Ok(result);
    }

    [HttpPut("update-internal-note")]
    public async Task<IActionResult> UpdateInternalNote([FromBody] UpdateOrderInternalNoteRequest request, CancellationToken cancellationToken)
    {
        var updatedBy = _context.GetUserId();
        var updatedByName = _context.FullName;

        var cmd = new UpdateInternalNoteCommand();
        cmd.Id = request.Id;
        cmd.InternalNote = request.InternalNote;
        cmd.UpdatedBy = updatedBy;
        cmd.UpdatedByName = updatedByName;

        var result = await _mediator.SendCommand(cmd, cancellationToken);
        return Ok(result);
    }

    [HttpPost("confirm-delivered")]
    public async Task<IActionResult> ConfirmDelivered([FromBody] OrderConfirmDeliveredCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendCommand(request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(OrderCountAuctionUnpaidDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Get order auction unpaid count by user")]
    [HttpGet("count-auction-unpaid")]
    public async Task<OrderCountAuctionUnpaidDto> OrderCountAuctionUnpaid([FromQuery] Guid accountId, CancellationToken cancellationToken)
    {
        var query = new OrderCountAuctionUnpaidQuery(accountId);
        var result = await _mediator.Send(query, cancellationToken);
        return result;
    }

    private DataTable ToDataTable<TType>(List<TType> data)
    {
        var props =
            TypeDescriptor.GetProperties(typeof(TType));
        var table = new DataTable();
        for (var i = 0; i < props.Count; i++)
        {
            var prop = props[i];
            table.Columns.Add(prop.Name, prop.PropertyType);
        }
        var values = new object[props.Count];
        foreach (var item in data)
        {
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = props[i].GetValue(item);
            }
            table.Rows.Add(values);
        }
        return table;
    }
}
