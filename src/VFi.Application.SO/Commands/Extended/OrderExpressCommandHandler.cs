using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Enums;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands.Extended;

internal class OrderExpressCommandHandler : CommandHandler,
    IRequestHandler<OrderExpressCreateByCustomerCommand, ValidationResult>,
    IRequestHandler<OrderExpressDeleteCommand, ValidationResult>,
    IRequestHandler<OrderExpressAddByCustomerCommand, ValidationResult>,
    IRequestHandler<OrderExpressAddCommand, ValidationResult>,
    IRequestHandler<OrderExpressEditCommand, ValidationResult>,
    IRequestHandler<ApprovalOrderExpressCommand, ValidationResult>,
    IRequestHandler<ManagePaymentOrderExpressCommand, ValidationResult>,
    IRequestHandler<ManageServiceOrderExpressCommand, ValidationResult>,
    IRequestHandler<NoteOrderExpressCommand, ValidationResult>,
    IRequestHandler<CreateInvoicePayOrderExpressCommand, ValidationResult>
{
    private readonly IOrderExpressRepository _repository;
    private readonly IOrderExpressDetailRepository _orderExpressDetailRepository;
    private readonly IOrderServiceAddRepository _orderServiceAddRepository;
    private readonly ISOExtProcedures _procedures;
    private readonly IContextUser _context;
    private readonly IOrderTrackingRepository _orderTrackingRepository;
    private readonly ISyntaxCodeRepository _synctaxCodeRepository;
    private readonly IPaymentInvoiceRepository _paymentInvoiceRepository;
    private readonly IOrderInvoiceRepository _orderInvoiceRepository;

    public OrderExpressCommandHandler(IOrderExpressRepository repository, IContextUser contextUser, ISOExtProcedures sOContextProcedures, IOrderExpressDetailRepository orderExpressDetailRepository, IOrderServiceAddRepository orderServiceAddRepository, IOrderTrackingRepository orderTrackingRepository, ISyntaxCodeRepository synctaxCodeRepository, IPaymentInvoiceRepository paymentInvoiceRepository, IOrderInvoiceRepository orderInvoiceRepository)
    {
        _repository = repository;
        _context = contextUser;
        _procedures = sOContextProcedures;
        _orderExpressDetailRepository = orderExpressDetailRepository;
        _orderServiceAddRepository = orderServiceAddRepository;
        _orderTrackingRepository = orderTrackingRepository;
        _synctaxCodeRepository = synctaxCodeRepository;
        _paymentInvoiceRepository = paymentInvoiceRepository;
        _orderInvoiceRepository = orderInvoiceRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(OrderExpressCreateByCustomerCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;

        var item = new OrderExpress
        {
            Id = request.Id,
            Code = request.Code,
            CustomerId = request.CustomerId,
            AccountId = request.AccountId,
            AccountName = request.AccountName,
            DeliveryName = request.DeliveryName,
            DeliveryPhone = request.DeliveryPhone,
            DeliveryAddress = request.DeliveryAddress,
            DeliveryCountry = request.DeliveryCountry,
            DeliveryProvince = request.DeliveryProvince,
            DeliveryDistrict = request.DeliveryDistrict,
            DeliveryWard = request.DeliveryWard,
            DeliveryNote = request.DeliveryNote,
            ShippingMethodId = request.ShippingMethodId,
            ShippingMethodName = request.ShippingMethodName,
            ShippingMethodCode = request.ShippingMethodCode,
            Note = request.Note,
            RouterShipping = request.RouterShipping,
            Currency = request.Currency,
            Weight = request.Weight,
            Width = request.Width,
            Height = request.Height,
            Length = request.Length,
            Image = request.Image,
            DomesticTracking = request.DomesticTracking,
            Status = string.IsNullOrEmpty(request.DomesticTracking) ? 30 : 40,
            OrderDate = DateTime.Now,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
            CreatedByName = request.CreatedByName
        };
        if (request.OrderExpressDetail?.Count > 0)
        {
            var description = string.Join(", ", request.OrderExpressDetail.Select(x => x.ProductName));
            item.Description = description.Length > 999 ? description.Substring(0, 999) : description;
        }
        _repository.Add(item);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        if (request.Detail?.Count > 0)
        {
            item.Description = string.Join(",", request.Detail.Select(x => x.ProductName)).Substring(0, 999);

            var list = request.Detail.Select((x, index) => new OrderExpressDetail
            {
                Id = x.Id,
                OrderExpressId = x.OrderExpressId,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage,
                Origin = x.Origin,
                UnitName = x.UnitName,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                Note = x.Note,
                CommodityGroup = x.CommodityGroup,
                DisplayOrder = index,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate,
                CreatedByName = request.CreatedByName
            }).ToList();

            _orderExpressDetailRepository.Add(list);
            _ = await CommitNoCheck(_orderExpressDetailRepository.UnitOfWork);
        }

        if (request.OrderServiceAdd?.Count > 0)
        {
            var list = request.OrderServiceAdd.Select(x => new OrderServiceAdd
            {
                Id = (Guid)x.Id,
                OrderExpressId = x.OrderExpressId,
                ServiceAddId = x.ServiceAddId,
                ServiceAddName = x.ServiceAddName,
                Price = x.Price,
                Status = x.Status,
                CreatedBy = request.CreatedBy,
                CreatedDate = (DateTime)request.CreatedDate,
                CreatedByName = request.CreatedByName
            }).ToList();

            _orderServiceAddRepository.Add(list);
            _ = await CommitNoCheck(_orderServiceAddRepository.UnitOfWork);
        }

        return result;
    }
    public async Task<ValidationResult> Handle(OrderExpressAddByCustomerCommand request, CancellationToken cancellationToken)
    {
        var result = await _procedures.SP_CREATE_EXPRESS_ORDERAsync(request.Id,
                                                                             request.Code,
                                                                             request.CustomerId,
                                                                             request.StoreCode,
                                                                             request.CurrencyCode,
                                                                             request.ShippingMethodCode,
                                                                             request.RouterShipping,
                                                                             request.TrackingCode,
                                                                             request.TrackingCarrier,
                                                                             request.Weight,
                                                                             request.Width,
                                                                             request.Height,
                                                                             request.Length,
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
                                                                             _context.UserId,
                                                                             _context.UserName);

        return request.ValidationResult;
    }

    public async Task<ValidationResult> Handle(OrderExpressDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new OrderExpress
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(OrderExpressEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.Id = request.Id;
        item.OrderType = request.OrderType;
        item.Code = request.Code;
        item.OrderDate = request.OrderDate;
        item.CustomerId = request.CustomerId;
        item.CustomerName = request.CustomerName;
        item.CustomerCode = request.CustomerCode;
        item.StoreId = request.StoreId;
        item.StoreCode = request.StoreCode;
        item.StoreName = request.StoreName;
        item.ContractId = request.ContractId;
        item.ContractName = request.ContractName;
        item.Currency = request.Currency;
        item.ExchangeRate = request.ExchangeRate;
        item.ShippingMethodId = request.ShippingMethodId;
        item.ShippingMethodCode = request.ShippingMethodCode;
        item.ShippingMethodName = request.ShippingMethodName;
        item.RouterShipping = request.RouterShipping;
        item.DomesticTracking = request.DomesticTracking;
        item.DomesticCarrier = request.DomesticCarrier;
        item.Status = request.Status;
        item.PaymentTermId = request.PaymentTermId;
        item.PaymentTermName = request.PaymentTermName;
        item.PaymentMethodName = request.PaymentMethodName;
        item.PaymentMethodId = request.PaymentMethodId;
        item.PaymentStatus = request.PaymentStatus;
        item.ShipperName = request.ShipperName;
        item.ShipperPhone = request.ShipperPhone;
        item.ShipperZipCode = request.ShipperZipCode;
        item.ShipperAddress = request.ShipperAddress;
        item.ShipperCountry = request.ShipperCountry;
        item.ShipperProvince = request.ShipperProvince;
        item.ShipperDistrict = request.ShipperDistrict;
        item.ShipperWard = request.ShipperWard;
        item.ShipperNote = request.ShipperNote;
        item.DeliveryName = request.DeliveryName;
        item.DeliveryPhone = request.DeliveryPhone;
        item.DeliveryZipCode = request.DeliveryZipCode;
        item.DeliveryAddress = request.DeliveryAddress;
        item.DeliveryCountry = request.DeliveryCountry;
        item.DeliveryProvince = request.DeliveryProvince;
        item.DeliveryDistrict = request.DeliveryDistrict;
        item.DeliveryWard = request.DeliveryWard;
        item.DeliveryNote = request.DeliveryNote;
        item.EstimatedDeliveryDate = request.EstimatedDeliveryDate;
        item.DeliveryMethodId = request.DeliveryMethodId;
        item.DeliveryMethodCode = request.DeliveryMethodCode;
        item.DeliveryMethodName = request.DeliveryMethodName;
        item.DeliveryStatus = request.DeliveryStatus;
        item.IsBill = request.IsBill;
        item.BillName = request.BillName;
        item.BillAddress = request.BillAddress;
        item.BillCountry = request.BillCountry;
        item.BillProvince = request.BillProvince;
        item.BillDistrict = request.BillDistrict;
        item.BillWard = request.BillWard;
        item.BillStatus = request.BillStatus;
        item.Description = request.Description;
        item.Note = request.Note;
        item.GroupEmployeeId = request.GroupEmployeeId;
        item.GroupEmployeeName = request.GroupEmployeeName;
        item.CommodityGroup = request.CommodityGroup;
        item.AirFreight = request.AirFreight;
        item.SeaFreight = request.SeaFreight;
        item.Surcharge = request.Surcharge;
        item.Weight = request.Weight;
        item.Width = request.Width;
        item.Height = request.Height;
        item.Length = request.Length;
        item.Image = request.Image;
        item.Paid = request.Paid;
        item.Total = request.Total;
        item.AccountId = request.AccountId;
        item.AccountName = request.AccountName;
        item.RouterShippingId = request.RouterShippingId;
        item.ShippingCodePost = request.ShippingCodePost;
        item.TrackingCode = request.TrackingCode;
        item.TrackingCarrier = request.TrackingCarrier;
        item.Package = request.Package;
        item.ToDeliveryDate = request.ToDeliveryDate;
        item.Description = request.Description;

        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        //if (request.OrderExpressDetail?.Count > 0)
        //{
        //    string description = string.Join(", ", request.OrderExpressDetail.Select(x => x.ProductName));
        //    item.Description = description.Length > 999 ? description.Substring(0, 999) : description;
        //}    

        _repository.Update(item);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        if (request.OrderExpressDetail.Any())
        {
            var listUpdate = new List<OrderExpressDetail>();
            var listDelete = new List<OrderExpressDetail>();

            foreach (var x in item.OrderExpressDetail)
            {
                var u = request.OrderExpressDetail.FirstOrDefault(o => o.Id == x.Id);
                if (u is not null)
                {
                    x.Id = u.Id;
                    x.OrderExpressId = u.OrderExpressId;
                    x.ProductCode = u.ProductCode;
                    x.ProductName = u.ProductName;
                    x.ProductImage = u.ProductImage;
                    x.ProductLink = u.ProductLink;
                    x.Origin = u.Origin;
                    x.UnitName = u.UnitName;
                    x.Quantity = u.Quantity;
                    x.UnitPrice = u.UnitPrice;
                    x.DisplayOrder = u.DisplayOrder;
                    x.Note = u.Note;
                    x.UpdatedBy = updatedBy;
                    x.UpdatedDate = updatedDate;
                    x.UpdatedByName = updateName;
                    x.CommodityGroup = u.CommodityGroup;
                    x.SurchargeGroup = u.SurchargeGroup;
                    x.Surcharge = u.Surcharge;

                    listUpdate.Add(x);
                    request.OrderExpressDetail.Remove(u);
                }
                else
                {
                    listDelete.Add(x);
                }
            }

            var listAdd = new List<OrderExpressDetail>();
            for (var i = 0; i < request.OrderExpressDetail.Count; i++)
            {
                var u = request.OrderExpressDetail[i];
                var orderExpressDetail = item.OrderExpressDetail.FirstOrDefault(x => x.Id.Equals(request.OrderExpressDetail[i].Id));
                if (orderExpressDetail is null)
                {
                    listAdd.Add(new OrderExpressDetail()
                    {
                        Id = u.Id,
                        ProductName = u.ProductName,
                        ProductCode = u.ProductCode,
                        ProductImage = u.ProductImage,
                        ProductLink = u.ProductLink,
                        Origin = u.Origin,
                        UnitName = u.UnitName,
                        Quantity = u.Quantity,
                        UnitPrice = u.UnitPrice,
                        Note = u.Note,
                        DisplayOrder = u.DisplayOrder,
                        CreatedBy = updatedBy,
                        CreatedDate = updatedDate,
                        CreatedByName = updateName,
                        CommodityGroup = u.CommodityGroup,
                        Surcharge = u.Surcharge,
                        OrderExpressId = u.OrderExpressId,
                        SurchargeGroup = u.SurchargeGroup,
                    });

                    request.OrderExpressDetail.Remove(request.OrderExpressDetail[i]);
                    i--;
                }
            }

            _orderExpressDetailRepository.Update(listUpdate);
            _orderExpressDetailRepository.Add(listAdd);
            _orderExpressDetailRepository.Remove(listDelete);
            _ = await CommitNoCheck(_orderExpressDetailRepository.UnitOfWork);
        }
        else
        {
            item.OrderExpressDetail.Clear();
        }

        var listSerViceUpdate = new List<OrderServiceAdd>();
        var listSerViceDelete = new List<OrderServiceAdd>();
        if (request.OrderServiceAdd.Any())
        {
            foreach (var x in item.OrderServiceAdd)
            {
                var u = request.OrderServiceAdd.Where(o => o.Id == x.Id).FirstOrDefault();
                if (u != null)
                {
                    x.OrderExpressId = request.Id;
                    x.ServiceAddId = u.ServiceAddId;
                    x.ServiceAddName = u.ServiceAddName;
                    x.Price = u.Price;
                    x.Currency = u.Currency;
                    x.Calculation = u.Calculation;
                    x.Status = u.Status;
                    x.Note = u.Note;
                    x.UpdatedBy = updatedBy;
                    x.UpdatedDate = updatedDate;
                    x.UpdatedByName = updateName;
                    x.ExchangeRate = u.ExchangeRate;
                    x.DisplayOrder = u.DisplayOrder;
                    listSerViceUpdate.Add(x);
                    request.OrderServiceAdd.Remove(u);
                }
                else
                {
                    listSerViceDelete.Add(x);
                }
            }

            var listSerViceAdd = new List<OrderServiceAdd>();
            for (var i = 0; i < request.OrderServiceAdd.Count; i++)
            {
                var u = request.OrderServiceAdd[i];
                var orderServiceAdd = item.OrderServiceAdd.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (orderServiceAdd is null)
                {
                    listSerViceAdd.Add(new OrderServiceAdd()
                    {
                        Id = Guid.NewGuid(),
                        OrderExpressId = request.Id,
                        ServiceAddId = u.ServiceAddId,
                        ServiceAddName = u.ServiceAddName,
                        Price = u.Price,
                        Currency = u.Currency,
                        Calculation = u.Calculation,
                        Status = u.Status,
                        Note = u.Note,
                        CreatedBy = updatedBy,
                        CreatedDate = updatedDate,
                        CreatedByName = updateName,
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
        else if (item.OrderServiceAdd.Any())
        {
            foreach (var x in item.OrderServiceAdd)
            {
                listSerViceDelete.Add(x);
            }
            _orderServiceAddRepository.Remove(listSerViceDelete);
            _ = await CommitNoCheck(_orderServiceAddRepository.UnitOfWork);
        }

        var listPaymentInvoiceUpdate = new List<PaymentInvoice>();
        var listPaymentInvoiceDelete = new List<PaymentInvoice>();
        if (request.PaymentInvoice?.Count > 0)
        {
            foreach (var x in item.PaymentInvoice)
            {
                var u = request.PaymentInvoice.Where(p => p.Id == x.Id).FirstOrDefault();
                if (u != null)
                {
                    x.Type = u.Type;
                    x.Code = u.Code;
                    x.Description = u.Description;
                    x.Amount = u.Amount;
                    x.Currency = u.Currency;
                    x.CurrencyName = u.CurrencyName;
                    x.Calculation = u.Calculation;
                    x.ExchangeRate = u.ExchangeRate;
                    x.PaymentDate = u.PaymentDate;
                    x.PaymentMethodName = u.PaymentMethodName;
                    x.PaymentMethodCode = u.PaymentMethodCode;
                    x.PaymentMethodId = u.PaymentMethodId;
                    x.BankName = u.BankName;
                    x.BankAccount = u.BankAccount;
                    x.BankNumber = u.BankNumber;
                    x.PaymentCode = u.PaymentCode;
                    x.PaymentNote = u.PaymentNote;
                    x.Note = u.Note;
                    x.Status = u.Status;
                    x.Locked = u.Locked;
                    x.PaymentStatus = u.PaymentStatus;
                    x.AccountId = u.AccountId;
                    x.AccountName = u.AccountName;
                    x.UpdatedBy = updatedBy;
                    x.UpdatedDate = updatedDate;
                    x.UpdatedByName = updateName;
                    x.CustomerId = u.CustomerId;
                    x.CustomerName = u.CustomerName;
                    listPaymentInvoiceUpdate.Add(x);
                    request.PaymentInvoice.Remove(u);
                }
                else
                {
                    listPaymentInvoiceDelete.Add(x);
                }
            }
            var listPaymentInvoiceAdd = new List<PaymentInvoice>();
            for (var i = 0; i < request.PaymentInvoice.Count; i++)
            {
                var u = request.PaymentInvoice[i];
                var paymentInvoice = item.PaymentInvoice.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (paymentInvoice is null)
                {
                    u.Code = _synctaxCodeRepository.GetCode("SO_FT", 1).Result;
                    listPaymentInvoiceAdd.Add(new PaymentInvoice()
                    {
                        Id = Guid.NewGuid(),
                        Type = u.Type,
                        Code = u.Code,
                        OrderExpressId = request.Id,
                        OrderExpressCode = request.Code,
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
                        CreatedByName = updateName,
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
        else if (item.PaymentInvoice.Any())
        {
            foreach (var x in item.PaymentInvoice)
            {
                listPaymentInvoiceDelete.Add(x);
            }
            _paymentInvoiceRepository.Remove(listPaymentInvoiceDelete);
            _ = await CommitNoCheck(_paymentInvoiceRepository.UnitOfWork);
        }

        var listOrderInvoiceUpdate = new List<OrderInvoice>();
        var listOrderInvoiceDelete = new List<OrderInvoice>();
        if (request.OrderInvoice.Any())
        {
            foreach (var x in item.OrderInvoice)
            {
                var u = request.OrderInvoice.Where(x => x.Id == item.Id).FirstOrDefault();
                if (u != null)
                {
                    x.OrderExpressId = request.Id;
                    x.OrderExpressCode = request.Code;
                    x.Serial = u.Serial;
                    x.Symbol = u.Symbol;
                    x.Number = u.Number;
                    x.Value = u.Value;
                    x.Date = u.Date;
                    x.Note = u.Note;
                    x.DisplayOrder = u.DisplayOrder;
                    x.UpdatedBy = updatedBy;
                    x.UpdatedDate = updatedDate;
                    x.UpdatedByName = updateName;
                    listOrderInvoiceUpdate.Add(x);
                    request.OrderInvoice.Remove(u);
                }
                else
                {
                    listOrderInvoiceDelete.Add(x);
                }
            }

            var listOrderInvoiceAdd = new List<OrderInvoice>();
            for (var i = 0; i < request.OrderInvoice.Count; i++)
            {
                var u = request.OrderInvoice[i];
                var orderInvoice = item.OrderInvoice.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (orderInvoice is null)
                {
                    listOrderInvoiceAdd.Add(new OrderInvoice()
                    {
                        Id = Guid.NewGuid(),
                        OrderExpressId = request.Id,
                        OrderExpressCode = request.Code,
                        Serial = u.Serial,
                        Symbol = u.Symbol,
                        Number = u.Number,
                        Value = u.Value,
                        Date = u.Date,
                        Note = u.Note,
                        DisplayOrder = u.DisplayOrder,
                        CreatedBy = updatedBy,
                        CreatedDate = updatedDate,
                        CreatedByName = updateName
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
        else if (item.OrderInvoice.Any())
        {
            foreach (var x in item.OrderInvoice)
            {
                listOrderInvoiceDelete.Add(x);
            }
            _orderInvoiceRepository.Remove(listOrderInvoiceDelete);
            _ = await CommitNoCheck(_orderInvoiceRepository.UnitOfWork);
        }

        return result;
    }

    public async Task<ValidationResult> Handle(OrderExpressAddCommand request, CancellationToken cancellationToken)
    {
        var orderExpress = new OrderExpress
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
            ContractId = request.ContractId,
            ContractName = request.ContractName,
            Currency = request.Currency,
            ExchangeRate = request.ExchangeRate,
            ShippingMethodId = request.ShippingMethodId,
            ShippingMethodCode = request.ShippingMethodCode,
            ShippingMethodName = request.ShippingMethodName,
            RouterShipping = request.RouterShipping,
            DomesticTracking = request.DomesticTracking,
            DomesticCarrier = request.DomesticCarrier,
            Status = request.Status,
            PaymentTermId = request.PaymentTermId,
            PaymentTermName = request.PaymentTermName,
            PaymentMethodName = request.PaymentMethodName,
            PaymentMethodId = request.PaymentMethodId,
            PaymentStatus = request.PaymentStatus,
            ShipperName = request.ShipperName,
            ShipperPhone = request.ShipperPhone,
            ShipperZipCode = request.ShipperZipCode,
            ShipperAddress = request.ShipperAddress,
            ShipperCountry = request.ShipperCountry,
            ShipperProvince = request.ShipperProvince,
            ShipperDistrict = request.ShipperDistrict,
            ShipperWard = request.ShipperWard,
            ShipperNote = request.ShipperNote,
            DeliveryName = request.DeliveryName,
            DeliveryPhone = request.DeliveryPhone,
            DeliveryZipCode = request.DeliveryZipCode,
            DeliveryAddress = request.DeliveryAddress,
            DeliveryCountry = request.DeliveryCountry,
            DeliveryProvince = request.DeliveryProvince,
            DeliveryDistrict = request.DeliveryDistrict,
            DeliveryWard = request.DeliveryWard,
            DeliveryNote = request.DeliveryNote,
            EstimatedDeliveryDate = request.EstimatedDeliveryDate,
            DeliveryMethodId = request.DeliveryMethodId,
            DeliveryMethodCode = request.DeliveryMethodCode,
            DeliveryMethodName = request.DeliveryMethodName,
            DeliveryStatus = request.DeliveryStatus,
            IsBill = request.IsBill,
            BillName = request.BillName,
            BillAddress = request.BillAddress,
            BillCountry = request.BillCountry,
            BillProvince = request.BillProvince,
            BillDistrict = request.BillDistrict,
            BillWard = request.BillWard,
            BillStatus = request.BillStatus,
            Description = request.Description,
            Note = request.Note,
            GroupEmployeeId = request.GroupEmployeeId,
            GroupEmployeeName = request.GroupEmployeeName,
            CommodityGroup = request.CommodityGroup,
            AirFreight = request.AirFreight,
            SeaFreight = request.SeaFreight,
            Surcharge = request.Surcharge,
            Weight = request.Weight,
            Width = request.Width,
            Height = request.Height,
            Length = request.Length,
            Image = request.Image,
            Paid = request.Paid,
            Total = request.Total,
            AccountId = request.AccountId,
            AccountName = request.AccountName,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
            CreatedByName = request.CreatedByName,
            RouterShippingId = request.RouterShippingId,
            ShippingCodePost = request.ShippingCodePost,
            TrackingCode = request.TrackingCode,
            TrackingCarrier = request.TrackingCarrier,
            Package = request.Package,
            ToDeliveryDate = request.ToDeliveryDate,
        };

        //if (request.OrderExpressDetail?.Count > 0)
        //{
        //    string description = string.Join(", ", request.OrderExpressDetail.Select(x => x.ProductName));
        //    orderExpress.Description = description.Length > 999 ? description.Substring(0, 999) : description;
        //}

        _repository.Add(orderExpress);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        var list = new List<OrderExpressDetail>();
        if (request.OrderExpressDetail?.Count > 0)
        {
            foreach (var x in request.OrderExpressDetail)
            {
                list.Add(new OrderExpressDetail
                {
                    Id = x.Id,
                    OrderExpressId = x.OrderExpressId,
                    ProductName = x.ProductName,
                    ProductCode = x.ProductCode,
                    ProductImage = x.ProductImage,
                    Origin = x.Origin,
                    UnitName = x.UnitName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    DisplayOrder = x.DisplayOrder,
                    Note = x.Note,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    CreatedByName = x.CreatedByName,
                    CommodityGroup = x.CommodityGroup,
                    SurchargeGroup = x.SurchargeGroup,
                    Surcharge = x.Surcharge,
                });
            }

            _orderExpressDetailRepository.Add(list);
            _ = await CommitNoCheck(_orderExpressDetailRepository.UnitOfWork);
        }

        var listService = new List<OrderServiceAdd>();
        if (request.OrderServiceAdd?.Count > 0)
        {
            foreach (var u in request.OrderServiceAdd)
            {
                listService.Add(new OrderServiceAdd()
                {
                    Id = Guid.NewGuid(),
                    OrderExpressId = request.Id,
                    ServiceAddId = u.ServiceAddId,
                    ServiceAddName = u.ServiceAddName,
                    Currency = u.Currency,
                    Calculation = u.Calculation,
                    Price = u.Price,
                    Status = u.Status,
                    Note = u.Note,
                    CreatedBy = u.CreatedBy,
                    CreatedDate = u.CreatedDate,
                    CreatedByName = u.CreatedByName,
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
                u.Code = _synctaxCodeRepository.GetCode("SO_FT", 1).Result;
                listPaymentInvoice.Add(new PaymentInvoice()
                {
                    Id = Guid.NewGuid(),
                    Type = u.Type,
                    Code = u.Code,
                    OrderExpressId = u.OrderExpressId,
                    OrderExpressCode = u.OrderCode,
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
                    CreatedBy = u.CreatedBy,
                    CreatedDate = u.CreatedDate,
                    CreatedByName = u.CreatedByName,
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
                    OrderExpressId = request.Id,
                    OrderExpressCode = request.Code,
                    Serial = u.Serial,
                    Symbol = u.Symbol,
                    Number = u.Number,
                    Value = u.Value,
                    Date = u.Date,
                    Note = u.Note,
                    DisplayOrder = u.DisplayOrder,
                    CreatedBy = u.CreatedBy,
                    CreatedDate = u.CreatedDate,
                    CreatedByName = u.CreatedByName
                });
            }
            _orderInvoiceRepository.Add(listOrderInvoice);
            _ = await CommitNoCheck(_orderInvoiceRepository.UnitOfWork);
        }

        return result;
    }

    public async Task<ValidationResult> Handle(ApprovalOrderExpressCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Order is not exist") } };
        }
        var obj = new OrderExpress
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
                0 => "Chờ xác nhận",
                30 => "Vận chuyển nội địa",
                40 => "Đến kho Megabuy",
                50 => "Chờ tất toán",
                60 => "Đang giao hàng",
                70 => "Đã giao hàng",
                80 => "Đơn trả hàng",
                90 => "Đơn huỷ",
                _ => ""
            };

            _orderTrackingRepository.Add(new List<OrderTracking>() { new OrderTracking()
            {
                Id = Guid.NewGuid(),
                OrderExpressId = request.Id,
                Name = text,
                Status = 1,
                Description = request.ApproveComment,
                Image = "",
                TrackingDate = obj.ApproveDate,
                CreatedDate = DateTime.Now,
                CreatedBy = _context.GetUserId(),
                CreatedByName = _context.UserName
            }});

            await Commit(_orderTrackingRepository.UnitOfWork);
        }
        catch (Exception)
        {
        }
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ManagePaymentOrderExpressCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var obj = await _repository.GetById(request.Id);

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Order express is not exist") } };
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
                    u.Code = _synctaxCodeRepository.GetCode("SO_FT", 1).Result;
                    listPaymentInvoiceAdd.Add(new PaymentInvoice()
                    {
                        Id = Guid.NewGuid(),
                        Type = u.Type,
                        Code = u.Code,
                        OrderExpressId = request.Id,
                        OrderExpressCode = obj.Code,
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

    public async Task<ValidationResult> Handle(ManageServiceOrderExpressCommand request, CancellationToken cancellationToken)
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
                    item.OrderExpressId = request.Id;
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
                        OrderExpressId = request.Id,
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

    public async Task<ValidationResult> Handle(NoteOrderExpressCommand request, CancellationToken cancellationToken)
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
                        OrderExpressId = request.Id,
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


    public async Task<ValidationResult> Handle(CreateInvoicePayOrderExpressCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!request.IsValid())
            return request.ValidationResult;

        var order = await _repository.GetById(request.Id);

        if (order is null)
        {
            throw new ErrorCodeException("ORDER_EXPRESS_NOT_FOUND");
        }

        var result = await _procedures.SP_CREATE_INVOICE_PAY_ORDER_EXPRESSAsync(
            request.Id,
            request.TotalPay,
            request.AccountId,
            request.CreatedBy,
            request.CreatedByName);

        if (result != null)
        {
            if (order.Status == (int)OrderStatus.WaitForSettlement)
            {
                order.Status = (int)OrderStatus.Delivering;
            }

            if (order.PaymentStatus == (int)PaymentStatus.Pending)
            {
                order.PaymentStatus = (int)PaymentStatus.PartialPayment;
            }

            if (order.PaymentStatus == (int)PaymentStatus.PartialPayment)
            {
                order.PaymentStatus = (int)PaymentStatus.Paid;
            }

            _repository.Update(order);
            await _repository.UnitOfWork.Commit();
        }

        return request.ValidationResult;
    }

}
