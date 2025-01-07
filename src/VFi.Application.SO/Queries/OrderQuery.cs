using System.Globalization;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MassTransit;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using VFi.NetDevPack.Utilities;


namespace VFi.Application.SO.Queries;

public class SendTransactionQueryByOrder : IQuery<IEnumerable<SendTransactionDto>>
{
    public SendTransactionQueryByOrder(string keyword, string order)
    {
        Order = order;
        Keyword = keyword;
    }

    public string Keyword { get; set; }
    public string Order { get; set; }
}

public class PreviewRecalculatedPriceQuery : IQuery<PreviewRecalculatedPriceDto>
{
    public PreviewRecalculatedPriceQuery(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}

public class OrderQuerySendConfigCombobox : IQuery<IEnumerable<SendConfigComboboxDto>>
{
    public OrderQuerySendConfigCombobox()
    {
    }
}

public class OrderQuerySendTemplateCombobox : IQuery<IEnumerable<SendTemplateComboboxDto>>
{

    public OrderQuerySendTemplateCombobox()
    {
    }
}

public class OrderEmailBuilderQuery : IQuery<EmailBodyDto>
{
    public OrderEmailBuilderQuery()
    {
    }
    public string Template { get; set; }
    public string Subject { get; set; }
    public string JBody { get; set; }
}

public class OrderQueryComboBox : IQuery<IEnumerable<OrderListBoxDto>>
{
    public OrderQueryComboBox(string? keyword, OrderParams queryParams, int pagesize, int pageindex)
    {
        QueryParams = queryParams;
        Keyword = keyword;
        PageSize = pagesize;
        PageIndex = pageindex;
    }
    public OrderParams? QueryParams { get; set; }
    public string? Keyword { get; set; }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}
public class OrderQueryCheckCode : IQuery<bool>
{

    public OrderQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class OrderQueryById : IQuery<OrderDto>
{
    public OrderQueryById()
    {
    }

    public OrderQueryById(Guid requestQuoteId)
    {
        OrderId = requestQuoteId;
    }

    public Guid OrderId { get; set; }
}
public class OrderQueryByIds : IQuery<IEnumerable<OrderDto>>
{

    public OrderQueryByIds(IList<Guid> ids)
    {
        OrderIds = ids;
    }

    public IList<Guid> OrderIds { get; set; }
}
public class OrderGetDataPrint : IQuery<OrderPrintDto>
{
    public OrderGetDataPrint()
    {
    }

    public OrderGetDataPrint(Guid id)
    {
        OrderId = id;
    }

    public Guid OrderId { get; set; }
}

public class OrderPagingQuery : FopQuery, IQuery<PagedResult<List<OrderDto>>>
{
    public OrderPagingQuery(
        string? keyword,
        string? customerId,
        string? employeeId,
        int? status,
        int? domesticStatus,
        string filter,
        string order,
        int pageNumber,
        int pageSize)
    {
        Keyword = keyword;
        CustomerId = customerId;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        EmployeeId = employeeId;
        DomesticStatus = domesticStatus;
    }
    public string? EmployeeId { get; set; }
    public string? CustomerId { get; set; }
    public int? DomesticStatus { get; set; }
}

public class OrderQueryRelatedById : IQuery<List<Relate>>
{
    public OrderQueryRelatedById(Guid Id)
    {
        OrderId = Id;
    }
    public Guid OrderId { get; set; }
}

public class OrderGetReferenceQuery : IQuery<IEnumerable<OrderReferenceDto>>
{
    public OrderGetReferenceQuery(OrderParams queryParams)
    {
        QueryParams = queryParams;
    }
    public OrderParams QueryParams { get; set; }
}

public class OrderPagingDetailQuery : FopQuery, IQuery<PagedResult<IEnumerable<OrderProducReferenceDto>>>
{
    public OrderPagingDetailQuery(string? keyword, OrderParams queryParams, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        QueryParams = queryParams;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public OrderParams? QueryParams { get; set; }
}
public class OrderExportTemplateQuery : IQuery<MemoryStream>
{
    public OrderExportTemplateQuery()
    { }
}
public class ValidateExcelOrderQuery : IQuery<List<OrderValidateDto>>
{
    public ValidateExcelOrderQuery(IFormFile file, string sheetId, int headerRow, List<ValidateField> listField)
    {
        File = file;
        ListField = listField;
        SheetId = sheetId;
        HeaderRow = headerRow;
    }

