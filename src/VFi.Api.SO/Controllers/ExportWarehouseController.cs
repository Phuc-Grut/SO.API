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
public class ExportWarehouseController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ExportWarehouseController> _logger;

    public ExportWarehouseController(IMediatorHandler mediator, IContextUser context, ILogger<ExportWarehouseController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpPost("duplicate")]
    public async Task<IActionResult> Duplicate([FromBody] DuplicateExportWarehouse request)
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

        var item = new ExportWarehouseDuplicateCommand(
            request.Id,
            Code
        );
        var result = await _mediator.SendCommand(item);

        return Ok(new { errors = result.Errors, isValid = result.IsValid, ruleSetsExecuted = result.RuleSetsExecuted, returnCode = Code });
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new ExportWarehouseQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new ExportWarehouseQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] ExportWarehouseRequest request)
    {
        if (_context.QueryMyData())
        {
            request.EmployeeId = _context.GetUserId().ToString();
        }

        var query = new ExportWarehousePagingQuery(
            request.Keyword,
            request.CustomerId,
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

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddExportWarehouseRequest request)
    {
        var UsedStatus = 1;
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
        var create = _context.GetUserId();
        var createdDate = DateTime.Now;
        var addDetail = request.Details?.Select(x => new ExportWarehouseProductDto()
        {
            OrderId = x.OrderId,
            OrderCode = x.OrderCode,
            OrderProductId = x.OrderProductId,
            ExportWarehouseId = Id,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            WarehouseCode = x.WarehouseCode,
            WarehouseName = x.WarehouseName,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            QuantityRequest = x.QuantityRequest,
            QuantityExported = x.QuantityExported,
            Note = x.Note,
            CustomerId = x.CustomerId,
            ExportWarehouseStatus = x.ExportWarehouseStatus,
            Status= x.Status
        }).ToList();
        var ExportWarehouseAddCommand = new ExportWarehouseAddCommand(
            Id,
            Code,
            !string.IsNullOrEmpty(request.OrderId) ? new Guid(request.OrderId) : null,
            request.OrderCode,
            !string.IsNullOrEmpty(request.CustomerId) ? new Guid(request.CustomerId) : null,
            request.CustomerCode,
            request.CustomerName,
            request.Description,
            !string.IsNullOrEmpty(request.WarehouseId) ? new Guid(request.WarehouseId) : null,
            request.WarehouseCode,
            request.WarehouseName,
            request.DeliveryStatus,
            request.DeliveryName,
            request.DeliveryAddress,
            request.DeliveryCountry,
            request.DeliveryProvince,
            request.DeliveryDistrict,
            request.DeliveryWard,
            request.DeliveryNote,
            request.EstimatedDeliveryDate,
            !string.IsNullOrEmpty(request.DeliveryMethodId) ? new Guid(request.DeliveryMethodId) : null,
            request.DeliveryMethodCode,
            request.DeliveryMethodName,
            !string.IsNullOrEmpty(request.ShippingMethodId) ? new Guid(request.ShippingMethodId) : null,
            request.ShippingMethodCode,
            request.ShippingMethodName,
            request.Status,
            request.Note,
            request.RequestBy,
            request.RequestByName,
            request.RequestDate,
            _context.GetUserId(),
            _context.UserClaims.FullName,
            JsonConvert.SerializeObject(request.File),
            addDetail
        );
        var result = await _mediator.SendCommand(ExportWarehouseAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditExportWarehouseRequest request)
    {
        var useBy = _context.GetUserId();
        var ExportWarehouse = await _mediator.Send(new ExportWarehouseQueryById(request.Id));
        if (ExportWarehouse == null)
        {
            return BadRequest(new ValidationResult("ExportWarehouse not exists"));
        }

        //chi tiết
        var updateDetail = request.Details?.Select(x => new ExportWarehouseProductDto()
        {
            Id = x.Id != null ? x.Id : Guid.NewGuid(),
            OrderId = x.OrderId,
            OrderCode = x.OrderCode,
            OrderProductId = x.OrderProductId,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            WarehouseCode = x.WarehouseCode,
            WarehouseName = x.WarehouseName,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            QuantityRequest = x.QuantityRequest,
            QuantityExported = x.QuantityExported,
            Note = x.Note,
        }).ToList();

        var deleteDetail = ExportWarehouse.Details.Where(x => request.Details.Where(f => f.Id != null).Select(f => f.Id).Contains(x.Id) == false)?.Select(x => new ListId()
        {
            Id = (Guid)x.Id
        }).ToList();

        var ExportWarehouseEditCommand = new ExportWarehouseEditCommand(
            request.Id,
            request.Code,
            !string.IsNullOrEmpty(request.OrderId) ? new Guid(request.OrderId) : null,
            request.OrderCode,
            !string.IsNullOrEmpty(request.CustomerId) ? new Guid(request.CustomerId) : null,
            request.CustomerCode,
            request.CustomerName,
            request.Description,
            !string.IsNullOrEmpty(request.WarehouseId) ? new Guid(request.WarehouseId) : null,
            request.WarehouseCode,
            request.WarehouseName,
            request.DeliveryStatus,
            request.DeliveryName,
            request.DeliveryAddress,
            request.DeliveryCountry,
            request.DeliveryProvince,
            request.DeliveryDistrict,
            request.DeliveryWard,
            request.DeliveryNote,
            request.EstimatedDeliveryDate,
            !string.IsNullOrEmpty(request.DeliveryMethodId) ? new Guid(request.DeliveryMethodId) : null,
            request.DeliveryMethodCode,
            request.DeliveryMethodName,
            !string.IsNullOrEmpty(request.ShippingMethodId) ? new Guid(request.ShippingMethodId) : null,
            request.ShippingMethodCode,
            request.ShippingMethodName,
            request.Status,
            request.Note,
            request.RequestBy,
            request.RequestByName,
            request.RequestDate,
            _context.GetUserId(),
            _context.UserClaims.FullName,
            JsonConvert.SerializeObject(request.File),
            updateDetail,
            deleteDetail
        );

        var result = await _mediator.SendCommand(ExportWarehouseEditCommand);

        return Ok(result);
    }

    [HttpPut("put-order")]
    public async Task<IActionResult> PutOrder([FromBody] ExportWarehouseAddOrdersRequest request)
    {
        var exportWarehouseUpdateCommand = new ExportWarehouseAddOrderIdsCommand(request.Id, request.OrderIds);
        var result = await _mediator.SendCommand(exportWarehouseUpdateCommand);
        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var ExportWarehouseId = new Guid(id);
        if (await _mediator.Send(new ExportWarehouseQueryById(ExportWarehouseId)) == null)
            return BadRequest(new ValidationResult("ExportWarehouse not exists"));

        var result = await _mediator.SendCommand(new ExportWarehouseDeleteCommand(ExportWarehouseId));

        return Ok(result);
    }

    [HttpPost("approval")]
    public async Task<IActionResult> approval([FromBody] ApprovalExportWarehouseRequest request)
    {
        var command = new ApprovalExportWarehouseCommand(
            request.Id,
            request.Status,
            request.ApproveComment,
            request.Type,
            request.WarehouseCode,
            request.Sonumber,
            request.CustomerCode,
            request.CustomerName,
            request.DeliveryName,
            request.DeliveryAddress,
            request.DeliveryCountry,
            request.DeliveryProvince,
            request.DeliveryDistrict,
            request.DeliveryWard,
            request.DeliveryNote,
            request.EstimatedDeliveryDate,
            request.ShippingMethodCode,
            request.DeliveryMethodCode,
            request.WMSStatus,
            request.Note,
            request.RequestByName,
            request.RequestBy,
            request.Details
        );

        var result = await _mediator.SendCommand(command);

        return Ok(result);
    }

    [HttpGet("excel-template")]
    public async Task<IActionResult> GetExcelTemplate()
    {
        var rs = await _mediator.Send(new ExportWarehouseExportTemplateQuery());
        if (rs != null)
        {
            return File(rs.ToArray(), "application/xlsx");
        }

        return Ok(rs);
    }

    [HttpPost("validate-excel")]
    public async Task<IActionResult> ValidateExcel([FromForm] ValidateExcelExportWarehouset request)
    {
        var listField = new List<ValidateField>()
        {
            new ValidateField() { Field = "productCode", IndexColumn = request.ProductCode },
            new ValidateField() { Field = "productName", IndexColumn = request.ProductName },
            new ValidateField() { Field = "unitCode", IndexColumn = request.UnitCode },
            new ValidateField() { Field = "unitName", IndexColumn = request.UnitName },
            new ValidateField() { Field = "quantityRequest", IndexColumn = request.QuantityRequest },
            new ValidateField() { Field = "note", IndexColumn = request.Note },
        };

        var data = new ValidateExcelExportWarehouseQuery(request.File,
            request.SheetId,
            request.HeaderRow,
            listField);
        var result = await _mediator.Send(data);
        return Ok(result);
    }

    [HttpPut("update-service-fees")]
    public async Task<IActionResult> UpdateServiceFees([FromBody] UpdateServiceFeesRequest request)
    {
        var command = new UpdateServiceFeesCommand(
            request.Id,
            request.ServiceAddId,
            request.ServiceAddCurrencyId,
            request.ServiceAddCurrencyCode,
            request.ServiceAddName,
            request.PriceTotal
        );

        var result = await _mediator.SendCommand(command);

        return Ok(result);
    }
}