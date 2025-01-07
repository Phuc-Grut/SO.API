using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries.Extended;

public class OrderExpressByCodeQuery : IQuery<OrderExpressDto>
{
    public OrderExpressByCodeQuery(Guid accountId, string code)
    {
        AccountId = accountId;
        Code = code;
    }

    public Guid AccountId { get; set; }
    public string Code { get; set; }
}

public class OrderExpressQueryAll : IQuery<IEnumerable<OrderExpressDto>>
{
    public OrderExpressQueryAll()
    {
    }
}

public class OrderExpressQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public OrderExpressQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class OrderExpressQueryCheckCode : IQuery<bool>
{

    public OrderExpressQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class OrderExpressQueryById : IQuery<OrderExpressDto>
{
    public OrderExpressQueryById()
    {
    }

    public OrderExpressQueryById(Guid orderExpressId)
    {
        OrderExpressId = orderExpressId;
    }

    public Guid OrderExpressId { get; set; }
}
public class OrderExpressPagingByAccountQuery : FopQuery, IQuery<PagedResult<List<OrderExpressDto>>>
{
    public OrderExpressPagingByAccountQuery(string? keyword, Guid accountId, string filter, int? status, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        AccountId = accountId;
        Status = status;
    }
    public string? Keyword { get; set; }
    public Guid AccountId { get; set; }
}
public class OrderExpressPagingQuery : FopQuery, IQuery<PagedResult<List<OrderExpressDto>>>
{
    public OrderExpressPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        Keyword = keyword;
    }
    public int? Status { get; set; }
    public string? Keyword { get; set; }

}