    public string SheetId { get; set; } = null!;
    public IFormFile File { get; set; }
    public int HeaderRow { get; set; }
    public List<ValidateField> ListField { get; set; }
}
public class OrderQueryHandler : IQueryHandler<OrderQueryComboBox, IEnumerable<OrderListBoxDto>>,
                                 IQueryHandler<OrderQueryCheckCode, bool>,
                                 IQueryHandler<OrderQueryById, OrderDto>,
                                 IQueryHandler<OrderQueryByIds, IEnumerable<OrderDto>>,
                                 IQueryHandler<OrderPagingQuery, PagedResult<List<OrderDto>>>,
                                 IQueryHandler<OrderGetReferenceQuery, IEnumerable<OrderReferenceDto>>,
                                 IQueryHandler<OrderQueryRelatedById, List<Relate>>,
                                 IQueryHandler<OrderGetDataPrint, OrderPrintDto>,
                                 IQueryHandler<OrderExportTemplateQuery, MemoryStream>,
                                 IQueryHandler<OrderPagingDetailQuery, PagedResult<IEnumerable<OrderProducReferenceDto>>>,
                                 IQueryHandler<ValidateExcelOrderQuery, List<OrderValidateDto>>,
                                 IQueryHandler<OrderQuerySendConfigCombobox, IEnumerable<SendConfigComboboxDto>>,
                                 IQueryHandler<OrderQuerySendTemplateCombobox, IEnumerable<SendTemplateComboboxDto>>,
                                 IQueryHandler<OrderEmailBuilderQuery, EmailBodyDto>,
                                 IQueryHandler<SendTransactionQueryByOrder, IEnumerable<SendTransactionDto>>,
                                 IQueryHandler<PreviewRecalculatedPriceQuery, PreviewRecalculatedPriceDto>

{
    private readonly IOrderRepository _repository;
    private readonly IPIMRepository _PIMRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IExportWarehouseProductRepository _exportWarehouseProductRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IExportTemplateRepository _exportTemplateRepository;
    private readonly IReturnOrderProductRepository _returnOrderProductRepository;
    private readonly IRequestPurchaseProductRepository _requestPurchaseProductRepository;
    private readonly ISalesDiscountProductRepository _salesDiscountProductRepository;
    private readonly IDeliveryProductRepository _deliveryProductRepository;
    private readonly IEmailMasterRepository _emailMasterRepository;
    private readonly ISOContextProcedures _repositoryProcedure;

    public OrderQueryHandler(IOrderRepository repository, IPIMRepository PIMRepository,
             IEmployeeRepository employeeRepository,
             IExportWarehouseProductRepository exportWarehouseProductRepository,
             IStoreRepository storeRepository,
             IOrderProductRepository orderProductRepository,
             IExportTemplateRepository exportTemplateRepository,
             IReturnOrderProductRepository returnOrderProductRepository,
             IRequestPurchaseProductRepository requestPurchaseProductRepository,
             ISalesDiscountProductRepository salesDiscountProductRepository,
             IDeliveryProductRepository deliveryProductRepository,
             IEmailMasterRepository emailMasterRepository,
             ISOContextProcedures repositoryProcedure
             )
    {
        _repository = repository;
        _PIMRepository = PIMRepository;
        _employeeRepository = employeeRepository;
        _exportWarehouseProductRepository = exportWarehouseProductRepository;
        _storeRepository = storeRepository;
        _orderProductRepository = orderProductRepository;
        _exportTemplateRepository = exportTemplateRepository;
        _returnOrderProductRepository = returnOrderProductRepository;
        _requestPurchaseProductRepository = requestPurchaseProductRepository;
        _salesDiscountProductRepository = salesDiscountProductRepository;
        _deliveryProductRepository = deliveryProductRepository;
        _emailMasterRepository = emailMasterRepository;
        _repositoryProcedure = repositoryProcedure;
    }

    public async Task<PagedResult<IEnumerable<OrderProducReferenceDto>>> Handle(OrderPagingDetailQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<IEnumerable<OrderProducReferenceDto>>();

        var fopRequest = FopExpressionBuilder<OrderProduct>.Build(request.QueryParams?.Filter, request.Order, request.PageNumber, request.PageSize);

        var filterParams = new Dictionary<string, object>();

        if (request.QueryParams.CustomerId != null)
        {
            filterParams.Add("customerId", request.QueryParams.CustomerId);
        }
        if (!String.IsNullOrEmpty(request.QueryParams.DiferenceStatus))
        {
            filterParams.Add("diferenceStatus", request.QueryParams.DiferenceStatus);
        }
        if (!String.IsNullOrEmpty(request.QueryParams.OrderType))
        {
            filterParams.Add("orderType", request.QueryParams.OrderType);
        }
        if (!String.IsNullOrEmpty(request.QueryParams.Status))
        {
            filterParams.Add("status", request.QueryParams.Status);
        }
        if (request.QueryParams.FromDate != null)
        {
            filterParams.Add("fromDate", request.QueryParams.FromDate);
        }
        if (request.QueryParams.ToDate != null)
        {
            filterParams.Add("toDate", request.QueryParams.ToDate);
        }
        if (request.QueryParams.StatusReturn != null)
        {
            filterParams.Add("statusReturn", request.QueryParams.StatusReturn.ToString());
        }
        if (request.QueryParams.WarehouseId != null)
        {
            filterParams.Add("warehouseId", request.QueryParams.WarehouseId);
        }
        if (request.QueryParams.Currency != null)
        {
            filterParams.Add("currency", request.QueryParams.Currency);
        }
        if (request.QueryParams.StatusPurchase != null)
        {
            filterParams.Add("statusPurchase", request.QueryParams.StatusPurchase.ToString());
        }
        if (request.QueryParams.StatusSales != null)
        {
            filterParams.Add("statusSales", request.QueryParams.StatusSales.ToString());
        }
        if (request.QueryParams.StatusProduction != null)
        {
            filterParams.Add("statusProduction", request.QueryParams.StatusProduction.ToString());
        }
        if (request.QueryParams.StatusExport != null)
        {
            filterParams.Add("statusExport", request.QueryParams.StatusExport.ToString());
        }
        var (datas, count) = await _orderProductRepository.Filter(request.Keyword, filterParams, fopRequest);

        var inventory = await _PIMRepository.GetInventoryDetail(string.Join(",", datas.Select(x => x.ProductId).ToArray()));
        var result = datas.Select(item => new OrderProducReferenceDto()
        {
            Id = item.Id,
            OrderId = item.OrderId,
            Code = item.Order.Code,
            Currency = item.Order.Currency,
            CurrencyName = item.Order.CurrencyName,
            ExchangeRate = item.Order.ExchangeRate,
            Status = item.Order.Status,
            OrderDate = item.Order.OrderDate,
            CreatedDate = item.Order.CreatedDate,
            ContractId = item.ContractId,
            ContractName = item.ContractName,
            QuotationId = item.QuotationId,
            QuotationName = item.QuotationName,
            ProductId = item.ProductId,
            ProductCode = item.ProductCode,
            ProductName = item.ProductName,
            ProductImage = item.ProductImage,
            Origin = item.Origin,
            WarehouseId = item.WarehouseId,
            WarehouseCode = item.WarehouseCode,
            WarehouseName = item.WarehouseName,
            PriceListId = item.PriceListId,
            PriceListName = item.PriceListName,
            UnitType = item.UnitType,
            UnitCode = item.UnitCode,
            UnitName = item.UnitName,
            Quantity = item.Quantity,
            StockQuantity = !String.IsNullOrEmpty(item.WarehouseCode) ? inventory.Where(y => y.ProductId == item.ProductId && y.Code == item.WarehouseCode).FirstOrDefault()?.StockQuantity : inventory.Where(y => y.ProductId == item.ProductId).Sum(y => y.StockQuantity),
            UnitPrice = item.UnitPrice,
            DiscountAmountDistribution = item.DiscountAmountDistribution,
            DiscountType = item.DiscountType,
            DiscountPercent = item.DiscountPercent,
            AmountDiscount = item.AmountDiscount,
            DiscountTotal = item.DiscountTotal,
            TaxRate = item.TaxRate,
            Tax = item.Tax,
            TaxCode = item.TaxCode,
            ExpectedDate = item.ExpectedDate,
            Note = item.Note,
            QuantitySales = item.QuantitySales,
            QuantityReturned = item.QuantityReturned,
            QuantityReturnedActual = item.QuantityReturnedActual,
            QuantityExported = item.QuantityExported,
            QuantityPurchased = item.QuantityPurchased,
            QuantityProductioned = item.QuantityProductioned,
            DisplayOrder = item.DisplayOrder,
            DeliveryStatus = item.DeliveryStatus,
            DeliveryQuantity = item.DeliveryQuantity,
            EstimatedDeliveryDate = item.EstimatedDeliveryDate,
        }).ToList();

        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<bool> Handle(OrderQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<OrderDto> Handle(OrderQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.OrderId);
        if (item is null)
        {
            return new OrderDto();
        }
        var reference = GetDocument(item.Code, item);
        var inventory = new List<INVENTORY_DETAIL_BY_LISTID>();
        var productPrice = new List<SP_GET_PRODUCT_PRICE_BY_LISTID>();
        var listId = new List<string>();
        var listProductId = new List<Guid?>();
        var i = 1;
        var source = item.OrderProduct.Select(x => x.ProductId).Distinct();
        foreach (var x in source)
        {
            listProductId.Add(x);
            if (listProductId.Count() == 50)
            {
                listId.Add(string.Join(",", listProductId));
                listProductId.RemoveRange(0, 50);
            }
            if (i == source.Count() && listProductId.Count() > 0)
            {
                listId.Add(string.Join(",", listProductId));
            }
            i++;
        }
        foreach (var o in listId)
        {
            var dataInventory = await _PIMRepository.GetInventoryDetail(o);
            foreach (var x in dataInventory)
            {
                var rs = new INVENTORY_DETAIL_BY_LISTID()
                {
                    Id = x.Id,
                    WarehouseId = x.WarehouseId,
                    Code = x.Code,
                    Name = x.Name,
                    ProductId = x.ProductId,
                    StockQuantity = x.StockQuantity,
                    ReservedQuantity = x.ReservedQuantity,
                    PlannedQuantity = x.PlannedQuantity
                };
                inventory.Add(rs);
            }
            var dataPrice = await _PIMRepository.GetProductPrice(o);
            foreach (var x in dataPrice)
            {
                var rs = new SP_GET_PRODUCT_PRICE_BY_LISTID()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Price = x.Price,
                    UnitId = x.UnitId,
                    Currency = x.Currency
                };
                productPrice.Add(rs);
            }
        }
        var expectedDelivery = await _deliveryProductRepository.GetByOrderId(item.Id);
        var orderProduct = item.OrderProduct.Select(x => new OrderProductDto(item.Calculation, item.ExchangeRate)
        {
            Id = x.Id,
            OrderId = x.OrderId,
            ContractId = x.ContractId,
            ContractName = x.ContractName,
            QuotationId = x.QuotationId,
            QuotationName = x.QuotationName,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            ProductPrice = (item.ContractId != null || item.QuotationId != null) ? x.UnitPrice : productPrice.Where(y => y.Id == x.ProductId).FirstOrDefault()?.Price,
            ProductCurrency = productPrice.Where(y => y.Id == x.ProductId).FirstOrDefault()?.Currency,
            Origin = x.Origin,
            WarehouseId = x.WarehouseId,
            WarehouseCode = x.WarehouseCode,
            WarehouseName = x.WarehouseName,
            StockQuantity = !String.IsNullOrEmpty(x.WarehouseCode) ? inventory.Where(y => y.ProductId == x.ProductId && y.Code == x.WarehouseCode).FirstOrDefault()?.StockQuantity : inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.StockQuantity),
            TotalStockQuantity = inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.StockQuantity),
            ReservedQuantity = !String.IsNullOrEmpty(x.WarehouseCode) ? inventory.Where(y => y.ProductId == x.ProductId && y.Code == x.WarehouseCode).FirstOrDefault()?.ReservedQuantity : inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.ReservedQuantity),
            PlannedQuantity = !String.IsNullOrEmpty(x.WarehouseCode) ? inventory.Where(y => y.ProductId == x.ProductId && y.Code == x.WarehouseCode).FirstOrDefault()?.PlannedQuantity : inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.PlannedQuantity),
            Currency = item.Currency,
            PriceListId = x.PriceListId,
            PriceListName = x.PriceListName,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice,
            DiscountAmountDistribution = x.DiscountAmountDistribution,
            DiscountType = x.DiscountType,
            DiscountPercent = x.DiscountPercent,
            AmountDiscount = x.AmountDiscount,
            DiscountTotal = x.DiscountTotal,
            TaxRate = x.TaxRate,
            Tax = x.Tax,
            TaxCode = x.TaxCode,
            ExpectedDate = x.ExpectedDate,
            Note = x.Note,
            QuantityReturned = x.QuantityReturned,
            QuantityReturnedActual = x.QuantityReturnedActual.HasValue ? x.QuantityReturnedActual.Value : 0,
            QuantityExported = x.QuantityExported.HasValue ? x.QuantityExported.Value : 0,
            QuantityPurchased = x.QuantityPurchased.HasValue ? x.QuantityPurchased.Value : 0,
            QuantityProductioned = x.QuantityProductioned.HasValue ? x.QuantityProductioned.Value : 0,
            QuantitySales = x.QuantitySales.HasValue ? x.QuantitySales.Value : 0,
            QuantitySaleActual = x.QuantitySaleActual,
            DisplayOrder = x.DisplayOrder,
            CreatedBy = x.CreatedBy,
            CreatedDate = x.CreatedDate,
            UpdatedBy = x.UpdatedBy,
            UpdatedDate = x.UpdatedDate,
            CreatedByName = x.CreatedByName,
            UpdatedByName = x.UpdatedByName,
            DeliveryStatus = x.DeliveryStatus,
            DeliveryQuantity = x.DeliveryQuantity,
            IsDiscount = (x.DiscountPercent > 0 || x.AmountDiscount > 0),
            EstimatedDeliveryDate = x.EstimatedDeliveryDate,
            ReturnAmount = x.ReturnAmount,
            SaleAmount = x.SaleAmount,
            SpecificationCode1 = x.SpecificationCode1,
            SpecificationCode2 = x.SpecificationCode2,
            SpecificationCode3 = x.SpecificationCode3,
            SpecificationCode4 = x.SpecificationCode4,
            SpecificationCode5 = x.SpecificationCode5,
            SpecificationCode6 = x.SpecificationCode6,
            SpecificationCode7 = x.SpecificationCode7,
            SpecificationCode8 = x.SpecificationCode8,
            SpecificationCode9 = x.SpecificationCode9,
            SpecificationCode10 = x.SpecificationCode10,
            SpecificationCodeJson = x.SpecificationCodeJson,
            BidUsername = x.BidUsername,
            SellerId = x.SellerId

        }).OrderBy(x => x.DisplayOrder).ToList();

