using System.Collections.Generic;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class PriceListCommandHandler : CommandHandler, IRequestHandler<AddPriceListCommand, ValidationResult>,
                                                         IRequestHandler<DeletePriceListCommand, ValidationResult>,
                                                         IRequestHandler<EditPriceListCommand, ValidationResult>,
                                                         IRequestHandler<PriceListSortCommand, ValidationResult>
{
    private readonly IPriceListRepository _repository;
    private readonly IPriceListDetailRepository _priceListDetailRepository;
    private readonly IContextUser _context;

    public PriceListCommandHandler(
        IPriceListRepository priceListRepository,
        IPriceListDetailRepository priceListDetailRepository,
        IContextUser context)
    {
        _repository = priceListRepository;
        _priceListDetailRepository = priceListDetailRepository;
        _context = context;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(AddPriceListCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createdBy = _context.GetUserId();
        var createdByname = _context.UserClaims.FullName;
        var createdDate = DateTime.Now;
        var item = new PriceList
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Status = request.Status,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Currency = request.Currency,
            CurrencyName = request.CurrencyName,
            DisplayOrder = request.DisplayOrder,
            CreatedDate = createdDate,
            CreatedBy = createdBy,
            CreatedByName = createdByname
        };
        _repository.Add(item);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        List<PriceListDetail> list = new List<PriceListDetail>();
        if (request.PriceListDetail?.Count > 0)
        {
            foreach (var u in request.PriceListDetail)
            {
                list.Add(new PriceListDetail()
                {
                    Id = Guid.NewGuid(),
                    PriceListId = request.Id,
                    ProductId = u.ProductId,
                    ProductCode = u.ProductCode,
                    ProductName = u.ProductName,
                    UnitType = u.UnitType,
                    UnitCode = u.UnitCode,
                    UnitName = u.UnitName,
                    CurrencyCode = u.CurrencyCode,
                    CurrencyName = u.CurrencyName,
                    QuantityMin = u.QuantityMin,
                    Type = u.Type,
                    FixPrice = u.FixPrice,
                    TypeDiscount = u.TypeDiscount,
                    DiscountRate = u.DiscountRate,
                    DiscountValue = u.DiscountValue,
                    DisplayOrder = u.DisplayOrder,
                    CreatedDate = createdDate,
                    CreatedBy = createdBy,
                    CreatedByName = createdByname
                });
            }
            _priceListDetailRepository.Add(list);
            _ = await CommitNoCheck(_priceListDetailRepository.UnitOfWork);
        }
        return result;
    }

    public async Task<ValidationResult> Handle(DeletePriceListCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new PriceList
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(EditPriceListCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var obj = await _repository.GetById(request.Id);

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "PriceList is not exist") } };
        }
        var updatedBy = _context.GetUserId();
        var updatedByname = _context.UserClaims.FullName;
        var updatedDate = DateTime.Now;

        obj.Code = request.Code;
        obj.Name = request.Name;
        obj.Description = request.Description;
        obj.Status = request.Status;
        obj.StartDate = request.StartDate;
        obj.EndDate = request.EndDate;
        obj.Currency = request.Currency;
        obj.CurrencyName = request.CurrencyName;
        obj.DisplayOrder = request.DisplayOrder;
        obj.UpdatedDate = updatedDate;
        obj.UpdatedBy = updatedBy;
        obj.UpdatedByName = updatedByname;
        _repository.Update(obj);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        List<PriceListDetail> listUpdate = new List<PriceListDetail>();
        List<PriceListDetail> listDelete = new List<PriceListDetail>();
        if (request.PriceListDetail.Any())
        {
            foreach (var item in obj.PriceListDetail)
            {
                var u = request.PriceListDetail.Where(x => x.Id == item.Id).FirstOrDefault();
                if (u != null)
                {
                    item.ProductId = u.ProductId;
                    item.ProductCode = u.ProductCode;
                    item.ProductName = u.ProductName;
                    item.UnitType = u.UnitType;
                    item.UnitCode = u.UnitCode;
                    item.UnitName = u.UnitName;
                    item.CurrencyCode = u.CurrencyCode;
                    item.CurrencyName = u.CurrencyName;
                    item.QuantityMin = u.QuantityMin;
                    item.Type = u.Type;
                    item.FixPrice = u.FixPrice;
                    item.TypeDiscount = u.TypeDiscount;
                    item.DiscountRate = u.DiscountRate;
                    item.DiscountValue = u.DiscountValue;
                    item.UpdatedBy = updatedBy;
                    item.UpdatedDate = updatedDate;
                    item.UpdatedByName = updatedByname;
                    item.DisplayOrder = u.DisplayOrder;
                    listUpdate.Add(item);
                    request.PriceListDetail.Remove(u);
                }
                else
                {
                    listDelete.Add(item);
                }
            }

            List<PriceListDetail> listAdd = new List<PriceListDetail>();
            for (int i = 0; i < request.PriceListDetail.Count; i++)
            {
                var u = request.PriceListDetail[i];
                var priceListDetail = obj.PriceListDetail.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (priceListDetail is null)
                {
                    listAdd.Add(new PriceListDetail()
                    {
                        Id = Guid.NewGuid(),
                        PriceListId = request.Id,
                        ProductId = u.ProductId,
                        ProductCode = u.ProductCode,
                        ProductName = u.ProductName,
                        UnitType = u.UnitType,
                        UnitCode = u.UnitCode,
                        UnitName = u.UnitName,
                        CurrencyCode = u.CurrencyCode,
                        CurrencyName = u.CurrencyName,
                        QuantityMin = u.QuantityMin,
                        Type = u.Type,
                        FixPrice = u.FixPrice,
                        TypeDiscount = u.TypeDiscount,
                        DiscountRate = u.DiscountRate,
                        DiscountValue = u.DiscountValue,
                        DisplayOrder = u.DisplayOrder,
                        CreatedBy = updatedBy,
                        CreatedDate = updatedDate,
                        CreatedByName = updatedByname
                    });

                    request.PriceListDetail.Remove(request.PriceListDetail[i]);
                    i--;
                }
            }

            _priceListDetailRepository.Update(listUpdate);
            _priceListDetailRepository.Add(listAdd);
            _priceListDetailRepository.Remove(listDelete);
            _ = await CommitNoCheck(_priceListDetailRepository.UnitOfWork);
        }
        else if (obj.PriceListDetail.Any())
        {
            foreach (var item in obj.PriceListDetail)
            {
                listDelete.Add(item);
            }
            _priceListDetailRepository.Remove(listDelete);
            _ = await CommitNoCheck(_priceListDetailRepository.UnitOfWork);
        }
        return result;
    }

    public async Task<ValidationResult> Handle(PriceListSortCommand request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetAll();
        List<PriceList> list = new List<PriceList>();

        foreach (var sort in request.SortList)
        {
            PriceList obj = data.FirstOrDefault(c => c.Id == sort.Id);
            if (obj != null)
            {
                obj.DisplayOrder = sort.SortOrder;
                list.Add(obj);
            }
        }
        _repository.Update(list);
        return await Commit(_repository.UnitOfWork);
    }
}