public class OrderExpressCountByCustomerId : IQuery<Dictionary<int, int>>
{
    public OrderExpressCountByCustomerId(Guid customerId)
    {
        CustomerId = customerId;
    }
    public Guid CustomerId { get; set; }
}
public class OrderExpressQueryHandler : IQueryHandler<OrderExpressQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<OrderExpressQueryAll, IEnumerable<OrderExpressDto>>,
                                         IQueryHandler<OrderExpressQueryCheckCode, bool>,
                                         IQueryHandler<OrderExpressQueryById, OrderExpressDto>,
                                         IQueryHandler<OrderExpressPagingQuery, PagedResult<List<OrderExpressDto>>>,
                                         IQueryHandler<OrderExpressPagingByAccountQuery, PagedResult<List<OrderExpressDto>>>,
                                         IQueryHandler<OrderExpressCountByCustomerId, Dictionary<int, int>>,
                                         IQueryHandler<OrderExpressByCodeQuery, OrderExpressDto>
{
    private readonly IOrderExpressRepository _repository;
    private readonly ISOExtProcedures _procedures;
    private readonly ICustomerRepository _customerRepository;
    public OrderExpressQueryHandler(IOrderExpressRepository repository,
        ISOExtProcedures sOContextProcedures,
        ICustomerRepository customerRepository)
    {
        _repository = repository;
        _procedures = sOContextProcedures;
        _customerRepository = customerRepository;
    }
    public async Task<bool> Handle(OrderExpressQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<OrderExpressDto> Handle(OrderExpressQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.OrderExpressId);
        var result = new OrderExpressDto()
        {
            Id = item.Id,
            OrderType = item.OrderType,
            Code = item.Code,
            OrderDate = item.OrderDate,
            CustomerId = item.CustomerId,
            CustomerName = item.CustomerName,
            CustomerCode = item.CustomerCode,
            StoreId = item.StoreId,
            StoreCode = item.StoreCode,
            StoreName = item.StoreName,
            ContractId = item.ContractId,
            ContractName = item.ContractName,
            Currency = item.Currency,
            ExchangeRate = item.ExchangeRate,
            ShippingMethodId = item.ShippingMethodId,
            ShippingMethodCode = item.ShippingMethodCode,
            ShippingMethodName = item.ShippingMethodName,
            RouterShipping = item.RouterShipping,
            DomesticTracking = item.DomesticTracking,
            DomesticCarrier = item.DomesticCarrier,
            Status = item.Status,
            PaymentTermId = item.PaymentTermId,
            PaymentTermName = item.PaymentTermName,
            PaymentMethodName = item.PaymentMethodName,
            PaymentMethodId = item.PaymentMethodId,
            PaymentStatus = item.PaymentStatus,
            ShipperName = item.ShipperName,
            ShipperPhone = item.ShipperPhone,
            ShipperZipCode = item.ShipperZipCode,
            ShipperAddress = item.ShipperAddress,
            ShipperCountry = item.ShipperCountry,
            ShipperProvince = item.ShipperProvince,
            ShipperDistrict = item.ShipperDistrict,
            ShipperWard = item.ShipperWard,
            ShipperNote = item.ShipperNote,
            DeliveryName = item.DeliveryName,
            DeliveryPhone = item.DeliveryPhone,
            DeliveryZipCode = item.DeliveryZipCode,
            DeliveryAddress = item.DeliveryAddress,
            DeliveryCountry = item.DeliveryCountry,
            DeliveryProvince = item.DeliveryProvince,
            DeliveryDistrict = item.DeliveryDistrict,
            DeliveryWard = item.DeliveryWard,
            DeliveryNote = item.DeliveryNote,
            EstimatedDeliveryDate = item.EstimatedDeliveryDate,
            DeliveryMethodId = item.DeliveryMethodId,
            DeliveryMethodCode = item.DeliveryMethodCode,
            DeliveryMethodName = item.DeliveryMethodName,
            DeliveryStatus = item.DeliveryStatus,
            IsBill = item.IsBill,
            BillName = item.BillName,
            BillAddress = item.BillAddress,
            BillCountry = item.BillCountry,
            BillProvince = item.BillProvince,
            BillDistrict = item.BillDistrict,
            BillWard = item.BillWard,
            BillStatus = item.BillStatus,
            Description = item.Description,
            Note = item.Note,
            GroupEmployeeId = item.GroupEmployeeId,
            GroupEmployeeName = item.GroupEmployeeName,
            CommodityGroup = item.CommodityGroup,
            AirFreight = item.AirFreight,
            SeaFreight = item.SeaFreight,
            Surcharge = item.Surcharge,
            Weight = item.Weight,
            Width = item.Width,
            Height = item.Height,
            Length = item.Length,
            Image = item.Image,
            Paid = item.Paid,
            Total = item.Total,
            AccountId = item.AccountId,
            AccountName = item.AccountName,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            RouterShippingId = item.RouterShippingId,
            ShippingCodePost = item.ShippingCodePost,
            TrackingCode = item.TrackingCode,
            TrackingCarrier = item.TrackingCarrier,
            Package = item.Package,
            ToDeliveryDate = item.ToDeliveryDate,
            OrderExpressDetail = item.OrderExpressDetail.Select(x => new OrderExpressDetailDto
            {
                Id = x.Id,
                OrderExpressId = x.OrderExpressId,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
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
                Surcharge = x.Surcharge
            }).ToList(),
            OrderServiceAdd = item.OrderServiceAdd.Select(x => new OrderServiceAddDto()
            {
                Id = x.Id,
                OrderExpressId = x.OrderExpressId,
                QuotationId = x.QuotationId,
                ServiceAddId = x.ServiceAddId,
                ServiceAddName = x.ServiceAddName,
                Price = x.Price,
                Currency = x.Currency,
                Calculation = x.Calculation,
                Status = x.Status,
                Note = x.Note,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                UpdatedByName = x.UpdatedByName,
                CreatedByName = x.CreatedByName,
                ExchangeRate = x.ExchangeRate,
                DisplayOrder = x.DisplayOrder
            }).OrderBy(x => x.DisplayOrder).ToList(),
            PaymentInvoice = item.PaymentInvoice.Select(x => new PaymentInvoiceDto()
            {
                Id = x.Id,
                Type = x.Type,
                Code = x.Code,
                OrderExpressId = x.OrderExpressId,
                OrderExpressCode = x.OrderExpressCode,
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
                Bank = x.BankName + " - " + x.BankNumber,
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
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                UpdatedByName = x.UpdatedByName,
                CreatedByName = x.CreatedByName,
                CustomerId = x.CustomerId,
                CustomerName = x.CustomerName
            }).ToList(),
            OrderInvoice = item.OrderInvoice.Select(x => new OrderInvoiceDto()
            {
                Id = x.Id,
                OrderExpressId = x.OrderExpressId,
                OrderExpressCode = x.OrderExpressCode,
                Serial = x.Serial,
                Symbol = x.Symbol,
                Number = x.Number,
                Value = x.Value,
                Date = x.Date,
                Note = x.Note,
                DisplayOrder = x.DisplayOrder,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                CreatedByName = x.CreatedByName,
                UpdatedByName = x.UpdatedByName
            }).OrderBy(x => x.DisplayOrder).ToList(),
            OrderTracking = item.OrderTracking.Select(x => new OrderTrackingDto()
            {
                Id = x.Id,
                OrderExpressId = x.OrderExpressId,
                Name = x.Name,
                Status = x.Status,
                Description = x.Description,
                Image = x.Image,
                TrackingDate = x.TrackingDate,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                CreatedByName = x.CreatedByName,
                UpdatedByName = x.UpdatedByName
            }).OrderByDescending(x => x.TrackingDate).ToList(),
        };
        return result;
    }

    public async Task<PagedResult<List<OrderExpressDto>>> Handle(OrderExpressPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<OrderExpressDto>>();
        var filter = new Dictionary<string, object>();
        var fopRequest = FopExpressionBuilder<OrderExpress>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, request.Status, fopRequest);
        var data = items.Select(item => new OrderExpressDto()
        {
            Id = item.Id,
            OrderType = item.OrderType,
            Code = item.Code,
            OrderDate = item.OrderDate,
            CustomerId = item.CustomerId,
            CustomerName = item.CustomerName,
            CustomerCode = item.CustomerCode,
            StoreId = item.StoreId,
            StoreCode = item.StoreCode,
            StoreName = item.StoreName,
            ContractId = item.ContractId,
            ContractName = item.ContractName,
            ShipperName = item.ShipperName,
            ShipperPhone = item.ShipperPhone,
            ShipperZipCode = item.ShipperZipCode,
            ShipperAddress = item.ShipperAddress,
            ShipperCountry = item.ShipperCountry,
            ShipperProvince = item.ShipperProvince,
            ShipperDistrict = item.ShipperDistrict,
            ShipperWard = item.ShipperWard,
            ShipperNote = item.ShipperNote,
            DeliveryName = item.DeliveryName,
            DeliveryPhone = item.DeliveryPhone,
            DeliveryZipCode = item.DeliveryZipCode,
            DeliveryAddress = item.DeliveryAddress,
            DeliveryCountry = item.DeliveryCountry,
            DeliveryProvince = item.DeliveryProvince,
            DeliveryDistrict = item.DeliveryDistrict,
            DeliveryWard = item.DeliveryWard,
            DeliveryNote = item.DeliveryNote,
            EstimatedDeliveryDate = item.EstimatedDeliveryDate,
            ExchangeRate = item.ExchangeRate,
            DeliveryMethodId = item.DeliveryMethodId,
            DeliveryMethodName = item.DeliveryMethodName,
            DeliveryStatus = item.DeliveryStatus,
            BillName = item.BillName,
            BillAddress = item.BillAddress,
            BillCountry = item.BillCountry,
            BillProvince = item.BillProvince,
            BillDistrict = item.BillDistrict,
            BillWard = item.BillWard,
            BillStatus = item.BillStatus,
            ShippingMethodId = item.ShippingMethodId,
            ShippingMethodName = item.ShippingMethodName,
            AccountName = item.AccountName,
            Note = item.Note,
            GroupEmployeeId = item.GroupEmployeeId,
            GroupEmployeeName = item.GroupEmployeeName,
            AccountId = item.AccountId,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            RouterShipping = item.RouterShipping,
            CommodityGroup = item.CommodityGroup,
            AirFreight = item.AirFreight,
            SeaFreight = item.SeaFreight,
            Surcharge = item.Surcharge,
            Image = item.Image,
            Currency = item.Currency,
            Description = item.Description,
            Weight = item.Weight,
            Width = item.Width,
            Height = item.Height,
            Length = item.Length,
            Total = item.Total,

            RouterShippingId = item.RouterShippingId,
            ShippingCodePost = item.ShippingCodePost,
            TrackingCode = item.TrackingCode,
            TrackingCarrier = item.TrackingCarrier,
            Package = item.Package,
            ToDeliveryDate = item.ToDeliveryDate,

            TotalProductAmount = item?.OrderExpressDetail.Sum(x => x.Quantity * x.UnitPrice),
            TotalServiceAmount = item?.OrderServiceAdd.Sum(x => x.Price * (x.Calculation == "/" ? 1 / (x.ExchangeRate ?? 1) : x.ExchangeRate ?? 1)),
            TotalAmountCollected = item?.PaymentInvoice
                .Where(x => (new[] { "11", "12", "13" }).Contains(x.Type))
                .Sum(x => x.Amount * (x.Calculation == "/" ? 1 / (x.ExchangeRate ?? 1) : x.ExchangeRate ?? 1)),
        }).ToList();

        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<PagedResult<List<OrderExpressDto>>> Handle(OrderExpressPagingByAccountQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<OrderExpressDto>>();
        var fopRequest = FopExpressionBuilder<OrderExpress>.Build(request.Filter, "orderDate;desc", request.PageNumber, request.PageSize);
        var customerId = await _customerRepository.GetIdByAccountId(request.AccountId);

        var (items, count) = await _repository.Filter(request.Keyword, customerId, request.Status, fopRequest);
        var data = items.Select(item => new OrderExpressDto()
        {
            Id = item.Id,
            OrderType = item.OrderType,
            Code = item.Code,
            OrderDate = item.OrderDate,
            CustomerId = item.CustomerId,
            CustomerName = item.CustomerName,
            CustomerCode = item.CustomerCode,
            StoreId = item.StoreId,
            StoreCode = item.StoreCode,
            StoreName = item.StoreName,
            ContractId = item.ContractId,
            ContractName = item.ContractName,
            ShipperName = item.ShipperName,
            ShipperPhone = item.ShipperPhone,
            ShipperZipCode = item.ShipperZipCode,
            ShipperAddress = item.ShipperAddress,
            ShipperCountry = item.ShipperCountry,
            ShipperProvince = item.ShipperProvince,
            ShipperDistrict = item.ShipperDistrict,
            ShipperWard = item.ShipperWard,
            ShipperNote = item.ShipperNote,
            DeliveryName = item.DeliveryName,
            DeliveryPhone = item.DeliveryPhone,
            DeliveryZipCode = item.DeliveryZipCode,
            DeliveryAddress = item.DeliveryAddress,
            DeliveryCountry = item.DeliveryCountry,
            DeliveryProvince = item.DeliveryProvince,
            DeliveryDistrict = item.DeliveryDistrict,
            DeliveryWard = item.DeliveryWard,
            DeliveryNote = item.DeliveryNote,
            EstimatedDeliveryDate = item.EstimatedDeliveryDate,
            DeliveryMethodId = item.DeliveryMethodId,
            DeliveryMethodName = item.DeliveryMethodName,
            DeliveryStatus = item.DeliveryStatus,
            BillName = item.BillName,
            BillAddress = item.BillAddress,
            BillCountry = item.BillCountry,
            BillProvince = item.BillProvince,
            BillDistrict = item.BillDistrict,
            BillWard = item.BillWard,
            BillStatus = item.BillStatus,
            ShippingMethodId = item.ShippingMethodId,
            ShippingMethodName = item.ShippingMethodName,
            AccountName = item.AccountName,
            Note = item.Note,
            GroupEmployeeId = item.GroupEmployeeId,
            GroupEmployeeName = item.GroupEmployeeName,
            AccountId = item.AccountId,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            RouterShipping = item.RouterShipping,
            CommodityGroup = item.CommodityGroup,
            AirFreight = item.AirFreight,
            SeaFreight = item.SeaFreight,
            Surcharge = item.Surcharge,
            Image = item.Image,
            Currency = item.Currency,
            DeliveryMethodCode = item.DeliveryMethodCode,
            Width = item.Width,
            Description = item.Description,
            DomesticCarrier = item.DomesticCarrier,
            DomesticTracking = item.DomesticTracking,
            ExchangeRate = item.ExchangeRate,
            Height = item.Height,
            IsBill = item.IsBill,
            Length = item.Length,
            Paid = item.Paid,
            PaymentMethodId = item.PaymentMethodId,
            PaymentMethodName = item.PaymentMethodName,
            PaymentStatus = item.PaymentStatus,
            PaymentTermId = item.PaymentTermId,
            PaymentTermName = item.PaymentTermName,
            ShippingMethodCode = item.ShippingMethodCode,
            Weight = item.Weight,
            Total = item.Total,

            RouterShippingId = item.RouterShippingId,
            ShippingCodePost = item.ShippingCodePost,
            TrackingCode = item.TrackingCode,
            TrackingCarrier = item.TrackingCarrier,
            Package = item.Package,
            ToDeliveryDate = item.ToDeliveryDate,

            TotalProductAmount = item.OrderExpressDetail.Sum(x => x.Quantity * x.UnitPrice),
            TotalServiceAmount = item?.OrderServiceAdd.Sum(x => x.Price * (x.Calculation == "/" ? 1 / (x.ExchangeRate ?? 1) : x.ExchangeRate ?? 1)),
            TotalAmountCollected = item?.PaymentInvoice
                .Where(x => (new[] { "11", "12", "13" }).Contains(x.Type))
                .Sum(x => x.Amount * (x.Calculation == "/" ? 1 / (x.ExchangeRate ?? 1) : x.ExchangeRate ?? 1)),
            OrderExpressDetail = item?.OrderExpressDetail.Select(x => new OrderExpressDetailDto()
            {
                Id = x.Id,
                OrderExpressId = x.OrderExpressId,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage,
                Origin = x.Origin,
                UnitName = x.UnitName,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                DisplayOrder = x.DisplayOrder,
                Note = x.Note,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                CreatedByName = x.CreatedByName,
                UpdatedByName = x.UpdatedByName,
                CommodityGroup = x.CommodityGroup,
                SurchargeGroup = x.SurchargeGroup,
                Surcharge = x.Surcharge
            }).ToList(),

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
                    Status = service.Status,
                    Currency = service.Currency,
                    Note = service.Note,
                    UpdatedBy = service.UpdatedBy,
                    UpdatedByName = service.UpdatedByName,
                    UpdatedDate = service.UpdatedDate,
                };
            }).ToList(),

            OrderTracking = item?.OrderTracking?.Select(service =>
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
            }).ToList()
        }).ToList();

        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<OrderExpressDto>> Handle(OrderExpressQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new OrderExpressDto()
        {
            Id = item.Id,
            OrderType = item.OrderType,
            Code = item.Code,
            OrderDate = item.OrderDate,
            CustomerId = item.CustomerId,
            CustomerName = item.CustomerName,
            CustomerCode = item.CustomerCode,
            StoreId = item.StoreId,
            StoreCode = item.StoreCode,
            StoreName = item.StoreName,
            ContractId = item.ContractId,
            ContractName = item.ContractName,
            ShipperName = item.ShipperName,
            ShipperPhone = item.ShipperPhone,
            ShipperZipCode = item.ShipperZipCode,
            ShipperAddress = item.ShipperAddress,
            ShipperCountry = item.ShipperCountry,
            ShipperProvince = item.ShipperProvince,
            ShipperDistrict = item.ShipperDistrict,
            ShipperWard = item.ShipperWard,
            ShipperNote = item.ShipperNote,
            DeliveryName = item.DeliveryName,
            DeliveryPhone = item.DeliveryPhone,
            DeliveryZipCode = item.DeliveryZipCode,
            DeliveryAddress = item.DeliveryAddress,
            DeliveryCountry = item.DeliveryCountry,
            DeliveryProvince = item.DeliveryProvince,
            DeliveryDistrict = item.DeliveryDistrict,
            DeliveryWard = item.DeliveryWard,
            DeliveryNote = item.DeliveryNote,
            EstimatedDeliveryDate = item.EstimatedDeliveryDate,
            DeliveryMethodId = item.DeliveryMethodId,
            DeliveryMethodName = item.DeliveryMethodName,
            DeliveryStatus = item.DeliveryStatus,
            BillName = item.BillName,
            BillAddress = item.BillAddress,
            BillCountry = item.BillCountry,
            BillProvince = item.BillProvince,
            BillDistrict = item.BillDistrict,
            BillWard = item.BillWard,
            BillStatus = item.BillStatus,
            ShippingMethodId = item.ShippingMethodId,
            ShippingMethodName = item.ShippingMethodName,
            AccountName = item.AccountName,
            Note = item.Note,
            GroupEmployeeId = item.GroupEmployeeId,
            GroupEmployeeName = item.GroupEmployeeName,
            AccountId = item.AccountId,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            RouterShipping = item.RouterShipping,
            CommodityGroup = item.CommodityGroup,
            AirFreight = item.AirFreight,
            SeaFreight = item.SeaFreight,
            Surcharge = item.Surcharge,
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(OrderExpressQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        filter.Add("status", request.Status);
        var items = await _repository.GetListBox(filter);
        var result = items.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Code
        });
        return result;
    }
    public async Task<Dictionary<int, int>> Handle(OrderExpressCountByCustomerId request, CancellationToken cancellationToken)
    {
        var dbresult = await _procedures.SP_GET_ORDEREXPRESS_COUNTERAsync(request.CustomerId);
        var result = new Dictionary<int, int>();
        foreach (var item in dbresult)
        {
            result.Add(item.Status, item.Count);
        }
        return result;
    }

    public async Task<OrderExpressDto> Handle(OrderExpressByCodeQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var customerId = await _customerRepository.GetIdByAccountId(request.AccountId);
        var orderExpress = await _repository.GetByCode(customerId, request.Code);
        if (orderExpress is null)
        {
            throw new ErrorCodeException("ORDER_EXPRESS_NOT_FOUND");
        }

        var result = new OrderExpressDto
        {
            Id = orderExpress.Id,
            OrderType = orderExpress.OrderType,
            Code = orderExpress.Code,
            OrderDate = orderExpress.OrderDate,
            CustomerId = orderExpress.CustomerId,
            CustomerName = orderExpress.CustomerName,
            CustomerCode = orderExpress.CustomerCode,
            StoreId = orderExpress.StoreId,
            StoreCode = orderExpress.StoreCode,
            StoreName = orderExpress.StoreName,
            ContractId = orderExpress.ContractId,
            ContractName = orderExpress.ContractName,
            Currency = orderExpress.Currency,
            ExchangeRate = orderExpress.ExchangeRate,
            ShippingMethodId = orderExpress.ShippingMethodId,
            ShippingMethodCode = orderExpress.ShippingMethodCode,
            ShippingMethodName = orderExpress.ShippingMethodName,
            RouterShipping = orderExpress.RouterShipping,
            DomesticTracking = orderExpress.DomesticTracking,
            DomesticCarrier = orderExpress.DomesticCarrier,
            Status = orderExpress.Status,
            PaymentTermId = orderExpress.PaymentTermId,
            PaymentTermName = orderExpress.PaymentTermName,
            PaymentMethodName = orderExpress.PaymentMethodName,
            PaymentMethodId = orderExpress.PaymentMethodId,
            PaymentStatus = orderExpress.PaymentStatus,
            ShipperName = orderExpress.ShipperName,
            ShipperPhone = orderExpress.ShipperPhone,
            ShipperZipCode = orderExpress.ShipperZipCode,
            ShipperAddress = orderExpress.ShipperAddress,
            ShipperCountry = orderExpress.ShipperCountry,
            ShipperProvince = orderExpress.ShipperProvince,
            ShipperDistrict = orderExpress.ShipperDistrict,
            ShipperWard = orderExpress.ShipperWard,
            ShipperNote = orderExpress.ShipperNote,
            DeliveryName = orderExpress.DeliveryName,
            DeliveryPhone = orderExpress.DeliveryPhone,
            DeliveryZipCode = orderExpress.DeliveryZipCode,
            DeliveryAddress = orderExpress.DeliveryAddress,
            DeliveryCountry = orderExpress.DeliveryCountry,
            DeliveryProvince = orderExpress.DeliveryProvince,
            DeliveryDistrict = orderExpress.DeliveryDistrict,
            DeliveryWard = orderExpress.DeliveryWard,
            DeliveryNote = orderExpress.DeliveryNote,
            EstimatedDeliveryDate = orderExpress.EstimatedDeliveryDate,
            DeliveryMethodId = orderExpress.DeliveryMethodId,
            DeliveryMethodCode = orderExpress.DeliveryMethodCode,
            DeliveryMethodName = orderExpress.DeliveryMethodName,
            DeliveryStatus = orderExpress.DeliveryStatus,
            IsBill = orderExpress.IsBill,
            BillName = orderExpress.BillName,
            BillAddress = orderExpress.BillAddress,
            BillCountry = orderExpress.BillCountry,
            BillProvince = orderExpress.BillProvince,
            BillDistrict = orderExpress.BillDistrict,
            BillWard = orderExpress.BillWard,
            BillStatus = orderExpress.BillStatus,
            Description = orderExpress.Description,
            Note = orderExpress.Note,
            GroupEmployeeId = orderExpress.GroupEmployeeId,
            GroupEmployeeName = orderExpress.GroupEmployeeName,
            CommodityGroup = orderExpress.CommodityGroup,
            AirFreight = orderExpress.AirFreight,
            SeaFreight = orderExpress.SeaFreight,
            Surcharge = orderExpress.Surcharge,
            Weight = orderExpress.Weight,
            Width = orderExpress.Width,
            Height = orderExpress.Height,
            Length = orderExpress.Length,
            Image = orderExpress.Image,
            Paid = orderExpress.Paid,
            Total = orderExpress.Total,
            TotalProductAmount = orderExpress.OrderExpressDetail.Sum(x => x.Quantity * x.UnitPrice),
            AccountId = orderExpress.AccountId,
            AccountName = orderExpress.AccountName,
            CreatedBy = orderExpress.CreatedBy,
            CreatedDate = orderExpress.CreatedDate,
            UpdatedBy = orderExpress.UpdatedBy,
            UpdatedDate = orderExpress.UpdatedDate,
            CreatedByName = orderExpress.CreatedByName,
            UpdatedByName = orderExpress.UpdatedByName,

            RouterShippingId = orderExpress.RouterShippingId,
            ShippingCodePost = orderExpress.ShippingCodePost,
            TrackingCode = orderExpress.TrackingCode,
            TrackingCarrier = orderExpress.TrackingCarrier,
            Package = orderExpress.Package,
            ToDeliveryDate = orderExpress.ToDeliveryDate,

            OrderExpressDetail = orderExpress.OrderExpressDetail.Select(x => new OrderExpressDetailDto
            {
                Id = x.Id,
                OrderExpressId = x.OrderExpressId,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage,
                Origin = x.Origin,
                UnitName = x.UnitName,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                DisplayOrder = x.DisplayOrder,
                Note = x.Note,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                CreatedByName = x.CreatedByName,
                UpdatedByName = x.UpdatedByName,
                CommodityGroup = x.CommodityGroup,
                SurchargeGroup = x.SurchargeGroup,
                Surcharge = x.Surcharge,
            }).ToList(),

            PaymentInvoice = orderExpress?.PaymentInvoice?.Select(paymentInvoice => new PaymentInvoiceDto
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


            OrderServiceAdd = orderExpress?.OrderServiceAdd?.Select(service =>
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

            OrderTracking = orderExpress?.OrderTracking?.Select(service =>
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
            }).ToList()
        };

        return result;
    }
}