        var result = new OrderDto()
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
            TypeDocument = item.TypeDocument,
            ContractId = item.ContractId,
            ContractName = item.ContractName,
            QuotationId = item.QuotationId,
            QuotationName = item.QuotationName,
            ChannelId = item.ChannelId,
            ChannelName = item.ChannelName,
            Status = item.Status,
            Currency = item.Currency,
            CurrencyName = item.CurrencyName,
            Calculation = item.Calculation,
            ExchangeRate = item.ExchangeRate,
            PriceListId = item.PriceListId,
            PriceListName = item.PriceListName,
            PaymentTermId = item.PaymentTermId,
            PaymentTermName = item.PaymentTermName,
            PaymentMethodName = item.PaymentMethodName,
            PaymentMethodId = item.PaymentMethodId,
            PaymentStatus = item.PaymentStatus,
            DeliveryPhone = item.DeliveryPhone,
            DeliveryName = item.DeliveryName,
            DeliveryAddress = item.DeliveryAddress,
            DeliveryCountry = item.DeliveryCountry,
            DeliveryProvince = item.DeliveryProvince,
            DeliveryDistrict = item.DeliveryDistrict,
            DeliveryWard = item.DeliveryWard,
            DeliveryNote = item.DeliveryNote,
            EstimatedDeliveryDate = item.EstimatedDeliveryDate,
            DeliveryTracking = item.DeliveryTracking,
            DeliveryCarrier = item.DeliveryCarrier,
            DeliveryPackage = item.DeliveryPackage,
            IsBill = item.IsBill,
            BillName = item.BillName,
            BillAddress = item.BillAddress,
            BillCountry = item.BillCountry,
            BillProvince = item.BillProvince,
            BillDistrict = item.BillDistrict,
            BillWard = item.BillWard,
            BillStatus = item.BillStatus,
            DeliveryMethodId = item.DeliveryMethodId,
            DeliveryMethodCode = item.DeliveryMethodCode,
            DeliveryMethodName = item.DeliveryMethodName,
            DeliveryStatus = item.DeliveryStatus,
            ShippingMethodId = item.ShippingMethodId,
            ShippingMethodCode = item.ShippingMethodCode,
            ShippingMethodName = item.ShippingMethodName,
            TypeDiscount = item.TypeDiscount,
            DiscountRate = item.DiscountRate,
            TypeCriteria = item.TypeCriteria,
            AmountDiscount = item.AmountDiscount,
            Note = item.Note,
            GroupEmployeeId = item.GroupEmployeeId,
            GroupEmployeeName = item.GroupEmployeeName,
            AccountId = item.AccountId,
            AccountName = item.AccountName,
            ApproveDate = item.ApproveDate,
            ApproveBy = item.ApproveBy,
            ApproveComment = item.ApproveComment,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            PurchaseGroup = item.PurchaseGroup,
            BuyFee = item.BuyFee,
            RouterShipping = item.RouterShipping,
            CommodityGroup = item.CommodityGroup,
            AirFreight = item.AirFreight,
            SeaFreight = item.SeaFreight,
            Surcharge = item.Surcharge,
            DomesticShiping = item.DomesticShiping,
            DomesticTracking = item.DomesticTracking,
            DomesticCarrier = item.DomesticCarrier,
            DomesticPackage = item.DomesticPackage,

            BidUsername = orderProduct.FirstOrDefault()?.BidUsername,
            SellerId = orderProduct.FirstOrDefault()?.SellerId,
            ShipmentRouting = item.ShipmentRouting,

