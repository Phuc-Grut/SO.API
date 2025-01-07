using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.Queries;

namespace VFi.Api.SO.Controllers;

public partial class PaymentInvoiceController : ControllerBase
{
    [HttpGet("top-by-accountid")]
    public async Task<IActionResult> TopByAccountId(Guid accountId)
    {
        var query = new TopPaymentByAccountIdQuery(accountId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
