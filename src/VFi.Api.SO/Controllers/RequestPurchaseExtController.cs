using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;

namespace VFi.Api.SO.Controllers;

public partial class RequestPurchaseController : ControllerBase
{
    [HttpPost("purchase")]
    public async Task<IActionResult> Purchase([FromBody] PurchaseRequestPurchase request)
    {
        var cmd = new RequestPurchasePurchaseCommand(
            request.Id,
            request.POStatus
            );

        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("update-quantity-purchased")]
    public async Task<IActionResult> UpdateQuantityPurchased([FromBody] PurchaseRequestUpdateQuantityRequest request)
    {
        var listUpdate = request.ListUpdate?.Select(x => new POPurchaseProductDto()
        {
            StatusPurchase = x.StatusPurchase,
            ProductId = x.ProductId,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            Quantity = x.Quantity
        }).ToList();

        var cmd = new POPurchaseProductCommand(
            listUpdate
            );

        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("update-purchase-qty")]
    public async Task<IActionResult> UpdatePurchaseQty([FromBody] UpdatePurchaseQtyRequest request)
    {
        var cmd = new UpdatePurchaseQtyCommand(
            request.NotCancel,
            request.Id,
            request.PurchaseRequestCode
            );
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }
}
