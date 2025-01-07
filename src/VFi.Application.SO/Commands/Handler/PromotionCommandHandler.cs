using System.Collections.Generic;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class PromotionCommandHandler : CommandHandler, IRequestHandler<AddPromotionCommand, ValidationResult>,
                                                         IRequestHandler<DeletePromotionCommand, ValidationResult>,
                                                         IRequestHandler<EditPromotionCommand, ValidationResult>
{
    private readonly IPromotionRepository _repository;
    private readonly IPromotionByValueRepository _detailrepository;
    private readonly IPromotionCustomerRepository _promotionCustomerRepository;
    private readonly IPromotionCustomerGroupRepository _promotionCustomerGroupRepository;
    private readonly IPromotionProductRepository _promotionProductRepository;
    private readonly IPromotionProductBuyRepository _promotionProductBuyRepository;

    public PromotionCommandHandler(
                                    IPromotionRepository respository,
                                    IPromotionByValueRepository detailrepository,
                                    IPromotionCustomerRepository promotionCustomerRepository,
                                    IPromotionCustomerGroupRepository promotionCustomerGroupRepository,
                                    IPromotionProductRepository promotionProductRepository,
                                    IPromotionProductBuyRepository promotionProductBuyRepository
        )
    {
        _repository = respository;
        _detailrepository = detailrepository;
        _promotionCustomerRepository = promotionCustomerRepository;
        _promotionCustomerGroupRepository = promotionCustomerGroupRepository;
        _promotionProductRepository = promotionProductRepository;
        _promotionProductBuyRepository = promotionProductBuyRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(AddPromotionCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var promotion = new Promotion
        {
            Id = request.Id,
            PromotionGroupId = request.PromotionGroupId,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Status = request.Status,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Stores = request.Stores,
            SalesChannel = request.SalesChannel,
            ApplyTogether = request.ApplyTogether,
            ApplyAllCustomer = request.ApplyAllCustomer,
            Type = request.Type,
            PromotionMethod = request.PromotionMethod,
            UsingCode = request.UsingCode,
            ApplyBirthday = request.ApplyBirthday,
            PromotionalCode = request.PromotionalCode,
            IsLimit = request.IsLimit,
            PromotionLimit = request.PromotionLimit,
            Applytax = request.Applytax,
            DisplayType = request.DisplayType,
            PromotionBase = request.PromotionBase,
            ObjectApply = request.ObjectApply,
            Condition = request.Condition,
            Apply = request.Apply,
            CreatedDate = DateTime.Now,
            CreatedBy = request.CreatedBy,
            CreatedByName = request.CreatedByName
        };
        _repository.Add(promotion);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;
        //group
        List<PromotionCustomerGroup> listGroup = new List<PromotionCustomerGroup>();
        if (!String.IsNullOrEmpty(request.CustomerGroups))
        {
            var data = request.CustomerGroups.Split(',').ToList();
            foreach (string item in data)
            {
                listGroup.Add(new PromotionCustomerGroup()
                {
                    Id = Guid.NewGuid(),
                    PromotionId = request.Id,
                    CustomerGroupId = new Guid(item)
                });
            }
            if (listGroup.Count > 0)
            {
                _promotionCustomerGroupRepository.Add(listGroup);
                _ = await CommitNoCheck(_promotionCustomerGroupRepository.UnitOfWork);
            }

        }

        //customer
        List<PromotionCustomer> listCustomer = new List<PromotionCustomer>();
        if (!String.IsNullOrEmpty(request.Customers))
        {
            var data = request.Customers.Split(',').ToList();
            foreach (string item in data)
            {
                listCustomer.Add(new PromotionCustomer()
                {
                    Id = Guid.NewGuid(),
                    PromotionId = request.Id,
                    CustomerId = new Guid(item)
                });
            }
            if (listCustomer.Count > 0)
            {
                _promotionCustomerRepository.Add(listCustomer);
                _ = await CommitNoCheck(_promotionCustomerRepository.UnitOfWork);
            }
        }

        List<PromotionByValue> list = new List<PromotionByValue>();
        List<PromotionProduct> listBonus = new List<PromotionProduct>();
        List<PromotionProductBuy> listBuy = new List<PromotionProductBuy>();
        if (request.Detail?.Count > 0)
        {
            foreach (var u in request.Detail)
            {
                u.Id = Guid.NewGuid();
                list.Add(new PromotionByValue()
                {
                    Id = (Guid)u.Id,
                    PromotionId = u.PromotionId,
                    ProductId = u.ProductId,
                    ProductCode = u.ProductCode,
                    ProductName = u.ProductName,
                    Type = u.Type,
                    MinOrderPrice = u.MinOrderPrice,
                    LimitTotalValue = u.LimitTotalValue,
                    DiscountPercent = u.DiscountPercent,
                    ReduceAmount = u.ReduceAmount,
                    FixPrice = u.FixPrice,
                    TypeBonus = u.TypeBonus,
                    TypeBuy = u.TypeBuy,
                    Quantity = u.Quantity,
                    QuantityBuy = u.QuantityBuy,
                    CreatedDate = DateTime.Now,
                    CreatedBy = u.CreatedBy,
                    CreatedByName = u.CreatedByName
                });
                if (u.ProductBonus?.Count > 0)
                {
                    foreach (var i in u.ProductBonus)
                    {
                        listBonus.Add(new PromotionProduct()
                        {
                            Id = Guid.NewGuid(),
                            PromotionByValueId = (Guid)u.Id,
                            ProductId = i.ProductId,
                            ProductCode = i.ProductCode,
                            ProductName = i.ProductName,
                            Quantity = i.Quantity,
                            CreatedDate = DateTime.Now,
                            CreatedBy = u.CreatedBy,
                            CreatedByName = u.CreatedByName
                        });
                    }
                }
                if (u.ProductBuy?.Count > 0)
                {
                    foreach (var i in u.ProductBuy)
                    {
                        listBuy.Add(new PromotionProductBuy()
                        {
                            Id = Guid.NewGuid(),
                            PromotionByValueId = (Guid)u.Id,
                            ProductId = i.ProductId,
                            ProductCode = i.ProductCode,
                            ProductName = i.ProductName,
                            Quantity = i.Quantity,
                            CreatedDate = DateTime.Now,
                            CreatedBy = u.CreatedBy,
                            CreatedByName = u.CreatedByName
                        });
                    }
                }
            }
            _detailrepository.Add(list);
            _ = await CommitNoCheck(_detailrepository.UnitOfWork);
            if (listBonus.Count > 0)
            {
                _promotionProductRepository.Add(listBonus);
                _ = await CommitNoCheck(_promotionProductRepository.UnitOfWork);
            }
            if (listBonus.Count > 0)
            {
                _promotionProductBuyRepository.Add(listBuy);
                _ = await CommitNoCheck(_promotionProductBuyRepository.UnitOfWork);
            }
        }

        return result;
    }

    public async Task<ValidationResult> Handle(DeletePromotionCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var item = new Promotion
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(EditPromotionCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var obj = await _repository.GetById(request.Id);

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Promotion is not exist") } };
        }
        obj.PromotionGroupId = request.PromotionGroupId;
        obj.Code = request.Code;
        obj.Name = request.Name;
        obj.Description = request.Description;
        obj.Status = request.Status;
        obj.StartDate = request.StartDate;
        obj.EndDate = request.EndDate;
        obj.StartTime = request.StartTime;
        obj.EndTime = request.EndTime;
        obj.Stores = request.Stores;
        obj.SalesChannel = request.SalesChannel;
        obj.ApplyTogether = request.ApplyTogether;
        obj.ApplyAllCustomer = request.ApplyAllCustomer;
        obj.Type = request.Type;
        obj.PromotionMethod = request.PromotionMethod;
        obj.UsingCode = request.UsingCode;
        obj.ApplyBirthday = request.ApplyBirthday;
        obj.PromotionalCode = request.PromotionalCode;
        obj.IsLimit = request.IsLimit;
        obj.PromotionLimit = request.PromotionLimit;
        obj.Applytax = request.Applytax;
        obj.DisplayType = request.DisplayType;
        obj.PromotionBase = request.PromotionBase;
        obj.ObjectApply = request.ObjectApply;
        obj.Condition = request.Condition;
        obj.Apply = request.Apply;
        obj.UpdatedDate = DateTime.Now;
        obj.UpdatedBy = request.UpdatedBy;
        obj.UpdatedByName = request.UpdatedByName;

        _repository.Update(obj);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        //groups
        var dataGroup = await _promotionCustomerGroupRepository.Filter(request.Id);
        _promotionCustomerGroupRepository.Remove(dataGroup);
        _ = await CommitNoCheck(_promotionCustomerGroupRepository.UnitOfWork);
        List<PromotionCustomerGroup> listGroup = new List<PromotionCustomerGroup>();
        if (!String.IsNullOrEmpty(request.CustomerGroups))
        {
            var data = request.CustomerGroups.Split(',').ToList();
            foreach (string item in data)
            {
                listGroup.Add(new PromotionCustomerGroup()
                {
                    Id = Guid.NewGuid(),
                    PromotionId = request.Id,
                    CustomerGroupId = new Guid(item)
                });
            }
            if (listGroup.Count > 0)
            {
                _promotionCustomerGroupRepository.Add(listGroup);
                _ = await CommitNoCheck(_promotionCustomerGroupRepository.UnitOfWork);
            }
        }
        //customer
        var dataCustomer = await _promotionCustomerRepository.Filter(request.Id);
        _promotionCustomerRepository.Remove(dataCustomer);
        _ = await CommitNoCheck(_promotionCustomerRepository.UnitOfWork);
        List<PromotionCustomer> listCustomer = new List<PromotionCustomer>();
        if (!String.IsNullOrEmpty(request.Customers))
        {
            var data = request.Customers.Split(',').ToList();
            foreach (string item in data)
            {
                listCustomer.Add(new PromotionCustomer()
                {
                    Id = Guid.NewGuid(),
                    PromotionId = request.Id,
                    CustomerId = new Guid(item)
                });
            }
            if (listCustomer.Count > 0)
            {
                _promotionCustomerRepository.Add(listCustomer);
                _ = await CommitNoCheck(_promotionCustomerRepository.UnitOfWork);
            }
        }

        //chi tiết
        if (request.Delete?.Count > 0)
        {
            foreach (var d in request.Delete)
            {
                var item = await _detailrepository.GetById(d.Id);
                if (item is not null)
                {
                    _detailrepository.Remove(item);
                }

            }
            _ = await CommitNoCheck(_detailrepository.UnitOfWork);
        }

        //chi tiết tặng
        if (request.DeleteBonus?.Count > 0)
        {
            foreach (var d in request.DeleteBonus)
            {
                var item = await _promotionProductRepository.GetById(d.Id);
                if (item is not null)
                {
                    _promotionProductRepository.Remove(item);
                }
            }
            _ = await CommitNoCheck(_promotionProductRepository.UnitOfWork);
        }
        //chi tiết sp mua
        if (request.DeleteBuy?.Count > 0)
        {
            foreach (var d in request.DeleteBuy)
            {
                var item = await _promotionProductBuyRepository.GetById(d.Id);
                if (item is not null)
                {
                    _promotionProductBuyRepository.Remove(item);
                }
            }
            _ = await CommitNoCheck(_promotionProductBuyRepository.UnitOfWork);
        }
        List<PromotionByValue> listAdd = new List<PromotionByValue>();
        List<PromotionByValue> listUpdate = new List<PromotionByValue>();
        List<PromotionProduct> listAddBonus = new List<PromotionProduct>();
        List<PromotionProduct> listupdateBonus = new List<PromotionProduct>();
        List<PromotionProductBuy> listAddBuy = new List<PromotionProductBuy>();
        List<PromotionProductBuy> listupdateBuy = new List<PromotionProductBuy>();
        if (request.Detail?.Count > 0)
        {
            foreach (var u in request.Detail)
            {
                var item = obj.PromotionByValue.Where(x => x.Id == u.Id).FirstOrDefault();
                if (item != null)
                {
                    item.ProductId = u.ProductId;
                    item.ProductCode = u.ProductCode;
                    item.ProductName = u.ProductName;
                    item.Type = u.Type;
                    item.MinOrderPrice = u.MinOrderPrice;
                    item.LimitTotalValue = u.LimitTotalValue;
                    item.DiscountPercent = u.DiscountPercent;
                    item.ReduceAmount = u.ReduceAmount;
                    item.FixPrice = u.FixPrice;
                    item.TypeBonus = u.TypeBonus;
                    item.TypeBuy = u.TypeBuy;
                    item.Quantity = u.Quantity;
                    item.QuantityBuy = u.QuantityBuy;
                    item.UpdatedDate = DateTime.Now;
                    item.UpdatedBy = u.UpdatedBy;
                    item.UpdatedByName = u.UpdatedByName;

                    listUpdate.Add(item);

                    if (item.PromotionProduct?.Count > 0)
                    {
                        foreach (var i in item.PromotionProduct)
                        {
                            var _product = item.PromotionProduct.Where(x => x.Id == i.Id).FirstOrDefault();
                            if (_product != null)
                            {
                                _product.ProductId = i.ProductId;
                                _product.ProductCode = i.ProductCode;
                                _product.ProductName = i.ProductName;
                                _product.UpdatedDate = DateTime.Now;
                                _product.UpdatedBy = item.UpdatedBy;
                                _product.UpdatedByName = item.UpdatedByName;

                                listupdateBonus.Add(_product);
                            }
                            else
                            {
                                listAddBonus.Add(new PromotionProduct()
                                {
                                    Id = Guid.NewGuid(),
                                    PromotionByValueId = item.Id,
                                    ProductId = i.ProductId,
                                    ProductCode = i.ProductCode,
                                    ProductName = i.ProductName,
                                    Quantity = i.Quantity,
                                    CreatedDate = DateTime.Now,
                                    CreatedBy = u.CreatedBy,
                                    CreatedByName = u.CreatedByName
                                });
                            }
                        }
                    }
                    if (item.PromotionProductBuy?.Count > 0)
                    {
                        foreach (var i in item.PromotionProductBuy)
                        {
                            var _product = item.PromotionProductBuy.Where(x => x.Id == i.Id).FirstOrDefault();
                            if (_product != null)
                            {
                                _product.ProductId = i.ProductId;
                                _product.ProductCode = i.ProductCode;
                                _product.ProductName = i.ProductName;
                                _product.UpdatedDate = DateTime.Now;
                                _product.UpdatedBy = item.UpdatedBy;
                                _product.UpdatedByName = item.UpdatedByName;

                                listupdateBuy.Add(_product);
                            }
                            else
                            {
                                listAddBuy.Add(new PromotionProductBuy()
                                {
                                    Id = Guid.NewGuid(),
                                    PromotionByValueId = item.Id,
                                    ProductId = i.ProductId,
                                    ProductCode = i.ProductCode,
                                    ProductName = i.ProductName,
                                    Quantity = i.Quantity,
                                    CreatedDate = DateTime.Now,
                                    CreatedBy = u.CreatedBy,
                                    CreatedByName = u.CreatedByName
                                });
                            }
                        }
                    }
                }
                else
                {
                    u.Id = Guid.NewGuid();
                    listAdd.Add(new PromotionByValue()
                    {
                        Id = (Guid)u.Id,
                        PromotionId = u.PromotionId,
                        ProductId = u.ProductId,
                        ProductCode = u.ProductCode,
                        ProductName = u.ProductName,
                        Type = u.Type,
                        MinOrderPrice = u.MinOrderPrice,
                        LimitTotalValue = u.LimitTotalValue,
                        DiscountPercent = u.DiscountPercent,
                        ReduceAmount = u.ReduceAmount,
                        FixPrice = u.FixPrice,
                        TypeBonus = u.TypeBonus,
                        TypeBuy = u.TypeBuy,
                        Quantity = u.Quantity,
                        QuantityBuy = u.QuantityBuy,
                        CreatedDate = DateTime.Now,
                        CreatedBy = u.CreatedBy,
                        CreatedByName = u.CreatedByName
                    });
                    if (u.ProductBonus?.Count > 0)
                    {
                        foreach (var i in u.ProductBonus)
                        {
                            listAddBonus.Add(new PromotionProduct()
                            {
                                Id = Guid.NewGuid(),
                                PromotionByValueId = (Guid)u.Id,
                                ProductId = i.ProductId,
                                ProductCode = i.ProductCode,
                                ProductName = i.ProductName,
                                Quantity = i.Quantity,
                                CreatedDate = DateTime.Now,
                                CreatedBy = u.CreatedBy,
                                CreatedByName = u.CreatedByName
                            });
                        }
                    }
                    if (u.ProductBuy?.Count > 0)
                    {
                        foreach (var i in u.ProductBuy)
                        {
                            listAddBuy.Add(new PromotionProductBuy()
                            {
                                Id = Guid.NewGuid(),
                                PromotionByValueId = (Guid)u.Id,
                                ProductId = i.ProductId,
                                ProductCode = i.ProductCode,
                                ProductName = i.ProductName,
                                Quantity = i.Quantity,
                                CreatedDate = DateTime.Now,
                                CreatedBy = u.CreatedBy,
                                CreatedByName = u.CreatedByName
                            });
                        }
                    }
                }
            }
            if (listAdd.Count > 0)
            {
                _detailrepository.Add(listAdd);
                _ = await CommitNoCheck(_detailrepository.UnitOfWork);
            }
            if (listUpdate.Count > 0)
            {
                _detailrepository.Update(listUpdate);
                _ = await CommitNoCheck(_detailrepository.UnitOfWork);
            }
            if (listAddBonus.Count > 0)
            {
                _promotionProductRepository.Add(listAddBonus);
                _ = await CommitNoCheck(_promotionProductRepository.UnitOfWork);
            }
            if (listupdateBonus.Count > 0)
            {
                _promotionProductRepository.Update(listupdateBonus);
                _ = await CommitNoCheck(_promotionProductRepository.UnitOfWork);
            }
            if (listAddBuy.Count > 0)
            {
                _promotionProductBuyRepository.Add(listAddBuy);
                _ = await CommitNoCheck(_promotionProductBuyRepository.UnitOfWork);
            }
            if (listupdateBuy.Count > 0)
            {
                _promotionProductBuyRepository.Update(listupdateBuy);
                _ = await CommitNoCheck(_promotionProductBuyRepository.UnitOfWork);
            }
        }

        return result;
    }
}
