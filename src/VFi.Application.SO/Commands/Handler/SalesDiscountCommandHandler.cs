using FluentValidation.Results;
using MediatR;
using Microsoft.CodeAnalysis;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class SalesDiscountCommandHandler : CommandHandler,
    IRequestHandler<SalesDiscountAddCommand, ValidationResult>,
    IRequestHandler<SalesDiscountDeleteCommand, ValidationResult>,
    IRequestHandler<SalesDiscountEditCommand, ValidationResult>,
    IRequestHandler<SalesDiscountProcessCommand, ValidationResult>,
    IRequestHandler<SalesDiscountDuplicateCommand, ValidationResult>,
    IRequestHandler<SalesDiscountUploadFileCommand, ValidationResult>,
    IRequestHandler<ManagePaymentSDCommand, ValidationResult>

{
    private readonly ISalesDiscountRepository _repository;
    private readonly ISalesDiscountProductRepository _salesDiscountProductRepository;
    private readonly IContextUser _context;
    private readonly ISyntaxCodeRepository _synctaxCodeRepository;
    private readonly IPaymentInvoiceRepository _paymentInvoiceRepository;

    public SalesDiscountCommandHandler(
        ISalesDiscountRepository repository,
        ISalesDiscountProductRepository salesDiscountProductRepository,
        IContextUser context,
        ISyntaxCodeRepository synctaxCodeRepository,
        IPaymentInvoiceRepository paymentInvoiceRepository
        )
    {
        _repository = repository;
        _salesDiscountProductRepository = salesDiscountProductRepository;
        _context = context;
        _synctaxCodeRepository = synctaxCodeRepository;
        _paymentInvoiceRepository = paymentInvoiceRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(SalesDiscountAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValidSalesDiscount(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new SalesDiscount
        {
            Id = request.Id,
            Code = request.Code,
            CustomerId = request.CustomerId,
            CustomerCode = request.CustomerCode,
            CustomerName = request.CustomerName,
            CustomerAddressId = request.CustomerAddressId,
            SalesOrderCode = request.SalesOrderCode,
            SalesOrderId = request.SalesOrderId,
            CurrencyId = request.CurrencyId,
            CurrencyCode = request.CurrencyCode,
            CurrencyName = request.CurrencyName,
            ExchangeRate = request.ExchangeRate,
            EmployeeId = request.EmployeeId,
            EmployeeCode = request.EmployeeCode,
            EmployeeName = request.EmployeeName,
            Note = request.Note,
            Status = request.Status,
            DiscountDate = request.DiscountDate,
            TypeDiscount = request.TypeDiscount,
            File = request.File,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };
        _repository.Add(item);

        if (request.ListDetail?.Count > 0)
        {
            List<SalesDiscountProduct> list = new List<SalesDiscountProduct>();
            foreach (var x in request.ListDetail)
            {
                var detail = new SalesDiscountProduct()
                {
                    Id = (Guid)x.Id,
                    SalesDiscountId = x.SalesDiscountId,
                    SalesOrderId = x.SalesOrderId,
                    SalesOrderCode = x.SalesOrderCode,
                    OrderProductId = x.OrderProductId,
                    ProductName = x.ProductName,
                    ProductCode = x.ProductCode,
                    ProductId = x.ProductId,
                    ProductImage = x.ProductImage,
                    Quantity = x.Quantity,
                    ReasonDiscount = x.ReasonDiscount,
                    TaxCategoryId = x.TaxCategoryId,
                    TaxRate = x.TaxRate,
                    Tax = x.Tax,
                    UnitPrice = x.UnitPrice,
                    UnitId = x.UnitId,
                    UnitCode = x.UnitCode,
                    UnitName = x.UnitName,
                    GroupUnitId = x.GroupUnitId,
                    UnitType = x.UnitType,
                    DisplayOrder = x.DisplayOrder,
                    DiscountAmountDistribution = x.DiscountAmountDistribution,
                    DiscountPercent = x.DiscountPercent,
                    AmountDiscount = x.AmountDiscount,
                };
                list.Add(detail);
            }
            _salesDiscountProductRepository.Add(list);
        }

        return await Commit(_repository.UnitOfWork);
    }

    private async Task<bool> UpdatePaymentStatus(IEnumerable<SalesDiscount> listSalesOrders)
    {
        try
        {
            /* var listDiscounts = await _repository.GetByOrder(listSalesOrders.Select(x => x.Code).ToList(), true);
             var listReturnOrderProducts = await _returnOrderProductRepository.GetByOrderCode(listSalesOrders.Select(x => x.Code).ToList());

             var listPaymentInfo = await _paymentInfomationRepository.GetByListParentId(listSalesOrders.Select(x => x.Id).ToList());
             List<PaymentInfomation> listAllPaymentInfo = new List<PaymentInfomation>();
             if (listPaymentInfo.Count() > 0)
             {
                 foreach (var x in listPaymentInfo)
                 {
                     listAllPaymentInfo.Add(x);
                     listAllPaymentInfo.AddRange(x.PaymentInfomations.ToList());
                 }
             }

             listSalesOrders.Select(order =>
             {
                 var totalAmount = order.PurchaseProduct.Sum(x => x.TotalAmount);
                 var paidAmount = listAllPaymentInfo.Where(x => x.RecordId == order.Id && x.Status == 2 && x.Type == 2).ToList().Sum(x => x.Amount);
                 var refundAmount = listAllPaymentInfo.Where(x => x.RecordId == order.Id && x.Status == 2 && x.Type == 1).ToList().Sum(x => x.Amount);
                 var disAmount = listDiscounts.Where(x => x.SalesOrderId == order.Id && x.Status == 2)
                     .Select(x => new { Amount = x.SalesDiscountProducts?.Sum(s => (decimal)(s.Quantity ?? 0) * (s.UnitPrice ?? 0) + (decimal)(s.Quantity ?? 0) * (s.UnitPrice) * (decimal)(s.TaxRate ?? 0) / 100) })
                     .Sum(x => x.Amount);
                 var returnAmount = listReturnOrderProducts?.Where(s => s.ReturnOrder.Status == 2 && s.SalesOrderCode == order.Code)
                     .Sum(s => (decimal)(s.QuantityReturn ?? 0) * (s.UnitPrice ?? 0) + (decimal)(s.QuantityReturn ?? 0) * (s.UnitPrice ?? 0) * (decimal)(s.TaxRate ?? 0) / 100);

                 var check = (totalAmount ?? 0) - (paidAmount ?? 0) - (disAmount ?? 0) - (returnAmount ?? 0) + (refundAmount ?? 0);
                 if ((paidAmount ?? 0) == 0)
                 {
                     order.PaymentStatus = 0;
                 }
                 else if (check <= 0)
                 {
                     order.PaymentStatus = 2;
                 }
                 else if (check > 0)
                 {
                     order.PaymentStatus = 1;
                 }
                 return order;
             }).ToList();
             _orderRepository.Update(listSalesOrders);*/
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

    public async Task<ValidationResult> Handle(SalesDiscountDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValidSalesDiscount(_repository))
            return request.ValidationResult;

        var SalesDiscount = new SalesDiscount
        {
            Id = request.Id
        };

        _repository.Remove(SalesDiscount);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(SalesDiscountProcessCommand request, CancellationToken cancellationToken)
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
        _repository.Update(item);
        if (request.Status == 2)
        {
            if (item.SalesOrderId != null)
            {
                /*var listSalesOrders = await _orderRepository.GetListById((Guid)item.SalesOrderId);
                if (listSalesOrders != null)
                {
                    await _repository.UnitOfWork.Commit();
                    var check = await UpdatePaymentStatus(listSalesOrders);
                    if (check)
                    {
                        return await Commit(_orderRepository.UnitOfWork);
                    }
                    else
                    {
                        return new ValidationResult();
                    }
                }*/
            }
        }
        return await Commit(_repository.UnitOfWork);

    }
    public async Task<ValidationResult> Handle(SalesDiscountEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValidSalesDiscount(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.CustomerId = request.CustomerId;
        item.CurrencyName = request.CurrencyName;
        item.CurrencyCode = request.CurrencyCode;
        item.CustomerAddressId = request.CustomerAddressId;
        item.SalesOrderCode = request.SalesOrderCode;
        item.SalesOrderId = request.SalesOrderId;
        item.CurrencyId = request.CurrencyId;
        item.CurrencyCode = request.CurrencyCode;
        item.CurrencyName = request.CurrencyName;
        item.ExchangeRate = request.ExchangeRate;
        item.EmployeeId = request.EmployeeId;
        item.EmployeeName = request.EmployeeName;
        item.EmployeeCode = request.EmployeeCode;
        item.Note = request.Note;
        item.Status = request.Status;
        item.DiscountDate = request.DiscountDate;
        item.File = request.File;
        item.TypeDiscount = request.TypeDiscount;
        item.UpdatedByName = updateName;
        item.UpdatedBy = updatedBy;
        item.UpdatedDate = updatedDate;
        item.PaymentInvoice = null;

        // detail
        if (request.ListDetail.Any())
        {
            var listDelete = new List<SalesDiscountProduct>();
            var listUpdate = new List<SalesDiscountProduct>();
            foreach (var itemDetail in item.SalesDiscountProduct)
            {
                var x = request.ListDetail.FirstOrDefault(u => u.Id == itemDetail.Id);
                if (x != null)
                {
                    itemDetail.SalesDiscountId = x.SalesDiscountId;
                    itemDetail.OrderProductId = x.OrderProductId;
                    itemDetail.ProductName = x.ProductName;
                    itemDetail.ProductCode = x.ProductCode;
                    itemDetail.ProductId = x.ProductId;
                    itemDetail.ProductImage = x.ProductImage;
                    itemDetail.Quantity = x.Quantity;
                    itemDetail.ReasonDiscount = x.ReasonDiscount;
                    itemDetail.TaxCategoryId = x.TaxCategoryId;
                    itemDetail.TaxRate = x.TaxRate;
                    itemDetail.Tax = x.Tax;
                    itemDetail.UnitPrice = x.UnitPrice;
                    itemDetail.UnitId = x.UnitId;
                    itemDetail.UnitCode = x.UnitCode;
                    itemDetail.UnitName = x.UnitName;
                    itemDetail.GroupUnitId = x.GroupUnitId;
                    itemDetail.UnitType = x.UnitType;
                    itemDetail.DisplayOrder = x.DisplayOrder;
                    itemDetail.DiscountAmountDistribution = x.DiscountAmountDistribution;
                    itemDetail.DiscountPercent = x.DiscountPercent;
                    itemDetail.AmountDiscount = x.AmountDiscount;
                    /*                        itemDetail.UpdatedBy = updatedBy;
                                            itemDetail.UpdatedDate = updatedDate;
                                            itemDetail.UpdatedByName = updateName;*/
                    listUpdate.Add(itemDetail);
                    request.ListDetail.Remove(x);
                }
                else
                {
                    listDelete.Add(itemDetail);
                }

            }
            var listAdd = new List<SalesDiscountProduct>();
            for (int i = 0; i < request.ListDetail.Count; i++)
            {
                var x = request.ListDetail[i];
                var newList = item.SalesDiscountProduct.FirstOrDefault(x => x.Id.Equals(request.ListDetail[i].Id));
                if (newList is null)
                {
                    listAdd.Add(new SalesDiscountProduct()
                    {
                        Id = Guid.NewGuid(),
                        SalesDiscountId = request.Id,
                        OrderProductId = x.OrderProductId,
                        ProductName = x.ProductName,
                        ProductCode = x.ProductCode,
                        ProductId = x.ProductId,
                        ProductImage = x.ProductImage,
                        Quantity = x.Quantity,
                        ReasonDiscount = x.ReasonDiscount,
                        TaxCategoryId = x.TaxCategoryId,
                        TaxRate = x.TaxRate,
                        Tax = x.Tax,
                        UnitPrice = x.UnitPrice,
                        UnitId = x.UnitId,
                        UnitCode = x.UnitCode,
                        UnitName = x.UnitName,
                        GroupUnitId = x.GroupUnitId,
                        UnitType = x.UnitType,
                        DisplayOrder = x.DisplayOrder,
                        DiscountAmountDistribution = x.DiscountAmountDistribution,
                        DiscountPercent = x.DiscountPercent,
                        AmountDiscount = x.AmountDiscount,
                    });
                    request.ListDetail.Remove(request.ListDetail[i]);
                    i--;
                }
            }
            _salesDiscountProductRepository.Update(listUpdate);
            _salesDiscountProductRepository.Add(listAdd);
            _salesDiscountProductRepository.Remove(listDelete);
            _ = await CommitNoCheck(_salesDiscountProductRepository.UnitOfWork);
        }
        else
        {
            item.SalesDiscountProduct.Clear();
            _ = await CommitNoCheck(_salesDiscountProductRepository.UnitOfWork);
        }
        _repository.Update(item);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;
        return result;
        //return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(SalesDiscountDuplicateCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValidSalesDiscount(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = await _repository.GetById(request.SalesDiscountId);
        item.Id = request.Id;
        item.Code = request.Code;
        item.Status = 0;
        item.CreatedBy = createdBy;
        item.CreatedByName = createName;
        item.CreatedDate = createdDate;

        _repository.Add(item);

        var details = await _salesDiscountProductRepository.GetByParentId(request.SalesDiscountId);
        List<SalesDiscountProduct> list = new List<SalesDiscountProduct>();
        if (details?.Count() > 0)
        {
            foreach (SalesDiscountProduct x in details)
            {
                x.Id = Guid.NewGuid();
                x.SalesDiscountId = request.Id;
                x.Quantity = 0;
                list.Add(x);
            }
            _salesDiscountProductRepository.Add(list);
        }
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(SalesDiscountUploadFileCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);
        item.File = request.File;
        item.UpdatedBy = _context.GetUserId();
        item.UpdatedByName = _context.UserClaims.FullName;
        item.UpdatedDate = DateTime.Now;
        _repository.Update(item);

        return await Commit(_repository.UnitOfWork);
    }
    public async Task<ValidationResult> Handle(ManagePaymentSDCommand request, CancellationToken cancellationToken)
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
                    u.Code = _synctaxCodeRepository.GetCode("PTC", 1).Result;
                    listPaymentInvoiceAdd.Add(new PaymentInvoice()
                    {
                        Id = Guid.NewGuid(),
                        Type = u.Type,
                        Code = u.Code,
                        SaleDiscountId = request.Id,
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
