using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
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
public class PriceListController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<PriceListController> _logger;

    public PriceListController(IMediatorHandler mediator, IContextUser context, ILogger<PriceListController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] PriceListListBoxRequest request)
    {
        var result = await _mediator.Send(new PriceListQueryComboBox(request.ToBaseQuery()));
        return Ok(result);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new PriceListQueryById(id));
        return Ok(rs);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PriceListPagingRequest request)
    {
        var query = new PriceListPagingQuery(
              request.Keyword ?? "",
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
    public async Task<IActionResult> Post([FromBody] AddPriceListRequest request)
    {
        var Id = Guid.NewGuid();
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
        var addDetail = request.PriceListDetail?.Select(x => new PriceListDetailDto()
        {
            Id = Id,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            CurrencyCode = x.CurrencyCode,
            CurrencyName = x.CurrencyName,
            QuantityMin = x.QuantityMin,
            Type = x.Type,
            FixPrice = x.FixPrice,
            TypeDiscount = x.TypeDiscount,
            DiscountRate = x.DiscountRate,
            DiscountValue = x.DiscountValue,
            DisplayOrder = x.DisplayOrder
        }).ToList();

        var data = new AddPriceListCommand(
            Id,
            Code,
            request.Name,
            request.Description,
            request.Status,
            request.StartDate,
            request.EndDate,
            request.Currency,
            request.CurrencyName,
            request.DisplayOrder,
            addDetail
        );

        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditPriceListRequest request)
    {
        var updateDetail = request.PriceListDetail?.Select(x => new PriceListDetailDto()
        {
            Id = !String.IsNullOrEmpty(x.Id) ? new Guid(x.Id) : null,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            CurrencyCode = x.CurrencyCode,
            CurrencyName = x.CurrencyName,
            QuantityMin = x.QuantityMin,
            Type = x.Type,
            FixPrice = x.FixPrice,
            TypeDiscount = x.TypeDiscount,
            DiscountRate = x.DiscountRate,
            DiscountValue = x.DiscountValue,
            DisplayOrder = x.DisplayOrder
        }).ToList();

        var data = new EditPriceListCommand(
            request.Id,
            request.Code,
            request.Name,
            request.Description,
            request.Status,
            request.StartDate,
            request.EndDate,
            request.Currency,
            request.CurrencyName,
            request.DisplayOrder,
            updateDetail
       );

        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new PriceListSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

            var result = await _mediator.SendCommand(sortCommand);
            return Ok(result);
        }
        else
        {
            return BadRequest("Please input list sort");
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var PriceList = await _mediator.Send(new PriceListQueryById(id));

        if (PriceList == null)
        {
            return BadRequest(new ValidationResult("PriceList not exists"));
        }

        var data = new DeletePriceListCommand(id);

        var result = await _mediator.SendCommand(data);

        return Ok(result);
    }

    [HttpGet("excel-template")]
    public async Task<IActionResult> GetExcelTemplate()
    {
        var rs = await _mediator.Send(new PriceListExportTemplateQuery());
        if (rs != null)
        {
            return File(rs.ToArray(), "application/xlsx");
        }
        return Ok(rs);
    }

    [HttpPost("validate-excel")]
    public async Task<IActionResult> ValidateExcel([FromForm] ValidateExcelPriceList request)
    {
        List<ValidateField> listField = new List<ValidateField>()
        {
            new ValidateField(){Field="productCode", IndexColumn= request.ProductCode},
            new ValidateField(){Field="productName", IndexColumn= request.ProductName},
            new ValidateField(){Field="type", IndexColumn= request.Type},
            new ValidateField(){Field="fixPrice", IndexColumn= request.FixPrice},
            new ValidateField(){Field="quantityMin", IndexColumn= request.QuantityMin},
        };

        var data = new ValidateExcelPriceListQuery(request.File,
                                                    request.SheetId,
                                                    request.HeaderRow,
                                                    listField);
        var result = await _mediator.Send(data);
        return Ok(result);
    }
}
