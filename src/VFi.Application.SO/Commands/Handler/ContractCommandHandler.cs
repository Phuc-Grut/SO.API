using System.Reflection.Emit;
using FluentValidation.Results;
using MediatR;
using VFi.Application.SO.Commands;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CRM.Commands;

internal class ContractCommandHandler : CommandHandler,
                                    IRequestHandler<ContractAddCommand, ValidationResult>,
                                    IRequestHandler<ContractEditCommand, ValidationResult>,
                                    IRequestHandler<ContractDeleteCommand, ValidationResult>,
                                    IRequestHandler<ApprovalContractCommand, ValidationResult>,
                                    IRequestHandler<LiquidationContractCommand, ValidationResult>,
                                    IRequestHandler<ContractUploadFileCommand, ValidationResult>
{
    private readonly IContractRepository _repository;
    private readonly IOrderProductRepository _orderDetailRepository;
    private readonly IContextUser _context;

    public ContractCommandHandler(
        IContractRepository ContractRepository,
        IOrderProductRepository orderDetailRepository,
        IContextUser context)
    {
        _repository = ContractRepository;
        _orderDetailRepository = orderDetailRepository;
        _context = context;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(ContractAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createdBy = _context.GetUserId();
        var createdByname = _context.UserClaims.FullName;
        var createdDate = DateTime.Now;
        var Contract = new Contract
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            ContractTypeId = request.ContractTypeId,
            ContractTypeName = request.ContractTypeName,
            QuotationId = request.QuotationId,
            QuotationName = request.QuotationName,
            OrderId = request.OrderId,
            OrderCode = request.OrderCode,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            SignDate = request.SignDate,
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName,
            Country = request.Country,
            Province = request.Province,
            District = request.District,
            Ward = request.Ward,
            Address = request.Address,
            Currency = request.Currency,
            CurrencyName = request.CurrencyName,
            Calculation = request.Calculation,
            ExchangeRate = request.ExchangeRate,
            Status = request.Status,
            TypeDiscount = request.TypeDiscount,
            DiscountRate = request.DiscountRate,
            TypeCriteria = request.TypeCriteria,
            AmountDiscount = request.AmountDiscount,
            AccountName = request.AccountName,
            GroupEmployeeId = request.GroupEmployeeId,
            GroupEmployeeName = request.GroupEmployeeName,
            AccountId = request.AccountId,
            ContractTermId = request.ContractTermId,
            ContractTermName = request.ContractTermName,
            ContractTermContent = request.ContractTermContent,
            PaymentDueDate = request.PaymentDueDate,
            DeliveryDate = request.DeliveryDate,
            Buyer = request.Buyer,
            Saler = request.Saler,
            Description = request.Description,
            Note = request.Note,
            File = request.File,
            HasPreviousContract = request.HasPreviousContract,
            Paid = request.Paid,
            Received = request.Received,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createdByname
        };


        _repository.Add(Contract);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;
        List<OrderProduct> list = new List<OrderProduct>();
        if (request.OrderProduct?.Count > 0)
        {
            foreach (var u in request.OrderProduct)
            {
                list.Add(new OrderProduct()
                {
                    Id = Guid.NewGuid(),
                    ContractId = request.Id,
                    ContractName = request.Code,
                    //OrderId = u.OrderId,
                    //OrderCode = u.OrderCode,
                    //QuotationId = u.QuotationId,
                    //QuotationName = u.QuotationName,
                    ProductId = u.ProductId,
                    ProductCode = u.ProductCode,
                    ProductName = u.ProductName,
                    ProductImage = u.ProductImage,
                    Origin = u.Origin,
                    WarehouseId = u.WarehouseId,
                    WarehouseCode = u.WarehouseCode,
                    WarehouseName = u.WarehouseName,
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
            _orderDetailRepository.Add(list);
            await CommitNoCheck(_orderDetailRepository.UnitOfWork);
        }
        return result;
    }

    public async Task<ValidationResult> Handle(ContractDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var Contract = new Contract
        {
            Id = request.Id
        };

        _repository.Remove(Contract);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ContractEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;

        var obj = await _repository.GetById(request.Id);

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Contract is not exist") } };
        }
        var updatedBy = _context.GetUserId();
        var updatedByname = _context.UserClaims.FullName;
        var updatedDate = DateTime.Now;

        obj.Code = request.Code;
        obj.Name = request.Name;
        obj.ContractTypeId = request.ContractTypeId;
        obj.ContractTypeName = request.ContractTypeName;
        obj.QuotationId = request.QuotationId;
        obj.QuotationName = request.QuotationName;
        obj.OrderId = request.OrderId;
        obj.OrderCode = request.OrderCode;
        obj.StartDate = request.StartDate;
        obj.EndDate = request.EndDate;
        obj.SignDate = request.SignDate;
        obj.CustomerId = request.CustomerId;
        obj.CustomerCode = request.CustomerCode;
        obj.CustomerName = request.CustomerName;
        obj.Country = request.Country;
        obj.Province = request.Province;
        obj.District = request.District;
        obj.Ward = request.Ward;
        obj.Address = request.Address;
        obj.Currency = request.Currency;
        obj.CurrencyName = request.CurrencyName;
        obj.Calculation = request.Calculation;
        obj.ExchangeRate = request.ExchangeRate;
        obj.Status = request.Status;
        obj.TypeDiscount = request.TypeDiscount;
        obj.DiscountRate = request.DiscountRate;
        obj.TypeCriteria = request.TypeCriteria;
        obj.AmountDiscount = request.AmountDiscount;
        obj.AccountName = request.AccountName;
        obj.GroupEmployeeId = request.GroupEmployeeId;
        obj.GroupEmployeeName = request.GroupEmployeeName;
        obj.AccountId = request.AccountId;
        obj.ContractTermId = request.ContractTermId;
        obj.ContractTermName = request.ContractTermName;
        obj.ContractTermContent = request.ContractTermContent;
        obj.PaymentDueDate = request.PaymentDueDate;
        obj.DeliveryDate = request.DeliveryDate;
        obj.Buyer = request.Buyer;
        obj.Saler = request.Saler;
        obj.Description = request.Description;
        obj.Note = request.Note;
        obj.File = request.File;
        obj.HasPreviousContract = request.HasPreviousContract;
        obj.Paid = request.Paid;
        obj.Received = request.Received;
        obj.UpdatedBy = updatedBy;
        obj.UpdatedDate = updatedDate;
        obj.UpdatedByName = updatedByname;

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
                    //item.OrderId = u.OrderId;
                    //item.OrderCode = u.OrderCode;
                    //item.QuotationId = u.QuotationId;
                    //item.QuotationName = u.QuotationName;
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
                        ContractId = request.Id,
                        ContractName = request.Code,
                        //OrderId = u.OrderId,
                        //OrderCode = u.OrderCode,
                        //QuotationId = u.QuotationId,
                        //QuotationName = u.QuotationName,
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

            _orderDetailRepository.Update(listUpdate);
            _orderDetailRepository.Add(listAdd);
            _orderDetailRepository.Remove(listDelete);
            _ = await CommitNoCheck(_orderDetailRepository.UnitOfWork);
        }
        else
        {
            obj.OrderProduct.Clear();
        }
        return result;
    }
    public async Task<ValidationResult> Handle(ApprovalContractCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Contract is not exist") } };
        }
        var obj = new Contract
        {
            Id = request.Id,
            Status = request.Status,
            ApproveDate = DateTime.Now,
            ApproveBy = _context.GetUserId(),
            ApproveByName = _context.UserClaims.FullName,
            //ApproveComment = request.ApproveComment
        };
        _repository.Approve(obj);
        return await Commit(_repository.UnitOfWork);
    }
    public async Task<ValidationResult> Handle(LiquidationContractCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Contract is not exist") } };
        }

        item.AmountLiquidation = request.AmountLiquidation;
        item.LiquidationDate = request.LiquidationDate;
        item.LiquidationReason = request.LiquidationReason;
        item.Status = 2;
        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
    public async Task<ValidationResult> Handle(ContractUploadFileCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Contract is not exist") } };
        }
        var obj = new Contract
        {
            Id = request.Id,
            File = request.File
        };
        _repository.UploadFile(obj);
        return await Commit(_repository.UnitOfWork);
    }
}