            Image = item.Image,
            Description = item.Description,
            Total = item.Total,
            Paid = item.Paid,
            Weight = item.Weight,
            Width = item.Width,
            Height = item.Height,
            Length = item.Length,
            TotalDiscountAmount = orderProduct.Sum(x => x.AmountDiscount),
            File = JsonConvert.DeserializeObject<List<FileDto>>(string.IsNullOrEmpty(item.File) ? "" : item.File),
            Reference = reference.Select(x => new DocumentDto()
            {
                RefId = (Guid)x.Id,
                RefCode = x.Code,
                RefResourceCode = x.Type

            }).ToList(),
            OrderProduct = orderProduct,
            OrderServiceAdd = item.OrderServiceAdd.Select(x => new OrderServiceAddDto()
            {
                Id = x.Id,
                OrderId = x.OrderId,
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
                OrderId = x.OrderId,
                OrderCode = x.OrderCode,
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
            OrderTracking = item.OrderTracking.Select(x => new OrderTrackingDto()
            {
                Id = x.Id,
                OrderId = x.OrderId,
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
            OrderInvoice = item.OrderInvoice.Select(x => new OrderInvoiceDto()
            {
                Id = x.Id,
                OrderId = x.OrderId,
                OrderCode = x.OrderCode,
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
            DeliveryProduct = expectedDelivery.Select(x => new DeliveryProductDto()
            {
                Id = x.Id,
                OrderProductId = x.OrderProductId,
                DeliveryDate = x.DeliveryDate,
                QuantityExpected = x.QuantityExpected,
                Description = x.Description
            }).ToList()
        };

        try
        {
            result.AccountId = item.AccountId ?? item.Customer?.AccountId;
            result.CustomerEmail = item.Customer?.Email;
        }
        catch
        {

        }

        return result;
    }

    public async Task<IEnumerable<OrderDto>> Handle(OrderQueryByIds request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetByIds(request.OrderIds);
        var orderList = new List<OrderDto>();
        foreach (var item in items)
        {
            if (item == null)
            {
                continue;
            }
            var reference = GetDocument(item.Code, item);
            var inventory = new List<INVENTORY_DETAIL_BY_LISTID>();
            var productPrice = new List<SP_GET_PRODUCT_PRICE_BY_LISTID>();
            var listId = new List<string>();
            var listProductId = new List<Guid?>();
            var i = 1;
            var source = item.OrderProduct.Select(x => x.ProductId).Distinct();
            foreach (var x in source)
            {
                listProductId.Add(x);
                if (listProductId.Count() == 50)
                {
                    listId.Add(string.Join(",", listProductId));
                    listProductId.RemoveRange(0, 50);
                }
                if (i == source.Count() && listProductId.Count() > 0)
                {
                    listId.Add(string.Join(",", listProductId));
                }
                i++;
            }
            foreach (var o in listId)
            {
                var dataInventory = await _PIMRepository.GetInventoryDetail(o);
                foreach (var x in dataInventory)
                {
                    var rs = new INVENTORY_DETAIL_BY_LISTID()
                    {
                        Id = x.Id,
                        WarehouseId = x.WarehouseId,
                        Code = x.Code,
                        Name = x.Name,
                        ProductId = x.ProductId,
                        StockQuantity = x.StockQuantity,
                        ReservedQuantity = x.ReservedQuantity,
                        PlannedQuantity = x.PlannedQuantity
                    };
                    inventory.Add(rs);
                }
                var dataPrice = await _PIMRepository.GetProductPrice(o);
                foreach (var x in dataPrice)
                {
                    var rs = new SP_GET_PRODUCT_PRICE_BY_LISTID()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Price = x.Price,
                        UnitId = x.UnitId,
                        Currency = x.Currency
                    };
                    productPrice.Add(rs);
                }
            }
            var expectedDelivery = await _deliveryProductRepository.GetByOrderId(item.Id);
            var orderProduct = item.OrderProduct.Select(x => new OrderProductDto(item.Calculation, item.ExchangeRate)
            {
                Id = x.Id,
                OrderId = x.OrderId,
                ContractId = x.ContractId,
                ContractName = x.ContractName,
                QuotationId = x.QuotationId,
                QuotationName = x.QuotationName,
                ProductId = x.ProductId,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage,
                ProductPrice = (item.ContractId != null || item.QuotationId != null) ? x.UnitPrice : productPrice.Where(y => y.Id == x.ProductId).FirstOrDefault()?.Price,
                ProductCurrency = productPrice.Where(y => y.Id == x.ProductId).FirstOrDefault()?.Currency,
                Origin = x.Origin,
                WarehouseId = x.WarehouseId,
                WarehouseCode = x.WarehouseCode,
                WarehouseName = x.WarehouseName,
                StockQuantity = !String.IsNullOrEmpty(x.WarehouseCode) ? inventory.Where(y => y.ProductId == x.ProductId && y.Code == x.WarehouseCode).FirstOrDefault()?.StockQuantity : inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.StockQuantity),
                TotalStockQuantity = inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.StockQuantity),
                ReservedQuantity = !String.IsNullOrEmpty(x.WarehouseCode) ? inventory.Where(y => y.ProductId == x.ProductId && y.Code == x.WarehouseCode).FirstOrDefault()?.ReservedQuantity : inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.ReservedQuantity),
                PlannedQuantity = !String.IsNullOrEmpty(x.WarehouseCode) ? inventory.Where(y => y.ProductId == x.ProductId && y.Code == x.WarehouseCode).FirstOrDefault()?.PlannedQuantity : inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.PlannedQuantity),
                Currency = item.Currency,
                PriceListId = x.PriceListId,
                PriceListName = x.PriceListName,
                UnitType = x.UnitType,
                UnitCode = x.UnitCode,
                UnitName = x.UnitName,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                DiscountAmountDistribution = x.DiscountAmountDistribution,
                DiscountType = x.DiscountType,
                DiscountPercent = x.DiscountPercent,
                AmountDiscount = x.AmountDiscount,
                DiscountTotal = x.DiscountTotal,
                TaxRate = x.TaxRate,
                Tax = x.Tax,
                TaxCode = x.TaxCode,
                ExpectedDate = x.ExpectedDate,
                Note = x.Note,
                QuantityReturned = x.QuantityReturned,
                QuantityReturnedActual = x.QuantityReturnedActual.HasValue ? x.QuantityReturnedActual.Value : 0,
                QuantityExported = x.QuantityExported.HasValue ? x.QuantityExported.Value : 0,
                QuantityPurchased = x.QuantityPurchased.HasValue ? x.QuantityPurchased.Value : 0,
                QuantityProductioned = x.QuantityProductioned.HasValue ? x.QuantityProductioned.Value : 0,
                QuantitySales = x.QuantitySales.HasValue ? x.QuantitySales.Value : 0,
                QuantitySaleActual = x.QuantitySaleActual,
                DisplayOrder = x.DisplayOrder,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                CreatedByName = x.CreatedByName,
                UpdatedByName = x.UpdatedByName,
                DeliveryStatus = x.DeliveryStatus,
                DeliveryQuantity = x.DeliveryQuantity,
                IsDiscount = (x.DiscountPercent > 0 || x.AmountDiscount > 0),
                EstimatedDeliveryDate = x.EstimatedDeliveryDate,
                ReturnAmount = x.ReturnAmount,
                SaleAmount = x.SaleAmount,
                SpecificationCode1 = x.SpecificationCode1,
                SpecificationCode2 = x.SpecificationCode2,
                SpecificationCode3 = x.SpecificationCode3,
                SpecificationCode4 = x.SpecificationCode4,
                SpecificationCode5 = x.SpecificationCode5,
                SpecificationCode6 = x.SpecificationCode6,
                SpecificationCode7 = x.SpecificationCode7,
                SpecificationCode8 = x.SpecificationCode8,
                SpecificationCode9 = x.SpecificationCode9,
                SpecificationCode10 = x.SpecificationCode10,
                SpecificationCodeJson = x.SpecificationCodeJson,
                BidUsername = x.BidUsername,
                SellerId = x.SellerId

            }).OrderBy(x => x.DisplayOrder).ToList();

            var result = new OrderDto()
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
                TypeDocument = item.TypeDocument,
                ContractId = item.ContractId,
                ContractName = item.ContractName,
                QuotationId = item.QuotationId,
                QuotationName = item.QuotationName,
                ChannelId = item.ChannelId,
                ChannelName = item.ChannelName,
                Status = item.Status,
                Currency = item.Currency,
                CurrencyName = item.CurrencyName,
                Calculation = item.Calculation,
                ExchangeRate = item.ExchangeRate,
                PriceListId = item.PriceListId,
                PriceListName = item.PriceListName,
                PaymentTermId = item.PaymentTermId,
                PaymentTermName = item.PaymentTermName,
                PaymentMethodName = item.PaymentMethodName,
                PaymentMethodId = item.PaymentMethodId,
                PaymentStatus = item.PaymentStatus,
                DeliveryPhone = item.DeliveryPhone,
                DeliveryName = item.DeliveryName,
                DeliveryAddress = item.DeliveryAddress,
                DeliveryCountry = item.DeliveryCountry,
                DeliveryProvince = item.DeliveryProvince,
                DeliveryDistrict = item.DeliveryDistrict,
                DeliveryWard = item.DeliveryWard,
                DeliveryNote = item.DeliveryNote,
                EstimatedDeliveryDate = item.EstimatedDeliveryDate,
                DeliveryTracking = item.DeliveryTracking,
                DeliveryCarrier = item.DeliveryCarrier,
                DeliveryPackage = item.DeliveryPackage,
                IsBill = item.IsBill,
                BillName = item.BillName,
                BillAddress = item.BillAddress,
                BillCountry = item.BillCountry,
                BillProvince = item.BillProvince,
                BillDistrict = item.BillDistrict,
                BillWard = item.BillWard,
                BillStatus = item.BillStatus,
                DeliveryMethodId = item.DeliveryMethodId,
                DeliveryMethodCode = item.DeliveryMethodCode,
                DeliveryMethodName = item.DeliveryMethodName,
                DeliveryStatus = item.DeliveryStatus,
                ShippingMethodId = item.ShippingMethodId,
                ShippingMethodCode = item.ShippingMethodCode,
                ShippingMethodName = item.ShippingMethodName,
                TypeDiscount = item.TypeDiscount,
                DiscountRate = item.DiscountRate,
                TypeCriteria = item.TypeCriteria,
                AmountDiscount = item.AmountDiscount,
                Note = item.Note,
                GroupEmployeeId = item.GroupEmployeeId,
                GroupEmployeeName = item.GroupEmployeeName,
                AccountId = item.AccountId,
                AccountName = item.AccountName,
                ApproveDate = item.ApproveDate,
                ApproveBy = item.ApproveBy,
                ApproveComment = item.ApproveComment,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                CreatedByName = item.CreatedByName,
                UpdatedByName = item.UpdatedByName,
                PurchaseGroup = item.PurchaseGroup,
                BuyFee = item.BuyFee,
                RouterShipping = item.RouterShipping,
                CommodityGroup = item.CommodityGroup,
                AirFreight = item.AirFreight,
                SeaFreight = item.SeaFreight,
                Surcharge = item.Surcharge,
                DomesticShiping = item.DomesticShiping,
                DomesticTracking = item.DomesticTracking,
                DomesticCarrier = item.DomesticCarrier,
                DomesticPackage = item.DomesticPackage,

                BidUsername = orderProduct.FirstOrDefault()?.BidUsername,
                SellerId = orderProduct.FirstOrDefault()?.SellerId,
                ShipmentRouting = item.ShipmentRouting,

                Image = item.Image,
                Description = item.Description,
                Total = item.Total,
                Paid = item.Paid,
                Weight = item.Weight,
                Width = item.Width,
                Height = item.Height,
                Length = item.Length,
                TotalDiscountAmount = orderProduct.Sum(x => x.AmountDiscount),
                File = JsonConvert.DeserializeObject<List<FileDto>>(string.IsNullOrEmpty(item.File) ? "" : item.File),
                Reference = reference.Select(x => new DocumentDto()
                {
                    RefId = (Guid)x.Id,
                    RefCode = x.Code,
                    RefResourceCode = x.Type

                }).ToList(),
                OrderProduct = orderProduct,
                OrderServiceAdd = item.OrderServiceAdd.Select(x => new OrderServiceAddDto()
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
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
                    OrderId = x.OrderId,
                    OrderCode = x.OrderCode,
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
                OrderTracking = item.OrderTracking.Select(x => new OrderTrackingDto()
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
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
                OrderInvoice = item.OrderInvoice.Select(x => new OrderInvoiceDto()
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    OrderCode = x.OrderCode,
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
                DeliveryProduct = expectedDelivery.Select(x => new DeliveryProductDto()
                {
                    Id = x.Id,
                    OrderProductId = x.OrderProductId,
                    DeliveryDate = x.DeliveryDate,
                    QuantityExpected = x.QuantityExpected,
                    Description = x.Description
                }).ToList()
            };

            try
            {
                result.AccountId = item.AccountId ?? item.Customer?.AccountId;
                result.CustomerEmail = item.Customer?.Email;
            }
            catch
            {

            }
            orderList.Add(result);
        }
        return orderList;
    }

    public async Task<PagedResult<List<OrderDto>>> Handle(OrderPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<OrderDto>>();
        var fopRequest = FopExpressionBuilder<VFi.Domain.SO.Models.Order>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status.HasValue)
            filter.Add("status", request.Status.ToString());
        if (request.DomesticStatus.HasValue)
            filter.Add("domesticStatus", request.DomesticStatus.ToString());
        if (!string.IsNullOrEmpty(request.CustomerId))
            filter.Add("customerId", new Guid(request.CustomerId));
        if (!string.IsNullOrEmpty(request.EmployeeId))
            filter.Add("employeeId", new Guid(request.EmployeeId));

        List<string>? trackings = null;
        List<string>? codes = null;

        if (!string.IsNullOrEmpty(request.Keyword) && Regex.Match(request.Keyword, "^\\d+([-\\s]\\d+)*$").Success)
        {
            trackings = request.Keyword.Replace("-", "").Split(" ").Select(x => x.Trim()).ToList();
            if (trackings.Count > 1)
            {
                request.Keyword = null;
            }
        }

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            var regexOrderCode = Regex.Match(request.Keyword, "(?<code>MG\\d+)(([\\s\\n\\t\\r])?)+");
            if (regexOrderCode.Success)
            {
                string[] groupsArray = regexOrderCode.Groups.Cast<System.Text.RegularExpressions.Group>()
                               .Select(g => g.Value)
                               .ToArray();
                codes = groupsArray.ToList();
            }

        }

