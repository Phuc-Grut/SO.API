using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExportController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ExportController> _logger;

    public ExportController(IMediatorHandler mediator, IContextUser context, ILogger<ExportController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new ExportQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new ExportQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] ExportRequest request)
    {
        var query = new ExportPagingQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddExportRequest request)
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
        request.TotalQuantity = request.Details.Sum(x => x.Quantity);
        var addDetail = request.Details?.Select(x => new ExportProductDto()
        {
            ExportId = Id,
            ExportWarehouseProductId = x.ExportWarehouseProductId,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            Origin = x.Origin,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            Quantity = x.Quantity,
            Note = x.Note
        }).ToList();
        var ExportAddCommand = new ExportAddCommand(
          Id,
          !string.IsNullOrEmpty(request.ExportWarehouseId) ? new Guid(request.ExportWarehouseId) : null,
          Code,
          request.ExportDate,
          request.EmployeeId,
          request.EmployeeCode,
          request.EmployeeName,
          request.Status,
          request.TotalQuantity,
          request.Note,
          JsonConvert.SerializeObject(request.File),
          request.ApproveBy,
          request.ApproveDate,
          request.ApproveByName,
          request.ApproveComment,
          _context.GetUserId(),
          _context.UserClaims.FullName,
          addDetail
      );
        var result = await _mediator.SendCommand(ExportAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditExportRequest request)
    {
        var useBy = _context.GetUserId();
        var Export = await _mediator.Send(new ExportQueryById(request.Id));
        if (Export == null)
        {
            return BadRequest(new ValidationResult("Export not exists"));
        }
        request.TotalQuantity = request.Details.Sum(x => x.Quantity);
        //chi tiết
        var updateDetail = request.Details?.Select(x => new ExportProductDto()
        {
            Id = x.Id != null ? x.Id : Guid.NewGuid(),
            ExportId = x.ExportId,
            ExportWarehouseProductId = x.ExportWarehouseProductId,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            Origin = x.Origin,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            Quantity = x.Quantity,
            Note = x.Note
        }).ToList();

        var deleteDetail = Export.Details.Where(x => request.Details.Where(f => f.Id != null).Select(f => f.Id).Contains(x.Id) == false)?.Select(x => new ListId()
        {
            Id = (Guid)x.Id
        }).ToList();

        var ExportEditCommand = new ExportEditCommand(
          request.Id,
           !string.IsNullOrEmpty(request.ExportWarehouseId) ? new Guid(request.ExportWarehouseId) : null,
          request.Code,
          request.ExportDate,
          request.EmployeeId,
          request.EmployeeCode,
          request.EmployeeName,
          request.Status,
          request.TotalQuantity,
          request.Note,
          JsonConvert.SerializeObject(request.File),
          request.ApproveBy,
          request.ApproveDate,
          request.ApproveByName,
          request.ApproveComment,
          _context.GetUserId(),
          _context.UserClaims.FullName,
          updateDetail,
          deleteDetail
       );

        var result = await _mediator.SendCommand(ExportEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var ExportId = new Guid(id);
        if (await _mediator.Send(new ExportQueryById(ExportId)) == null)
            return BadRequest(new ValidationResult("Export not exists"));

        var result = await _mediator.SendCommand(new ExportDeleteCommand(ExportId));

        return Ok(result);
    }

    [HttpPost("approval")]
    public async Task<IActionResult> approval([FromBody] ApprovalExportRequest request)
    {
        var command = new ApprovalExportCommand(
            request.Id,
            request.Code,
            request.ExportDate,
            request.EmployeeId,
            request.EmployeeCode,
            request.EmployeeName,
            request.Status,
            request.TotalQuantity,
            request.Note,
            request.ApproveBy,
            request.ApproveDate,
            request.ApproveByName,
            request.ApproveComment
            );

        var result = await _mediator.SendCommand(command);

        return Ok(result);
    }
}
