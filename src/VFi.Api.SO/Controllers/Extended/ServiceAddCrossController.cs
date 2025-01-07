using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Queries;

namespace VFi.Api.SO.Controllers;

public partial class ServiceAddController : ControllerBase
{
    [HttpGet("get-checkout")]
    public async Task<IActionResult> GetCheckout([FromQuery] ListBoxServiceAddRequest request)
    {
        var result = await _mediator.Send(new ServiceAddCheckoutQueryListBox(request.Keyword, request.Status));
        return Ok(result);
    }
}
