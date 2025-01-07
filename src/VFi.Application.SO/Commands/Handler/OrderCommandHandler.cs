using FluentValidation.Results;
using MediatR;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Events;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands.Handler;

internal class OrderCommandHandler : CommandHandler, IRequestHandler<AddOrderCommand, ValidationResult>,
    IRequestHandler<DeleteOrderCommand, ValidationResult>,
    IRequestHandler<EditOrderCommand, ValidationResult>,
    IRequestHandler<CreateOrderCommand, ValidationResult>,
    IRequestHandler<ApprovalOrderCommand, ValidationResult>,
    IRequestHandler<ApprovalOrdersCommand, ValidationResult>,
    IRequestHandler<ManagePaymentOrderCommand, ValidationResult>,
    IRequestHandler<ManageServiceOrderCommand, ValidationResult>,
    IRequestHandler<NoteOrderCommand, ValidationResult>,
    IRequestHandler<OrderUploadFileCommand, ValidationResult>,
    IRequestHandler<OrderEmailNotifyCommand, ValidationResult>,
    IRequestHandler<RecalculatePriceCommand, ValidationResult>
{
    private readonly IOrderRepository _repository;
    private readonly IOrderProductRepository _orderDetailRepository;
    private readonly IOrderServiceAddRepository _orderServiceAddRepository;
    private readonly IPaymentInvoiceRepository _paymentInvoiceRepository;
    private readonly IContextUser _context;
    private readonly ISOContextProcedures _repositoryProcedure;
    private readonly ISyntaxCodeRepository _syntaxCodeRepository;
    private readonly IOrderTrackingRepository _orderTrackingRepository;
    private readonly IOrderInvoiceRepository _orderInvoiceRepository;
    private readonly IDeliveryProductRepository _deliveryProductRepository;
    private readonly IEmailMasterRepository _emailMasterRepository;
    private readonly IEventRepository _eventRepository;

    public OrderCommandHandler(
        IOrderRepository orderRepository,
        IOrderProductRepository orderDetailRepository,
        IOrderServiceAddRepository orderServiceAddRepository,
        IPaymentInvoiceRepository paymentInvoiceRepository,
        ISOContextProcedures repositoryProcedure,
        IContextUser context,
        ISyntaxCodeRepository syntaxCodeRepository,
        IOrderTrackingRepository orderTrackingRepository,
        IOrderInvoiceRepository orderInvoiceRepository,
        IDeliveryProductRepository deliveryProductRepository,
        IEmailMasterRepository emailMasterRepository,
        IEventRepository eventRepository)
    {
        _repository = orderRepository;
        _orderDetailRepository = orderDetailRepository;
        _orderServiceAddRepository = orderServiceAddRepository;
        _paymentInvoiceRepository = paymentInvoiceRepository;
        _context = context;
        _repositoryProcedure = repositoryProcedure;
        _syntaxCodeRepository = syntaxCodeRepository;
        _orderTrackingRepository = orderTrackingRepository;
        _orderInvoiceRepository = orderInvoiceRepository;
        _deliveryProductRepository = deliveryProductRepository;
        _emailMasterRepository = emailMasterRepository;
        _eventRepository = eventRepository;
    }

    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(RecalculatePriceCommand request, CancellationToken cancellationToken)
    {
        var result = await _repositoryProcedure.SP_CROSS_ORDER_RECALCULATE_PRICEAsync(
            request.Id,
            _context.UserId,
            _context.UserName,
            cancellationToken: cancellationToken
        );

        return request.ValidationResult;
    }

    public async Task<ValidationResult> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createdBy = _context.GetUserId();
        var createdByName = _context.UserClaims.FullName;
        var createdDate = DateTime.Now;
        var item = new Order
        {
            Id = request.Id,
            OrderType = request.OrderType,
            Code = request.Code,
            OrderDate = request.OrderDate,
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName,
            CustomerCode = request.CustomerCode,
            StoreId = request.StoreId,
            StoreCode = request.StoreCode,
            StoreName = request.StoreName,
            TypeDocument = request.TypeDocument,
            ContractId = request.ContractId,
            ContractName = request.ContractName,
            QuotationId = request.QuotationId,
            QuotationName = request.QuotationName,
            ChannelId = request.ChannelId,
            ChannelName = request.ChannelName,
            Status = request.Status,
            Currency = request.Currency,
            CurrencyName = request.CurrencyName,
            Calculation = request.Calculation,
            ExchangeRate = request.ExchangeRate,
            PriceListId = request.PriceListId,
            PriceListName = request.PriceListName,
            PaymentTermId = request.PaymentTermId,
            PaymentTermName = request.PaymentTermName,
            PaymentMethodName = request.PaymentMethodName,
            PaymentMethodId = request.PaymentMethodId,
            PaymentStatus = request.PaymentStatus,
            DeliveryAddress = request.DeliveryAddress,
            DeliveryPhone = request.DeliveryPhone,
            DeliveryName = request.DeliveryName,
            DeliveryCountry = request.DeliveryCountry,
            DeliveryProvince = request.DeliveryProvince,
            DeliveryDistrict = request.DeliveryDistrict,
            DeliveryWard = request.DeliveryWard,
            DeliveryNote = request.DeliveryNote,
            EstimatedDeliveryDate = request.EstimatedDeliveryDate,
            IsBill = request.IsBill,
            BillAddress = request.BillAddress,
            BillCountry = request.BillCountry,
            BillProvince = request.BillProvince,
            BillDistrict = request.BillDistrict,
            BillWard = request.BillWard,
            BillStatus = request.BillStatus,
            DeliveryMethodId = request.DeliveryMethodId,
            DeliveryMethodName = request.DeliveryMethodName,
            DeliveryStatus = request.DeliveryStatus,
            ShippingMethodId = request.ShippingMethodId,
            ShippingMethodName = request.ShippingMethodName,
            TypeDiscount = request.TypeDiscount,
            DiscountRate = request.DiscountRate,
            TypeCriteria = request.TypeCriteria,
            AmountDiscount = request.AmountDiscount,
            Note = request.Note,
            GroupEmployeeId = request.GroupEmployeeId,
            GroupEmployeeName = request.GroupEmployeeName,
            AccountId = request.AccountId,
            AccountName = request.AccountName,
            Image = request.Image,
            Description = request.Description,
            File = request.File,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createdByName
        };
        item.DeliveryTracking = request.DeliveryTracking;
        item.DeliveryCarrier = request.DeliveryCarrier;
        item.DeliveryPackage = request.DeliveryPackage;
        item.RouterShipping = request.RouterShipping;
        item.DomesticTracking = request.DomesticTracking;
        item.DomesticCarrier = request.DomesticCarrier;
        item.DomesticPackage = request.DomesticPackage;
        item.Weight = request.Weight;
        item.Width = request.Width;
        item.Height = request.Height;
        item.Length = request.Length;
        _repository.Add(item);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        var list = new List<OrderProduct>();
        if (request.OrderProduct?.Count > 0)
        {
            foreach (var u in request.OrderProduct)
            {
                list.Add(new OrderProduct()
                {
                    Id = (Guid)u.Id,
                    OrderId = u.OrderId,
                    ProductId = u.ProductId,
                    ProductCode = u.ProductCode,
                    ProductName = u.ProductName,
                    ProductImage = u.ProductImage,
                    Origin = u.Origin,
                    WarehouseId = u.WarehouseId,
                    WarehouseCode = u.WarehouseCode,
                    WarehouseName = u.WarehouseName,
                    PriceListId = u.PriceListId,
                    PriceListName = u.PriceListName,
                    UnitType = u.UnitType,
                    UnitCode = u.UnitCode,
                    UnitName = u.UnitName,
                    Quantity = u.Quantity,
                    UnitPrice = u.UnitPrice,
                    DiscountAmountDistribution = u.DiscountAmountDistribution,
                    DiscountType = u.DiscountType,
                    DiscountPercent = u.DiscountPercent,
                    AmountDiscount = u.AmountDiscount,
                    DiscountTotal = u.DiscountTotal,
                    TaxRate = u.TaxRate,
                    Tax = u.Tax,
                    TaxCode = u.TaxCode,
                    ExpectedDate = u.ExpectedDate,
                    Note = u.Note,
                    DisplayOrder = u.DisplayOrder,
                    CreatedBy = createdBy,
                    CreatedDate = createdDate,
                    CreatedByName = createdByName,
                    DeliveryStatus = u.DeliveryStatus,
                    DeliveryQuantity = u.DeliveryQuantity,
                    EstimatedDeliveryDate = u.EstimatedDeliveryDate,
                    SpecificationCode1 = u.SpecificationCode1,
                    SpecificationCode2 = u.SpecificationCode2,
                    SpecificationCode3 = u.SpecificationCode3,
                    SpecificationCode4 = u.SpecificationCode4,
                    SpecificationCode5 = u.SpecificationCode5,
                    SpecificationCode6 = u.SpecificationCode6,
                    SpecificationCode7 = u.SpecificationCode7,
                    SpecificationCode8 = u.SpecificationCode8,
                    SpecificationCode9 = u.SpecificationCode9,
                    SpecificationCode10 = u.SpecificationCode10,
                    SpecificationCodeJson = u.SpecificationCodeJson,
                    SourceCode = u.SourceCode,
                    SourceLink = u.SourceLink
                });
            }

            _orderDetailRepository.Add(list);
            _ = await CommitNoCheck(_orderDetailRepository.UnitOfWork);
        }

        var listService = new List<OrderServiceAdd>();
        if (request.OrderServiceAdd?.Count > 0)
        {
            foreach (var u in request.OrderServiceAdd)
            {
                listService.Add(new OrderServiceAdd()
                {
                    Id = Guid.NewGuid(),
                    OrderId = request.Id,
                    ServiceAddId = u.ServiceAddId,
                    ServiceAddName = u.ServiceAddName,
                    Currency = u.Currency,
                    Calculation = u.Calculation,
                    Price = u.Price,
                    Status = u.Status,
                    Note = u.Note,
                    CreatedBy = createdBy,
                    CreatedDate = createdDate,
                    CreatedByName = createdByName,
                    ExchangeRate = u.ExchangeRate,
                    DisplayOrder = u.DisplayOrder
                });
            }

            _orderServiceAddRepository.Add(listService);
            _ = await CommitNoCheck(_orderServiceAddRepository.UnitOfWork);
        }

        var listPaymentInvoice = new List<PaymentInvoice>();
        if (request.PaymentInvoice?.Count > 0)
        {
            foreach (var u in request.PaymentInvoice)
            {
                u.Code = _syntaxCodeRepository.GetCode("PTC", 1).Result;
                listPaymentInvoice.Add(new PaymentInvoice()
                {
                    Id = Guid.NewGuid(),
                    Type = u.Type,
                    Code = u.Code,
                    OrderId = request.Id,
                    OrderCode = request.Code,
                    Description = u.Description,
                    Amount = u.Amount,
                    Currency = u.Currency,
                    CurrencyName = u.CurrencyName,
                    Calculation = u.Calculation,
                    ExchangeRate = u.ExchangeRate,
                    PaymentDate = u.PaymentDate,
                    PaymentMethodName = u.PaymentMethodName,
                    PaymentMethodCode = u.PaymentMethodCode,
                    PaymentMethodId = u.PaymentMethodId,
                    BankName = u.BankName,
                    BankAccount = u.BankAccount,
                    BankNumber = u.BankNumber,
                    PaymentCode = u.PaymentCode,
                    PaymentNote = u.PaymentNote,
                    Note = u.Note,
                    Status = u.Status,
                    Locked = u.Locked,
                    PaymentStatus = u.PaymentStatus,
                    AccountId = u.AccountId,
                    AccountName = u.AccountName,
                    CreatedBy = createdBy,
                    CreatedDate = createdDate,
                    CreatedByName = createdByName,
                    CustomerId = u.CustomerId,
                    CustomerName = u.CustomerName,
                });
            }

            _paymentInvoiceRepository.Add(listPaymentInvoice);
            _ = await CommitNoCheck(_paymentInvoiceRepository.UnitOfWork);
        }

        var listOrderInvoice = new List<OrderInvoice>();
        if (request.OrderInvoice?.Count > 0)
        {
            foreach (var u in request.OrderInvoice)
            {
                listOrderInvoice.Add(new OrderInvoice()
                {
                    Id = Guid.NewGuid(),
                    OrderId = request.Id,
                    OrderCode = request.Code,
                    Serial = u.Serial,
                    Symbol = u.Symbol,
                    Number = u.Number,
                    Value = u.Value,
                    Date = u.Date,
                    Note = u.Note,
                    DisplayOrder = u.DisplayOrder,
                    CreatedBy = createdBy,
                    CreatedDate = createdDate,
                    CreatedByName = createdByName
                });
            }

            _orderInvoiceRepository.Add(listOrderInvoice);
            _ = await CommitNoCheck(_orderInvoiceRepository.UnitOfWork);
        }

        if (request.ListExpectedDelivery?.Count > 0)
        {
            var listDeliveryProduct = new List<DeliveryProduct>();
            foreach (var x in request.ListExpectedDelivery)
            {
                var parent = request.OrderProduct.Where(a => a.Guid == x.OrderProductId).FirstOrDefault();
                var newList = new DeliveryProduct()
                {
                    Id = Guid.NewGuid(),
                    OrderProductId = (Guid)parent.Id,
                    DeliveryDate = x.DeliveryDate,
                    QuantityExpected = x.QuantityExpected,
                    Description = x.Description,
                    CreatedBy = createdBy,
                    CreatedDate = createdDate,
                    CreatedByName = createdByName
                };
                listDeliveryProduct.Add(newList);
            }

            _deliveryProductRepository.Add(listDeliveryProduct);
            _ = await CommitNoCheck(_deliveryProductRepository.UnitOfWork);
        }
        
        return result;
    }

    public async Task<ValidationResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        return request.ValidationResult;
    }

    public async Task<ValidationResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;

        var Order = new Order
        {
            Id = request.Id
        };

        _repository.Remove(Order);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(EditOrderCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var obj = await _repository.GetById(request.Id);

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Order is not exist") } };
        }

        var updatedBy = _context.GetUserId();
        var updatedByname = _context.UserClaims.FullName;
        var updatedDate = DateTime.Now;
        obj.OrderType = request.OrderType;
        obj.Code = request.Code;
        obj.OrderDate = request.OrderDate;
        obj.CustomerId = request.CustomerId;
        obj.CustomerName = request.CustomerName;
        obj.CustomerCode = request.CustomerCode;
        obj.StoreId = request.StoreId;
        obj.StoreCode = request.StoreCode;
        obj.StoreName = request.StoreName;
        obj.TypeDocument = request.TypeDocument;
        obj.ContractId = request.ContractId;
        obj.ContractName = request.ContractName;
        obj.QuotationId = request.QuotationId;
        obj.QuotationName = request.QuotationName;
        obj.ChannelId = request.ChannelId;
        obj.ChannelName = request.ChannelName;
        obj.Status = request.Status;
        obj.Currency = request.Currency;
        obj.CurrencyName = request.CurrencyName;
        obj.Calculation = request.Calculation;
        obj.ExchangeRate = request.ExchangeRate;
        obj.PriceListId = request.PriceListId;
        obj.PriceListName = request.PriceListName;
        obj.PaymentTermId = request.PaymentTermId;
        obj.PaymentTermName = request.PaymentTermName;
        obj.PaymentMethodName = request.PaymentMethodName;
        obj.PaymentMethodId = request.PaymentMethodId;
        obj.PaymentStatus = request.PaymentStatus;
        obj.DeliveryAddress = request.DeliveryAddress;
        obj.DeliveryPhone = request.DeliveryPhone;
        obj.DeliveryName = request.DeliveryName;
        obj.DeliveryCountry = request.DeliveryCountry;
        obj.DeliveryProvince = request.DeliveryProvince;
        obj.DeliveryDistrict = request.DeliveryDistrict;
        obj.DeliveryWard = request.DeliveryWard;
        obj.DeliveryNote = request.DeliveryNote;
        obj.EstimatedDeliveryDate = request.EstimatedDeliveryDate;
        obj.IsBill = request.IsBill;
        obj.BillAddress = request.BillAddress;
        obj.BillCountry = request.BillCountry;
        obj.BillProvince = request.BillProvince;
        obj.BillDistrict = request.BillDistrict;
        obj.BillWard = request.BillWard;
        obj.BillStatus = request.BillStatus;
        obj.DeliveryMethodId = request.DeliveryMethodId;
        obj.DeliveryMethodName = request.DeliveryMethodName;
        obj.DeliveryStatus = request.DeliveryStatus;
        obj.ShippingMethodId = request.ShippingMethodId;
        obj.ShippingMethodName = request.ShippingMethodName;
        obj.TypeDiscount = request.TypeDiscount;
        obj.DiscountRate = request.DiscountRate;
        obj.TypeCriteria = request.TypeCriteria;
        obj.AmountDiscount = request.AmountDiscount;
        obj.Note = request.Note;
        obj.GroupEmployeeId = request.GroupEmployeeId;
        obj.GroupEmployeeName = request.GroupEmployeeName;
        obj.AccountId = request.AccountId;
        obj.AccountName = request.AccountName;
        obj.Image = request.Image;
        obj.Description = request.Description;
        obj.File = request.File;
        obj.UpdatedBy = updatedBy;
        obj.UpdatedDate = updatedDate;
        obj.UpdatedByName = updatedByname;

        obj.DeliveryTracking = request.DeliveryTracking;
        obj.DeliveryCarrier = request.DeliveryCarrier;
        obj.DeliveryPackage = request.DeliveryPackage;
        obj.RouterShipping = request.RouterShipping;
        obj.DomesticTracking = request.DomesticTracking;
        obj.DomesticCarrier = request.DomesticCarrier;
        obj.DomesticPackage = request.DomesticPackage;
        obj.Weight = request.Weight;
        obj.Width = request.Width;
        obj.Height = request.Height;
        obj.Length = request.Length;

        _repository.Update(obj);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        if (request.OrderProduct.Any())
        {
            var listUpdate = new List<OrderProduct>();
            var listDelete = new List<OrderProduct>();
            foreach (var item in obj.OrderProduct)
            {
                var u = request.OrderProduct.FirstOrDefault(x => x.Id == item.Id);
                if (u != null)
                {
                    item.Id = (Guid)u.Id;
                    item.OrderId = u.OrderId;
                    item.OrderCode = u.OrderCode;
                    item.ProductId = u.ProductId;
                    item.ProductCode = u.ProductCode;
                    item.ProductName = u.ProductName;
                    item.ProductImage = u.ProductImage;
                    item.Origin = u.Origin;
                    item.WarehouseId = u.WarehouseId;
                    item.WarehouseCode = u.WarehouseCode;
                    item.WarehouseName = u.WarehouseName;
                    item.PriceListId = u.PriceListId;
                    item.PriceListName = u.PriceListName;
                    item.UnitType = u.UnitType;
                    item.UnitCode = u.UnitCode;
                    item.UnitName = u.UnitName;
                    item.Quantity = u.Quantity;
                    item.UnitPrice = u.UnitPrice;
                    item.DiscountAmountDistribution = u.DiscountAmountDistribution;
                    item.DiscountType = u.DiscountType;
                    item.DiscountPercent = u.DiscountPercent;
                    item.AmountDiscount = u.AmountDiscount;
                    item.DiscountTotal = u.DiscountTotal;
                    item.TaxRate = u.TaxRate;
                    item.Tax = u.Tax;
                    item.TaxCode = u.TaxCode;
                    item.ExpectedDate = u.ExpectedDate;
                    item.Note = u.Note;
                    item.UpdatedBy = updatedBy;
                    item.UpdatedDate = updatedDate;
                    item.UpdatedByName = updatedByname;
                    item.DeliveryStatus = u.DeliveryStatus;
                    item.DeliveryQuantity = u.DeliveryQuantity;
                    item.EstimatedDeliveryDate = u.EstimatedDeliveryDate;
                    item.SpecificationCode1 = u.SpecificationCode1;
                    item.SpecificationCode2 = u.SpecificationCode2;
                    item.SpecificationCode3 = u.SpecificationCode3;
                    item.SpecificationCode4 = u.SpecificationCode4;
                    item.SpecificationCode5 = u.SpecificationCode5;
                    item.SpecificationCode6 = u.SpecificationCode6;
                    item.SpecificationCode7 = u.SpecificationCode7;
                    item.SpecificationCode8 = u.SpecificationCode8;
                    item.SpecificationCode9 = u.SpecificationCode9;
                    item.SpecificationCode10 = u.SpecificationCode10;
                    item.SpecificationCodeJson = u.SpecificationCodeJson;
                    item.BidUsername = u.BidUsername;
                    listUpdate.Add(item);
                    request.OrderProduct.Remove(u);
                }
                else
                {
                    listDelete.Add(item);
                }
            }

            var listAdd = new List<OrderProduct>();
            for (var i = 0; i < request.OrderProduct.Count; i++)
            {
                var u = request.OrderProduct[i];
                var orderProduct = obj.OrderProduct.FirstOrDefault(x => x.Id.Equals(request.OrderProduct[i].Id));
                if (orderProduct is null)
                {
                    listAdd.Add(new OrderProduct()
                    {
                        Id = (Guid)u.Id,
                        OrderId = request.Id,
                        ProductId = u.ProductId,
                        ProductCode = u.ProductCode,
                        ProductName = u.ProductName,
                        ProductImage = u.ProductImage,
                        Origin = u.Origin,
                        WarehouseId = u.WarehouseId,
                        WarehouseCode = u.WarehouseCode,
                        WarehouseName = u.WarehouseName,
                        PriceListId = u.PriceListId,
                        PriceListName = u.PriceListName,
                        UnitType = u.UnitType,
                        UnitCode = u.UnitCode,
                        UnitName = u.UnitName,
                        Quantity = u.Quantity,
                        UnitPrice = u.UnitPrice,
                        DiscountAmountDistribution = u.DiscountAmountDistribution,
                        DiscountType = u.DiscountType,
                        DiscountPercent = u.DiscountPercent,
                        AmountDiscount = u.AmountDiscount,
                        DiscountTotal = u.DiscountTotal,
                        TaxRate = u.TaxRate,
                        Tax = u.Tax,
                        TaxCode = u.TaxCode,
                        ExpectedDate = u.ExpectedDate,
                        Note = u.Note,
                        DisplayOrder = u.DisplayOrder,
                        CreatedBy = updatedBy,
                        CreatedDate = updatedDate,
                        CreatedByName = updatedByname,
                        DeliveryStatus = u.DeliveryStatus,
                        DeliveryQuantity = u.DeliveryQuantity,
                        EstimatedDeliveryDate = u.EstimatedDeliveryDate,
                        SpecificationCode1 = u.SpecificationCode1,
                        SpecificationCode2 = u.SpecificationCode2,
                        SpecificationCode3 = u.SpecificationCode3,
                        SpecificationCode4 = u.SpecificationCode4,
                        SpecificationCode5 = u.SpecificationCode5,
                        SpecificationCode6 = u.SpecificationCode6,
                        SpecificationCode7 = u.SpecificationCode7,
                        SpecificationCode8 = u.SpecificationCode8,
                        SpecificationCode9 = u.SpecificationCode9,
                        SpecificationCode10 = u.SpecificationCode10,
                        SpecificationCodeJson = u.SpecificationCodeJson
                    });

                    request.OrderProduct.Remove(request.OrderProduct[i]);
                    i--;
                }
            }

            _orderDetailRepository.Update(listUpdate);
            _orderDetailRepository.Add(listAdd);
            _orderDetailRepository.Remove(listDelete);
            _ = await CommitNoCheck(_orderDetailRepository.UnitOfWork);
        }
        else
        {
            obj.OrderProduct.Clear();
        }

        //DeliveryProduct
        var expectedDelivery = await _deliveryProductRepository.GetByOrderId(request.Id);
        if (request.ListExpectedDelivery.Any())
        {
            var listEdit = new List<DeliveryProduct>();
            var listExist = new List<DeliveryProduct>();
            foreach (var item in expectedDelivery)
            {
                var u = request.ListExpectedDelivery.Where(y => y.Id == item.Id).FirstOrDefault();
                if (u is null)
                {
                    listExist.Add(item);
                }
                else
                {
                    item.QuantityExpected = u.QuantityExpected;
                    item.Description = u.Description;
                    item.DeliveryDate = u.DeliveryDate;
                    item.UpdatedBy = updatedBy;
                    item.UpdatedDate = updatedDate;
                    item.UpdatedByName = updatedByname;
                    listEdit.Add(item);
                    request.ListExpectedDelivery.Remove(u);
                }
            }

            var listAdd = new List<DeliveryProduct>();
            for (var i = 0; i < request.ListExpectedDelivery.Count; i++)
            {
                var d = request.ListExpectedDelivery[i];
                var data = new DeliveryProduct()
                {
                    Id = Guid.NewGuid(),
                    OrderProductId = d.OrderProductId,
                    DeliveryDate = d.DeliveryDate,
                    QuantityExpected = d.QuantityExpected,
                    Description = d.Description,
                    CreatedBy = updatedBy,
                    CreatedDate = updatedDate,
                    CreatedByName = updatedByname
                };
                listAdd.Add(data);
                request.ListExpectedDelivery.Remove(request.ListExpectedDelivery[i]);
                i--;
            }

            _deliveryProductRepository.Update(listEdit);
            _deliveryProductRepository.Add(listAdd);
            _deliveryProductRepository.Remove(listExist);
            _ = await CommitNoCheck(_deliveryProductRepository.UnitOfWork);
        }

        var listSerViceUpdate = new List<OrderServiceAdd>();
        var listSerViceDelete = new List<OrderServiceAdd>();
        if (request.OrderServiceAdd.Any())
        {
            foreach (var item in obj.OrderServiceAdd)
            {
                var u = request.OrderServiceAdd.Where(x => x.Id == item.Id).FirstOrDefault();
                if (u != null)
                {
                    item.OrderId = request.Id;
                    item.ServiceAddId = u.ServiceAddId;
                    item.ServiceAddName = u.ServiceAddName;
                    item.Price = u.Price;
                    item.Currency = u.Currency;
                    item.Calculation = u.Calculation;
                    item.Status = u.Status;
                    item.Note = u.Note;
                    item.UpdatedBy = updatedBy;
                    item.UpdatedDate = updatedDate;
                    item.UpdatedByName = updatedByname;
                    item.ExchangeRate = u.ExchangeRate;
                    item.DisplayOrder = u.DisplayOrder;
                    listSerViceUpdate.Add(item);
                    request.OrderServiceAdd.Remove(u);
                }
                else
                {
                    listSerViceDelete.Add(item);
                }
            }

            var listSerViceAdd = new List<OrderServiceAdd>();
            for (var i = 0; i < request.OrderServiceAdd.Count; i++)
            {
                var u = request.OrderServiceAdd[i];
                var orderServiceAdd = obj.OrderServiceAdd.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (orderServiceAdd is null)
                {
                    listSerViceAdd.Add(new OrderServiceAdd()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = request.Id,
                        ServiceAddId = u.ServiceAddId,
                        ServiceAddName = u.ServiceAddName,
                        Price = u.Price,
                        Currency = u.Currency,
                        Calculation = u.Calculation,
                        Status = u.Status,
                        Note = u.Note,
                        CreatedBy = updatedBy,
                        CreatedDate = updatedDate,
                        CreatedByName = updatedByname,
                        ExchangeRate = u.ExchangeRate,
                        DisplayOrder = u.DisplayOrder
                    });

                    request.OrderServiceAdd.Remove(request.OrderServiceAdd[i]);
                    i--;
                }
            }

            _orderServiceAddRepository.Update(listSerViceUpdate);
            _orderServiceAddRepository.Add(listSerViceAdd);
            _orderServiceAddRepository.Remove(listSerViceDelete);
            _ = await CommitNoCheck(_orderServiceAddRepository.UnitOfWork);
        }
        else if (obj.OrderServiceAdd.Any())
        {
            foreach (var item in obj.OrderServiceAdd)
            {
                listSerViceDelete.Add(item);
            }

            _orderServiceAddRepository.Remove(listSerViceDelete);
            _ = await CommitNoCheck(_orderServiceAddRepository.UnitOfWork);
        }

        var listPaymentInvoiceUpdate = new List<PaymentInvoice>();
        var listPaymentInvoiceDelete = new List<PaymentInvoice>();
        if (request.PaymentInvoice?.Count > 0)
        {
            foreach (var item in obj.PaymentInvoice)
            {
                var u = request.PaymentInvoice.Where(x => x.Id == item.Id).FirstOrDefault();
                if (u != null)
                {
                    item.Type = u.Type;
                    item.Code = u.Code;
                    item.Description = u.Description;
                    item.Amount = u.Amount;
                    item.Currency = u.Currency;
                    item.CurrencyName = u.CurrencyName;
                    item.Calculation = u.Calculation;
                    item.ExchangeRate = u.ExchangeRate;
                    item.PaymentDate = u.PaymentDate;
                    item.PaymentMethodName = u.PaymentMethodName;
                    item.PaymentMethodCode = u.PaymentMethodCode;
                    item.PaymentMethodId = u.PaymentMethodId;
                    item.BankName = u.BankName;
                    item.BankAccount = u.BankAccount;
                    item.BankNumber = u.BankNumber;
                    item.PaymentCode = u.PaymentCode;
                    item.PaymentNote = u.PaymentNote;
                    item.Note = u.Note;
                    item.Status = u.Status;
                    item.Locked = u.Locked;
                    item.PaymentStatus = u.PaymentStatus;
                    item.AccountId = u.AccountId;
                    item.AccountName = u.AccountName;
                    item.UpdatedBy = updatedBy;
                    item.UpdatedDate = updatedDate;
                    item.UpdatedByName = updatedByname;
                    item.CustomerId = u.CustomerId;
                    item.CustomerName = u.CustomerName;
                    listPaymentInvoiceUpdate.Add(item);
                    request.PaymentInvoice.Remove(u);
                }
                else
                {
                    listPaymentInvoiceDelete.Add(item);
                }
            }

            var listPaymentInvoiceAdd = new List<PaymentInvoice>();
            for (var i = 0; i < request.PaymentInvoice.Count; i++)
            {
                var u = request.PaymentInvoice[i];
                var paymentInvoice = obj.PaymentInvoice.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (paymentInvoice is null)
                {
                    u.Code = _syntaxCodeRepository.GetCode("PTC", 1).Result;
                    listPaymentInvoiceAdd.Add(new PaymentInvoice()
                    {
                        Id = Guid.NewGuid(),
                        Type = u.Type,
                        Code = u.Code,
                        OrderId = request.Id,
                        OrderCode = request.Code,
                        Description = u.Description,
                        Amount = u.Amount,
                        Currency = u.Currency,
                        CurrencyName = u.CurrencyName,
                        Calculation = u.Calculation,
                        ExchangeRate = u.ExchangeRate,
                        PaymentDate = u.PaymentDate,
                        PaymentMethodName = u.PaymentMethodName,
                        PaymentMethodCode = u.PaymentMethodCode,
                        PaymentMethodId = u.PaymentMethodId,
                        BankName = u.BankName,
                        BankAccount = u.BankAccount,
                        BankNumber = u.BankNumber,
                        PaymentCode = u.PaymentCode,
                        PaymentNote = u.PaymentNote,
                        Note = u.Note,
                        Status = u.Status,
                        Locked = u.Locked,
                        PaymentStatus = u.PaymentStatus,
                        AccountId = u.AccountId,
                        AccountName = u.AccountName,
                        CreatedBy = updatedBy,
                        CreatedDate = updatedDate,
                        CreatedByName = updatedByname,
                        CustomerId = request.CustomerId,
                        CustomerName = request.CustomerName
                    });

                    request.PaymentInvoice.Remove(u);
                    i--;
                }
            }

            _paymentInvoiceRepository.Update(listPaymentInvoiceUpdate);
            _paymentInvoiceRepository.Add(listPaymentInvoiceAdd);
            _paymentInvoiceRepository.Remove(listPaymentInvoiceDelete);
            _ = await CommitNoCheck(_paymentInvoiceRepository.UnitOfWork);
        }
        else if (obj.PaymentInvoice.Any())
        {
            foreach (var item in obj.PaymentInvoice)
            {
                listPaymentInvoiceDelete.Add(item);
            }

            _paymentInvoiceRepository.Remove(listPaymentInvoiceDelete);
            _ = await CommitNoCheck(_paymentInvoiceRepository.UnitOfWork);
        }

        var listOrderInvoiceUpdate = new List<OrderInvoice>();
        var listOrderInvoiceDelete = new List<OrderInvoice>();
        if (request.OrderInvoice.Any())
        {
            foreach (var item in obj.OrderInvoice)
            {
                var u = request.OrderInvoice.Where(x => x.Id == item.Id).FirstOrDefault();
                if (u != null)
                {
                    item.OrderId = request.Id;
                    item.OrderCode = request.Code;
                    item.Serial = u.Serial;
                    item.Symbol = u.Symbol;
                    item.Number = u.Number;
                    item.Value = u.Value;
                    item.Date = u.Date;
                    item.Note = u.Note;
                    item.DisplayOrder = u.DisplayOrder;
                    item.UpdatedBy = updatedBy;
                    item.UpdatedDate = updatedDate;
                    item.UpdatedByName = updatedByname;
                    listOrderInvoiceUpdate.Add(item);
                    request.OrderInvoice.Remove(u);
                }
                else
                {
                    listOrderInvoiceDelete.Add(item);
                }
            }

            var listOrderInvoiceAdd = new List<OrderInvoice>();
            for (var i = 0; i < request.OrderInvoice.Count; i++)
            {
                var u = request.OrderInvoice[i];
                var orderInvoice = obj.OrderInvoice.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (orderInvoice is null)
                {
                    listOrderInvoiceAdd.Add(new OrderInvoice()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = request.Id,
                        OrderCode = request.Code,
                        Serial = u.Serial,
                        Symbol = u.Symbol,
                        Number = u.Number,
                        Value = u.Value,
                        Date = u.Date,
                        Note = u.Note,
                        DisplayOrder = u.DisplayOrder,
                        CreatedBy = updatedBy,
                        CreatedDate = updatedDate,
                        CreatedByName = updatedByname
                    });

                    request.OrderInvoice.Remove(request.OrderInvoice[i]);
                    i--;
                }
            }

            _orderInvoiceRepository.Update(listOrderInvoiceUpdate);
            _orderInvoiceRepository.Add(listOrderInvoiceAdd);
            _orderInvoiceRepository.Remove(listOrderInvoiceDelete);
            _ = await CommitNoCheck(_orderInvoiceRepository.UnitOfWork);
        }
        else if (obj.OrderInvoice.Any())
        {
            foreach (var item in obj.OrderInvoice)
            {
                listOrderInvoiceDelete.Add(item);
            }

            _orderInvoiceRepository.Remove(listOrderInvoiceDelete);
            _ = await CommitNoCheck(_orderInvoiceRepository.UnitOfWork);
        }

        return result;
    }

    public async Task<ValidationResult> Handle(ApprovalOrderCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Order is not exist") } };
        }

        var oldStatus = item.Status;
        var obj = new Order
        {
            Id = request.Id,
            Status = request.Status,
            ApproveDate = DateTime.Now,
            ApproveBy = _context.GetUserId(),
            ApproveByName = _context.UserClaims.FullName,
            ApproveComment = request.ApproveComment
        };
        _repository.Approve(obj);
        try
        {
            var text = request.Status switch
            {
                0 => "Chờ thanh toán",
                10 => "Đang chờ mua",
                20 => "Đã mua hàng",
                30 => "Đang vận chuyển",
                40 => "Đã về kho",
                50 => "Chờ thanh toán",
                60 => "Đang giao hàng",
                70 => "Đã giao hàng",
                80 => "Đơn hàng đổi trả",
                90 => "Đơn hàng bị huỷ",
                _ => ""
            };

            _orderTrackingRepository.Add(new List<OrderTracking>()
            {
                new OrderTracking()
                {
                    Id = Guid.NewGuid(),
                    OrderId = request.Id,
                    Name = text,
                    Status = 1,
                    Description = request.ApproveComment,
                    Image = "",
                    TrackingDate = obj.ApproveDate,
                    CreatedDate = DateTime.Now,
                    CreatedBy = _context.GetUserId(),
                    CreatedByName = _context.UserName
                }
            });

            await Commit(_orderTrackingRepository.UnitOfWork);
        }
        catch (Exception)
        {
        }

        var result = await Commit(_repository.UnitOfWork);
        if (result.IsValid)
        {
            var message = new Domain.SO.Events.OrderStatusChangedQueueEvent();
            message.OrderId = request.Id;
            message.FromStatus = oldStatus;
            message.ToStatus = obj.Status;
            message.ChangeDate = DateTime.UtcNow;
            message.Tenant = _context.Tenant;
            message.Data = _context.Data;
            message.Data_Zone = _context.Data_Zone;
            message.RequestBy = _context.GetUserId();
            message.RequestByName = _context.FullName;
            message.RequestByEmail = _context.GetUserEmail();

            _ = await _eventRepository.OrderStatusChangedEvent(message);
        }

        return result;
    }

    public async Task<ValidationResult> Handle(ApprovalOrdersCommand request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetByIds(request.Ids);

        if (items is null || !items.Any())
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Order is not exist") } };
        }

        foreach (var item in items)
        {
            var oldStatus = item.Status;
            var obj = new Order
            {
                Id = item.Id,
                Status = request.Status,
                ApproveDate = DateTime.Now,
                ApproveBy = _context.GetUserId(),
                ApproveByName = _context.UserClaims.FullName,
                ApproveComment = request.ApproveComment
            };
            _repository.Approve(obj);
            try
            {
                var text = request.Status switch
                {
                    0 => "Chờ thanh toán",
                    10 => "Đang chờ mua",
                    20 => "Đã mua hàng",
                    30 => "Đang vận chuyển",
                    40 => "Đã về kho",
                    50 => "Chờ thanh toán",
                    60 => "Đang giao hàng",
                    70 => "Đã giao hàng",
                    80 => "Đơn hàng đổi trả",
                    90 => "Đơn hàng bị huỷ",
                    _ => ""
                };

                _orderTrackingRepository.Add(new List<OrderTracking>()
                {
                    new OrderTracking()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = item.Id,
                        Name = text,
                        Status = 1,
                        Description = request.ApproveComment,
                        Image = "",
                        TrackingDate = obj.ApproveDate,
                        CreatedDate = DateTime.Now,
                        CreatedBy = _context.GetUserId(),
                        CreatedByName = _context.UserName
                    }
                });
                var message = new Domain.SO.Events.OrderStatusChangedQueueEvent
                {
                    OrderId = item.Id,
                    FromStatus = oldStatus,
                    ToStatus = obj.Status,
                    ChangeDate = DateTime.UtcNow,
                    Tenant = _context.Tenant,
                    Data = _context.Data,
                    Data_Zone = _context.Data_Zone
                };

                _ = await _eventRepository.OrderStatusChangedEvent(message);
            }
            catch (Exception)
            {
            }
        }

        try
        {
            await Commit(_orderTrackingRepository.UnitOfWork);
        }
        catch (Exception)
        {
        }

        return await Commit(_repository.UnitOfWork);
    }


    public async Task<ValidationResult> Handle(ManagePaymentOrderCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var obj = await _repository.GetById(request.Id);

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Order is not exist") } };
        }

        obj.PaymentStatus = request.PaymentStatus;
        _repository.Update(obj);
        var result = await Commit(_repository.UnitOfWork);

        if (!result.IsValid)
            return result;
        var listPaymentInvoiceUpdate = new List<PaymentInvoice>();
        var listPaymentInvoiceDelete = new List<PaymentInvoice>();
        if (request.PaymentInvoice?.Count > 0)
        {
            foreach (var item in obj.PaymentInvoice)
            {
                var u = request.PaymentInvoice.Where(x => x.Id == item.Id).FirstOrDefault();
                if (u != null)
                {
                    item.Type = u.Type;
                    item.Code = u.Code;
                    item.Description = u.Description;
                    item.Amount = u.Amount;
                    item.Currency = u.Currency;
                    item.CurrencyName = u.CurrencyName;
                    item.Calculation = u.Calculation;
                    item.ExchangeRate = u.ExchangeRate;
                    item.PaymentDate = u.PaymentDate;
                    item.PaymentMethodName = u.PaymentMethodName;
                    item.PaymentMethodCode = u.PaymentMethodCode;
                    item.PaymentMethodId = u.PaymentMethodId;
                    item.BankName = u.BankName;
                    item.BankAccount = u.BankAccount;
                    item.BankNumber = u.BankNumber;
                    item.PaymentCode = u.PaymentCode;
                    item.PaymentNote = u.PaymentNote;
                    item.Note = u.Note;
                    item.Status = u.Status;
                    item.PaymentStatus = u.PaymentStatus;
                    item.AccountId = u.AccountId;
                    item.AccountName = u.AccountName;
                    item.UpdatedBy = _context.GetUserId();
                    item.UpdatedDate = DateTime.Now;
                    item.UpdatedByName = _context.UserName;
                    item.CustomerId = obj.CustomerId;
                    item.CustomerName = obj.CustomerName;
                    listPaymentInvoiceUpdate.Add(item);
                    request.PaymentInvoice.Remove(u);
                }
                else
                {
                    listPaymentInvoiceDelete.Add(item);
                }
            }

            var listPaymentInvoiceAdd = new List<PaymentInvoice>();
            for (var i = 0; i < request.PaymentInvoice.Count; i++)
            {
                var u = request.PaymentInvoice[i];
                var paymentInvoice = obj.PaymentInvoice.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (paymentInvoice is null)
                {
                    u.Code = _syntaxCodeRepository.GetCode("PTC", 1).Result;
                    listPaymentInvoiceAdd.Add(new PaymentInvoice()
                    {
                        Id = Guid.NewGuid(),
                        Type = u.Type,
                        Code = u.Code,
                        OrderId = request.Id,
                        OrderCode = obj.Code,
                        Description = u.Description,
                        Amount = u.Amount,
                        Currency = u.Currency,
                        CurrencyName = u.CurrencyName,
                        Calculation = u.Calculation,
                        ExchangeRate = u.ExchangeRate,
                        PaymentDate = u.PaymentDate,
                        PaymentMethodName = u.PaymentMethodName,
                        PaymentMethodCode = u.PaymentMethodCode,
                        PaymentMethodId = u.PaymentMethodId,
                        BankName = u.BankName,
                        BankAccount = u.BankAccount,
                        BankNumber = u.BankNumber,
                        PaymentCode = u.PaymentCode,
                        PaymentNote = u.PaymentNote,
                        Note = u.Note,
                        Status = u.Status,
                        PaymentStatus = u.PaymentStatus,
                        AccountId = u.AccountId,
                        AccountName = u.AccountName,
                        CreatedDate = DateTime.Now,
                        CreatedBy = _context.GetUserId(),
                        CreatedByName = _context.UserName,
                        CustomerId = obj.CustomerId,
                        CustomerName = obj.CustomerName
                    });

                    request.PaymentInvoice.Remove(u);
                    i--;
                }
            }

            _paymentInvoiceRepository.Update(listPaymentInvoiceUpdate);
            _paymentInvoiceRepository.Add(listPaymentInvoiceAdd);
            _paymentInvoiceRepository.Remove(listPaymentInvoiceDelete);
            _ = await CommitNoCheck(_paymentInvoiceRepository.UnitOfWork);
        }
        else if (obj.PaymentInvoice.Any())
        {
            foreach (var item in obj.PaymentInvoice)
            {
                listPaymentInvoiceDelete.Add(item);
            }

            _paymentInvoiceRepository.Remove(listPaymentInvoiceDelete);
            _ = await CommitNoCheck(_paymentInvoiceRepository.UnitOfWork);
        }

        return result;
    }

    public async Task<ValidationResult> Handle(ManageServiceOrderCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var obj = await _repository.GetById(request.Id);

        var listSerViceUpdate = new List<OrderServiceAdd>();
        var listSerViceDelete = new List<OrderServiceAdd>();
        if (request.OrderServiceAdd.Any())
        {
            foreach (var item in obj.OrderServiceAdd)
            {
                var u = request.OrderServiceAdd.Where(x => x.Id == item.Id).FirstOrDefault();
                if (u != null)
                {
                    item.OrderId = request.Id;
                    item.ServiceAddId = u.ServiceAddId;
                    item.ServiceAddName = u.ServiceAddName;
                    item.Price = u.Price;
                    item.Currency = u.Currency;
                    item.Calculation = u.Calculation;
                    item.Status = u.Status;
                    item.Note = u.Note;
                    item.UpdatedBy = _context.GetUserId();
                    item.UpdatedDate = DateTime.Now;
                    item.UpdatedByName = _context.UserName;
                    item.ExchangeRate = u.ExchangeRate;
                    item.DisplayOrder = u.DisplayOrder;
                    listSerViceUpdate.Add(item);
                    request.OrderServiceAdd.Remove(u);
                }
                else
                {
                    listSerViceDelete.Add(item);
                }
            }

            var listSerViceAdd = new List<OrderServiceAdd>();
            for (var i = 0; i < request.OrderServiceAdd.Count; i++)
            {
                var u = request.OrderServiceAdd[i];
                var orderServiceAdd = obj.OrderServiceAdd.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (orderServiceAdd is null)
                {
                    listSerViceAdd.Add(new OrderServiceAdd()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = request.Id,
                        ServiceAddId = u.ServiceAddId,
                        ServiceAddName = u.ServiceAddName,
                        Price = u.Price,
                        Currency = u.Currency,
                        Calculation = u.Calculation,
                        Status = u.Status,
                        Note = u.Note,
                        CreatedDate = DateTime.Now,
                        CreatedBy = _context.GetUserId(),
                        CreatedByName = _context.UserName,
                        ExchangeRate = u.ExchangeRate,
                        DisplayOrder = u.DisplayOrder
                    });

                    request.OrderServiceAdd.Remove(request.OrderServiceAdd[i]);
                    i--;
                }
            }

            _orderServiceAddRepository.Update(listSerViceUpdate);
            _orderServiceAddRepository.Add(listSerViceAdd);
            _orderServiceAddRepository.Remove(listSerViceDelete);
            var result = await Commit(_orderServiceAddRepository.UnitOfWork);
            return result;
        }
        else if (obj.OrderServiceAdd.Any())
        {
            foreach (var item in obj.OrderServiceAdd)
            {
                listSerViceDelete.Add(item);
            }

            _orderServiceAddRepository.Remove(listSerViceDelete);
            var result = await Commit(_orderServiceAddRepository.UnitOfWork);
            return result;
        }

        return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "No change") } };
    }

    public bool isSameOrderTrackinng(OrderTracking orderTracking, OrderTrackingDto orderTrackingDto)
    {
        if (orderTracking.Name == orderTrackingDto.Name
            && orderTracking.Status == orderTrackingDto.Status
            && orderTracking.Description == orderTrackingDto.Description
            && orderTracking.Image == orderTrackingDto.Image
            && orderTracking.TrackingDate == orderTrackingDto.TrackingDate)
        {
            return true;
        }

        return false;
    }

    public async Task<ValidationResult> Handle(NoteOrderCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var obj = await _repository.GetById(request.Id);

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Order is not exist") } };
        }

        //_repository.Update(obj);
        var listUpdate = new List<OrderTracking>();
        var listDelete = new List<OrderTracking>();
        if (request.OrderTracking.Any())
        {
            foreach (var item in obj.OrderTracking)
            {
                var u = request.OrderTracking.Where(x => x.Id == item.Id).FirstOrDefault();
                if (u != null)
                {
                    if (!isSameOrderTrackinng(item, u))
                    {
                        item.Name = u.Name;
                        item.Status = u.Status;
                        item.Description = u.Description;
                        item.Image = u.Image;
                        item.TrackingDate = u.TrackingDate;
                        item.UpdatedBy = _context.GetUserId();
                        item.UpdatedDate = DateTime.Now;
                        item.UpdatedByName = _context.UserName;
                        listUpdate.Add(item);
                        request.OrderTracking.Remove(u);
                    }
                    else
                    {
                        request.OrderTracking.Remove(u);
                    }
                }
                else
                {
                    listDelete.Add(item);
                }
            }

            var listAdd = new List<OrderTracking>();
            for (var i = 0; i < request.OrderTracking.Count; i++)
            {
                var u = request.OrderTracking[i];
                var orderTracking = obj.OrderTracking.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (orderTracking is null)
                {
                    listAdd.Add(new OrderTracking()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = request.Id,
                        Name = u.Name,
                        Status = u.Status,
                        Description = u.Description,
                        Image = u.Image,
                        TrackingDate = u.TrackingDate,
                        CreatedDate = DateTime.Now,
                        CreatedBy = _context.GetUserId(),
                        CreatedByName = _context.UserName
                    });

                    request.OrderTracking.Remove(request.OrderTracking[i]);
                    i--;
                }
            }

            _orderTrackingRepository.Update(listUpdate);
            _orderTrackingRepository.Add(listAdd);
            _orderTrackingRepository.Remove(listDelete);
            var result = await Commit(_orderTrackingRepository.UnitOfWork);
            return result;
        }
        else if (obj.OrderServiceAdd.Any())
        {
            foreach (var item in obj.OrderTracking)
            {
                listDelete.Add(item);
            }

            _orderTrackingRepository.Remove(listDelete);
            var result = await Commit(_orderTrackingRepository.UnitOfWork);
            return result;
        }

        return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "No change") } };
    }

    public async Task<ValidationResult> Handle(OrderUploadFileCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Order is not exist") } };
        }

        var obj = new Order
        {
            Id = request.Id,
            File = request.File
        };
        _repository.UploadFile(obj);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(OrderEmailNotifyCommand request, CancellationToken cancellationToken)
    {
        var emailNotify = new EmailNotify
        {
            Order = request.Order,
            SenderCode = request.SenderCode,
            SenderName = request.SenderName,
            Subject = request.Subject,
            From = request.From,
            To = request.To,
            CC = request.CC,
            BCC = request.BCC,
            Body = request.Body,
            TemplateCode = request.TemplateCode,
        };

        _emailMasterRepository.EmailNotify(emailNotify);

        return new ValidationResult();
    }
}
