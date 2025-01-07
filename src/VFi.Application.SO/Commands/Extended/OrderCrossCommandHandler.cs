using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Enums;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands.Extended;

internal class OrderCrossCommandHandler : CommandHandler,
    IRequestHandler<AddOrderCrossCommand, ValidationResult>,
    IRequestHandler<CreateOrderCrossCommand, NetDevPack.Domain.ValidationResult>,
    IRequestHandler<CreateInvoicePayOrderCrossCommand, ValidationResult>,
    IRequestHandler<OrderCrossUpdateAddressCommand, ValidationResult>,
    IRequestHandler<OrderCancelCommand, ValidationResult>,
    IRequestHandler<OrderCrossAddServiceCommand, ValidationResult>,
    IRequestHandler<OrderConfirmDeliveredCommand, ValidationResult>,
    IRequestHandler<UpdateTrackingOrderCommand, ValidationResult>,
    IRequestHandler<OrderCrossUpdateBidUsernameCommand, ValidationResult>,
    IRequestHandler<UpdateShippmentRoutingOrderCommand, ValidationResult>,
    IRequestHandler<UpdateInternalNoteCommand, ValidationResult>,
    IDisposable
{
    private readonly IEventRepository _eventRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderProductRepository _orderDetailRepository;
    private readonly IOrderServiceAddRepository _orderServiceAddRepository;
    private readonly IPaymentInvoiceRepository _paymentInvoiceRepository;
    private readonly IPurchaseGroupRepository _purchaseGroupRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPriceListPurchaseRepository _priceListPurchaseRepository;
    private readonly IExchangeRateRepository _exchangeRateRepository;
    private readonly ISalesChannelRepository _salesChannelRepository;
    private readonly ISOExtProcedures _procedures;
    private readonly IOrderTrackingRepository _orderTrackingRepository;
    private readonly IContextUser _context; 
    private bool _disposedValue;

    public OrderCrossCommandHandler(
        IOrderRepository OrderRepository,
        IOrderProductRepository OrderDetailRepository,
        IOrderServiceAddRepository OrderServiceAddRepository,
        IPaymentInvoiceRepository paymentInvoiceRepository,
        IPurchaseGroupRepository purchaseGroupRepository,
        ICustomerRepository customerRepository,
        IPriceListPurchaseRepository priceListPurchaseRepository,
        IExchangeRateRepository exchangeRateRepository,
        ISalesChannelRepository salesChannelRepository,
        ISOExtProcedures repositoryProcedure,
        IOrderTrackingRepository orderTrackingRepository, 
        IEventRepository eventRepository, 
        IContextUser context)
    {
        _orderRepository = OrderRepository;
        _orderDetailRepository = OrderDetailRepository;
        _orderServiceAddRepository = OrderServiceAddRepository;
        _paymentInvoiceRepository = paymentInvoiceRepository;
        _purchaseGroupRepository = purchaseGroupRepository;
        _customerRepository = customerRepository;
        _priceListPurchaseRepository = priceListPurchaseRepository;
        _exchangeRateRepository = exchangeRateRepository;
        _salesChannelRepository = salesChannelRepository;
        _procedures = repositoryProcedure;
        _orderTrackingRepository = orderTrackingRepository;
        _context = context;
        this._eventRepository = eventRepository;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _orderRepository.Dispose();
                _orderDetailRepository.Dispose();
                _orderServiceAddRepository.Dispose();
                _paymentInvoiceRepository.Dispose();
            }
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async Task<ValidationResult> Handle(AddOrderCrossCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!request.IsValid(_orderRepository))
            return request.ValidationResult;


        var customer = await _customerRepository.GetByAccountId(request.CustomerId.GetValueOrDefault());
        if (customer is null)
        {
            throw new ValidationException("CustomerNotFound");
        }

        var purchaseGroup = await _purchaseGroupRepository.GetByCode(request.PurchaseGroup);
        if (purchaseGroup is null)
        {
            throw new ValidationException("PurchaseGroupInvalid");
        }

        var salesChannel = await _salesChannelRepository.GetByCode(request.ChannelCode);
        if (salesChannel is null)
        {
            throw new ValidationException("ChannelInvalid");
        }

        int? buyfee = 0;

        PriceListPurchase? priceListPurchase = null;

        if (customer.PriceListPurchaseId.HasValue)
        {
            priceListPurchase = await _priceListPurchaseRepository.GetById(customer.PriceListPurchaseId.Value);
        }
        else
        {
            priceListPurchase = await _priceListPurchaseRepository.GetDefault();
        }
        if (priceListPurchase is { })
        {
            var priceListPurchaseDetail = priceListPurchase.PriceListPurchaseDetail.FirstOrDefault(x => x.PurchaseGroupId == purchaseGroup.Id);

            if (priceListPurchaseDetail is { } && priceListPurchaseDetail.BuyFee.HasValue)
            {
                buyfee = priceListPurchaseDetail.BuyFee.Value;
            }
        }

        var rate = await _exchangeRateRepository.GetRate(request.Currency);


        var Order = new Order
        {
            Id = request.Id,
            Code = request.Code,
            OrderDate = request.OrderDate,
            CustomerId = customer.Id,
            CustomerName = customer.Name,
            CustomerCode = customer.Code,
            Status = request.Status,
            Currency = request.Currency,
            ExchangeRate = (decimal)rate,
            Note = request.Note,
            AccountId = request.AccountId,
            AccountName = request.AccountName,
            CreatedBy = request.CreatedBy,
            CreatedDate = DateTime.Now,
            BuyFee = buyfee,
            ChannelId = salesChannel.Id,
            ChannelName = salesChannel.Name,
            CreatedByName = request.CreatedByName,
        };
        _orderRepository.Add(Order);

        if (request.Detail?.Count > 0)
        {
            var i = 1;
            var list = request.Detail.Select(orderDetail => new OrderProduct()
            {
                Id = Guid.NewGuid(),
                OrderId = request.Id,
                ProductId = orderDetail.ProductId,
                ProductCode = orderDetail.ProductCode,
                ProductName = orderDetail.ProductName,
                ProductImage = orderDetail.ProductImage,
                Origin = orderDetail.Origin,
                WarehouseId = orderDetail.WarehouseId,
                WarehouseCode = orderDetail.WarehouseCode,
                WarehouseName = orderDetail.WarehouseName,
                PriceListId = orderDetail.PriceListId,
                PriceListName = orderDetail.PriceListName,
                UnitCode = orderDetail.UnitCode,
                UnitName = orderDetail.UnitName,
                Quantity = orderDetail.Quantity,
                UnitPrice = orderDetail.UnitPrice,
                DiscountType = orderDetail.DiscountType,
                DiscountPercent = orderDetail.DiscountPercent,
                DiscountAmount = orderDetail.AmountDiscount,
                DiscountTotal = orderDetail.DiscountTotal,
                TaxRate = orderDetail.TaxRate,
                Tax = orderDetail.Tax,
                TaxCode = orderDetail.TaxCode,
                ExpectedDate = orderDetail.ExpectedDate,
                Note = orderDetail.Note,
                DisplayOrder = i++,
                CreatedBy = orderDetail.CreatedBy,
                CreatedDate = DateTime.Now,
                CreatedByName = orderDetail.CreatedByName,
                DeliveryStatus = orderDetail.DeliveryStatus,
                DeliveryQuantity = orderDetail.DeliveryQuantity,
                BidUsername = orderDetail.BidUsername,
            }).ToList();
            _orderDetailRepository.Add(list);
        }

        if (request.OrderServiceAdd?.Count > 0)
        {
            var listService = request.OrderServiceAdd.Select(orderService => new OrderServiceAdd()
            {
                Id = Guid.NewGuid(),
                OrderId = request.Id,
                ServiceAddId = orderService.ServiceAddId,
                ServiceAddName = orderService.ServiceAddName,
                Price = orderService.Price,
                Status = orderService.Status,
                CreatedDate = DateTime.Now,
                CreatedBy = orderService.CreatedBy,
                CreatedByName = orderService.CreatedByName
            }).ToList();
            _orderServiceAddRepository.Add(listService);
        }

        if (request.PaymentInvoice?.Count > 0)
        {
            var listPaymentInvoice = request.PaymentInvoice.Select(paymentInvoice => new PaymentInvoice()
            {
                Id = Guid.NewGuid(),
                Code = paymentInvoice.Code,
                OrderId = request.Id,
                OrderCode = request.Code,
                Description = paymentInvoice.Description,
                Amount = paymentInvoice.Amount,
                Currency = paymentInvoice.Currency,
                CurrencyName = paymentInvoice.CurrencyName,
                ExchangeRate = paymentInvoice.ExchangeRate,
                PaymentDate = paymentInvoice.PaymentDate,
                PaymentMethodName = paymentInvoice.PaymentMethodName,
                PaymentMethodId = paymentInvoice.PaymentMethodId,
                Note = paymentInvoice.Note,
                Status = paymentInvoice.Status,
                AccountId = paymentInvoice.AccountId,
                AccountName = paymentInvoice.AccountName,
                CreatedDate = DateTime.Now,
                CreatedBy = paymentInvoice.CreatedBy,
                CreatedByName = paymentInvoice.CreatedByName
            }).ToList();
            _paymentInvoiceRepository.Add(listPaymentInvoice);
        }
        return await Commit(_orderRepository.UnitOfWork);
    }

    public async Task<NetDevPack.Domain.ValidationResult> Handle(CreateOrderCrossCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!request.IsValid())
            return request.ValidationResult;

        var data = await _procedures.SP_CREATE_CROSS_ORDERAsync(
            request.Id,
            request.Code,
            request.CustomerId,
            request.OrderType,
            request.PurchaseGroup,
            request.StoreCode,
            request.ChannelCode,
            request.CurrencyCode,
            request.ExchangeRate,
            request.PaymentTermCode,
            request.PaymentMethodCode,
            request.ShippingMethodCode,
            request.BuyFee,
            request.DomesticShipping,
            request.RouterShipping,
            request.DeliveryCountry,
            request.DeliveryProvince,
            request.DeliveryDistrict,
            request.DeliveryWard,
            request.DeliveryAddress,
            request.DeliveryName,
            request.DeliveryPhone,
            request.DeliveryNote,
            request.Products,
            request.ServiceAdd,
            request.Image,
            request.Images,
            request.Description,
            request.Note,
            request.TotalPay,
            request.PayNow,
            request.CreatedBy,
            request.CreatedByName);

        if (request.PayNow)
        {
            try
            {
                var order = data.First();
                var sourceLink = request.Products.Rows[0][3].ToString();
                var message = new VFi.Domain.SO.Events.PurchaseNotifyQueueEvent();
                message.OrderCode = order.Code;
                message.CustomerName = order.CustomerCode + " / " + order.CustomerName;
                message.Price = order.TotalAmountTax.Value.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("ja-JP"));
                message.Date = DateTime.Now.ToString("yyy/MM/dd HH:mm");
                message.Link = sourceLink;
                message.Status = "Chờ mua hàng";
                try
                {
                    if (request.OrderType.Equals("BARGAIN", StringComparison.OrdinalIgnoreCase))
                    {
                        message.Status = "Trả giá";
                    }
                }
                catch (Exception)
                { }
                await _eventRepository.PurchaseNotify(message);

            }
            catch (Exception)
            { }
        }
        //if (result == 0)
        //{
        //    return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("", "Create order failed") } };
        //}
        var result = new NetDevPack.Domain.ValidationResult(request.ValidationResult);
        result.Data = data;

        if (data.Any())
        {
            var item = data.First();
            _ = await _eventRepository.OrderStatusChangedEvent(new Domain.SO.Events.OrderStatusChangedQueueEvent()
            {
                OrderId = item.Id,
                FromStatus = null,
                ToStatus = item.Status,
                Tenant = _context.Tenant,
                Data = _context.Data,
                Data_Zone = _context.Data_Zone
            });
        }

        
        return result;
        //return request.ValidationResult;
    }


    public async Task<ValidationResult> Handle(CreateInvoicePayOrderCrossCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!request.IsValid())
            return request.ValidationResult;

        var order = await _orderRepository.GetById(request.Id);

        if (order is null)
        {
            throw new ErrorCodeException("ORDER_NOT_FOUND");
        }

        var result = await _procedures.SP_CREATE_INVOICE_PAY_ORDERAsync(
            request.Id,
            request.TotalPay,
            request.AccountId,
            request.CreatedBy,
            request.CreatedByName);

        //if (result != null)
        //{
        //    if (order.Status == (int)OrderStatus.PendingConfirm)
        //    {
        //        order.Status = (int)OrderStatus.WaitForPurchase;
        //    }

        //    if (order.Status == (int)OrderStatus.WaitForSettlement)
        //    {
        //        order.Status = (int)OrderStatus.Delivering;
        //    }

        //    if (order.PaymentStatus == (int)PaymentStatus.Pending)
        //    {
        //        order.PaymentStatus = (int)PaymentStatus.PartialPayment;
        //    }

        //    if (order.PaymentStatus == (int)PaymentStatus.PartialPayment)
        //    {
        //        order.PaymentStatus = (int)PaymentStatus.Paid;
        //    }

        //    _orderRepository.Update(order);
        //    await _orderRepository.UnitOfWork.Commit();
        //}
        if(result != null)
        {                
                var message = new Domain.SO.Events.OrderStatusChangedQueueEvent();
                message.OrderId = order.Id;
                message.FromStatus = null;
                message.ToStatus = null;
                message.ChangeDate = DateTime.UtcNow;
                message.Tenant = _context.Tenant;
                message.Data = _context.Data;
                message.Data_Zone = _context.Data_Zone;
            
            _ = await _eventRepository.OrderStatusChangedEvent(message);                
        }

        return request.ValidationResult;
    }

    public async Task<ValidationResult> Handle(OrderCrossUpdateAddressCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _ = await _procedures.SP_ORDER_UPDATE_ADDRESSAsync(
            request.Id,
            request.DeliveryCountry,
            request.DeliveryProvince,
            request.DeliveryDistrict,
            request.DeliveryWard,
            request.DeliveryAddress,
            request.DeliveryName,
            request.DeliveryPhone,
            request.DeliveryNote,
            request.UpdatedBy,
            request.UpdatedByName);


        return request.ValidationResult;
    }

    public async Task<ValidationResult> Handle(OrderCancelCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (request.IsPayFee)
        {
            _ = await _procedures.SP_CANCEL_ORDER_AUCTIONAsync(request.Id, request.UpdatedBy, request.UpdatedByName);
        }
        else
        {
            _ = await _procedures.SP_CANCEL_ORDER_NO_FEEAsync(request.Id, request.UpdatedBy, request.UpdatedByName);
        }
        return request.ValidationResult;
    }


    public async Task<ValidationResult> Handle(OrderCrossAddServiceCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _ = await _procedures.SP_CROSS_ORDER_ADD_SERVICEAsync(
            request.Id,
            request.ServiceAdd,
            request.UpdatedBy,
            request.UpdatedByName);


        return request.ValidationResult;
    }

    public async Task<ValidationResult> Handle(OrderConfirmDeliveredCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!request.IsValid())
            return request.ValidationResult;

        var customer = await _customerRepository.GetByAccountId(request.AccountId);

        if (customer is null)
        {
            throw new ErrorCodeException("CUSTOMER_NOT_FOUND");
        }

        var orders = await _orderRepository.GetByCode(customer.Id, request.OrderCode);

        if (orders is null || !orders.Any())
        {
            throw new ErrorCodeException("ORDER_NOT_FOUND");
        }

        if (request.OrderCode.Count != orders.Count())
        {
            throw new ErrorCodeException("ORDER_NOT_FOUND");
        }

        if (orders.Any(x => x.Status != (int)OrderStatus.Delivering))
        {
            throw new ErrorCodeException("ORDER_NOT_FOUND");
        }

        foreach (var order in orders)
        {
            order.Status = (int)OrderStatus.Delivered;
            _orderRepository.Update(order);


            _orderTrackingRepository.Add(new List<OrderTracking>() { new OrderTracking()
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                Name = "Đã giao hàng",
                Status = 1,
                Description = "Đã xác nhận giao hàng",
                Image = "",
                TrackingDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                CreatedBy = customer.Id,
                CreatedByName = customer.Name
            }});

            await Commit(_orderTrackingRepository.UnitOfWork);

        }
        return await Commit(_orderRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(OrderCrossUpdateBidUsernameCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _ = await _procedures.SP_ORDER_UPDATE_BID_USERNAMEAsync(
            request.Id,
            request.BidUsername,
            request.UpdatedBy,
            request.UpdatedByName);

        return request.ValidationResult;
    }

    public async Task<ValidationResult> Handle(UpdateTrackingOrderCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _ = await _procedures.SP_ORDER_UPDATE_TRACKINGAsync(
            request.Id,
            request.DomesticTracking,
            request.DomesticCarrier,
            request.DomesticStatus.GetValueOrDefault(0),
            request.UpdatedBy,
            request.UpdatedByName);

        return request.ValidationResult;
    }

    public async Task<ValidationResult> Handle(UpdateShippmentRoutingOrderCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _ = await _procedures.SP_ORDER_UPDATE_SHIPMENT_ROUTINGAsync(
            request.Id,
            request.ShipmentRouting,
            request.UpdatedBy,
            request.UpdatedByName);

        return request.ValidationResult;
    }

    public async Task<ValidationResult> Handle(UpdateInternalNoteCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _ = await _procedures.SP_ORDER_UPDATE_NOTEAsync(
            request.Id,
            request.InternalNote,
            request.UpdatedBy,
            request.UpdatedByName);

        return request.ValidationResult;
    }
}
