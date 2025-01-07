using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Commands.Extended;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.Application.SO.Queries.Extended;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public partial class OrderExpressController : ControllerBase
{
    private readonly IContextUser _contextUser;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<OrderExpressController> _logger;

    public OrderExpressController(IMediatorHandler mediator, ILogger<OrderExpressController> logger, IContextUser contextUser)
    {
        _mediator = mediator;
        _logger = logger;
        _contextUser = contextUser;
    }

    //[ProducesResponseType(typeof(PagedResult<List<OrderExpressDto>>), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[SwaggerOperation(Summary = "Get orders by user")]
    //[HttpGet("paging-by-account")]
    //public async Task<PagedResult<List<OrderExpressDto>>> Paging([FromQuery] OrderExpressPagingByAccountQuery request)
    //{
    //    var result = await _mediator.Send(request);
    //    return result;
    //}

    [ProducesResponseType(typeof(OrderExpressDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Get order express by user and order express code")]
    [HttpGet("code")]
    public async Task<OrderExpressDto> GetByCode(Guid accountId, string code)
    {
        var query = new OrderExpressByCodeQuery(accountId, code);
        var result = await _mediator.Send(query);
        return result;
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new OrderExpressQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new OrderExpressQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] OrderExpressRequest request)
    {
        var query = new OrderExpressPagingQuery(request.Keyword, request.Status, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("create-by-customer")]
    public async Task<IActionResult> CreateByCustomer([FromBody] CreateOrderExpressRequest request, CancellationToken cancellationToken)
    {
        var orderExpressId = Guid.NewGuid();
        var createdBy = _contextUser.GetUserId();
        var createdByName = _contextUser.FullName;

        var code = await _mediator.Send(new GetCodeQuery("OEX", 1));

        var details = request.Products.Select(x => new OrderExpressDetailDto
        {
            Id = Guid.NewGuid(),
            OrderExpressId = orderExpressId,
            ProductName = x.Name,
            ProductImage = x.Image,
            Origin = x.Origin,
            UnitName = x.UnitName,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice,
            Note = x.Note,
            CommodityGroup = x.CommodityGroup,
        }).ToList();

        var serviceAdd = request.ServiceAdd.Select(x => new OrderServiceAddDto
        {
            Id = Guid.NewGuid(),
            OrderExpressId = orderExpressId,
            ServiceAddId = x.ServiceAddId,
            ServiceAddName = x.ServiceAddName,
            Status = x.Status,
            Currency = x.Currency,
            ExchangeRate = x.ExchangeRate,
            Note = x.Note,
            Price = x.Price,
        }).ToList();

        var cmd = new OrderExpressCreateByCustomerCommand(
                                    orderExpressId,
                                    request.AccountId,
                                    request.AccountName,
                                    code,
                                    request.DeliveryName,
                                    request.DeliveryPhone,
                                    request.DeliveryAddress,
                                    request.DeliveryCountry,
                                    request.DeliveryProvince,
                                    request.DeliveryDistrict,
                                    request.DeliveryWard,
                                    request.DeliveryNote,
                                    request.ShippingMethodId,
                                    request.ShippingMethodName,
                                    request.ShippingMethodCode,
                                    request.Note,
                                    request.RouterShipping,
                                    request.Currency,
                                    request.Weight,
                                    request.Width,
                                    request.Height,
                                    request.Length,
                                    request.Image,
                                    request.DomesticTracking,
                                    details,
                                    serviceAdd,
                                    createdBy,
                                    DateTime.Now,
                                    createdByName);

        if (request.AccountId.HasValue)
        {
            var queryCustomerId = new CustomerIdQueryByAccountId() { AccountId = request.AccountId.Value };
            cmd.CustomerId = await _mediator.Send(queryCustomerId, cancellationToken);
        }

        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Post([FromBody] AddOrderExpressRequest request)
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
        var createdBy = _contextUser.GetUserId();
        var createdDate = DateTime.Now;
        var createdByName = _contextUser.FullName;

        var cmd = new OrderExpressAddCommand(
            Id,
            request.OrderType,
            request.Code,
            request.OrderDate,
            request.CustomerId,
            request.CustomerName,
            request.CustomerCode,
            request.StoreId,
            request.StoreCode,
            request.StoreName,
            request.ContractId,
            request.ContractName,
            request.Currency,
            request.ExchangeRate,
            request.ShippingMethodId,
            request.ShippingMethodCode,
            request.ShippingMethodName,
            request.RouterShipping,
            request.DomesticTracking,
            request.DomesticCarrier,
            request.Status,
            request.PaymentTermId,
            request.PaymentTermName,
            request.PaymentMethodName,
            request.PaymentMethodId,
            request.PaymentStatus,
            request.ShipperName,
            request.ShipperPhone,
            request.ShipperZipCode,
            request.ShipperAddress,
            request.ShipperCountry,
            request.ShipperProvince,
            request.ShipperDistrict,
            request.ShipperWard,
            request.ShipperNote,
            request.DeliveryName,
            request.DeliveryPhone,
            request.DeliveryZipCode,
            request.DeliveryAddress,
            request.DeliveryCountry,
            request.DeliveryProvince,
            request.DeliveryDistrict,
            request.DeliveryWard,
            request.DeliveryNote,
            request.EstimatedDeliveryDate,
            request.DeliveryMethodId,
            request.DeliveryMethodCode,
            request.DeliveryMethodName,
            request.DeliveryStatus,
            request.IsBill,
            request.BillName,
            request.BillAddress,
            request.BillCountry,
            request.BillProvince,
            request.BillDistrict,
            request.BillWard,
            request.BillStatus,
            request.Description,
            request.Note,
            request.GroupEmployeeId,
            request.GroupEmployeeName,
            request.CommodityGroup,
            request.AirFreight,
            request.SeaFreight,
            request.Surcharge,
            request.Weight,
            request.Width,
            request.Height,
            request.Length,
            request.Image,
            request.Paid,
            request.Total,
            request.AccountId,
            request.AccountName,
            createdBy,
            createdDate,
            createdByName,
            request.RouterShippingId,
            request.ShippingCodePost,
            request.TrackingCode,
            request.TrackingCarrier,
            request.Package,
            request.ToDeliveryDate,
            request.OrderExpressDetail?.Select((x, i) => new OrderExpressDetailDto
            {
                Id = Guid.NewGuid(),
                OrderExpressId = Id,
                ProductName = x.ProductName,
                ProductCode = x.ProductCode,
                ProductImage = x.ProductImage,
                ProductLink = x.ProductLink,
                Origin = x.Origin,
                UnitName = x.UnitName,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                DisplayOrder = x.DisplayOrder,
                Note = x.Note,
                CommodityGroup = x.CommodityGroup,
                SurchargeGroup = x.SurchargeGroup,
                Surcharge = x.Surcharge,
                CreatedBy = createdBy,
                CreatedDate = createdDate,
                CreatedByName = createdByName,
            }).ToList(),
            request.OrderServiceAdd?.Select(x => new OrderServiceAddDto()
            {
                OrderExpressId = Id,
                ServiceAddId = x.ServiceAddId,
                ServiceAddName = x.ServiceAddName,
                Price = x.Price,
                Currency = x.Currency,
                Calculation = x.Calculation,
                Status = x.Status,
                Note = x.Note,
                ExchangeRate = x.ExchangeRate,
                DisplayOrder = x.DisplayOrder,
                CreatedBy = createdBy,
                CreatedDate = createdDate,
                CreatedByName = createdByName,
            }).ToList(),
            request.PaymentInvoice?.Select(x => new PaymentInvoiceDto()
            {
                Type = x.Type,
                Code = x.Code,
                OrderExpressId = Id,
                OrderExpressCode = Code,
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
                Locked = x.Locked,
                PaymentStatus = x.PaymentStatus,
                AccountId = x.AccountId,
                AccountName = x.AccountName,
                CustomerId = request.CustomerId,
                CustomerName = request.CustomerName,
                CreatedBy = createdBy,
                CreatedDate = createdDate,
                CreatedByName = createdByName,
            }).ToList(),
            request.OrderInvoice?.Select(x => new OrderInvoiceDto()
            {
                Serial = x.Serial,
                Symbol = x.Symbol,
                Number = x.Number,
                Value = x.Value,
                Date = x.Date,
                Note = x.Note,
                DisplayOrder = x.DisplayOrder,
                CreatedBy = createdBy,
                CreatedDate = createdDate,
                CreatedByName = createdByName,
            }).ToList()
            );

        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditOrderExpressRequest request)
    {
        var item = await _mediator.Send(new OrderExpressQueryById(request.Id));

        if (item == null)
            return BadRequest(new ValidationResult("OrderExpress not exists"));

        var updatedBy = _contextUser.GetUserId();
        var updatedDate = DateTime.Now;
        var updatedByName = _contextUser.FullName;

        var cmd = new OrderExpressEditCommand(
            request.Id,
            request.OrderType,
            request.Code,
            request.OrderDate,
            request.CustomerId,
            request.CustomerName,
            request.CustomerCode,
            request.StoreId,
            request.StoreCode,
            request.StoreName,
            request.ContractId,
            request.ContractName,
            request.Currency,
            request.ExchangeRate,
            request.ShippingMethodId,
            request.ShippingMethodCode,
            request.ShippingMethodName,
            request.RouterShipping,
            request.DomesticTracking,
            request.DomesticCarrier,
            request.Status,
            request.PaymentTermId,
            request.PaymentTermName,
            request.PaymentMethodName,
            request.PaymentMethodId,
            request.PaymentStatus,
            request.ShipperName,
            request.ShipperPhone,
            request.ShipperZipCode,
            request.ShipperAddress,
            request.ShipperCountry,
            request.ShipperProvince,
            request.ShipperDistrict,
            request.ShipperWard,
            request.ShipperNote,
            request.DeliveryName,
            request.DeliveryPhone,
            request.DeliveryZipCode,
            request.DeliveryAddress,
            request.DeliveryCountry,
            request.DeliveryProvince,
            request.DeliveryDistrict,
            request.DeliveryWard,
            request.DeliveryNote,
            request.EstimatedDeliveryDate,
            request.DeliveryMethodId,
            request.DeliveryMethodCode,
            request.DeliveryMethodName,
            request.DeliveryStatus,
            request.IsBill,
            request.BillName,
            request.BillAddress,
            request.BillCountry,
            request.BillProvince,
            request.BillDistrict,
            request.BillWard,
            request.BillStatus,
            request.Description,
            request.Note,
            request.GroupEmployeeId,
            request.GroupEmployeeName,
            request.CommodityGroup,
            request.AirFreight,
            request.SeaFreight,
            request.Surcharge,
            request.Weight,
            request.Width,
            request.Height,
            request.Length,
            request.Image,
            request.Paid,
            request.Total,
            request.AccountId,
            request.AccountName,
            updatedBy,
            updatedDate,
            updatedByName,
            request.RouterShippingId,
            request.ShippingCodePost,
            request.TrackingCode,
            request.TrackingCarrier,
            request.Package,
            request.ToDeliveryDate,
            request.OrderExpressDetail?.Select(x => new OrderExpressDetailDto
            {
                Id = x.Id is not null ? (Guid)x.Id : Guid.NewGuid(),
                OrderExpressId = request.Id,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage,
                ProductLink = x.ProductLink,
                Origin = x.Origin,
                UnitName = x.UnitName,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                Note = x.Note,
                CommodityGroup = x.CommodityGroup,
                SurchargeGroup = x.SurchargeGroup,
                Surcharge = x.Surcharge,
                DisplayOrder = x.DisplayOrder,
                UpdatedBy = updatedBy,
                UpdatedDate = updatedDate,
                UpdatedByName = updatedByName,
            }).ToList(),
            request.OrderServiceAdd?.Select(x => new OrderServiceAddDto()
            {
                Id = !String.IsNullOrEmpty(x.Id) ? new Guid(x.Id) : null,
                OrderExpressId = request.Id,
                ServiceAddId = x.ServiceAddId,
                ServiceAddName = x.ServiceAddName,
                Price = x.Price,
                Currency = x.Currency,
                Calculation = x.Calculation,
                Status = x.Status,
                Note = x.Note,
                ExchangeRate = x.ExchangeRate,
                DisplayOrder = x.DisplayOrder
            }).ToList(),
            request.PaymentInvoice?.Select(x => new PaymentInvoiceDto()
            {
                Id = x.Id ?? Guid.NewGuid(),
                Type = x.Type,
                Code = x.Code,
                OrderExpressId = request.Id,
                OrderExpressCode = request.Code,
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
                Locked = x.Locked,
                PaymentStatus = x.PaymentStatus,
                AccountId = x.AccountId,
                AccountName = x.AccountName,
                CustomerId = request.CustomerId,
                CustomerName = request.CustomerName
            }).ToList(),
            request.OrderInvoice?.Select(x => new OrderInvoiceDto()
            {
                Id = x.Id,
                Serial = x.Serial,
                Symbol = x.Symbol,
                Number = x.Number,
                Value = x.Value,
                Date = x.Date,
                Note = x.Note,
                DisplayOrder = x.DisplayOrder
            }).ToList());

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (await _mediator.Send(new OrderExpressQueryById(id)) == null)
            return BadRequest(new ValidationResult("OrderExpress not exists"));

        var result = await _mediator.SendCommand(new OrderExpressDeleteCommand(id));

        return Ok(result);
    }

    [HttpPost("approval")]
    public async Task<IActionResult> UpdateStatus([FromBody] ApprovalOrderExpressRequest request)
    {
        var data = new ApprovalOrderExpressCommand(
            request.Id,
            request.Status,
            request.ApproveComment
            );

        var result = await _mediator.SendCommand(data);

        return Ok(result);
    }

    [HttpPost("manage-payment")]
    public async Task<IActionResult> ManagePaymentOrderExpress([FromBody] ManagePaymentOrderExpressRequest request)
    {
        var paymentInvoice = request.PaymentInvoice?.Select(x => new PaymentInvoiceDto()
        {
            Id = x.Id ?? Guid.NewGuid(),
            Type = x.Type,
            Code = x.Code,
            OrderExpressId = request.Id,
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

        var cmd = new ManagePaymentOrderExpressCommand(
            request.Id,
            request.PaymentStatus,
            paymentInvoice
            );

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpPost("manage-service")]
    public async Task<IActionResult> ManageServiceOrder([FromBody] ManageServiceOrderExpressRequest request)
    {
        var orderService = request.OrderServiceAdd?.Select(x => new OrderServiceAddDto()
        {
            Id = !String.IsNullOrEmpty(x.Id) ? new Guid(x.Id) : null,
            OrderExpressId = request.Id,
            ServiceAddId = x.ServiceAddId,
            ServiceAddName = x.ServiceAddName,
            Price = x.Price,
            Currency = x.Currency,
            Calculation = x.Calculation,
            Status = x.Status,
            Note = x.Note,
            ExchangeRate = x.ExchangeRate,
            DisplayOrder = x.DisplayOrder
        }).ToList();

        var cmd = new ManageServiceOrderExpressCommand(
            request.Id,
            orderService
            );

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpPost("note")]
    public async Task<IActionResult> Note([FromBody] NoteOrderExpressRequest request)
    {
        var orderTracking = request.OrderTracking?.Select(x => new OrderTrackingDto()
        {
            Id = x.Id ?? Guid.NewGuid(),
            OrderExpressId = request.Id,
            Name = x.Name,
            Status = x.Status,
            Description = x.Description,
            Image = x.Image,
            TrackingDate = x.TrackingDate
        }).ToList();

        var cmd = new NoteOrderExpressCommand(
            request.Id,
            orderTracking
            );

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    //[ProducesResponseType(typeof(PagedResult<List<OrderExpressDto>>), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[SwaggerOperation(Summary = "Get orders by user")]
    //[HttpGet("paging-by-account")]
    //public async Task<PagedResult<List<OrderExpressDto>>> PagingByAccount([FromQuery] OrderExpressPagingByAccountQuery request)
    //{
    //    var result = await _mediator.Send(request);
    //    return result;
    //}
}
