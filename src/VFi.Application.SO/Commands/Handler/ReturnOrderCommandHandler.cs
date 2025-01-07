using System.Collections.Generic;
using System.Reflection.Emit;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class ReturnOrderCommandHandler : CommandHandler, IRequestHandler<AddReturnOrderCommand, ValidationResult>,
                                                         IRequestHandler<DeleteReturnOrderCommand, ValidationResult>,
                                                         IRequestHandler<EditReturnOrderCommand, ValidationResult>,
                                                         IRequestHandler<ReturnOrderProcessCommand, ValidationResult>,
                                                         IRequestHandler<ReturnOrderDuplicateCommand, ValidationResult>,
                                                         IRequestHandler<ManagePaymentReturnCommand, ValidationResult>

{
    private readonly IReturnOrderRepository _repository;
    private readonly IReturnOrderProductRepository _returnOrderProductRepository;
    private readonly IContextUser _context;
    private readonly ISyntaxCodeRepository _synctaxCodeRepository;
    private readonly IPaymentInvoiceRepository _paymentInvoiceRepository;

    public ReturnOrderCommandHandler(
        IReturnOrderRepository ReturnOrderRepository,
        IReturnOrderProductRepository ReturnOrderProductRepository,
        IContextUser contextUser,
        ISyntaxCodeRepository synctaxCodeRepository,
        IPaymentInvoiceRepository paymentInvoiceRepository)
    {
        _repository = ReturnOrderRepository;
        _returnOrderProductRepository = ReturnOrderProductRepository;
        _context = contextUser;
        _synctaxCodeRepository = synctaxCodeRepository;
        _paymentInvoiceRepository = paymentInvoiceRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(AddReturnOrderCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var ReturnOrder = new ReturnOrder
        {
            Id = request.Id,
            Code = request.Code,
            CustomerId = request.CustomerId,
            CustomerCode = request.CustomerCode,
            CustomerName = request.CustomerName,
            OrderId = request.OrderId,
            OrderCode = request.OrderCode,
            Address = request.Address,
            Country = request.Country,
            Province = request.Province,
            District = request.District,
            Ward = request.Ward,
            Description = request.Description,
            Status = request.Status,
            WarehouseId = request.WarehouseId,
            WarehouseCode = request.WarehouseCode,
            WarehouseName = request.WarehouseName,
            ReturnDate = request.ReturnDate,
            AccountId = request.AccountId,
            AccountName = request.AccountName,
            CurrencyId = request.CurrencyId,
            Currency = request.Currency,
            CurrencyName = request.CurrencyName,
            Calculation = request.Calculation,
            ExchangeRate = request.ExchangeRate,
            TypeDiscount = request.TypeDiscount,
            DiscountRate = request.DiscountRate,
            TypeCriteria = request.TypeCriteria,
            AmountDiscount = request.AmountDiscount,
            ApproveBy = request.ApproveBy,
            ApproveDate = request.ApproveDate,
            ApproveByName = request.ApproveByName,
            ApproveComment = request.ApproveComment,
            File = request.File,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };
        _repository.Add(ReturnOrder);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        List<ReturnOrderProduct> list = new List<ReturnOrderProduct>();
        if (request.ReturnOrderProduct?.Count > 0)
        {
            foreach (var x in request.ReturnOrderProduct)
            {
                list.Add(new ReturnOrderProduct()
                {
                    Id = Guid.NewGuid(),
                    ReturnOrderId = request.Id,
                    OrderProductId = x.OrderProductId,
                    ProductId = x.ProductId,
                    ProductCode = x.ProductCode,
                    ProductName = x.ProductName,
                    QuantityReturn = x.QuantityReturn,
                    UnitPrice = x.UnitPrice,
                    UnitType = x.UnitType,
                    UnitCode = x.UnitCode,
                    UnitName = x.UnitName,
                    DiscountAmountDistribution = x.DiscountAmountDistribution,
                    DiscountType = x.DiscountType,
                    DiscountPercent = x.DiscountPercent,
                    AmountDiscount = x.AmountDiscount,
                    TaxRate = x.TaxRate,
                    Tax = x.Tax,
                    TaxCode = x.TaxCode,
                    ReasonId = x.ReasonId,
                    ReasonName = x.ReasonName,
                    WarehouseId = request.WarehouseId,
                    WarehouseName = request.WarehouseName,
                    DisplayOrder = x.DisplayOrder,
                    CreatedBy = createdBy,
                    CreatedDate = createdDate,
                    CreatedByName = createName
                });
            }
            _returnOrderProductRepository.Add(list);
            _ = await CommitNoCheck(_returnOrderProductRepository.UnitOfWork);
        }
        return result;
    }

    public async Task<ValidationResult> Handle(DeleteReturnOrderCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var ReturnOrder = new ReturnOrder
        {
            Id = request.Id
        };

        _repository.Remove(ReturnOrder);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(EditReturnOrderCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var obj = await _repository.GetById(request.Id);
        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "ReturnOrder is not exist") } };
        }
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;

        obj.CustomerId = request.CustomerId;
        obj.CustomerCode = request.CustomerCode;
        obj.CustomerName = request.CustomerName;
        obj.OrderId = request.OrderId;
        obj.OrderCode = request.OrderCode;
        obj.Address = request.Address;
        obj.Country = request.Country;
        obj.Province = request.Province;
        obj.District = request.District;
        obj.Ward = request.Ward;
        obj.Description = request.Description;
        obj.Status = request.Status;
        obj.WarehouseId = request.WarehouseId;
        obj.WarehouseCode = request.WarehouseCode;
        obj.WarehouseName = request.WarehouseName;
        obj.ReturnDate = request.ReturnDate;
        obj.AccountId = request.AccountId;
        obj.AccountName = request.AccountName;
        obj.CurrencyId = request.CurrencyId;
        obj.Currency = request.Currency;
        obj.CurrencyName = request.CurrencyName;
        obj.Calculation = request.Calculation;
        obj.ExchangeRate = request.ExchangeRate;
        obj.TypeDiscount = request.TypeDiscount;
        obj.DiscountRate = request.DiscountRate;
        obj.TypeCriteria = request.TypeCriteria;
        obj.AmountDiscount = request.AmountDiscount;
        obj.File = request.File;
        obj.UpdatedBy = updatedBy;
        obj.UpdatedDate = updatedDate;
        obj.UpdatedByName = updateName;
        obj.PaymentInvoice = null;

        if (request.ReturnOrderProduct?.Count > 0)
        {
            var listUpdate = new List<ReturnOrderProduct>();
            var listDelete = new List<ReturnOrderProduct>();
            foreach (var item in obj.ReturnOrderProduct)
            {
                var u = request.ReturnOrderProduct.FirstOrDefault(x => x.Id == item.Id);
                if (u != null)
                {
                    item.OrderProductId = u.OrderProductId;
                    item.ProductId = u.ProductId;
                    item.ProductCode = u.ProductCode;
                    item.ProductName = u.ProductName;
                    item.QuantityReturn = u.QuantityReturn;
                    item.UnitPrice = u.UnitPrice;
                    item.UnitType = u.UnitType;
                    item.UnitCode = u.UnitCode;
                    item.UnitName = u.UnitName;
                    item.DiscountAmountDistribution = u.DiscountAmountDistribution;
                    item.DiscountType = u.DiscountType;
                    item.DiscountPercent = u.DiscountPercent;
                    item.AmountDiscount = u.AmountDiscount;
                    item.TaxRate = u.TaxRate;
                    item.Tax = u.Tax;
                    item.TaxCode = u.TaxCode;
                    item.ReasonId = u.ReasonId;
                    item.ReasonName = u.ReasonName;
                    item.WarehouseId = request.WarehouseId;
                    item.WarehouseName = request.WarehouseName;
                    item.DisplayOrder = u.DisplayOrder;
                    item.UpdatedBy = updatedBy;
                    item.UpdatedDate = updatedDate;
                    item.UpdatedByName = updateName;
                    item.OrderProduct = null;

                    listUpdate.Add(item);
                    request.ReturnOrderProduct.Remove(new DTOs.ReturnOrderProductDto() { Id = u.Id });
                }
                else
                {
                    listDelete.Add(item);
                }
            }
            var listAdd = new List<ReturnOrderProduct>();
            for (int i = 0; i < request.ReturnOrderProduct.Count; i++)
            {
                var u = request.ReturnOrderProduct[i];
                var returnOrderProduct = obj.ReturnOrderProduct.FirstOrDefault(x => x.Id.Equals(request.ReturnOrderProduct[i].Id));
                if (returnOrderProduct is null)
                {
                    listAdd.Add(new ReturnOrderProduct()
                    {
                        Id = Guid.NewGuid(),
                        ReturnOrderId = request.Id,
                        OrderProductId = u.OrderProductId,
                        ProductId = u.ProductId,
                        ProductCode = u.ProductCode,
                        ProductName = u.ProductName,
                        QuantityReturn = u.QuantityReturn,
                        UnitPrice = u.UnitPrice,
                        UnitType = u.UnitType,
                        UnitCode = u.UnitCode,
                        UnitName = u.UnitName,
                        DiscountAmountDistribution = u.DiscountAmountDistribution,
                        DiscountType = u.DiscountType,
                        DiscountPercent = u.DiscountPercent,
                        AmountDiscount = u.AmountDiscount,
                        TaxRate = u.TaxRate,
                        Tax = u.Tax,
                        TaxCode = u.TaxCode,
                        ReasonId = u.ReasonId,
                        ReasonName = u.ReasonName,
                        WarehouseId = request.WarehouseId,
                        WarehouseName = request.WarehouseName,
                        DisplayOrder = u.DisplayOrder,
                        CreatedBy = updatedBy,
                        CreatedDate = updatedDate,
                        CreatedByName = updateName
                    });

                    request.ReturnOrderProduct.Remove(request.ReturnOrderProduct[i]);
                    i--;
                }
            }

            _returnOrderProductRepository.Update(listUpdate);
            _returnOrderProductRepository.Add(listAdd);
            _returnOrderProductRepository.Remove(listDelete);
            _ = await CommitNoCheck(_returnOrderProductRepository.UnitOfWork);
        }
        else
        {
            obj.ReturnOrderProduct.Clear();
            _ = await CommitNoCheck(_returnOrderProductRepository.UnitOfWork);
        }

        _repository.Update(obj);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;
        return result;
    }

    public async Task<ValidationResult> Handle(ReturnOrderProcessCommand request, CancellationToken cancellationToken)
    {
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);

        item.Status = request.Status;
        item.ApproveBy = updatedBy;
        item.ApproveByName = updateName;
        item.ApproveComment = request.ApproveComment;
        item.ApproveDate = updatedDate;
        item.ReturnOrderProduct = null;
        if (request.Status == 3)
        {
            var returnOrderProduct = await _returnOrderProductRepository.GetByParentId(request.Id);
            List<ReturnOrderProduct> listUpdate = new List<ReturnOrderProduct>();
            foreach (var x in returnOrderProduct)
            {
                var itemDetail = await _returnOrderProductRepository.GetById(x.Id);
                if (itemDetail is not null)
                {
                    itemDetail.QuantityReturn = itemDetail.QuantityReturn;
                    listUpdate.Add(itemDetail);
                }
            }
            _returnOrderProductRepository.Update(listUpdate);
            _ = await CommitNoCheck(_returnOrderProductRepository.UnitOfWork);
        }
        _repository.Update(item);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;
        return result;
    }
    public async Task<ValidationResult> Handle(ReturnOrderDuplicateCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValidReturnOrder(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = await _repository.GetById(request.ReturnOrderId);
        item.Id = request.Id;
        item.Code = request.Code;
        item.Status = 0;
        item.CreatedBy = createdBy;
        item.CreatedByName = createName;
        item.CreatedDate = createdDate;

        _repository.Add(item);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        var details = await _returnOrderProductRepository.GetByParentId(request.ReturnOrderId);
        List<ReturnOrderProduct> list = new List<ReturnOrderProduct>();
        if (details?.Count() > 0)
        {
            foreach (ReturnOrderProduct x in details)
            {
                x.Id = Guid.NewGuid();
                x.ReturnOrderId = request.Id;
                x.QuantityReturn = 0;
                list.Add(x);
            }
            _returnOrderProductRepository.Add(list);
            _ = await CommitNoCheck(_returnOrderProductRepository.UnitOfWork);
        }
        return result;
    }
    public async Task<ValidationResult> Handle(ManagePaymentReturnCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var obj = await _repository.GetById(request.Id);

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Order is not exist") } };
        }
        obj.PaymentStatus = request.PaymentStatus;
        obj.ReturnOrderProduct = null;
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
                    u.Code = _synctaxCodeRepository.GetCode("PTC", 1).Result;
                    listPaymentInvoiceAdd.Add(new PaymentInvoice()
                    {
                        Id = Guid.NewGuid(),
                        Type = u.Type,
                        Code = u.Code,
                        ReturnOrderId = request.Id,
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
}
