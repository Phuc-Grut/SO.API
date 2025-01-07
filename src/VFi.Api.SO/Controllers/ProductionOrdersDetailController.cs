using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductionOrdersDetailController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ProductionOrdersDetailController> _logger;

    public ProductionOrdersDetailController(IMediatorHandler mediator, IContextUser context, ILogger<ProductionOrdersDetailController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    public class FilterQueryDetail : FopPagingRequest
    {
        public int? Type { get; set; }
        public int? OrderStatus { get; set; }
        public int? ProductOrderStatus { get; set; }
        public Guid? ProcessId { get; set; }
    }

    /// <summary>
    /// Lấy danh sách  theo phân trang
    /// </summary>
    /// <param name="request"> phân trang</param>
    /// <returns>List Insurance</returns>
    [HttpGet("paging")]
    public async Task<IActionResult> Pagedresult([FromQuery] FilterQueryDetail request)
    {
        var query = new ProductionOrdersDetailPagingFilterQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize, request.Type, request.ProductOrderStatus);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("paging-count-total")]
    public async Task<IActionResult> PagedresultCountTotal([FromQuery] FilterQueryDetail request)
    {
        var query = new ProductionOrdersDetailPagingFilterQueryCountTotal(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Lấy thông tin
    /// </summary>
    /// <param name="id">Thông tin</param>
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new ProductionOrdersDetailQueryById(id));
        return Ok(rs);
    }

    [HttpPut("edit-packing")]
    public async Task<IActionResult> Put([FromBody] EditPackingProductionOrdersDetailRequest request)
    {
        ProductionOrdersDetailDto dataProductionOrderDetail = await _mediator.Send(new ProductionOrdersDetailQueryById(request.Id));

        if (dataProductionOrderDetail == null)
            return BadRequest(new ValidationResult("ProductionOrdersDetail not exists"));

        var productionOrdersDetailEditPackageCommand = new ProductionOrdersDetailEditPackageCommand(
            Id: request.Id,
            Solution: request.Solution,
            Transport: request.Transport,
            Height: request.Height,
            Package: request.Package,
            Volume: request.Volume,
            Length: request.Length,
            Weight: request.Weight,
            Width: request.Width
       );
        var result = await _mediator.SendCommand(productionOrdersDetailEditPackageCommand);
        return Ok(result);
    }

    [HttpPut("cancel")]
    public async Task<IActionResult> Put([FromBody] CancelProductionOrdersDetailRequest request)
    {
        ProductionOrdersDetailDto dataPo = await _mediator.Send(new ProductionOrdersDetailQueryById(request.Id));
        if (dataPo == null)
            return BadRequest(new ValidationResult("Product not exists"));

        var productionOrdersDetailCancelCommand = new ProductionOrdersDetailCancelCommand(
          Id: request.Id,
          Status: request.Status,
          CancelReason: request.CancelReason
       );

        var result = await _mediator.SendCommand(productionOrdersDetailCancelCommand);

        return Ok(result);
    }

    [HttpPut("complete")]
    public async Task<IActionResult> Complete([FromBody] CompleteProductionOrdersDetailRequest request)
    {
        ProductionOrdersDetailDto dataPo = await _mediator.Send(new ProductionOrdersDetailQueryById(request.Id));
        if (dataPo == null)
            return BadRequest(new ValidationResult("Product not exists"));

        var productionOrdersDetailCompleteCommand = new ProductionOrdersDetailCompleteCommand(
          Id: request.Id,
          Status: request.Status
       );

        var result = await _mediator.SendCommand(productionOrdersDetailCompleteCommand);

        return Ok(result);
    }
}
