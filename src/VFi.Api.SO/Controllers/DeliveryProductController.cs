using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveryProductController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<DeliveryProductController> _logger;

    public DeliveryProductController(IMediatorHandler mediator, IContextUser context, ILogger<DeliveryProductController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-by-list-orderproductid/{id}")]
    public async Task<IActionResult> GetByOrderId(string id)
    {
        var query = new DeliveryProductGetByOrder(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add-range")]
    public async Task<IActionResult> Add([FromBody] AddRangeDeliveryProductRequest request)
    {
        var data = new DeliveryProductAddRangeCommand(request.ListGuidDeliveryProduct, request.List);
        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddRange([FromBody] AddDeliveryProductRequest request)
    {
        var data = new DeliveryProductAddCommand(
                                                Guid.NewGuid(),
                                                request.OrderProductId,
                                                request.Code,
                                                request.Name,
                                                request.Description,
                                                request.Status,
                                                request.QuantityExpected,
                                                request.DeliveryDate,
                                                request.DisplayOrder
                                                );
        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditDeliveryProductRequest request)
    {
        var deliveryProductEditCommand = new DeliveryProductEditCommand(request.Id,
                                                                        request.OrderProductId,
                                                                        request.Code,
                                                                        request.Name,
                                                                        request.Description,
                                                                        request.Status,
                                                                        request.QuantityExpected,
                                                                        request.DeliveryDate,
                                                                        request.DisplayOrder);

        var result = await _mediator.SendCommand(deliveryProductEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.SendCommand(new DeliveryProductDeleteCommand(id));

        return Ok(result);
    }
}
