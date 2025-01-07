using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public partial class PaymentInvoiceController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<PaymentInvoiceController> _logger;

    public PaymentInvoiceController(IMediatorHandler mediator, IContextUser context, ILogger<PaymentInvoiceController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new PaymentInvoiceQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-box")]
    public async Task<IActionResult> Get([FromQuery] PaymentInvoiceListBoxRequest request)
    {
        var result = await _mediator.Send(new PaymentInvoiceQueryComboBox(request.ToBaseQuery()));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
    {
        var query = new PaymentInvoicePagingQuery(
            request.Keyword,
            request.Status,
            request.Filter ?? "",
            request.Order ?? "",
            request.PageNumber,
            request.PageSize
            );
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddPaymentInvoiceRequest request)
    {
        var Code = request.Code;
        if (request.IsAuto == true)
        {
            Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, 1));
        }
        else
        {
            var useCodeCommand = new UseCodeCommand(
                request.ModuleCode,
                Code
                );
            _mediator.SendCommand(useCodeCommand);
        }
        var cmd = new PaymentInvoiceAddCommand(
          Guid.NewGuid(),
          request.Type,
          request.Code,
          request.OrderId,
          request.OrderCode,
          request.SaleDiscountId,
          request.ReturnOrderId,
          request.Description,
          request.Amount,
          request.Currency,
          request.CurrencyName,
          request.Calculation,
          request.ExchangeRate,
          request.PaymentDate,
          request.PaymentMethodName,
          request.PaymentMethodCode,
          request.PaymentMethodId,
          request.BankName,
          request.BankAccount,
          request.BankNumber,
          request.PaymentCode,
          request.PaymentNote,
          request.Note,
          request.Status,
          request.Locked,
          request.PaymentStatus,
          request.AccountId,
          request.AccountName,
          request.CustomerId,
          request.CustomerName,
          JsonConvert.SerializeObject(request.File)
      );
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditPaymentInvoiceRequest request)
    {
        var cmd = new PaymentInvoiceEditCommand(
          request.Id ?? Guid.Empty,
          request.Type,
          request.Code,
          request.OrderId,
          request.OrderCode,
          request.SaleDiscountId,
          request.ReturnOrderId,
          request.Description,
          request.Amount,
          request.Currency,
          request.CurrencyName,
          request.Calculation,
          request.ExchangeRate,
          request.PaymentDate,
          request.PaymentMethodName,
          request.PaymentMethodCode,
          request.PaymentMethodId,
          request.BankName,
          request.BankAccount,
          request.BankNumber,
          request.PaymentCode,
          request.PaymentNote,
          request.Note,
          request.Status,
          request.Locked,
          request.PaymentStatus,
          request.AccountId,
          request.AccountName,
          request.CustomerId,
          request.CustomerName,
          JsonConvert.SerializeObject(request.File)
       );

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var PaymentInvoiceId = new Guid(id);
        if (await _mediator.Send(new PaymentInvoiceQueryById(PaymentInvoiceId)) == null)
            return BadRequest(new ValidationResult("PaymentInvoice not exists"));

        var result = await _mediator.SendCommand(new PaymentInvoiceDeleteCommand(PaymentInvoiceId));

        return Ok(result);
    }

    [HttpPost("change-locked")]
    public async Task<IActionResult> ChangeLocked([FromBody] PaymentInvoiceChangeLockedRequest request)
    {
        var cmd = new PaymentInvoiceChangeLockedCommand(
            request.Id,
            request.Locked
            );

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }
}
