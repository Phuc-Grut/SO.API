using VFi.Application.SO.DTOs;
using VFi.Application.SO.DTOs.Extended;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries.Extended.OrderCross;

public class OrderCrossByCodeQuery : IQuery<OrderCrossDto>
{
    public OrderCrossByCodeQuery(Guid accountId, string code)
    {
        AccountId = accountId;
        Code = code;
    }

    public Guid AccountId { get; set; }
    public string Code { get; set; }
}

public class OrderCrossByCodeQueryHandler :
    IQueryHandler<OrderCrossByCodeQuery, OrderCrossDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    public OrderCrossByCodeQueryHandler(IOrderRepository orderRespository,
        ICustomerRepository customerRepository)
    {
        _orderRepository = orderRespository;
        _customerRepository = customerRepository;
    }

    public async Task<OrderCrossDto> Handle(OrderCrossByCodeQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var customerId = await _customerRepository.GetIdByAccountId(request.AccountId);
        var order = await _orderRepository.GetByCode(customerId, request.Code);
        if (order is null)
        {
            throw new ErrorCodeException("ORDER_NOT_FOUND");
        }
        return new OrderCrossDto()
        {
            Id = order.Id,
            Code = order.Code,
            OrderType = order.OrderType,
            QuotationCode = order.Quotation?.Code,
            ContractCode = order.Contract?.Code,
            CustomerId = order.CustomerId,
            CustomerName = order.CustomerName,
            OrderDate = order.OrderDate,
            AccountId = order.AccountId,
            AccountName = order.AccountName,
            Status = order.Status,
            ApproveComment = order.ApproveComment,
            CustomerCode = order.CustomerCode,
            StoreId = order.StoreId,
            StoreCode = order.StoreCode,
            StoreName = order.StoreName,
            ContractId = order.ContractId,
            ContractName = order.ContractName,
            QuotationId = order.QuotationId,
            QuotationName = order.QuotationName,
            ChannelId = order.ChannelId,
            ChannelName = order.ChannelName,
            Currency = order.Currency,
            CurrencyName = order.CurrencyName,
            TypeDiscount = order.TypeDiscount,
            DiscountRate = order.DiscountRate,
            TypeCriteria = order.TypeCriteria,
            AmountDiscount = order.AmountDiscount,
            Note = order.Note,
            IsReturned = order?.OrderProduct?.All(x => x.Quantity == x.QuantityReturned && x.OrderId == order.Id),
            TotalAmount = order?.OrderProduct.Sum(x => x.Quantity * (decimal)x.UnitPrice.GetValueOrDefault())
                    + order?.OrderProduct.Sum(x => (x.Quantity * x.UnitPrice.GetValueOrDefault() - (decimal)x.DiscountTotal.GetValueOrDefault()) * (decimal)x.TaxRate.GetValueOrDefault() / 100)
                    - order?.OrderProduct.Sum(x => (decimal)x.DiscountTotal.GetValueOrDefault()),

            CreatedBy = order?.CreatedBy,
            CreatedDate = order?.CreatedDate,
            UpdatedBy = order?.UpdatedBy,
            UpdatedDate = order?.UpdatedDate,
            CreatedByName = order?.CreatedByName,
            UpdatedByName = order?.UpdatedByName,
            Image = order?.Image,
            ApproveBy = order?.ApproveBy,
            ApproveDate = order?.ApproveDate,
            BillAddress = order?.BillAddress,
            BillCountry = order?.BillCountry,
            BillDistrict = order?.BillDistrict,
            BillProvince = order?.BillProvince,
            BillStatus = order?.BillStatus,
            BillWard = order?.BillWard,
            DeliveryAddress = order?.DeliveryAddress,
            DeliveryCountry = order?.DeliveryCountry,
            DeliveryDistrict = order?.DeliveryDistrict,
            DeliveryMethodCode = order?.DeliveryMethodCode,
            DeliveryMethodId = order?.DeliveryMethodId,
            DeliveryMethodName = order?.DeliveryMethodName,
            DeliveryName = order?.DeliveryName,
            DeliveryNote = order?.DeliveryNote,
            DeliveryPhone = order?.DeliveryPhone,
            DeliveryProvince = order?.DeliveryProvince,
            DeliveryStatus = order?.DeliveryStatus,
            DeliveryWard = order?.DeliveryWard,
            EstimatedDeliveryDate = order?.EstimatedDeliveryDate,
            ExchangeRate = order?.ExchangeRate,
            GroupEmployeeId = order?.GroupEmployeeId,
            GroupEmployeeName = order?.GroupEmployeeName,
            IsBill = order?.IsBill,
            PaymentMethodId = order?.PaymentMethodId,
            PaymentMethodName = order?.PaymentMethodName,
            PaymentStatus = order?.PaymentStatus,
            PaymentTermId = order?.PaymentTermId,
            PaymentTermName = order?.PaymentTermName,
            PriceListId = order?.PriceListId,
            PriceListName = order?.PriceListName,
            ShippingMethodCode = order?.ShippingMethodCode,
            ShippingMethodId = order?.ShippingMethodId,
            ShippingMethodName = order?.ShippingMethodName,
            TotalDiscountAmount = order?.OrderProduct?.Sum(x => x.DiscountAmount),

            PaymentInvoice = order?.PaymentInvoice?.Select(paymentInvoice => new PaymentInvoiceDto
            {
                AccountId = paymentInvoice.AccountId,
                AccountName = paymentInvoice.AccountName,
                Amount = paymentInvoice.Amount,
                Code = paymentInvoice.Code,
                CreatedBy = paymentInvoice.CreatedBy,
                CreatedByName = paymentInvoice.CreatedByName,
                CreatedDate = paymentInvoice.CreatedDate,
                Currency = paymentInvoice.Currency,
                CurrencyName = paymentInvoice.CurrencyName,
                Description = paymentInvoice.Description,
                ExchangeRate = paymentInvoice.ExchangeRate,
                Id = paymentInvoice.Id,
                Note = paymentInvoice.Note,
                OrderCode = paymentInvoice.OrderCode,
                Type = paymentInvoice.Type,
                OrderId = paymentInvoice.OrderId,
                PaymentDate = paymentInvoice.PaymentDate,
                PaymentMethodId = paymentInvoice.PaymentMethodId,
                PaymentMethodName = paymentInvoice.PaymentMethodName,
                Status = paymentInvoice.Status,
                UpdatedBy = paymentInvoice.UpdatedBy,
                UpdatedByName = paymentInvoice.UpdatedByName,
                UpdatedDate = paymentInvoice.UpdatedDate,
            }).ToList(),


            OrderServiceAdd = order?.OrderServiceAdd?.Select(service =>
            {
                return new OrderServiceAddDto
                {
                    OrderId = service.OrderId,
                    CreatedBy = service.CreatedBy,
                    CreatedByName = service.CreatedByName,
                    CreatedDate = service.CreatedDate,
                    Id = service.Id,
                    Price = service.Price,
                    QuotationId = service.QuotationId,
                    ServiceAddId = service.ServiceAddId,
                    ServiceAddName = service.ServiceAddName,
                    Status = service.Status,
                    Currency = service.Currency,
                    Note = service.Note,
                    UpdatedBy = service.UpdatedBy,
                    UpdatedByName = service.UpdatedByName,
                    UpdatedDate = service.UpdatedDate,
                };
            }).ToList(),

            OrderTracking = order?.OrderTracking?.Select(service =>
            {
                return new OrderTrackingDto
                {
                    Id = service.Id,
                    OrderId = service.OrderId,
                    Name = service.Name,
                    Status = service.Status,
                    Description = service.Description,
                    Image = service.Image,
                    TrackingDate = service.TrackingDate,
                    CreatedBy = service.CreatedBy,
                    CreatedDate = service.CreatedDate,
                    UpdatedBy = service.UpdatedBy,
                    UpdatedDate = service.UpdatedDate,
                    CreatedByName = service.CreatedByName,
                    UpdatedByName = service.UpdatedByName
                };
            }).ToList(),

            Details = order?.OrderProduct?.Select(orderProduct =>
            {
                var orderDetail = new OrderProductDto()
                {
                    Id = orderProduct.Id,
                    OrderId = orderProduct.Id,
                    BidUsername = orderProduct.BidUsername,
                    ContractId = orderProduct.ContractId,
                    ContractName = orderProduct.ContractName,
                    CreatedDate = orderProduct.CreatedDate,
                    DeliveryQuantity = orderProduct.DeliveryQuantity,
                    DeliveryStatus = orderProduct.DeliveryStatus,
                    DiscountAmount = orderProduct.DiscountAmount,
                    DiscountTotal = orderProduct.DiscountTotal,
                    DiscountPercent = orderProduct.DiscountPercent,
                    DiscountType = orderProduct.DiscountType,
                    DisplayOrder = orderProduct.DisplayOrder,
                    ExpectedDate = orderProduct.ExpectedDate,
                    Note = orderProduct.Note,
                    Attributes = orderProduct.Attributes,
                    Origin = orderProduct.Origin,
                    PriceListId = orderProduct.PriceListId,
                    PriceListName = orderProduct.PriceListName,
                    ProductCode = orderProduct.ProductCode,
                    ProductId = orderProduct.ProductId,
                    ProductImage = orderProduct.ProductImage,
                    ProductName = orderProduct.ProductName,
                    Quantity = orderProduct.Quantity,
                    QuantityReturned = orderProduct.QuantityReturned,
                    QuotationId = orderProduct.QuotationId,
                    QuotationName = orderProduct.QuotationName,
                    Tax = orderProduct.Tax,
                    TaxCode = orderProduct.TaxCode,
                    TaxRate = orderProduct.TaxRate,
                    UnitCode = orderProduct.UnitCode,
                    UnitName = orderProduct.UnitName,
                    UnitPrice = orderProduct.UnitPrice,
                    UpdatedDate = orderProduct.UpdatedDate,
                    WarehouseCode = orderProduct.WarehouseCode,
                    WarehouseId = orderProduct.WarehouseId,
                    WarehouseName = orderProduct.WarehouseName,
                    SourceLink = orderProduct.SourceLink,
                    SourceCode = orderProduct.SourceCode
                };
                return orderDetail;
            }).ToList()
        };
    }


}