        var (datas, count) = await _repository.FilterCustom(request.Keyword, codes, trackings, filter, fopRequest);
        var exportWarehouseProducts = (await _exportWarehouseProductRepository.GetByOrderIds(datas.Select(x => x.Id))).Distinct().ToList();
        var requestPurchases = (await _requestPurchaseProductRepository?.GetByOrderIds(datas.Select(x => x.Id))).Distinct().ToList();
        var data = datas.Select(item =>
        {
            var exportWarehouseProduct = exportWarehouseProducts.FirstOrDefault(x => x.OrderId == item.Id);
            var requestPurchase = requestPurchases.FirstOrDefault(x => x.OrderId == item.Id);
            return new OrderDto()
            {
                Id = item.Id,
                OrderType = item.OrderType,
                Code = item.Code,
                QuotationCode = item.Quotation?.Code,
                ContractCode = item.Contract?.Code,
                CustomerId = item.CustomerId,
                CustomerName = item.CustomerName,
                OrderDate = item.OrderDate,
                AccountId = item.AccountId ?? item.Customer?.AccountId,
                CustomerEmail = item.Customer?.Email,
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
                ExchangeRate = item.ExchangeRate,
                TypeDiscount = item.TypeDiscount,
                DiscountRate = item.DiscountRate,
                TypeCriteria = item.TypeCriteria,
                AmountDiscount = item.AmountDiscount,
                Note = item.Note,
                InternalNote = item.InternalNote,
                IsReturned = item?.OrderProduct?.Any(x => x.Quantity > x.QuantityReturned),
                ProductLink = item?.OrderProduct?.FirstOrDefault()?.SourceLink,
                CreatedBy = item?.CreatedBy,
                CreatedDate = item?.CreatedDate,
                UpdatedBy = item?.UpdatedBy,
                UpdatedDate = item?.UpdatedDate,
                CreatedByName = item?.CreatedByName,
                UpdatedByName = item?.UpdatedByName,
                Image = item?.Image,
                Description = item?.Description,
                PaymentStatus = item?.PaymentStatus,
                TotalAmountTax = item?.TotalAmountTax,

                TotalAmountOriginal = item?.TotalAmountTax
                    + item?.OrderServiceAdd
                        ?.Where(x => x.ExchangeRate != 1 && x.ServiceAddName != "Phí mua hộ")
                        ?.Sum(x => x.Price),
                ShippingFee = (item?.OrderServiceAdd != null && item.OrderServiceAdd.Any(x => x.ServiceAddName == "Vận chuyển nội địa"))
                    ? item?.OrderServiceAdd
                        ?.Where(x => x.ExchangeRate != 1 && x.ServiceAddName == "Vận chuyển nội địa")
                        ?.Sum(x => x.Price)
                    : null,

                TotalServiceAmount = item?.OrderServiceAdd
                    .Sum(x => x.Price * (x.Calculation == "/" ? (1 / (x.ExchangeRate ?? 1)) : (x.ExchangeRate ?? 1))),

                TotalAmountCollected = item?.PaymentInvoice
                    .Where(x => (new[] { "11", "12", "13" }).Contains(x.Type))
                    .Sum(x => x.Amount * (x.Calculation == "/" ? (1 / (x.ExchangeRate ?? 1)) : (x.ExchangeRate ?? 1))),

                TotalAmountConvert = item?.TotalAmountTax.GetValueOrDefault() * item?.ExchangeRate.GetValueOrDefault()
                        + item?.OrderServiceAdd?.Sum(x => x.Price * x.ExchangeRate).GetValueOrDefault(),

                TotalAmountNeedPay = (item?.TotalAmountTax.GetValueOrDefault() * item?.ExchangeRate.GetValueOrDefault()
                        + item?.OrderServiceAdd?.Sum(x => x.Price * x.ExchangeRate).GetValueOrDefault()).GetValueOrDefault()
                        - (item?.PaymentInvoice
                    .Where(x => (new[] { "11", "12", "13" }).Contains(x.Type))
                    .Sum(x => x.Amount * (x.Calculation == "/" ? (1 / (x.ExchangeRate ?? 1)) : (x.ExchangeRate ?? 1)))).GetValueOrDefault(),

                CountPaymentInvoice = item?.PaymentInvoice?.Count ?? 0,

                DeliveryAddress = item?.DeliveryAddress,
                DeliveryWard = item?.DeliveryWard,
                DeliveryDistrict = item?.DeliveryDistrict,
                DeliveryProvince = item?.DeliveryProvince,
                DeliveryCountry = item?.DeliveryCountry,
                DeliveryPhone = item?.DeliveryPhone,
                DeliveryName = item?.DeliveryName,
                DeliveryNote = item?.DeliveryNote,
                DomesticTracking = item?.DomesticTracking,
                DomesticCarrier = item?.DomesticCarrier,

                BidUsername = item?.OrderProduct?.FirstOrDefault()?.BidUsername,
                SellerId = item?.OrderProduct?.FirstOrDefault()?.SellerId,
                ProductSourceCode = item?.OrderProduct?.FirstOrDefault()?.SourceCode,
                DomesticStatus = item?.DomesticStatus,
                ShipmentRouting = item?.ShipmentRouting,
                RouterShipping = item?.RouterShipping,
                DomesticDeliveryDate = item?.DomesticDeliveryDate,
                ExportWarehouseProductId = exportWarehouseProduct?.Id,
                ExportWarehouseId = exportWarehouseProduct?.ExportWarehouseId,
                ExportWarehouseCode = exportWarehouseProduct?.ExportWarehouse?.Code,
                ExportWarehouseStatus = exportWarehouseProduct?.ExportWarehouse?.Status,
                ExportWarehouseCreatedDate = exportWarehouseProduct?.ExportWarehouse?.CreatedDate,

                RequestPurchaseId = requestPurchase?.RequestPurchaseId,
                RequestPurchaseCode = requestPurchase?.RequestPurchase.Code,
                RequestPurchaseStatus = requestPurchase?.RequestPurchase.Status,
                RequestPurchaseCreateDate = requestPurchase?.RequestPurchase.CreatedDate,
            };
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }


    public async Task<IEnumerable<OrderListBoxDto>> Handle(OrderQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        if (!String.IsNullOrEmpty(request.QueryParams.Status))
        {
            filter.Add("status", request.QueryParams.Status);
        }
        if (!String.IsNullOrEmpty(request.QueryParams.DiferenceStatus))
        {
            filter.Add("diferenceStatus", request.QueryParams.DiferenceStatus);
        }
        if (request.QueryParams.OrderDate != null)
        {
            filter.Add("orderDate", request.QueryParams.OrderDate);
        }
        if (request.QueryParams.CustomerId != null)
        {
            filter.Add("customerId", request.QueryParams.CustomerId);
        }
        if (request.QueryParams.WarehouseId != null)
        {
            filter.Add("warehouseId", request.QueryParams.WarehouseId);
        }
        if (request.QueryParams.IsContract != null)
        {
            filter.Add("isContract", request.QueryParams.IsContract.ToString());
        }
        if (request.QueryParams.IsQuotation != null)
        {
            filter.Add("isQuotation", request.QueryParams.IsQuotation.ToString());
        }
        if (request.QueryParams.StatusExport != null)
        {
            filter.Add("statusExport", request.QueryParams.StatusExport.ToString());
        }
        if (request.QueryParams.StatusPurchase != null)
        {
            filter.Add("statusPurchase", request.QueryParams.StatusPurchase.ToString());
        }
        if (request.QueryParams.StatusReturn != null)
        {
            filter.Add("statusReturn", request.QueryParams.StatusReturn.ToString());
        }
        if (request.QueryParams.StatusSales != null)
        {
            filter.Add("statusSales", request.QueryParams.StatusSales.ToString());
        }
        if (request.QueryParams.FromDate != null)
        {
            filter.Add("fromDate", request.QueryParams.FromDate);
        }
        if (request.QueryParams.ToDate != null)
        {
            filter.Add("toDate", request.QueryParams.ToDate);
        }
        if (!String.IsNullOrEmpty(request.QueryParams.OrderType))
        {
            filter.Add("orderType", request.QueryParams.OrderType);
        }
        if (request.QueryParams.Currency != null)
        {
            filter.Add("currency", request.QueryParams.Currency);
        }
        if (request.QueryParams.StatusProduction != null)
        {
            filter.Add("statusProduction", request.QueryParams.StatusProduction.ToString());
        }
        var Orders = await _repository.GetListListBox(request.Keyword, filter, request.PageSize, request.PageIndex);
        var result = Orders.Select(x => new OrderListBoxDto()
        {
            Id = x.Id,
            Code = x.Code,
            OrderDate = x.OrderDate,
            AccountName = x.AccountName,
            CustomerId = x.CustomerId,
            CustomerCode = x.CustomerCode,
            CustomerName = x.CustomerName,
            Currency = x.Currency,
            CurrencyName = x.CurrencyName,
            TypeDiscount = x.TypeDiscount,
            DiscountRate = x.DiscountRate,
            AmountDiscount = x.AmountDiscount,
            TypeCriteria = x.TypeCriteria,
            TotalAmountTax = x.TotalAmountTax,
            Status = x.Status,
            Description = x.Description,
            Calculation = x.Calculation,
            ExchangeRate = x.ExchangeRate
        });
        return result.OrderByDescending(x => x.OrderDate);
    }

    public async Task<IEnumerable<OrderReferenceDto>> Handle(OrderGetReferenceQuery request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        if (!String.IsNullOrEmpty(request.QueryParams.Status))
        {
            filter.Add("status", request.QueryParams.Status);
        }
        if (!String.IsNullOrEmpty(request.QueryParams.DiferenceStatus))
        {
            filter.Add("diferenceStatus", request.QueryParams.DiferenceStatus);
        }
        if (request.QueryParams.ContractId != null)
        {
            filter.Add("contractId", request.QueryParams.ContractId);
        }
        if (request.QueryParams.ProductId != null)
        {
            filter.Add("productId", request.QueryParams.ProductId);
        }
        var data = await _repository.GetReference(filter);
        var result = data.Select(x => new OrderReferenceDto()
        {
            Id = x.Id,
            Code = x.Code,
            CustomerName = x.CustomerName,
            Currency = x.Currency,
            CurrencyName = x.CurrencyName,
            OrderDate = x.OrderDate,
            Status = x.Status,
            CreatedDate = x.CreatedDate
        });
        return result;
    }

    private List<OrderDetailPrintDto> CalculatorDiscount(VFi.Domain.SO.Models.Order obj)
    {
        List<OrderDetailPrintDto> list = new List<OrderDetailPrintDto>();

        decimal? totalAmountDiscount = 0;
        decimal? amountNoTax = 0;
        decimal? amountTax = 0;
        decimal? totalAmountTax = 0;
        foreach (var a in obj.OrderProduct.OrderBy(x => x.DisplayOrder))
        {
            totalAmountDiscount = (a.DiscountAmountDistribution ?? 0) + (a.AmountDiscount ?? 0);
            amountNoTax = (a.Quantity * a.UnitPrice) - totalAmountDiscount;
            amountTax = amountNoTax * (decimal)(a.TaxRate ?? 0) / 100;
            if (Domain.SO.Constants.OrderCurrency._currencyListRound.Contains(obj.Currency))
            {
                amountTax = Math.Ceiling(amountNoTax.GetValueOrDefault() * (decimal)(a.TaxRate ?? 0) / 100);
            }
            totalAmountTax = amountNoTax + amountTax;

            list.Add(new OrderDetailPrintDto()
            {
                Id = a.Id,
                ProductCode = a.ProductCode,
                ProductName = a.ProductName,
                UnitName = a.UnitName,
                Tax = a.Tax,
                Quantity = FormatNumber.AddPeriod(a.Quantity, 2),
                UnitPrice = FormatNumber.AddPeriod(a.UnitPrice, 2),
                AmountNoVat = FormatNumber.AddPeriod(a.Quantity * a.UnitPrice, 2),
                AmountNoDiscount = a.Quantity * a.UnitPrice,
                DiscountAmountDistribution = a.DiscountAmountDistribution,
                TotalAmountDiscount = totalAmountDiscount,
                AmountNoTax = amountNoTax,
                AmountTax = amountTax,
                TotalAmountTax = totalAmountTax
            });
        }
        return list;
    }
    public async Task<OrderPrintDto> Handle(OrderGetDataPrint request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetById(request.OrderId);
        List<OrderDetailPrintDto> detail = CalculatorDiscount(data);
        var AccountPhone = "";
        if (data.AccountId != null)
        { AccountPhone = _employeeRepository.GetByAccountId((Guid)data.AccountId).Result?.Phone ?? null; }
        var StorePhone = "";
        var StoreAddress = "";
        if (data.StoreId != null)
        {
            var store = await _storeRepository.GetById((Guid)data.StoreId);
            StorePhone = store?.Phone ?? "";
            StoreAddress = store?.Address ?? "";
        }
        var result = new OrderPrintDto()
        {
            Id = data.Id,
            Code = data.Code,
            CustomerCode = data.CustomerCode,
            CustomerName = data.CustomerName,
            CustomerPhone = data.Customer?.Phone,
            Description = data.Description,
            Tracking = data.DomesticTracking,
            Address = data.DeliveryAddress + (!String.IsNullOrEmpty(data.DeliveryWard) ? ", " + data.DeliveryWard : "") + (!String.IsNullOrEmpty(data.DeliveryDistrict) ? ", " + data.DeliveryDistrict : "") + (!String.IsNullOrEmpty(data.DeliveryProvince) ? ", " + data.DeliveryProvince : "") + (!String.IsNullOrEmpty(data.DeliveryCountry) ? ", " + data.DeliveryCountry : ""),
            StoreAddress = StoreAddress,
            StoreName = data.StoreName,
            StorePhone = StorePhone,
            OrderDate = "Ngày " + Convert.ToDateTime(data.OrderDate).ToString("dd") + " tháng " + Convert.ToDateTime(data.OrderDate).ToString("MM") + " năm " + Convert.ToDateTime(data.OrderDate).ToString("yyyy"),
            OrderDateLabel = Convert.ToDateTime(data.OrderDate).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
            Currency = data.Currency,
            CurrencyName = data.CurrencyName,
            AccountName = data.AccountName,
            AccountId = data.AccountId,
            Note = data.Description,
            PrintedDateString = "Ngày " + Convert.ToDateTime(DateTime.Now).ToString("dd") + " tháng " + Convert.ToDateTime(DateTime.Now).ToString("MM") + " năm " + Convert.ToDateTime(DateTime.Now).ToString("yyyy"),
            TotalAmountNoTax = FormatNumber.AddPeriod(detail?.Sum(x => x.AmountNoDiscount), data.Currency != "VND" ? 2 : 0),
            TotalDiscount = FormatNumber.AddPeriod(detail?.Sum(x => x.TotalAmountDiscount) * (1), data.Currency != "VND" ? 2 : 0),
            TotalTaxValue = FormatNumber.AddPeriod(detail?.Sum(x => x.AmountTax), data.Currency != "VND" ? 2 : 0),
            Total = FormatNumber.AddPeriod(detail?.Sum(x => decimal.Parse(x.Quantity))),
            OrderServiceAdd = data.OrderServiceAdd.Select(x => new OrderServiceAddPrintDto()
            {
                ServiceAddName = x.ServiceAddName,
                Price = x.Price,
                PriceString = FormatNumber.AddPeriod(x.Price, data.Currency != "VND" ? 2 : 0)
            }).ToList(),
            TotalAmount = FormatNumber.AddPeriod(data.TotalAmountTax + data.OrderServiceAdd.Sum(x => x.Price), data.Currency != "VND" ? 2 : 0),
            TotalAmountText = Utilities.NumberToText_Currency((double)(data.TotalAmountTax + data.OrderServiceAdd.Sum(x => x.Price)), data.Currency),
            Files = JsonConvert.DeserializeObject<List<FileDto>>(string.IsNullOrEmpty(data.File) ? "" : data.File),
            Details = detail
        };
        return result;
    }
    public async Task<MemoryStream> Handle(OrderExportTemplateQuery request, CancellationToken cancellationToken)
    {
        var contentBytes = await _exportTemplateRepository.GetTemplate("MAU_IMPORT_CHI_TIET_DON_HANG");

        MemoryStream memoryStream = new();

        memoryStream.SetLength(0);

        memoryStream.Write(contentBytes, 0, contentBytes.Length);

        string[] CellReferenceArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ" };
        using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(memoryStream, true))
        {
            SharedStringTablePart shareStringPart;
            if (spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
            {
                shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
            }
            else
            {
                shareStringPart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();
            }
        }
        return memoryStream;
    }

    public async Task<List<Relate>> Handle(OrderQueryRelatedById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.OrderId);
        if (item is null)
        {
            return new List<Relate>();
        }
        return GetDocument(item.Code, item);
    }
    private List<Relate> GetDocument(string code, Order item)
    {
        List<Relate> rela = new List<Relate>();
        List<Relate> datacheck = new List<Relate>();
        // Đề nghị xuất kho
        IEnumerable<ExportWarehouseProduct> exportWarehouseProduct = _exportWarehouseProductRepository.GetByOrderId(code).Result;
        // Hợp đồng
        IEnumerable<OrderProduct> contractRepository = _orderProductRepository.GetContractByOrderId(code).Result;
        // trả hàng
        IEnumerable<ReturnOrderProduct> returnOrderProductRepository = _returnOrderProductRepository.GetByOrderId(code).Result;
        //Đề nghị mua
        IEnumerable<RequestPurchaseProduct> requestPurchaseProductRepository = _requestPurchaseProductRepository.GetByOrderId(code).Result;
        //Giảm giá
        IEnumerable<SalesDiscountProduct> salesDicountProductRepository = _salesDiscountProductRepository.GetByOrderId(code).Result;

        if (exportWarehouseProduct.Count() > 0)
        {
            List<Relate> relates = exportWarehouseProduct.Select(x => new Relate() { Code = x.ExportWarehouse.Code, Id = x.ExportWarehouse.Id, Type = ResourceCode.DNX }).Distinct().ToList();
            rela.AddRange(relates);
        }
        if (contractRepository.Count() > 0)
        {
            List<Relate> relates = contractRepository.Where(x => x.ContractId != null).Select(x => new Relate() { Code = x.ContractName, Id = x.ContractId, Type = ResourceCode.HD }).Distinct().ToList();
            rela.AddRange(relates);
        }
        if (returnOrderProductRepository.Count() > 0)
        {
            List<Relate> relates = returnOrderProductRepository.Select(x => new Relate() { Code = x.ReturnOrder.Code, Id = x.ReturnOrder.Id, Type = ResourceCode.RO }).Distinct().ToList();
            rela.AddRange(relates);
        }
        if (requestPurchaseProductRepository.Count() > 0)
        {
            List<Relate> relates = requestPurchaseProductRepository.Select(x => new Relate() { Code = x.RequestPurchase.Code, Id = x.RequestPurchase.Id, Type = ResourceCode.RQ }).Distinct().ToList();
            rela.AddRange(relates);
        }
        if (salesDicountProductRepository.Count() > 0)
        {
            List<Relate> relates = salesDicountProductRepository.Select(x => new Relate() { Code = x.SalesDiscount.Code, Id = x.SalesDiscount.Id, Type = ResourceCode.GGB }).Distinct().ToList();
            rela.AddRange(relates);
        }
        if (item.ContractId != null)
        {
            rela.Add(new Relate()
            {
                Code = item.ContractName,
                Id = item.ContractId,
                Type = ResourceCode.HD
            });
        }
        if (item.QuotationId != null)
        {
            rela.Add(new Relate()
            {
                Code = item.QuotationName,
                Id = item.QuotationId,
                Type = ResourceCode.BG
            });
        }
        var data = rela.Select(x => new { x.Id, x.Code, x.Type }).Distinct().ToList();
        foreach (var o in data)
        {
            datacheck.Add(new Relate
            {
                Id = o.Id,
                Code = o.Code,
                Type = o.Type
            });
        }
        return datacheck.Distinct().ToList();
    }
    public static int? GetColumnIndex(string cellRef)
    {
        if (string.IsNullOrEmpty(cellRef))
            return null;

        cellRef = cellRef.ToUpper();

        int columnIndex = -1;
        int mulitplier = 1;

        foreach (char c in cellRef.ToCharArray().Reverse())
        {
            if (char.IsLetter(c))
            {
                columnIndex += mulitplier * (c - 64);
                mulitplier = mulitplier * 26;
            }
        }

        return columnIndex;
    }
    public async Task<List<OrderValidateDto>> Handle(ValidateExcelOrderQuery request, CancellationToken cancellationToken)
    {
        List<OrderValidateDto> result = new List<OrderValidateDto>();
        try
        {
            var taxlist = await _PIMRepository.GetListBox();

            using MemoryStream stream = new MemoryStream();
            request.File.CopyTo(stream);
            using SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, false);
            WorkbookPart workbookPart = doc.WorkbookPart;
            Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(request.SheetId)).Worksheet;
            SheetData? thesheetdata = theWorksheet?.GetFirstChild<SheetData>();


