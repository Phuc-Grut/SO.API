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
public class ContractController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ContractController> _logger;

    public ContractController(IMediatorHandler mediator, IContextUser context, ILogger<ContractController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new ContractQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ContractListBoxRequest request)
    {
        var result = await _mediator.Send(new ContractQueryComboBox(request.Keyword, request.ToBaseQuery(), request.PageSize, request.PageIndex));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] ContractPagingRequest request)
    {
        if (_context.QueryMyData())
        {
            request.EmployeeId = _context.GetUserId().ToString();
        }
        var query = new ContractPagingQuery(
              request.Keyword ?? "",
              request.Status,
              request.EmployeeId,
              request.Filter ?? "",
              request.Order ?? "",
              request.PageNumber,
              request.PageSize
            );
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddContractRequest request)
    {
        int UsedStatus = 1;
        var Code = request.Code;
        if (request.IsAuto == true)
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
        var Id = Guid.NewGuid();
        var addDetail = request.OrderProduct?.Select(x => new OrderProductDto(request.Calculation, request.ExchangeRate)
        {
            OrderId = x.OrderId,
            OrderCode = x.OrderCode,
            ContractId = Id,
            ContractName = Code,
            QuotationId = x.QuotationId,
            QuotationName = x.QuotationName,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            Origin = x.Origin,
            WarehouseId = x.WarehouseId,
            WarehouseCode = x.WarehouseCode,
            WarehouseName = x.WarehouseName,
            PriceListId = x.PriceListId,
            PriceListName = x.PriceListName,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice,
            DiscountAmountDistribution = x.DiscountAmountDistribution,
            DiscountType = x.DiscountType,
            DiscountPercent = x.DiscountPercent,
            AmountDiscount = x.AmountDiscount,
            DiscountTotal = x.DiscountTotal,
            TaxRate = x.TaxRate,
            Tax = x.Tax,
            TaxCode = x.TaxCode,
            ExpectedDate = x.ExpectedDate,
            Note = x.Note,
            DisplayOrder = x.DisplayOrder,
            DeliveryStatus = x.DeliveryStatus,
            DeliveryQuantity = x.DeliveryQuantity,
            SpecificationCode1 = x.SpecificationCode1,
            SpecificationCode2 = x.SpecificationCode2,
            SpecificationCode3 = x.SpecificationCode3,
            SpecificationCode4 = x.SpecificationCode4,
            SpecificationCode5 = x.SpecificationCode5,
            SpecificationCode6 = x.SpecificationCode6,
            SpecificationCode7 = x.SpecificationCode7,
            SpecificationCode8 = x.SpecificationCode8,
            SpecificationCode9 = x.SpecificationCode9,
            SpecificationCode10 = x.SpecificationCode10,
            SpecificationCodeJson = x.SpecificationCodeJson
        }).ToList();

        var cmd = new ContractAddCommand(
           Id,
           Code,
           request.Name,
           !String.IsNullOrEmpty(request.ContractTypeId) ? new Guid(request.ContractTypeId) : null,
           request.ContractTypeName,
           !String.IsNullOrEmpty(request.QuotationId) ? new Guid(request.QuotationId) : null,
           request.QuotationName,
           !String.IsNullOrEmpty(request.OrderId) ? new Guid(request.OrderId) : null,
           request.OrderCode,
           request.StartDate,
           request.EndDate,
           request.SignDate,
           !String.IsNullOrEmpty(request.CustomerId) ? new Guid(request.CustomerId) : null,
           request.CustomerCode,
           request.CustomerName,
           request.Country,
           request.Province,
           request.District,
           request.Ward,
           request.Address,
           request.Currency,
           request.CurrencyName,
           request.Calculation,
           request.ExchangeRate,
           request.Status,
           request.TypeDiscount,
           request.DiscountRate,
           request.TypeCriteria,
           request.AmountDiscount,
           request.AccountName,
           !String.IsNullOrEmpty(request.GroupEmployeeId) ? new Guid(request.GroupEmployeeId) : null,
           request.GroupEmployeeName,
           !String.IsNullOrEmpty(request.AccountId) ? new Guid(request.AccountId) : null,
           !String.IsNullOrEmpty(request.ContractTermId) ? new Guid(request.ContractTermId) : null,
           request.ContractTermName,
           request.ContractTermContent,
           request.PaymentDueDate,
           request.DeliveryDate,
           request.Buyer,
           request.Saler,
           request.Description,
           request.Note,
           JsonConvert.SerializeObject(request.File),
           request.HasPreviousContract,
           request.Paid,
           request.Received,
           addDetail
      );
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditContractRequest request)
    {
        //chi tiết
        var updateDetail = request.OrderProduct?.Select(x => new OrderProductDto(request.Calculation, request.ExchangeRate)
        {
            Id = !String.IsNullOrEmpty(x.Id) ? new Guid(x.Id) : null,
            OrderId = x.OrderId,
            OrderCode = x.OrderCode,
            ContractId = request.Id,
            ContractName = request.Code,
            QuotationId = x.QuotationId,
            QuotationName = x.QuotationName,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            Origin = x.Origin,
            WarehouseId = x.WarehouseId,
            WarehouseCode = x.WarehouseCode,
            WarehouseName = x.WarehouseName,
            PriceListId = x.PriceListId,
            PriceListName = x.PriceListName,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice,
            DiscountAmountDistribution = x.DiscountAmountDistribution,
            DiscountType = x.DiscountType,
            DiscountPercent = x.DiscountPercent,
            AmountDiscount = x.AmountDiscount,
            DiscountTotal = x.DiscountTotal,
            TaxRate = x.TaxRate,
            Tax = x.Tax,
            TaxCode = x.TaxCode,
            ExpectedDate = x.ExpectedDate,
            Note = x.Note,
            DisplayOrder = x.DisplayOrder,
            DeliveryStatus = x.DeliveryStatus,
            DeliveryQuantity = x.DeliveryQuantity,
            SpecificationCode1 = x.SpecificationCode1,
            SpecificationCode2 = x.SpecificationCode2,
            SpecificationCode3 = x.SpecificationCode3,
            SpecificationCode4 = x.SpecificationCode4,
            SpecificationCode5 = x.SpecificationCode5,
            SpecificationCode6 = x.SpecificationCode6,
            SpecificationCode7 = x.SpecificationCode7,
            SpecificationCode8 = x.SpecificationCode8,
            SpecificationCode9 = x.SpecificationCode9,
            SpecificationCode10 = x.SpecificationCode10,
            SpecificationCodeJson = x.SpecificationCodeJson
        }).ToList();

        var ContractEditCommand = new ContractEditCommand(
           request.Id,
           request.Code,
           request.Name,
           !String.IsNullOrEmpty(request.ContractTypeId) ? new Guid(request.ContractTypeId) : null,
           request.ContractTypeName,
           !String.IsNullOrEmpty(request.QuotationId) ? new Guid(request.QuotationId) : null,
           request.QuotationName,
           !String.IsNullOrEmpty(request.OrderId) ? new Guid(request.OrderId) : null,
           request.OrderCode,
           request.StartDate,
           request.EndDate,
           request.SignDate,
           !String.IsNullOrEmpty(request.CustomerId) ? new Guid(request.CustomerId) : null,
           request.CustomerCode,
           request.CustomerName,
           request.Country,
           request.Province,
           request.District,
           request.Ward,
           request.Address,
           request.Currency,
           request.CurrencyName,
           request.Calculation,
           request.ExchangeRate,
           request.Status,
           request.TypeDiscount,
           request.DiscountRate,
           request.TypeCriteria,
           request.AmountDiscount,
           request.AccountName,
           !String.IsNullOrEmpty(request.GroupEmployeeId) ? new Guid(request.GroupEmployeeId) : null,
           request.GroupEmployeeName,
           !String.IsNullOrEmpty(request.AccountId) ? new Guid(request.AccountId) : null,
           !String.IsNullOrEmpty(request.ContractTermId) ? new Guid(request.ContractTermId) : null,
           request.ContractTermName,
           request.ContractTermContent,
           request.PaymentDueDate,
           request.DeliveryDate,
           request.Buyer,
           request.Saler,
           request.Description,
           request.Note,
           JsonConvert.SerializeObject(request.File),
           request.HasPreviousContract,
           request.Paid,
           request.Received,
           updateDetail
       );

        var result = await _mediator.SendCommand(ContractEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var ContractId = new Guid(id);
        if (await _mediator.Send(new ContractQueryById(ContractId)) == null)
            return BadRequest(new ValidationResult("Contract not exists"));

        var result = await _mediator.SendCommand(new ContractDeleteCommand(ContractId));

        return Ok(result);
    }

    [HttpPost("approval")]
    public async Task<IActionResult> UpdateStatus([FromBody] ApprovalContractRequest request)
    {
        var cmd = new ApprovalContractCommand(
            request.Id,
            request.Status,
            request.ApproveComment
            );
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("liquidation")]
    public async Task<IActionResult> Liquidation([FromBody] LiquidationContractRequest request)
    {
        var cmd = new LiquidationContractCommand(
            request.Id,
            request.AmountLiquidation,
            request.LiquidationDate,
            request.LiquidationReason
            );
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpGet("excel-template")]
    public async Task<IActionResult> GetExcelTemplate()
    {
        var rs = await _mediator.Send(new ContractExportTemplateQuery());
        if (rs != null)
        {
            return File(rs.ToArray(), "application/xlsx");
        }
        return Ok(rs);
    }

    [HttpPost("validate-excel")]
    public async Task<IActionResult> ValidateExcel([FromForm] ValidateExcelContract request)
    {
        List<ValidateField> listField = new List<ValidateField>()
        {
            new ValidateField(){Field="productCode", IndexColumn= request.ProductCode},
            new ValidateField(){Field="productName", IndexColumn= request.ProductName},
            new ValidateField(){Field="unitCode", IndexColumn= request.UnitCode},
            new ValidateField(){Field="unitName", IndexColumn= request.UnitName},
            new ValidateField(){Field="unitPrice", IndexColumn= request.UnitPrice},
            new ValidateField(){Field="quantity", IndexColumn= request.Quantity},
            new ValidateField(){Field="discountPercent", IndexColumn= request.DiscountPercent},
            new ValidateField(){Field="tax", IndexColumn= request.Tax},
            new ValidateField(){Field="note", IndexColumn= request.Note},
        };

        var data = new ValidateExcelContractQuery(request.File,
                                                    request.SheetId,
                                                    request.HeaderRow,
                                                    listField);
        var result = await _mediator.Send(data);
        return Ok(result);
    }
}
