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
public class ProductionOrderController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ProductionOrderController> _logger;

    public ProductionOrderController(IMediatorHandler mediator, IContextUser context, ILogger<ProductionOrderController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddProductionOrderRequest request)
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
        var listProductionOrdersDetail = request.ListProductionOrdersDetail?.Select(x => new ProductionOrdersDetailDto()
        {
            Id = Guid.NewGuid(),
            ProductionOrdersId = Id,
            OrderId = x.OrderId,
            OrderCode = x.OrderCode,
            OrderProductId = x.OrderProductId,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            Sku = x.Sku,
            Gtin = x.Gtin,
            Origin = x.Origin,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            Quantity = x.Quantity,
            Note = x.Note,
            EstimatedDeliveryQuantity = x.EstimatedDeliveryQuantity,
            DeliveryDate = x.DeliveryDate,
            IsWorkOrdered = x.IsWorkOrdered,
            ProductionOrdersCode = x.ProductionOrdersCode,
            IsEstimated = x.IsEstimated,
            IsBom = x.IsBom,
            Status = x.Status,
            EstimatedDate = x.EstimatedDate,
            EstimateStatus = x.EstimateStatus,
            Solution = x.Solution,
            Transport = x.Transport,
            Height = x.Height,
            Package = x.Package,
            Volume = x.Volume,
            Length = x.Length,
            Weight = x.Weight,
            Width = x.Width,
            CancelReason = x.CancelReason,
            DisplayOrder = x.DisplayOrder
        }).ToList();
        var data = new ProductionOrderAddCommand(
            Id,
            Code: Code,
            Note: request.Note,
            Status: request.Status,
            RequestDate: request.RequestDate,
            CustomerId: request.CustomerId,
            CustomerCode: request.CustomerCode,
            CustomerName: request.CustomerName,
            Email: request.Email,
            Phone: request.Phone,
            Address: request.Address,
            EmployeeId: request.EmployeeId,
            EmployeeCode: request.EmployeeCode,
            EmployeeName: request.EmployeeName,
            DateNeed: request.DateNeed,
            OrderId: request.OrderId,
            OrderNumber: request.OrderNumber,
            SaleEmployeeId: request.SaleEmployeeId,
            SaleEmployeeCode: request.SaleEmployeeCode,
            SaleEmployeeName: request.SaleEmployeeName,
            Type: request.Type,
            estimateDate: request.EstimateDate,
            file: JsonConvert.SerializeObject(request.File),
            listProductionOrdersDetail
    );
        var result = await _mediator.SendCommand(data);
        if (result.IsValid == false && request.IsAuto == true && result.Errors[0].ToString() == "Code AlreadyExists")
        {
            int loopTime = 5;
            for (int i = 0; i < loopTime; i++)
            {
                data.Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
                var res = await _mediator.SendCommand(data);
                if (res.IsValid == true)
                {
                    return Ok(res);
                }
            }
        }
        return Ok(result);
    }

    /// <summary>
    /// Lấy danh sách  theo phân trang
    /// </summary>
    /// <param name="request"> phân trang</param>
    /// <returns>List Insurance</returns>
    [HttpGet("paging")]
    public async Task<IActionResult> Pagedresult([FromQuery] FopPagingRequest request)
    {
        var query = new ProductionOrderPagingFilterQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditProductionOrderRequest request)
    {
        var Id = new Guid(request.Id);
        ProductionOrderDto dataProductionOrder = await _mediator.Send(new ProductionOrderQueryById(Id));

        if (dataProductionOrder == null)
            return BadRequest(new ValidationResult("Code not exists"));

        var useBy = _context.GetUserId();
        var updateProductionOrdersDetail = request.ListProductionOrdersDetail?.Select(x => new ProductionOrdersDetailDto()
        {
            Id = (Guid)(x.Id == null ? Guid.NewGuid() : x.Id),
            ProductionOrdersId = Id,
            OrderId = x.OrderId,
            OrderCode = x.OrderCode,
            OrderProductId = x.OrderProductId,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            Sku = x.Sku,
            Gtin = x.Gtin,
            Origin = x.Origin,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            Quantity = x.Quantity,
            Note = x.Note,
            EstimatedDeliveryQuantity = x.EstimatedDeliveryQuantity,
            DeliveryDate = x.DeliveryDate,
            IsWorkOrdered = x.IsWorkOrdered,
            ProductionOrdersCode = x.ProductionOrdersCode,
            IsEstimated = x.IsEstimated,
            IsBom = x.IsBom,
            Status = x.Status,
            EstimatedDate = x.EstimatedDate,
            EstimateStatus = x.EstimateStatus,
            Solution = x.Solution,
            Transport = x.Transport,
            Height = x.Height,
            Package = x.Package,
            Volume = x.Volume,
            Length = x.Length,
            Weight = x.Weight,
            Width = x.Width,
            CancelReason = x.CancelReason,
            DisplayOrder = x.DisplayOrder
        }).ToList();
        var deleteProductionOrdersDetail = dataProductionOrder.ListProductionOrdersDetail?.Where(x => request.ListProductionOrdersDetail.Where(f => f.Id != null).Select(f => f.Id).Contains(x.Id) == false)?.Select(x => new ListId()
        {
            Id = x.Id
        }).ToList();
        var ProductionOrderEditCommand = new ProductionOrderEditCommand(
            Id,
            Code: request.Code,
            Note: request.Note,
            Status: request.Status,
            RequestDate: request.RequestDate,
            CustomerId: request.CustomerId,
            CustomerCode: request.CustomerCode,
            CustomerName: request.CustomerName,
            Email: request.Email,
            Phone: request.Phone,
            Address: request.Address,
            EmployeeId: request.EmployeeId,
            EmployeeCode: request.EmployeeCode,
            EmployeeName: request.EmployeeName,
            DateNeed: request.DateNeed,
            OrderId: request.OrderId,
            OrderNumber: request.OrderNumber,
            SaleEmployeeId: request.SaleEmployeeId,
            SaleEmployeeCode: request.SaleEmployeeCode,
            SaleEmployeeName: request.SaleEmployeeName,
            Type: request.Type,
            estimateDate: request.EstimateDate,
            file: JsonConvert.SerializeObject(request.File),
            updateProductionOrdersDetail,
            deleteProductionOrdersDetail
       );

        var result = await _mediator.SendCommand(ProductionOrderEditCommand);

        return Ok(result);
    }

    /// <summary>
    /// Lấy thông tin
    /// </summary>
    /// <param name="id">Thông tin</param>
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new ProductionOrderQueryById(id));
        return Ok(rs);
    }

    /// <summary>
    /// Lấy thông tin theo mã
    /// </summary>
    /// <param name="id">Thông tin</param>
    [HttpGet("get-by-code/{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var rs = await _mediator.Send(new ProductionOrderQueryByCode(code));
        return Ok(rs);
    }

    [HttpPut("confirm")]
    public async Task<IActionResult> Put([FromBody] ConfirmProductionOrdersRequest request)
    {
        ProductionOrderDto dataProductionOrder = await _mediator.Send(new ProductionOrderQueryById(request.Id));
        if (dataProductionOrder == null)
            return BadRequest(new ValidationResult("ProductionOrder not exists"));

        var productionOrderConfirmCommand = new ProductionOrdersConfirmCommand(
          Id: request.Id,
          Status: request.Status
       );

        var result = await _mediator.SendCommand(productionOrderConfirmCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var ProductionOrderId = new Guid(id);
        if (await _mediator.Send(new ProductionOrderQueryById(ProductionOrderId)) == null)
            return BadRequest(new ValidationResult("ProductionOrder not exists"));

        var result = await _mediator.SendCommand(new ProductionOrderDeleteCommand(ProductionOrderId));

        return Ok(result);
    }

    [HttpPut("edit-new")]
    public async Task<IActionResult> PutNew([FromBody] EditProductionOrderRequestNew request)
    {
        var Id = new Guid(request.Id);
        ProductionOrderDto dataProductionOrder = await _mediator.Send(new ProductionOrderQueryById(Id));

        if (dataProductionOrder == null)
            return BadRequest(new ValidationResult("ProductionOrder not exists"));

        var useBy = _context.GetUserId();
        var updateProductionOrdersDetail = request.ListProductionOrdersDetail?.Select(x => new ProductionOrdersDetailDto()
        {
            Id = x.Id == null || x.Id == "" ? Guid.NewGuid() : new Guid(x.Id),
            ProductionOrdersId = Id,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            Sku = x.Sku,
            Gtin = x.Gtin,
            Origin = x.Origin,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            Quantity = x.Quantity,
            Note = x.Note,
            EstimatedDeliveryQuantity = x.EstimatedDeliveryQuantity,
            DeliveryDate = x.DeliveryDate,
            IsWorkOrdered = x.IsWorkOrdered,
            ProductionOrdersCode = x.ProductionOrdersCode,
            IsEstimated = x.IsEstimated,
            IsBom = x.IsBom
        }).ToList();
        var deleteProductionOrdersDetail = request.DeleteProductionOrdersDetail?.Select(x => new ListId()
        {
            Id = x.Id
        }).ToList();
        var ProductionOrderEditCommand = new ProductionOrderEditCommand(
            Id,
            Code: request.Code,
            Note: request.Note,
            Status: request.Status,
            RequestDate: request.RequestDate,
            CustomerId: request.CustomerId,
            CustomerCode: request.CustomerCode,
            CustomerName: request.CustomerName,
            Email: request.Email,
            Phone: request.Phone,
            Address: request.Address,
            EmployeeId: request.EmployeeId,
            EmployeeCode: request.EmployeeCode,
            EmployeeName: request.EmployeeName,
            DateNeed: request.DateNeed,
            OrderId: request.OrderId,
            OrderNumber: request.OrderNumber,
            SaleEmployeeId: request.SaleEmployeeId,
            SaleEmployeeCode: request.SaleEmployeeCode,
            SaleEmployeeName: request.SaleEmployeeName,
            Type: request.Type,
            estimateDate: request.EstimateDate,
            file: JsonConvert.SerializeObject(request.File),
            updateProductionOrdersDetail,
            deleteProductionOrdersDetail
       );

        var result = await _mediator.SendCommand(ProductionOrderEditCommand);

        return Ok(result);
    }

    [HttpGet("excel-template")]
    public async Task<IActionResult> GetExcelTemplate([FromQuery] ImportProductionOrdersRequest request)
    {
        var rs = await _mediator.Send(new ProductionOrderExportTemplateQuery(request.Type));
        if (rs != null)
        {
            return File(rs.ToArray(), "application/xlsx");
        }
        return Ok(rs);
    }

    [HttpPost("validate-excel")]
    public async Task<IActionResult> ValidateExcel([FromForm] ValidateProductionOrders request)
    {
        List<ValidateField> listField = new List<ValidateField>()
        {
            new ValidateField(){Field="productCode", IndexColumn= request.ProductCode},
            new ValidateField(){Field="productName", IndexColumn= request.ProductName},
            new ValidateField(){Field="unitCode", IndexColumn= request.UnitCode},
            new ValidateField(){Field="unitName", IndexColumn= request.UnitName},
            new ValidateField(){Field="quantityRequest", IndexColumn= request.QuantityRequest},
            new ValidateField(){Field="note", IndexColumn= request.Note},
        };

        var data = new ValidateExcelExportWarehouseQuery(request.File,
                                                    request.SheetId,
                                                    request.HeaderRow,
                                                    listField);
        var result = await _mediator.Send(data);
        return Ok(result);
    }
}
