using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IMediatorHandler mediator, IContextUser context, ILogger<DashboardController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("count-customer")]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new DasboardCountCustomer());
        return Ok(result);
    }

    [HttpGet("overview")]
    public async Task<IActionResult> OverView([FromQuery] DashboardRequest request)
    {
        var result = await _mediator.Send(new DasboardOverView(request.ToBaseQuery()));
        return Ok(result);
    }

    [HttpGet("selling-product")]
    public async Task<IActionResult> SellingProduct([FromQuery] DashboardRequest request)
    {
        var result = await _mediator.Send(new DasboardSellingProduct(request.ToBaseQuery()));
        return Ok(result);
    }

    [HttpGet("order-status-counter")]
    public async Task<IActionResult> OrderStatusCounter([FromQuery] DashboardRequest request)
    {
        var result = await _mediator.Send(new DasboardOrderStatusCounter(request.ToBaseQuery()));
        return Ok(result);
    }

    [HttpGet("customer-sale")]
    public async Task<IActionResult> CustomerSale([FromQuery] DashboardRequest request)
    {
        var result = await _mediator.Send(new DasboardCustomerSale(request.ToBaseQuery()));
        return Ok(result);
    }

    [HttpGet("store-sale")]
    public async Task<IActionResult> StoreSale([FromQuery] DashboardRequest request)
    {
        var result = await _mediator.Send(new DasboardStoreSale(request.ToBaseQuery()));
        return Ok(result);
    }

    [HttpGet("channel-sale")]
    public async Task<IActionResult> ChannelSale([FromQuery] DashboardRequest request)
    {
        var result = await _mediator.Send(new DasboardSalesChannelSale(request.ToBaseQuery()));
        return Ok(result);
    }

    [HttpGet("sales-product")]
    public async Task<IActionResult> SalesProduct([FromQuery] DashboardSaleProductTimeRequest request)
    {
        var result = await _mediator.Send(new DasboardSalesProcduct(request.Currency, request.Year, request.Type));
        return Ok(result);
    }

    [HttpGet("contract")]
    public async Task<IActionResult> Contract([FromQuery] DashboardRequest request)
    {
        var result = await _mediator.Send(new DasboardContract(request.ToBaseQuery()));
        return Ok(result);
    }

    [HttpGet("employee-excellent")]
    public async Task<IActionResult> SalesProduct([FromQuery] DashboardRequest request)
    {
        var result = await _mediator.Send(new DasboardEmployeeExcellent(request.ToBaseQuery()));
        return Ok(result);
    }

    [HttpGet("payment")]
    public async Task<IActionResult> Payment([FromQuery] DashboardRequest request)
    {
        var result = await _mediator.Send(new DasboardPayment(request.ToBaseQuery()));
        return Ok(result);
    }
}
