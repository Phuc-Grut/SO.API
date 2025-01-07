using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentTermController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<PaymentTermController> _logger;

    public PaymentTermController(IMediatorHandler mediator, ILogger<PaymentTermController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Lấy thông tin
    /// </summary>
    /// <param name="id">Thông tin</param>
    /// <returns>PaymentTerm</returns>
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new PaymentTermQueryById(id));
        return Ok(rs);
    }

    /// <summary>
    /// Lấy danh sách  theo phân trang
    /// </summary>
    /// <param name="request"> phân trang</param>
    /// <returns>List PaymentTerm</returns>
    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PaymentTermRequest request)
    {
        var query = new PaymentTermPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Nhập thông tin
    /// </summary>
    /// <param name="request">thông tin</param>
    /// <returns>PaymentTerm</returns>
    [HttpPost("add")]
    public async Task<IActionResult> Post([FromBody] AddPaymentTermRequest request)
    {
        var data = new AddPaymentTermCommand(
            Guid.NewGuid(),
            request.Code,
            request.Name,
            request.Description,
            request.Day,
            request.Value,
            request.Percent,
            request.Status
        );

        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    /// <summary>
    /// Cập nhật thông tin
    /// </summary>
    /// <param name="request">Thông  tin</param>
    /// <returns>PaymentTerm</returns>
    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditPaymentTermRequest request)
    {
        var PaymentTerm = await _mediator.Send(new PaymentTermQueryById(request.Id));
        if (PaymentTerm == null)
        {
            return BadRequest(new ValidationResult("PaymentTerm not exists"));
        }

        var data = new EditPaymentTermCommand(
            request.Id,
            request.Code,
            request.Name,
            request.Description,
            request.Day,
            request.Value,
            request.Percent,
            request.Status
       );

        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    /// <summary>
    /// xoá
    /// </summary>
    /// <param name="id">Mã</param>
    /// <returns>PaymentTerm</returns>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var PaymentTerm = await _mediator.Send(new PaymentTermQueryById(id));

        if (PaymentTerm == null)
        {
            return BadRequest(new ValidationResult("PaymentTerm not exists"));
        }

        var data = new DeletePaymentTermCommand(id);

        var result = await _mediator.SendCommand(data);

        return Ok(result);
    }
}
