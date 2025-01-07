using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.Domain.SO.Enums;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public partial class RequestPurchaseController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<RequestPurchaseController> _logger;

    public RequestPurchaseController(IMediatorHandler mediator, IContextUser context,
        ILogger<RequestPurchaseController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new RequestPurchaseQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpPost("duplicate")]
    public async Task<IActionResult> Duplicate([FromBody] DuplicateRequestPurchase request)
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
        var item = new RequestPurchaseDuplicateCommand(
            Guid.NewGuid(),
            request.RequestPurchaseId,
            Code
            );
        var result = await _mediator.SendCommand(item);

        return Ok(new { errors = result.Errors, isValid = result.IsValid, ruleSetsExecuted = result.RuleSetsExecuted, returnCode = Code });
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new RequestPurchaseQueryById(id));
        return Ok(rs);
    }

    /// <summary>
    /// Lấy danh sách  theo phân trang
    /// </summary>
    /// <param name="request"> phân trang</param>
    /// <returns>List RequestPurchase</returns>
    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] RequestPurchasePagingRequest request)
    {
        if (_context.QueryMyData())
        {
            request.EmployeeId = _context.GetUserId().ToString();
        }
        var query = new RequestPurchasePagingQuery(
              request.Keyword,
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

    /// <summary>
    /// Nhập thông tin
    /// </summary>
    /// <param name="request">thông tin</param>
    /// <returns>RequestPurchase</returns>
    [HttpPost("add")]
    public async Task<IActionResult> Post([FromBody] AddRequestPurchaseRequest request)
    {
        var RequestPurchaseId = Guid.NewGuid();
        var create = _context.GetUserId();
        var createName = _context.UserClaims.FullName;
        var createdDate = DateTime.Now;
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
        var listDetail = request.Details?.Select(x => new RequestPurchaseProductDto()
        {
            Id = Guid.NewGuid(),
            RequestPurchaseId = RequestPurchaseId,
            OrderId = x.OrderId,
            OrderCode = x.OrderCode,
            OrderProductId = x.OrderProductId,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            SourceLink = x.SourceLink,
            ShippingFee = x.ShippingFee,
            Origin = x.Origin,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            UnitType = x.UnitType,
            QuantityRequest = x.QuantityRequest,
            QuantityApproved = x.QuantityApproved,
            UnitPrice = x.UnitPrice,
            Currency = x.Currency,
            DeliveryDate = x.DeliveryDate,
            PriorityLevel = x.PriorityLevel,
            Note = x.Note,
            VendorCode = x.VendorCode,
            VendorName = x.VendorName,
            BidUsername = x.BidUsername,
            StatusPurchase = (int)RequestPurchaseStatus.Pending,
            Status = x.Status,
        }).ToList();
        var sumQtyRequest = request.Details.Sum(x => x.QuantityRequest);
        var sumQtyApproved = request.Details.Sum(x => x.QuantityApproved);
        var addRequestPurchaseCommand = new AddRequestPurchaseCommand(
          RequestPurchaseId,
          Code,
          request.RequestBy,
          request.RequestByName,
          request.RequestByEmail,
          request.RequestDate,
          request.CurrencyCode,
          request.CurrencyName,
          request.Calculation,
          request.ExchangeRate,
          request.Proposal,
          request.Note,
          request.ApproveDate,
          request.ApproveBy,
          request.ApproveByName,
          request.ApproveComment,
          request.Status,
          sumQtyRequest,
          sumQtyApproved,
          request.OrderId,
          request.OrderCode,
          JsonConvert.SerializeObject(request.File),
          listDetail
      );
        var result = await _mediator.SendCommand(addRequestPurchaseCommand);
        return Ok(new { errors = result.Errors, isValid = result.IsValid, ruleSetsExecuted = result.RuleSetsExecuted, returnCode = Code, returnId = RequestPurchaseId });
    }

    [HttpPut("process")]
    public async Task<IActionResult> Put([FromBody] ProcessRequestPurchaseRequest request)
    {
        var _id = new Guid(request.Id);

        var RequestPurchaseEditCommand = new RequestPurchaseProcessCommand(
            _id,
            request.Status,
            request.ApproveComment,
            request.POStatus
        );

        var result = await _mediator.SendCommand(RequestPurchaseEditCommand);
        return Ok(result);
    }

    /// <summary>
    /// Cập nhật thông tin
    /// </summary>
    /// <param name="request">Thông  tin</param>
    /// <returns>RequestPurchase</returns>
    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditRequestPurchaseRequest request)
    {
        var RequestPurchase = await _mediator.Send(new RequestPurchaseQueryById(request.Id));
        if (RequestPurchase == null)
        {
            return BadRequest(new ValidationResult("RequestPurchase not exists"));
        }

        var updateDetail = request.Details?.Select(u => new RequestPurchaseProductDto()
        {
            Id = (Guid)(u.Id == null ? Guid.NewGuid() : u.Id),
            RequestPurchaseId = u.RequestPurchaseId,
            OrderId = u.OrderId,
            OrderCode = u.OrderCode,
            OrderProductId = u.OrderProductId,
            ProductId = u.ProductId,
            ProductCode = u.ProductCode,
            ProductName = u.ProductName,
            ProductImage = u.ProductImage,
            Origin = u.Origin,
            UnitCode = u.UnitCode,
            UnitName = u.UnitName,
            UnitType = u.UnitType,
            QuantityRequest = u.QuantityRequest,
            QuantityApproved = u.QuantityApproved,
            QuantityPurchased = u.QuantityPurchased,
            StatusPurchase = u.StatusPurchase,
            UnitPrice = u.UnitPrice,
            Currency = u.Currency,
            DeliveryDate = u.DeliveryDate,
            PriorityLevel = u.PriorityLevel,
            Status = u.Status,
            Note = u.Note,
            VendorCode = u.VendorCode,
            VendorName = u.VendorName,
        }).ToList();
        var sumQtyRequest = request.Details.Sum(x => x.QuantityRequest);
        var sumQtyApproved = request.Details.Sum(x => x.QuantityApproved);
        var deleteDetail = RequestPurchase.Details.Where(x => request.Details.Where(f => f.Id != null).Select(f => f.Id).Contains(x.Id) == false)?.Select(x => new ListId()
        {
            Id = (Guid)x.Id
        }).ToList();

        var data = new EditRequestPurchaseCommand(
            request.Id,
            request.Code,
            request.RequestBy,
            request.RequestByName,
            request.RequestByEmail,
            request.RequestDate,
            request.CurrencyCode,
            request.CurrencyName,
            request.Calculation,
            request.ExchangeRate,
            request.Proposal,
            request.Note,
            request.ApproveDate,
            request.ApproveBy,
            request.ApproveByName,
            request.ApproveComment,
            request.Status,
            sumQtyRequest,
            sumQtyApproved,
            request.OrderId,
            request.OrderCode,
            JsonConvert.SerializeObject(request.File),
            updateDetail,
            deleteDetail
       );

        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    /// <summary>
    /// xoá
    /// </summary>
    /// <param name="id">Mã</param>
    /// <returns>RequestPurchase</returns>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var RequestPurchase = await _mediator.Send(new RequestPurchaseQueryById(id));

        if (RequestPurchase == null)
        {
            return BadRequest(new ValidationResult("RequestPurchase not exists"));
        }

        var data = new DeleteRequestPurchaseCommand(id);

        var result = await _mediator.SendCommand(data);

        return Ok(result);
    }

    [HttpDelete("remove-order/{requestPurchaseId}")]
    public async Task<IActionResult> RemoveOrder(Guid requestPurchaseId, [FromQuery] Guid? orderId)
    {
        var data = new DeleteOrderRequestPurchaseCommand(requestPurchaseId, orderId);
        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    [HttpGet("excel-template")]
    public async Task<IActionResult> GetExcelTemplate()
    {
        var rs = await _mediator.Send(new RequestPurchaseExportTemplateQuery());
        if (rs != null)
        {
            return File(rs.ToArray(), "application/xlsx");
        }
        return Ok(rs);
    }

    [HttpPost("validate-excel")]
    public async Task<IActionResult> ValidateExcel([FromForm] ValidateExcelExportWarehouset request)
    {
        List<ValidateField> listField = new List<ValidateField>()
        {
            new ValidateField(){Field="productCode", IndexColumn= request.ProductCode},
            new ValidateField(){Field="productName", IndexColumn= request.ProductName},
            new ValidateField(){Field="unitCode", IndexColumn= request.UnitCode},
            new ValidateField(){Field="unitName", IndexColumn= request.UnitName},
            new ValidateField(){Field="unitPrice", IndexColumn= request.UnitPrice},
            new ValidateField(){Field="deliveryDate", IndexColumn= request.DeliveryDate},
            new ValidateField(){Field="priorityLevel", IndexColumn= request.PriorityLevel},
            new ValidateField(){Field="quantityRequest", IndexColumn= request.QuantityRequest},
            new ValidateField(){Field="note", IndexColumn= request.Note},
        };

        var data = new ValidateExcelRequestPurchaseQuery(request.File,
                                                    request.SheetId,
                                                    request.HeaderRow,
                                                    listField);
        var result = await _mediator.Send(data);
        return Ok(result);
    }


    [HttpPut("put-order")]
    public async Task<IActionResult> PutOrder([FromBody] RequestPurchaseAddOrdersRequest request)
    {
        var requestPurchaseUpdateCommand = new RequestPurchaseAddOrdersCommand(request.Id, request.OrderIds);
        var result = await _mediator.SendCommand(requestPurchaseUpdateCommand);
        return Ok(result);
    }
}
