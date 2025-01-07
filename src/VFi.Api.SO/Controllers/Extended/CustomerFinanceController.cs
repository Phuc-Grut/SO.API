using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;

namespace VFi.Api.SO.Controllers;

public partial class CustomerController : ControllerBase
{
    [HttpPut("update-finance/{id}")]
    public async Task<IActionResult> UpdateFinance(Guid id, [FromBody] UpdateFinanceExCustomerRequest request)
    {
        var detail = request.CustomerPriceListCross.Select(x => new CustomerPriceListCrossDto()
        {
            Id = x.Id ?? Guid.NewGuid(),
            PriceListCrossId = x.PriceListCrossId,
            PriceListCrossName = x.PriceListCrossName,
            RouterShippingId = x.RouterShippingId,
            RouterShipping = x.RouterShipping
        });

        var CustomerEditCommand = new CustomerExUpdateFinanceCommand(id,
                                                                   request.PriceListPurchaseId,
                                                                   request.PriceListPurchaseName,
                                                                   request.CurrencyId,
                                                                   request.Currency,
                                                                   request.CurrencyName,
                                                                   request.PriceListId,
                                                                   request.PriceListName,
                                                                   request.DebtLimit,
                                                                   request.RemainingDebt,
                                                                   detail);

        var result = await _mediator.SendCommand(CustomerEditCommand);

        return Ok(result);
    }

    [HttpGet("get-finance/{id}")]
    public async Task<IActionResult> GetFinance(Guid id)
    {
        var query = new CustomerFinaceQuery() { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("update-id-info/{id}")]
    public async Task<IActionResult> ActiveTran(Guid id, [FromBody] UpdateIdInfoCustomerRequest request)
    {
        var CustomerEditCommand = new CustomerExUpdateIdInfoActiveCommand(id, request.TranActive, request.IdStatus);

        var result = await _mediator.SendCommand(CustomerEditCommand);

        return Ok(result);
    }
}
