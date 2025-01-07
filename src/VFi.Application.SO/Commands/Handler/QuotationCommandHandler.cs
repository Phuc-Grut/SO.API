using Consul;
using FluentValidation.Results;
using MassTransit.Mediator;
using MediatR;
using VFi.Application.SO.Queries;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class QuotationCommandHandler : CommandHandler, IRequestHandler<AddQuotationCommand, ValidationResult>,
                                                            IRequestHandler<DeleteQuotationCommand, ValidationResult>,
                                                            IRequestHandler<EditQuotationCommand, ValidationResult>,
                                                            IRequestHandler<UpdateStatusQuotationCommand, ValidationResult>,
                                                            IRequestHandler<QuotationEmailNotifyCommand, ValidationResult>
{
    private readonly IQuotationRepository _repository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IOrderServiceAddRepository _orderServiceAddRepository;
    private readonly IRequestQuoteRepository _requestQuoteRepository;
    private readonly IContextUser _context;
    private readonly IEmailMasterRepository _emailMasterRepository;

    public QuotationCommandHandler(
                                    IQuotationRepository quotationRepository,
                                    IOrderProductRepository orderProductRepository,
                                    IOrderServiceAddRepository orderServiceAddRepository,
                                    IRequestQuoteRepository requestQuoteRepository,
                                    IContextUser context,
                                    IEmailMasterRepository emailMasterRepository
        )
    {
        _repository = quotationRepository;
        _orderProductRepository = orderProductRepository;
        _orderServiceAddRepository = orderServiceAddRepository;
        _requestQuoteRepository = requestQuoteRepository;
        _context = context;
        _emailMasterRepository = emailMasterRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(AddQuotationCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createdBy = _context.GetUserId();
        var createdByname = _context.UserClaims.FullName;
        var createdDate = DateTime.Now;
        var quotation = new Quotation
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Status = request.Status,
            CustomerId = request.CustomerId,
            CustomerCode = request.CustomerCode,
            CustomerName = request.CustomerName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            StoreId = request.StoreId,
            StoreCode = request.StoreCode,
            StoreName = request.StoreName,
            ChannelId = request.ChannelId,
            ChannelName = request.ChannelName,
            DeliveryNote = request.DeliveryNote,
            DeliveryName = request.DeliveryName,
            DeliveryAddress = request.DeliveryAddress,
            DeliveryCountry = request.DeliveryCountry,
            DeliveryProvince = request.DeliveryProvince,
            DeliveryDistrict = request.DeliveryDistrict,
            DeliveryWard = request.DeliveryWard,
            DeliveryStatus = request.DeliveryStatus,
            IsBill = request.IsBill,
            BillName = request.BillName,
            BillAddress = request.BillAddress,
            BillCountry = request.BillCountry,
            BillProvince = request.BillProvince,
            BillDistrict = request.BillDistrict,
            BillWard = request.BillWard,
            BillStatus = request.BillStatus,
            ShippingMethodId = request.ShippingMethodId,
            ShippingMethodName = request.ShippingMethodName,
            DeliveryMethodId = request.DeliveryMethodId,
            DeliveryMethodName = request.DeliveryMethodName,
            ExpectedDate = request.ExpectedDate,
            Currency = request.Currency,
            CurrencyName = request.CurrencyName,
            Calculation = request.Calculation,
            ExchangeRate = request.ExchangeRate,
            PriceListId = request.PriceListId,
            PriceListName = request.PriceListName,
            RequestQuoteId = request.RequestQuoteId,
            RequestQuoteCode = request.RequestQuoteCode,
            ContractId = request.ContractId,
            SaleOrderId = request.SaleOrderId,
            QuotationTermId = request.QuotationTermId,
            QuotationTermContent = request.QuotationTermContent,
            Date = request.Date,
            ExpiredDate = request.ExpiredDate,
            GroupEmployeeId = request.GroupEmployeeId,
            GroupEmployeeName = request.GroupEmployeeName,
            AccountId = request.AccountId,
            AccountName = request.AccountName,
            TypeDiscount = request.TypeDiscount,
            DiscountRate = request.DiscountRate,
            TypeCriteria = request.TypeCriteria,
            AmountDiscount = request.AmountDiscount,
            Note = request.Note,
            OldId = request.OldId,
            OldCode = request.OldCode,
            File = request.File,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createdByname
        };

        _repository.Add(quotation);
        if (request.OldId != null)
        {
            //trường hợp báo giá lại. sửa status báo giá cũ = 4 (Hết hiệu lực)
            var itemOld = await _repository.GetById((Guid)request.OldId);

            if (itemOld is null)
            {
                return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Quotation Old is not exist") } };
            }
            itemOld.Status = 4;
            _repository.Update(itemOld);
        }
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;
        //add domain event
        //Quotation.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));
        if (request.RequestQuoteId != null)
        {
            var requestQuote = new RequestQuote
            {
                Id = (Guid)request.RequestQuoteId,
                Status = 2
            };
            _requestQuoteRepository.UpdateStatus(requestQuote);
            _ = await CommitNoCheck(_requestQuoteRepository.UnitOfWork);
        }

        List<OrderProduct> list = new List<OrderProduct>();
        if (request.OrderProduct?.Count > 0)
        {
            foreach (var u in request.OrderProduct)
            {
                list.Add(new OrderProduct()
                {
                    Id = Guid.NewGuid(),
                    QuotationId = request.Id,
                    QuotationName = request.Code,
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
                    CreatedByName = createdByname,
                    DeliveryStatus = u.DeliveryStatus,
                    DeliveryQuantity = u.DeliveryQuantity,
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
            }
            _orderProductRepository.Add(list);
            _ = await CommitNoCheck(_orderProductRepository.UnitOfWork);
        }

        List<OrderServiceAdd> listService = new List<OrderServiceAdd>();
        if (request.OrderServiceAdd?.Count > 0)
        {
            foreach (var u in request.OrderServiceAdd)
            {
                listService.Add(new OrderServiceAdd()
                {
                    Id = Guid.NewGuid(),
                    QuotationId = request.Id,
                    ServiceAddId = u.ServiceAddId,
                    ServiceAddName = u.ServiceAddName,
                    Currency = u.Currency,
                    Calculation = u.Calculation,
                    Price = u.Price,
                    Status = u.Status,
                    Note = u.Note,
                    CreatedBy = createdBy,
                    CreatedDate = createdDate,
                    CreatedByName = createdByname,
                    ExchangeRate = u.ExchangeRate,
                    DisplayOrder = u.DisplayOrder
                });
            }
            _orderServiceAddRepository.Add(listService);
            _ = await CommitNoCheck(_orderServiceAddRepository.UnitOfWork);
        }


        return result;
    }

    public async Task<ValidationResult> Handle(DeleteQuotationCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var obj = await _repository.GetById(request.Id);

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Quotation is not exist") } };
        }

        _repository.Remove(obj);

        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(EditQuotationCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;

        var obj = await _repository.GetById(request.Id);

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Quotation is not exist") } };
        }
        var updatedBy = _context.GetUserId();
        var updatedByname = _context.UserClaims.FullName;
        var updatedDate = DateTime.Now;

        obj.Code = request.Code;
        obj.Name = request.Name;
        obj.Description = request.Description;
        obj.Status = request.Status;
        obj.CustomerId = request.CustomerId;
        obj.CustomerCode = request.CustomerCode;
        obj.CustomerName = request.CustomerName;
        obj.Email = request.Email;
        obj.Phone = request.Phone;
        obj.Address = request.Address;
        obj.StoreId = request.StoreId;
        obj.StoreCode = request.StoreCode;
        obj.StoreName = request.StoreName;
        obj.ChannelId = request.ChannelId;
        obj.ChannelName = request.ChannelName;
        obj.DeliveryNote = request.DeliveryNote;
        obj.DeliveryName = request.DeliveryName;
        obj.DeliveryAddress = request.DeliveryAddress;
        obj.DeliveryCountry = request.DeliveryCountry;
        obj.DeliveryProvince = request.DeliveryProvince;
        obj.DeliveryDistrict = request.DeliveryDistrict;
        obj.DeliveryWard = request.DeliveryWard;
        obj.DeliveryStatus = request.DeliveryStatus;
        obj.IsBill = request.IsBill;
        obj.BillName = request.BillName;
        obj.BillAddress = request.BillAddress;
        obj.BillCountry = request.BillCountry;
        obj.BillProvince = request.BillProvince;
        obj.BillDistrict = request.BillDistrict;
        obj.BillWard = request.BillWard;
        obj.BillStatus = request.BillStatus;
        obj.ShippingMethodId = request.ShippingMethodId;
        obj.ShippingMethodName = request.ShippingMethodName;
        obj.DeliveryMethodId = request.DeliveryMethodId;
        obj.DeliveryMethodName = request.DeliveryMethodName;
        obj.ExpectedDate = request.ExpectedDate;
        obj.Currency = request.Currency;
        obj.CurrencyName = request.CurrencyName;
        obj.Calculation = request.Calculation;
        obj.ExchangeRate = request.ExchangeRate;
        obj.PriceListId = request.PriceListId;
        obj.PriceListName = request.PriceListName;
        obj.ContractId = request.ContractId;
        obj.SaleOrderId = request.SaleOrderId;
        obj.QuotationTermId = request.QuotationTermId;
        obj.QuotationTermContent = request.QuotationTermContent;
        obj.Date = request.Date;
        obj.ExpiredDate = request.ExpiredDate;
        obj.GroupEmployeeId = request.GroupEmployeeId;
        obj.GroupEmployeeName = request.GroupEmployeeName;
        obj.AccountId = request.AccountId;
        obj.AccountName = request.AccountName;
        obj.TypeDiscount = request.TypeDiscount;
        obj.DiscountRate = request.DiscountRate;
        obj.TypeCriteria = request.TypeCriteria;
        obj.AmountDiscount = request.AmountDiscount;
        obj.Note = request.Note;
        obj.File = request.File;
        obj.UpdatedBy = updatedBy;
        obj.UpdatedDate = updatedDate;
        obj.UpdatedByName = updatedByname;
        if (obj.RequestQuoteId != request.RequestQuoteId)
        {
            if (obj.RequestQuoteId is null)
            {
                var requestQuote = new RequestQuote
                {
                    Id = (Guid)request.RequestQuoteId,
                    Status = 2
                };
                _requestQuoteRepository.UpdateStatus(requestQuote);
                _ = await CommitNoCheck(_requestQuoteRepository.UnitOfWork);
            }
            else if (request.RequestQuoteId is null)
            {
                //Update chờ xử lý
                var requestQuote1 = new RequestQuote
                {
                    Id = (Guid)obj.RequestQuoteId,
                    Status = 1
                };
                _requestQuoteRepository.UpdateStatus(requestQuote1);
                _ = await CommitNoCheck(_requestQuoteRepository.UnitOfWork);
            }
            else
            {
                //Update yêu cầu báo giá cũ
                var requestQuote1 = new RequestQuote
                {
                    Id = (Guid)obj.RequestQuoteId,
                    Status = 1
                };
                _requestQuoteRepository.UpdateStatus(requestQuote1);

                //update đã báo giá
                var requestQuote2 = new RequestQuote
                {
                    Id = (Guid)request.RequestQuoteId,
                    Status = 2
                };
                _requestQuoteRepository.UpdateStatus(requestQuote2);
                _ = await CommitNoCheck(_requestQuoteRepository.UnitOfWork);
            }
        }
        obj.RequestQuote = null;
        obj.RequestQuoteId = request.RequestQuoteId;
        _repository.Update(obj);

        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        //Detail
        if (request.OrderProduct.Any())
        {
            var listUpdate = new List<OrderProduct>();
            var listDelete = new List<OrderProduct>();
            foreach (var item in obj.OrderProduct)
            {
                var u = request.OrderProduct.FirstOrDefault(x => x.Id == item.Id);
                if (u != null)
                {
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
                    item.DisplayOrder = u.DisplayOrder;
                    item.UpdatedBy = updatedBy;
                    item.UpdatedDate = updatedDate;
                    item.UpdatedByName = updatedByname;
                    item.DeliveryStatus = u.DeliveryStatus;
                    item.DeliveryQuantity = u.DeliveryQuantity;
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

                    listUpdate.Add(item);
                    request.OrderProduct.Remove(u);
                }
                else
                {
                    listDelete.Add(item);
                }
            }
            var listAdd = new List<OrderProduct>();
            for (int i = 0; i < request.OrderProduct.Count; i++)
            {
                var u = request.OrderProduct[i];
                var orderProduct = obj.OrderProduct.FirstOrDefault(x => x.Id.Equals(request.OrderProduct[i].Id));
                if (orderProduct is null)
                {
                    listAdd.Add(new OrderProduct()
                    {
                        Id = Guid.NewGuid(),
                        QuotationId = request.Id,
                        QuotationName = request.Code,
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

            _orderProductRepository.Update(listUpdate);
            _orderProductRepository.Add(listAdd);
            _orderProductRepository.Remove(listDelete);
            _ = await CommitNoCheck(_orderProductRepository.UnitOfWork);
        }
        else
        {
            obj.OrderProduct.Clear();
            _ = await CommitNoCheck(_orderProductRepository.UnitOfWork);
        }
        List<OrderServiceAdd> listSerViceUpdate = new List<OrderServiceAdd>();
        List<OrderServiceAdd> listSerViceDelete = new List<OrderServiceAdd>();
        if (request.OrderServiceAdd.Any())
        {
            foreach (var item in obj.OrderServiceAdd)
            {
                var u = request.OrderServiceAdd.Where(x => x.Id == item.Id).FirstOrDefault();
                if (u != null)
                {
                    item.QuotationId = request.Id;
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

            List<OrderServiceAdd> listSerViceAdd = new List<OrderServiceAdd>();
            for (int i = 0; i < request.OrderServiceAdd.Count; i++)
            {
                var u = request.OrderServiceAdd[i];
                var orderServiceAdd = obj.OrderServiceAdd.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (orderServiceAdd is null)
                {
                    listSerViceAdd.Add(new OrderServiceAdd()
                    {
                        Id = Guid.NewGuid(),
                        QuotationId = request.Id,
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
        return result;
    }

    public async Task<ValidationResult> Handle(UpdateStatusQuotationCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Quotation is not exist") } };
        }
        var obj = new Quotation
        {
            Id = request.Id,
            Status = request.Status,
            ApproveDate = DateTime.Now,
            ApproveBy = _context.GetUserId(),
            ApproveByName = _context.UserClaims.FullName,
            ApproveComment = request.ApproveComment,
        };
        _repository.Approve(obj);
        return await Commit(_repository.UnitOfWork);
    }
    public async Task<ValidationResult> Handle(QuotationEmailNotifyCommand request, CancellationToken cancellationToken)
    {
        var emailNotify = new EmailNotify
        {
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
