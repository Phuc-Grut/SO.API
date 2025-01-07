using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Configuration;
using VFi.NetDevPack.Mediator;
using VFi.NetDevPack.Utilities;

namespace VFi.Api.SO.Controllers.Extended;

[Route("api/[controller]")]
[ApiController]
public class WalletController : ControllerBase
{
    private readonly CodeSyntaxConfig _codeSyntax;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<WalletController> _logger;

    public WalletController(IMediatorHandler mediator, ILogger<WalletController> logger, CodeSyntaxConfig codeSyntax)
    {
        _mediator = mediator;
        _logger = logger;
        _codeSyntax = codeSyntax;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new WalletQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] WalletRequest request)
    {
        var query = new WalletPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddWalletRequest request)
    {
        var cmd = new WalletAddCommand()
        {
            Id = Guid.NewGuid(),
            WalletCode = request.WalletCode
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (await _mediator.Send(new WalletQueryById(id)) == null)
            return BadRequest(new ValidationResult("Wallet not exists"));

        var result = await _mediator.SendCommand(new WalletDeleteCommand(id));

        return Ok(result);
    }

    [HttpGet("get-by-account")]
    public async Task<IActionResult> GetByAccount(Guid accountId, string? code)
    {
        var query = new WalletQueryByAccount();
        query.AccountId = accountId;
        if (!string.IsNullOrEmpty(code))
        {
            query.WalletCode = code;
        }
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositWalletRequest request)
    {
        var code = UniqueKeys.RNGCharacterMask();

        var cmd = new DepositWalletCommand()
        {
            CustomerCode = request.CustomerCode,
            WalletCode = request.WalletCode,
            Amount = request.Amount,
            PaymentCode = request.PaymentCode,
            PaymentNote = request.PaymentNote,
            PaymentDate = request.PaymentDate,
            Document = request.Document
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("deposit-by-code")]
    public async Task<IActionResult> DepositByCode([FromBody] DepositFromBankRequest request)
    {
        //var code = UniqueKeys.RNGCharacterMask();
        var cmd = new DepositWalletFromBankCommand()
        {
            CustomerCode = request.CustomerCode,
            WalletCode = "VND",
            Amount = request.Amount,
            PaymentCode = request.PaymentCode,
            PaymentNote = request.PaymentNote,
            PaymentDate = request.PaymentDate,
            BankName = request.BankName,
            BankAccount = request.BankAccount,
            BankNumber = request.BankNumber
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> WithDraw([FromBody] WithdrawWalletRequest request)
    {
        var code = UniqueKeys.RNGCharacterMask();

        var cmd = new WithdrawWalletCommand()
        {
            AccountId = request.AccountId,
            WalletCode = request.WalletCode,
            Amount = request.Amount,
            Method = request.Method,
            Note = request.Note,
            RawData = request.RawData,
            TransactionCode = code,
            RefDate = request.RefDate,
            RefId = request.RefId
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("pay")]
    public async Task<IActionResult> Pay([FromBody] PayWalletRequest request)
    {
        var code = UniqueKeys.RNGCharacterMask();

        var cmd = new PayWalletCommand()
        {
            AccountId = request.AccountId,
            WalletCode = request.WalletCode,
            Amount = request.Amount,
            Method = "WALLET",
            Note = request.Note,
            RawData = request.RawData,
            TransactionCode = code,
            RefDate = request.RefDate
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("refund-pay")]
    public async Task<IActionResult> RefundPay([FromBody] RefundPayWalletRequest request)
    {
        var code = UniqueKeys.RNGCharacterMask();

        var cmd = new RefundPayWalletCommand()
        {
            AccountId = request.AccountId,
            WalletCode = request.WalletCode,
            Amount = request.Amount,
            Method = "WALLET",
            Note = request.Note,
            RawData = request.RawData,
            TransactionCode = code,
            RefDate = request.RefDate,
            RefId = request.RefId
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("pay-order")]
    public async Task<IActionResult> PayOrder([FromBody] PayOrderWalletRequest request)
    {
        var code = UniqueKeys.RNGCharacterMask();

        var cmd = new PayOrderWalletCommand()
        {
            AccountId = request.AccountId,
            WalletCode = request.WalletCode,
            Amount = request.Amount,
            Method = "WALLET",
            Note = request.Note,
            RawData = request.RawData,
            TransactionCode = code,
            OrderCode = request.OrderCode,
            OrderId = request.OrderId
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("refund-pay-order")]
    public async Task<IActionResult> RefundPayOrder([FromBody] RefundOrderWalletRequest request)
    {
        var code = UniqueKeys.RNGCharacterMask();

        var cmd = new RefundPayOrderWalletCommand()
        {
            AccountId = request.AccountId,
            WalletCode = request.WalletCode,
            Amount = request.Amount,
            Method = "WALLET",
            Note = request.Note,
            RawData = request.RawData,
            TransactionCode = code,
            OrderCode = request.OrderCode,
            OrderId = request.OrderId,
            RefId = request.RefId
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("hold")]
    public async Task<IActionResult> Hold([FromBody] HoldWalletRequest request)
    {
        var code = UniqueKeys.RNGCharacterMask();

        var cmd = new HoldWalletCommand()
        {
            AccountId = request.AccountId,
            WalletCode = request.WalletCode,
            Amount = request.Amount,
            Method = "WALLET",
            Note = request.Note,
            RawData = request.RawData,
            TransactionCode = code,
            RefDate = request.RefDate
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("refund-hold")]
    public async Task<IActionResult> RefundHold([FromBody] RefundPayWalletRequest request)
    {
        var code = UniqueKeys.RNGCharacterMask();

        var cmd = new RefundHoldWalletCommand()
        {
            AccountId = request.AccountId,
            WalletCode = request.WalletCode,
            Amount = request.Amount,
            Method = "WALLET",
            Note = request.Note,
            RawData = request.RawData,
            TransactionCode = code,
            RefDate = request.RefDate,
            RefId = request.RefId
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("hold-bid")]
    public async Task<IActionResult> HoldBid([FromBody] HoldBidWalletRequest request)
    {
        var code = UniqueKeys.RNGCharacterMask();

        var cmd = new HoldBidWalletCommand()
        {
            AccountId = request.AccountId,
            WalletCode = request.WalletCode,
            Amount = request.Amount,
            Method = "WALLET",
            Note = request.Note,
            TransactionCode = code,
            RefDate = request.RefDate
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("refund-hold-bid")]
    public async Task<IActionResult> RefundHoldBid([FromBody] RefundHoldBidWalletRequest request)
    {
        var code = UniqueKeys.RNGCharacterMask();

        var cmd = new RefundHoldBidWalletCommand()
        {
            TransactionCode = code,
            AccountId = request.AccountId,
            RefId = request.RefId,
            Note = request.Note
        };
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }
}
