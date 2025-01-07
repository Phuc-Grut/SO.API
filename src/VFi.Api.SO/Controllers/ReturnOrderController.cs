using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReturnOrderController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ReturnOrderController> _logger;

    public ReturnOrderController(IMediatorHandler mediator, IContextUser context, ILogger<ReturnOrderController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new ReturnOrderQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new ReturnOrderQueryById(id));
        return Ok(rs);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] ReturnOrderPagingRequest request)
    {
        if (_context.QueryMyData())
        {
            request.EmployeeId = _context.GetUserId().ToString();
        }
        var query = new ReturnOrderPagingQuery(
            request.Keyword,
            request.Status,
            request.EmployeeId,
            request.Filter,
            request.Order,
            request.PageNumber,
            request.PageSize
            );
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("process")]
    public async Task<IActionResult> Put([FromBody] ProcessReturnOrderRequest request)
    {
        var _id = new Guid(request.Id);
        ReturnOrderDto dataReturnOrder = await _mediator.Send(new ReturnOrderQueryById(_id));

        if (dataReturnOrder == null)
            return BadRequest(new ValidationResult("Return order not exists"));
        var ReturnOrderEditCommand = new ReturnOrderProcessCommand(
            _id,
            request.ApproveComment,
            request.Status
       );

        var result = await _mediator.SendCommand(ReturnOrderEditCommand);
        return Ok(result);
    }

    /// <summary>
    /// Nhập thông tin
    /// </summary>
    /// <param name="request">thông tin</param>
    /// <returns>ReturnOrder</returns>
    [HttpPost("add")]
    public async Task<IActionResult> Post([FromBody] AddReturnOrderRequest request)
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
        var Id = Guid.NewGuid();

        var addDetail = request.ReturnOrderProduct?.Select(x => new ReturnOrderProductDto()
        {
            Id = Guid.NewGuid(),
            ReturnOrderId = Id,
            OrderProductId = x.OrderProductId,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            QuantityReturn = x.QuantityReturn,
            UnitPrice = x.UnitPrice,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            DiscountAmountDistribution = x.DiscountAmountDistribution,
            DiscountType = x.DiscountType,
            DiscountPercent = x.DiscountPercent,
            AmountDiscount = x.AmountDiscount,
            TaxRate = x.TaxRate,
            Tax = x.Tax,
            TaxCode = x.TaxCode,
            ReasonId = x.ReasonId,
            ReasonName = x.ReasonName,
            WarehouseId = x.WarehouseId,
            WarehouseName = x.WarehouseName,
            DisplayOrder = x.DisplayOrder
        }).ToList();

        var cmd = new AddReturnOrderCommand(
            Id,
            Code,
            request.CustomerId,
            request.CustomerCode,
            request.CustomerName,
            request.OrderId,
            request.OrderCode,
            request.Address,
            request.Country,
            request.Province,
            request.District,
            request.Ward,
            request.Description,
            request.Status,
            request.WarehouseId,
            request.WarehouseCode,
            request.WarehouseName,
            request.ReturnDate,
            request.AccountId,
            request.AccountName,
            request.CurrencyId,
            request.Currency,
            request.CurrencyName,
            request.Calculation,
            request.ExchangeRate,
            request.TypeDiscount,
            request.DiscountRate,
            request.TypeCriteria,
            request.AmountDiscount,
            request.ApproveBy,
            request.ApproveDate,
            request.ApproveByName,
            request.ApproveComment,
            JsonConvert.SerializeObject(request.File),
            addDetail
        );

        var result = await _mediator.SendCommand(cmd);
        return Ok(new { errors = result.Errors, isValid = result.IsValid, ruleSetsExecuted = result.RuleSetsExecuted, returnCode = Code, returnId = Id });
    }

    /// <summary>
    /// Cập nhật thông tin
    /// </summary>
    /// <param name="request">Thông  tin</param>
    /// <returns>ReturnOrder</returns>
    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditReturnOrderRequest request)
    {
        var updateDetail = request.ReturnOrderProduct?.Select(x => new ReturnOrderProductDto()
        {
            Id = (Guid)(x.Id == null ? Guid.NewGuid() : x.Id),
            ReturnOrderId = request.Id,
            OrderProductId = x.OrderProductId,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            QuantityReturn = x.QuantityReturn,
            UnitPrice = x.UnitPrice,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            DiscountAmountDistribution = x.DiscountAmountDistribution,
            DiscountType = x.DiscountType,
            DiscountPercent = x.DiscountPercent,
            AmountDiscount = x.AmountDiscount,
            TaxRate = x.TaxRate,
            Tax = x.Tax,
            TaxCode = x.TaxCode,
            ReasonId = x.ReasonId,
            ReasonName = x.ReasonName,
            WarehouseId = x.WarehouseId,
            WarehouseName = x.WarehouseName,
            DisplayOrder = x.DisplayOrder
        }).ToList();

        var cmd = new EditReturnOrderCommand(
            request.Id,
            request.Code,
            request.CustomerId,
            request.CustomerCode,
            request.CustomerName,
            request.OrderId,
            request.OrderCode,
            request.Address,
            request.Country,
            request.Province,
            request.District,
            request.Ward,
            request.Description,
            request.Status,
            request.WarehouseId,
            request.WarehouseCode,
            request.WarehouseName,
            request.ReturnDate,
            request.AccountId,
            request.AccountName,
            request.CurrencyId,
            request.Currency,
            request.CurrencyName,
            request.Calculation,
            request.ExchangeRate,
            request.TypeDiscount,
            request.DiscountRate,
            request.TypeCriteria,
            request.AmountDiscount,
            request.ApproveBy,
            request.ApproveDate,
            request.ApproveByName,
            request.ApproveComment,
            JsonConvert.SerializeObject(request.File),
            updateDetail
       );

        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("duplicate")]
    public async Task<IActionResult> Duplicate([FromBody] DuplicateReturnOrder request)
    {
        int UsedStatus = 1;
        var Code = request.Code;
        if (request.IsAuto == 1)
        {
            Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
        }
        else
        {
            var useCodeCommand = new UseCodeCommand(
                request.ModuleCode,
                Code
                );
            _mediator.SendCommand(useCodeCommand);
        }
        var item = new ReturnOrderDuplicateCommand(
            Guid.NewGuid(),
            request.ReturnOrderId,
            Code
            );
        var result = await _mediator.SendCommand(item);
        return Ok(new { errors = result.Errors, isValid = result.IsValid, ruleSetsExecuted = result.RuleSetsExecuted, returnCode = Code });
    }

    /// <summary>
    /// xoá
    /// </summary>
    /// <param name="id">Mã</param>
    /// <returns>ReturnOrder</returns>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ReturnOrder = await _mediator.Send(new ReturnOrderQueryById(id));

        if (ReturnOrder == null)
        {
            return BadRequest(new ValidationResult("ReturnOrder not exists"));
        }

        var data = new DeleteReturnOrderCommand(id);

        var result = await _mediator.SendCommand(data);

        return Ok(result);
    }

    [HttpGet("excel-template")]
    public async Task<IActionResult> GetExcelTemplate()
    {
        var rs = await _mediator.Send(new ReturnOrderExportTemplateQuery());
        if (rs != null)
        {
            return File(rs.ToArray(), "application/xlsx");
        }
        return Ok(rs);
    }

    [HttpPost("validate-excel")]
    public async Task<IActionResult> ValidateExcel([FromForm] ValidateExcelReturnOrder request)
    {
        List<ValidateField> listField = new List<ValidateField>()
        {
            new ValidateField(){Field="productCode", IndexColumn= request.ProductCode},
            new ValidateField(){Field="productName", IndexColumn= request.ProductName},
            new ValidateField(){Field="unitCode", IndexColumn= request.UnitCode},
            new ValidateField(){Field="unitName", IndexColumn= request.UnitName},
            new ValidateField(){Field="unitPrice", IndexColumn= request.UnitPrice},
            new ValidateField(){Field="quantityReturn", IndexColumn= request.QuantityReturn},
            new ValidateField(){Field="discountPercent", IndexColumn= request.DiscountPercent},

            new ValidateField(){Field="tax", IndexColumn= request.Tax},
            new ValidateField(){Field="reasonName", IndexColumn= request.ReasonName},
        };

        var data = new ValidateExcelReturnOrderQuery(request.File,
                                                    request.SheetId,
                                                    request.HeaderRow,
                                                    listField);
        var result = await _mediator.Send(data);
        return Ok(result);
    }

    [HttpPost("manage-payment")]
    public async Task<IActionResult> ManagePayment([FromBody] ManagePaymentRequest request)
    {
        var paymentInvoice = request.PaymentInvoice?.Select(x => new PaymentInvoiceDto()
        {
            Id = x.Id ?? Guid.NewGuid(),
            Type = x.Type,
            Code = x.Code,
            SaleDiscountId = request.Id,
            Description = x.Description,
            Amount = x.Amount,
            Currency = x.Currency,
            CurrencyName = x.CurrencyName,
            Calculation = x.Calculation,
            ExchangeRate = x.ExchangeRate,
            PaymentDate = x.PaymentDate,
            PaymentMethodName = x.PaymentMethodName,
            PaymentMethodCode = x.PaymentMethodCode,
            PaymentMethodId = x.PaymentMethodId,
            BankName = x.BankName,
            BankAccount = x.BankAccount,
            BankNumber = x.BankNumber,
            PaymentCode = x.PaymentCode,
            PaymentNote = x.PaymentNote,
            Note = x.Note,
            Status = x.Status,
            PaymentStatus = x.PaymentStatus,
            AccountId = x.AccountId,
            AccountName = x.AccountName
        }).ToList();

        var cmd = new ManagePaymentReturnCommand(
            request.Id,
            request.PaymentStatus,
            paymentInvoice
            );

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }
}
