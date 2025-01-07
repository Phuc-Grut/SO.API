using System.Collections.Generic;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class PriceListPurchaseCommandHandler : CommandHandler, IRequestHandler<PriceListPurchaseAddCommand, ValidationResult>,
                                                                 IRequestHandler<PriceListPurchaseDeleteCommand, ValidationResult>,
                                                                 IRequestHandler<PriceListPurchaseEditCommand, ValidationResult>,
                                                                 IRequestHandler<PriceListPurchaseSortCommand, ValidationResult>
{
    private readonly IPriceListPurchaseRepository _repository;
    private readonly IPriceListPurchaseDetailRepository _priceListPurchaseDetailRepository;
    private readonly IContextUser _context;

    public PriceListPurchaseCommandHandler(IPriceListPurchaseRepository PriceListPurchaseRepository, IPriceListPurchaseDetailRepository PriceListPurchaseDetailRepository, IContextUser context)
    {
        _repository = PriceListPurchaseRepository;
        _priceListPurchaseDetailRepository = PriceListPurchaseDetailRepository;
        _context = context;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(PriceListPurchaseAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var PriceListPurchase = new PriceListPurchase
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            DisplayOrder = request.DisplayOrder,
            Default = false,
            Status = request.Status,
            CreatedDate = DateTime.Now,
            CreatedBy = _context.GetUserId(),
            CreatedByName = _context.UserName,
            PriceListPurchaseDetail = request.Detail.Select(u => new PriceListPurchaseDetail()
            {
                Id = Guid.NewGuid(),
                BuyFeeMin = u.BuyFeeMin,
                PurchaseGroupId = u.PurchaseGroupId,
                PurchaseGroupCode = u.PurchaseGroupCode,
                PurchaseGroupName = u.PurchaseGroupName,
                PriceListPurchase = request.Name,
                PriceListPurchaseId = request.Id,
                BuyFee = u.BuyFee,
                Currency = u.Currency,
                BuyFeeFix = u.BuyFeeFix,
                Note = u.Note,
                Status = u.Status,
                DisplayOrder = u.DisplayOrder,
                CreatedDate = DateTime.Now,
                CreatedBy = _context.GetUserId(),
                CreatedByName = _context.UserName,
            }).ToList(),
        };
        _repository.Add(PriceListPurchase);

        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PriceListPurchaseDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var PriceListPurchase = new PriceListPurchase
        {
            Id = request.Id
        };

        _repository.Remove(PriceListPurchase);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PriceListPurchaseEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var obj = await _repository.GetById(request.Id);

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("PriceListPurchase", "is not exist") } };
        }

        if (request.Detail.Any())
        {
            var listUpdate = new List<PriceListPurchaseDetail>();
            var listDelete = new List<PriceListPurchaseDetail>();
            foreach (var item in obj.PriceListPurchaseDetail)
            {
                var detail = request.Detail.FirstOrDefault(x => x.Id.Equals(item.Id));
                if (detail is not null)
                {
                    item.BuyFeeMin = detail.BuyFeeMin;
                    item.BuyFeeFix = detail.BuyFeeFix;
                    item.BuyFee = detail.BuyFee;
                    item.PurchaseGroupId = detail.PurchaseGroupId;
                    item.PurchaseGroupCode = detail.PurchaseGroupCode;
                    item.PurchaseGroupName = detail.PurchaseGroupName;
                    item.Currency = detail.Currency;
                    item.Status = detail.Status;
                    item.DisplayOrder = detail.DisplayOrder;
                    item.Note = detail.Note;
                    item.UpdatedDate = DateTime.Now;
                    item.UpdatedBy = _context.GetUserId();
                    item.UpdatedByName = _context.UserName;
                    listUpdate.Add(item);
                    request.Detail.Remove(detail);
                }
                else
                {
                    listDelete.Add(item);
                }
            }

            var listAdd = new List<PriceListPurchaseDetail>();
            for (int i = 0; i < request.Detail.Count; i++)
            {
                var detail = obj.PriceListPurchaseDetail.FirstOrDefault(x => x.Id.Equals(request.Detail[i].Id));
                if (detail is null)
                {
                    listAdd.Add(new PriceListPurchaseDetail()
                    {
                        Id = Guid.NewGuid(),
                        BuyFeeMin = request.Detail[i].BuyFeeMin,
                        PurchaseGroupId = request.Detail[i].PurchaseGroupId,
                        PurchaseGroupCode = request.Detail[i].PurchaseGroupCode,
                        PurchaseGroupName = request.Detail[i].PurchaseGroupName,
                        PriceListPurchase = request.Name,
                        PriceListPurchaseId = request.Id,
                        BuyFee = request.Detail[i].BuyFee,
                        Currency = request.Detail[i].Currency,
                        BuyFeeFix = request.Detail[i].BuyFeeFix,
                        Note = request.Detail[i].Note,
                        Status = request.Detail[i].Status,
                        DisplayOrder = request.Detail[i].DisplayOrder,
                        CreatedDate = DateTime.Now,
                        CreatedBy = _context.GetUserId(),
                        CreatedByName = _context.UserName,
                    });
                }
            }

            obj.PriceListPurchaseDetail = listUpdate;
            if (listDelete.Count > 0 || listAdd.Count > 0)
            {
                _priceListPurchaseDetailRepository.Remove(listDelete);
                _priceListPurchaseDetailRepository.Add(listAdd);
                await Commit(_priceListPurchaseDetailRepository.UnitOfWork);
            }
        }
        else
        {
            obj.PriceListPurchaseDetail.Clear();
        }

        obj.Code = request.Code;
        obj.Name = request.Name;
        obj.Description = request.Description;
        obj.Status = request.Status;
        obj.UpdatedDate = DateTime.Now;

        _repository.Update(obj);

        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PriceListPurchaseSortCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<PriceListPurchase> list = request.SortList.Select(x => new PriceListPurchase()
        {
            Id = x.Id,
            DisplayOrder = x.SortOrder
        });
        _repository.Sort(list);
        return await Commit(_repository.UnitOfWork);
    }
}