            for (int i = (request.HeaderRow); thesheetdata is not null && i < thesheetdata.ChildElements.Count; i++)
            {
                Row thecurrentRow = (Row)thesheetdata.ChildElements[i];
                OrderValidateDto OrderValidateDto = new OrderValidateDto()
                {
                    Row = thecurrentRow.RowIndex
                };


                foreach (ValidateField field in request.ListField)
                {
                    if (field.IndexColumn >= 0 && field.IndexColumn < thecurrentRow.ChildElements.Count)
                    {
                        Cell thecurrentCell = thecurrentRow.Elements<Cell>().FirstOrDefault(cell => GetColumnIndex(cell.CellReference) == field.IndexColumn);
                        if (thecurrentCell != null)
                        {
                            string cellValue = "";
                            if (thecurrentCell.DataType != null && thecurrentCell.DataType == CellValues.SharedString)
                            {
                                int id;
                                if (Int32.TryParse(thecurrentCell.InnerText, out id))
                                {
                                    SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                    if (item != null && item.Text != null)
                                    {
                                        cellValue = item.Text.Text;
                                    }
                                }
                            }
                            else
                            {
                                cellValue = (thecurrentCell.CellValue == null) ? thecurrentCell.InnerText : thecurrentCell.CellValue.Text;
                            }

                            switch (field.Field)
                            {
                                case "productCode":
                                    if (String.IsNullOrEmpty(cellValue))
                                    {
                                        OrderValidateDto.Errors.Add($"productCode {cellValue} invalid");
                                    }
                                    else
                                    {
                                        OrderValidateDto.ProductCode = cellValue;
                                    }
                                    break;
                                case "productName":
                                    if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(OrderValidateDto.ProductCode))
                                    {
                                        OrderValidateDto.Errors.Add($"productName {cellValue} invalid");
                                        OrderValidateDto.ProductName = cellValue;
                                    }
                                    break;
                                case "unitCode":
                                    if (String.IsNullOrEmpty(cellValue))
                                    {
                                        OrderValidateDto.Errors.Add($"unitCode {cellValue} invalid");
                                    }
                                    else
                                    {
                                        OrderValidateDto.UnitCode = cellValue;
                                    }
                                    break;
                                case "unitName":
                                    if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(OrderValidateDto.UnitCode))
                                    {
                                        OrderValidateDto.UnitName = cellValue;
                                    }
                                    break;
                                case "unitPrice":
                                    try
                                    {
                                        double amount;
                                        if (!String.IsNullOrEmpty(cellValue))
                                        {
                                            if (!double.TryParse(cellValue, out amount))
                                            {
                                                OrderValidateDto.Errors.Add($"UnitPrice {cellValue} notincorrectformat");
                                            }
                                        }
                                    }

                                    catch
                                    {
                                        OrderValidateDto.Errors.Add("UnitPrice notincorrectformat");
                                    }

                                    OrderValidateDto.UnitPrice = cellValue.Replace(',', '.');
                                    ;

                                    break;
                                case "quantity":
                                    try
                                    {
                                        OrderValidateDto.Quantity = cellValue.Replace(',', '.');

                                        double quantityRequest;
                                        if (!String.IsNullOrEmpty(cellValue))
                                        {
                                            if (!double.TryParse(cellValue, out quantityRequest))
                                            {
                                                OrderValidateDto.Errors.Add($"Quantity {cellValue} notincorrectformat");
                                            }
                                            /*var cellV = double.Parse(cellValue, CultureInfo.InvariantCulture);
                                            var stockQuantity = double.Parse(OrderValidateDto.StockQuantity ?? "0", CultureInfo.InvariantCulture);
                                            if (cellV > stockQuantity)
                                            {
                                                OrderValidateDto.Errors.Add($"Quantity {cellValue} > {stockQuantity}");
                                                OrderValidateDto.Quantity = cellValue.Replace(',', '.');

                                            }*/
                                            else
                                            {
                                                OrderValidateDto.Quantity = cellValue.Replace(',', '.');
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        OrderValidateDto.Errors.Add($"QuantityReturn {cellValue} notincorrectformat");
                                    }

                                    break;
                                case "discountPercent":
                                    var a = cellValue.Replace(',', '.');
                                    double dp;
                                    if (!String.IsNullOrEmpty(a))
                                    {
                                        if (!double.TryParse(a, out dp))
                                        {
                                            OrderValidateDto.Errors.Add($"discountPercent {a} notincorrectformat");

                                        }
                                        else
                                        {
                                            OrderValidateDto.DiscountPercent = dp.ToString();
                                        }
                                    }
                                    break;

                                case "tax":
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(cellValue))
                                        {
                                            var taxcheck = taxlist.Where(x => x.Name == cellValue || x.Code == cellValue).FirstOrDefault();
                                            if (taxcheck != null)
                                            {
                                                OrderValidateDto.TaxCategoryId = taxcheck.Id.ToString();
                                                OrderValidateDto.Tax = taxcheck.Name;
                                                OrderValidateDto.TaxCode = taxcheck.Code;
                                                OrderValidateDto.TaxRate = taxcheck.Rate.ToString();
                                            }
                                            else
                                            {
                                                OrderValidateDto.Errors.Add("tax incorrect");
                                                OrderValidateDto.TaxCategoryId = cellValue;
                                            }
                                        }
                                        else
                                        {
                                            OrderValidateDto.TaxCategoryId = null;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        OrderValidateDto.Errors.Add("" + ex + "");
                                    }

                                    break;
                                case "note":
                                    if (!string.IsNullOrEmpty(cellValue))
                                    {
                                        /*var getReason = await _reason.GetByName(cellValue);

                                        if (getReason != null)
                                        {
                                            OrderValidateDto.ReasonName = cellValue;
                                            OrderValidateDto.ReasonId = getReason?.Id;

                                        }
                                        else
                                        {
                                            var item = new Reason
                                            {
                                                Id = Guid.NewGuid(),
                                                Code = "",
                                                Name = cellValue,
                                                Status = 1,
                                                CreatedDate = DateTime.Now,
                                                CreatedBy = _context.GetUserId(),
                                                CreatedByName = _context.UserClaims.FullName
                                            };

                                            _reason.Add(item);
                                        }*/
                                        OrderValidateDto.Note = cellValue;

                                    }
                                    else
                                    {
                                        OrderValidateDto.Note = null;
                                        OrderValidateDto.Note = null;
                                    }


                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                if (!String.IsNullOrEmpty(OrderValidateDto.ProductCode))
                {
                    result.Add(OrderValidateDto);
                }
            }
        }
        catch (Exception)
        {
        }
        var resultCode = result.Where(x => !string.IsNullOrEmpty(x.ProductCode)).Select(x => x.ProductCode);
        var getlistunti = await _PIMRepository.GetUnitPaging(1, 500, null, null);

        if (resultCode is not null && resultCode.Count() > 0)
        {
            var getproduct = await _PIMRepository.GetProductByListCode(resultCode.ToList());// get code true
            var e = getproduct.Select(x => x.Code);

            var getProductContai = result.Where(x => e.Contains(x.ProductCode)).ToList();
            var getProductError = result.Where(x => !e.Contains(x.ProductCode)).ToList();

            if (getProductContai != null && getProductContai.Count() > 0)
            {
                foreach (var i in getProductContai)
                {
                    var a = result.Where(x => x.ProductCode == i.ProductCode.Trim()).FirstOrDefault();
                    var getP = getproduct.Where(x => x.Code == i.ProductCode.Trim()).FirstOrDefault();
                    if (a != null)
                    {
                        a.ProductId = getP.Id;
                        a.ProductName = getP.Name;
                        a.ProductCode = getP.Code;
                        a.UnitType = getP.UnitType;
                        a.StockQuantity = getP.StockQuantity;
                        var unit = getlistunti.FirstOrDefault(x => x.Code.Equals(a.UnitCode) && x.GroupUnitCode == a.UnitType);
                        if (unit is not null)
                        {
                            a.UnitCode = unit.Code;
                            a.UnitName = unit.Name;
                            a.UnitId = unit.Id;
                        }
                        else
                        {
                            a.Errors.Add($"Unit code {a.UnitCode} invalid");
                            a.UnitCode = a.UnitCode;
                            a.UnitName = a.UnitName;

                        }
                    }
                }
            }

            if (getProductError != null && getProductError.Count() > 0)
            {
                foreach (var i in getProductError)
                {
                    var a = result.Where(x => x.ProductCode == i.ProductCode.Trim()).FirstOrDefault();

                    if (a != null)
                    {
                        a.Errors.Add($"{a.ProductCode} invalid");
                    }
                }
            }
        }
        return result;
    }

    public async Task<IEnumerable<SendConfigComboboxDto>> Handle(OrderQuerySendConfigCombobox request, CancellationToken cancellationToken)
    {
        var data = await _emailMasterRepository.GetListboxSendConfig();

        var result = data.Select(x => new SendConfigComboboxDto
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            From = x.From,
            FromName = x.FromName
        });

        return result;
    }

