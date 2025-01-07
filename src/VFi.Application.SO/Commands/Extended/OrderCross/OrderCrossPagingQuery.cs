using VFi.Application.SO.DTOs;
using VFi.Application.SO.DTOs.Extended;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Commands.Extended.OrderCross;

public class OrderCrossPagingQuery : IQuery<PagedResult<List<OrderCrossDto>>>
{
    public Guid AccountId { get; set; }
    public string? Keyword { get; set; }
    public string? OrderType { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int? Status { get; set; }
}

public class OrderCrossPagingQueryHandler :
    IQueryHandler<OrderCrossPagingQuery, PagedResult<List<OrderCrossDto>>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    public OrderCrossPagingQueryHandler(IOrderRepository orderRepository,
        ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }

    public async Task<PagedResult<List<OrderCrossDto>>> Handle(OrderCrossPagingQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var response = new PagedResult<List<OrderCrossDto>>();
        var fopRequest = FopExpressionBuilder<Domain.SO.Models.Order>.Build(string.Empty, "orderDate;desc", request.PageNumber, request.PageSize);

        var customerId = await _customerRepository.GetIdByAccountId(request.AccountId);

        var filter = new Dictionary<string, object>
        {
            { "customerId", customerId }
        };

        if (request.Status.HasValue)
            filter.Add("status", request.Status.Value.ToString());

        if (!string.IsNullOrEmpty(request.OrderType))
            filter.Add("orderType", request.OrderType);

        var (datas, count) = await _orderRepository.Filter(request.Keyword, filter, fopRequest);

        var data = datas.Select(item =>
        {
            return new OrderCrossDto()
            {
                Id = item.Id,
                OrderType = item.OrderType,
                Code = item.Code,
                QuotationCode = item.Quotation?.Code,
                ContractCode = item.Contract?.Code,
                CustomerId = item.CustomerId,
                CustomerName = item.CustomerName,
                OrderDate = item.OrderDate,
                AccountId = item.AccountId,
                AccountName = item.AccountName,
                Status = item.Status,
                ApproveComment = item.ApproveComment,
                CustomerCode = item.CustomerCode,
                StoreId = item.StoreId,
                StoreCode = item.StoreCode,
                StoreName = item.StoreName,
                ContractId = item.ContractId,
                ContractName = item.ContractName,
                QuotationId = item.QuotationId,
                QuotationName = item.QuotationName,
                ChannelId = item.ChannelId,
                ChannelName = item.ChannelName,
                Currency = item.Currency,
                CurrencyName = item.CurrencyName,
                TypeDiscount = item.TypeDiscount,
                DiscountRate = item.DiscountRate,
                TypeCriteria = item.TypeCriteria,
                AmountDiscount = item.AmountDiscount,
                Note = item.Note,
                IsReturned = item?.OrderProduct?.All(x => x.Quantity == x.QuantityReturned && x.OrderId == item.Id),

                TotalAmount = item?.OrderProduct.Sum(x => x.Quantity * x.UnitPrice.GetValueOrDefault())
                    + item?.OrderProduct.Sum(x => (x.Quantity * x.UnitPrice.GetValueOrDefault() - (decimal)x.DiscountTotal.GetValueOrDefault()) * (decimal)x.TaxRate.GetValueOrDefault() / 100)
                    - item?.OrderProduct.Sum(x => (decimal)x.DiscountTotal.GetValueOrDefault()),

                CreatedBy = item?.CreatedBy,
                CreatedDate = item?.CreatedDate,
                UpdatedBy = item?.UpdatedBy,
                UpdatedDate = item?.UpdatedDate,
                CreatedByName = item?.CreatedByName,
                UpdatedByName = item?.UpdatedByName,
                Image = item?.Image,
                ApproveBy = item?.ApproveBy,
                ApproveDate = item?.ApproveDate,
                BillAddress = item?.BillAddress,
                BillCountry = item?.BillCountry,
                BillDistrict = item?.BillDistrict,
                BillProvince = item?.BillProvince,
                BillStatus = item?.BillStatus,
                BillWard = item?.BillWard,
                DeliveryAddress = item?.DeliveryAddress,
                DeliveryCountry = item?.DeliveryCountry,
                DeliveryDistrict = item?.DeliveryDistrict,
                DeliveryMethodCode = item?.DeliveryMethodCode,
                DeliveryMethodId = item?.DeliveryMethodId,
                DeliveryMethodName = item?.DeliveryMethodName,
                DeliveryName = item?.DeliveryName,
                DeliveryNote = item?.DeliveryNote,
                DeliveryPhone = item?.DeliveryPhone,
                DeliveryProvince = item?.DeliveryProvince,
                DeliveryStatus = item?.DeliveryStatus,
                DeliveryWard = item?.DeliveryWard,
                EstimatedDeliveryDate = item?.EstimatedDeliveryDate,
                ExchangeRate = item?.ExchangeRate,
                GroupEmployeeId = item?.GroupEmployeeId,
                GroupEmployeeName = item?.GroupEmployeeName,
                IsBill = item?.IsBill,
                PaymentMethodId = item?.PaymentMethodId,
                PaymentMethodName = item?.PaymentMethodName,
                PaymentStatus = item?.PaymentStatus,
                PaymentTermId = item?.PaymentTermId,
                PaymentTermName = item?.PaymentTermName,
                PriceListId = item?.PriceListId,
                PriceListName = item?.PriceListName,
                ShippingMethodCode = item?.ShippingMethodCode,
                ShippingMethodId = item?.ShippingMethodId,
                ShippingMethodName = item?.ShippingMethodName,
                TotalDiscountAmount = item?.OrderProduct?.Sum(x => x.DiscountAmount),

                PaymentExpiryDate = item?.PaymentExpiryDate,

                PaymentInvoice = item?.PaymentInvoice?.Select(paymentInvoice => new PaymentInvoiceDto
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
                    Type = paymentInvoice.Type,
                    Note = paymentInvoice.Note,
                    OrderCode = paymentInvoice.OrderCode,
                    OrderId = paymentInvoice.OrderId,
                    PaymentDate = paymentInvoice.PaymentDate,
                    PaymentMethodId = paymentInvoice.PaymentMethodId,
                    PaymentMethodName = paymentInvoice.PaymentMethodName,
                    Status = paymentInvoice.Status,
                    UpdatedBy = paymentInvoice.UpdatedBy,
                    UpdatedByName = paymentInvoice.UpdatedByName,
                    UpdatedDate = paymentInvoice.UpdatedDate,
                }).OrderByDescending(x => x.CreatedDate).ToList(),

                OrderServiceAdd = item?.OrderServiceAdd?.Select(service =>
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
                        Currency = service.Currency,
                        Note = service.Note,
                        Status = service.Status,
                        UpdatedBy = service.UpdatedBy,
                        UpdatedByName = service.UpdatedByName,
                        UpdatedDate = service.UpdatedDate,
                    };
                }).OrderBy(x => x.CreatedDate).ToList(),

                Details = item?.OrderProduct?.Select(orderProduct =>
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
                        DiscountAmount = orderProduct.AmountDiscount,
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
        }).ToList();

        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

}
