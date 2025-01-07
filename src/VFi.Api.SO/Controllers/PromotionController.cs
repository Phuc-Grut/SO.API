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
public class PromotionController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<PromotionController> _logger;

    public PromotionController(IMediatorHandler mediator, IContextUser context, ILogger<PromotionController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new PromotionQueryComboBox(request.Status));
        return Ok(result);
    }

    /// <summary>
    /// Lấy thông tin
    /// </summary>
    /// <param name="id">Thông tin</param>
    /// <returns>Promotion</returns>
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new PromotionQueryById(id));
        return Ok(rs);
    }

    public class PromotionFopPagingRequest : FopPagingRequest
    {
        [FromQuery(Name = "$promotionGroupId")]
        public Guid? PromotionGroupId { get; set; }
    }

    /// <summary>
    /// Lấy danh sách  theo phân trang
    /// </summary>
    /// <param name="request"> phân trang</param>
    /// <returns>List Promotion</returns>
    [HttpGet("paging")]
    public async Task<IActionResult> Pagedresult([FromQuery] PromotionFopPagingRequest request)
    {
        var query = new PromotionPagingFilterQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize, request.PromotionGroupId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Nhập thông tin
    /// </summary>
    /// <param name="request">thông tin</param>
    /// <returns>Promotion</returns>
    [HttpPost("add")]
    public async Task<IActionResult> Post([FromBody] AddPromotionRequest request)
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
        var create = _context.GetUserId();
        var createdDate = DateTime.Now;

        var addDetail = request.Details?.Select(x => new PromotionByValueDto()
        {
            PromotionId = Id,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            Type = x.Type,
            MinOrderPrice = x.MinOrderPrice,
            LimitTotalValue = x.LimitTotalValue,
            DiscountPercent = x.DiscountPercent,
            ReduceAmount = x.ReduceAmount,
            FixPrice = x.FixPrice,
            TypeBonus = x.TypeBonus,
            TypeBuy = x.TypeBuy,
            Quantity = x.Quantity,
            QuantityBuy = x.QuantityBuy,
            CreatedDate = createdDate,
            CreatedBy = create,
            CreatedByName = _context.UserName,
            ProductBonus = x?.ProductBonus?.Select(y => new PromotionProductDto()
            {
                ProductId = y.ProductId,
                ProductCode = y.ProductCode,
                ProductName = y.ProductName,
                Quantity = y.Quantity
            }).ToList(),
            ProductBuy = x?.ProductBuy?.Select(y => new PromotionProductBuyDto()
            {
                ProductId = y.ProductId,
                ProductCode = y.ProductCode,
                ProductName = y.ProductName,
                Quantity = y.Quantity
            }).ToList(),
        }).ToList();

        var data = new AddPromotionCommand(
            Id,
            request.PromotionGroupId,
            Code,
            request.Name,
            request.Description,
            request.Status,
            request.StartDate,
            request.EndDate,
            request.StartTime,
            request.EndTime,
            request.Stores,
            request.SalesChannel,
            request.ApplyTogether,
            request.ApplyAllCustomer,
            request.Type,
            request.PromotionMethod,
            request.UsingCode,
            request.ApplyBirthday,
            request.PromotionalCode,
            request.IsLimit,
            request.PromotionLimit,
            request.Applytax,
            request.DisplayType,
            request.PromotionBase,
            request.ObjectApply,
            request.Condition,
            request.Apply,
            create,
            _context.UserName,
            request.CustomerGroups,
            request.Customers,
            addDetail
        );

        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    /// <summary>
    /// Cập nhật thông tin
    /// </summary>
    /// <param name="request">Thông  tin</param>
    /// <returns>Promotion</returns>
    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditPromotionRequest request)
    {
        var useBy = _context.GetUserId();
        var useDate = DateTime.Now;

        var updateDetail = request.Details?.Select(x => new PromotionByValueDto()
        {
            Id = !String.IsNullOrEmpty(x.Id) ? new Guid(x.Id) : null,
            PromotionId = request.Id,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            Type = x.Type,
            MinOrderPrice = x.MinOrderPrice,
            LimitTotalValue = x.LimitTotalValue,
            DiscountPercent = x.DiscountPercent,
            ReduceAmount = x.ReduceAmount,
            FixPrice = x.FixPrice,
            TypeBonus = x.TypeBonus,
            TypeBuy = x.TypeBuy,
            Quantity = x.Quantity,
            QuantityBuy = x.QuantityBuy,
            UpdatedBy = useBy,
            UpdatedByName = _context.UserName,
            ProductBonus = x?.ProductBonus?.Select(y => new PromotionProductDto()
            {
                Id = !String.IsNullOrEmpty(x.Id) ? new Guid(x.Id) : null,
                ProductId = y.ProductId,
                ProductCode = y.ProductCode,
                ProductName = y.ProductName,
                Quantity = y.Quantity
            }).ToList(),
            ProductBuy = x?.ProductBuy?.Select(y => new PromotionProductBuyDto()
            {
                Id = !String.IsNullOrEmpty(x.Id) ? new Guid(x.Id) : null,
                ProductId = y.ProductId,
                ProductCode = y.ProductCode,
                ProductName = y.ProductName,
                Quantity = y.Quantity
            }).ToList(),
        }).ToList();

        var deleteDetail = request.Deletes?.Select(x => new DeletePromotionByValueDto()
        {
            Id = (Guid)x.Id
        }).ToList();

        var deleteBonus = request.DeleteBonus?.Select(x => new DeletePromotionProductDto()
        {
            Id = (Guid)x.Id
        }).ToList();
        var deleteBuy = request.DeleteBuy?.Select(x => new DeletePromotionProductDto()
        {
            Id = (Guid)x.Id
        }).ToList();
        var data = new EditPromotionCommand(
            request.Id,
            request.PromotionGroupId,
            request.Code,
            request.Name,
            request.Description,
            request.Status,
            request.StartDate,
            request.EndDate,
            request.StartTime,
            request.EndTime,
            request.Stores,
            request.SalesChannel,
            request.ApplyTogether,
            request.ApplyAllCustomer,
            request.Type,
            request.PromotionMethod,
            request.UsingCode,
            request.ApplyBirthday,
            request.PromotionalCode,
            request.IsLimit,
            request.PromotionLimit,
            request.Applytax,
            request.DisplayType,
            request.PromotionBase,
            request.ObjectApply,
            request.Condition,
            request.Apply,
            useBy,
            _context.UserName,
            request.CustomerGroups,
            request.Customers,
            updateDetail,
            deleteDetail,
            deleteBonus,
            deleteBuy
       );

        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    /// <summary>
    /// xoá
    /// </summary>
    /// <param name="id">Mã</param>
    /// <returns>Promotion</returns>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var Promotion = await _mediator.Send(new PromotionQueryById(id));

        if (Promotion == null)
        {
            return BadRequest(new ValidationResult("Promotion not exists"));
        }

        var data = new DeletePromotionCommand(id);

        var result = await _mediator.SendCommand(data);

        return Ok(result);
    }
}