    public async Task<IEnumerable<SendTemplateComboboxDto>> Handle(OrderQuerySendTemplateCombobox request, CancellationToken cancellationToken)
    {
        var data = await _emailMasterRepository.GetListboxSendTemplate();

        var result = data.Select(x => new SendTemplateComboboxDto
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            Description = x.Description,
            Status = x.Status,
            Type = x.Type
        });

        return result;
    }

    public async Task<EmailBodyDto> Handle(OrderEmailBuilderQuery request, CancellationToken cancellationToken)
    {
        var data = await _emailMasterRepository.EmailBuilder(request.Subject, request.JBody, request.Template);

        var result = new EmailBodyDto
        {
            Subject = data.Subject,
            Body = data.Body,
            BodyText = data.BodyText
        };

        return result;
    }

    public async Task<IEnumerable<SendTransactionDto>> Handle(SendTransactionQueryByOrder request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, string?> {
            { "keyword", request.Keyword },
            { "order", request.Order }
        };

        var data = await _emailMasterRepository.GetListSendTransaction(filter);
        var result = data.Select(x => new SendTransactionDto
        {
            Id = x.Id,
            Order = x.Order,
            Subject = x.Subject,
            SendDate = x.SendDate,
            Open = x.Open,
            Click = x.Click,
        });

        return result;
    }

    public async Task<PreviewRecalculatedPriceDto> Handle(PreviewRecalculatedPriceQuery request, CancellationToken cancellationToken)
    {
        var data = await _repositoryProcedure.SP_GET_CROSS_ORDER_RECALCULATE_PRICEAsync(request.Id);
        if (data == null)
        {
            return null;
        }
        var result = new PreviewRecalculatedPriceDto
        {
            Id = data.Id,
            Code = data.Code,
            Description = data.Description,
            DomesticShipping = data.DomesticShipping,
            Status = data.Status,
            TotalAmountTax = data.TotalAmountTax,
            NewPaymentPrice = data.NewPaymentPrice,
            OldPaymentPrice = data.OldPaymentPrice
        };
        return result;
    }
}
