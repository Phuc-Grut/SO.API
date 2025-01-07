using System.Net;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Consul;
using FluentValidation.Results;
using MediatR;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class PriceListCrossCommandHandler : CommandHandler,
    IRequestHandler<PriceListCrossAddCommand, ValidationResult>,
    IRequestHandler<PriceListCrossDeleteCommand, ValidationResult>,
    IRequestHandler<PriceListCrossSortCommand, ValidationResult>,
    IRequestHandler<PriceListCrossEditCommand, ValidationResult>
{
    private readonly IPriceListCrossRepository _repository;
    private readonly IPriceListCrossDetailRepository _priceListCrossDetailRepository;
    private readonly IContextUser _context;

    public PriceListCrossCommandHandler(IPriceListCrossRepository repository, IContextUser contextUser, IPriceListCrossDetailRepository priceListCrossDetailRepository)
    {
        _repository = repository;
        _context = contextUser;
        _priceListCrossDetailRepository = priceListCrossDetailRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(PriceListCrossAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new PriceListCross
        {

            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Default = request.Default,
            DisplayOrder = request.DisplayOrder,
            Status = request.Status,
            RouterShippingId = request.RouterShippingId,
            RouterShipping = request.RouterShipping,
            PriceListCrossDetail = request.Detail.Select(x => new PriceListCrossDetail()
            {
                Id = Guid.NewGuid(),
                PriceListCrossId = request.Id,
                PriceListCross = request.Name,
                Note = x.Note,
                CommodityGroupId = x.CommodityGroupId,
                CommodityGroupCode = x.CommodityGroupCode,
                CommodityGroupName = x.CommodityGroupName,
                AirFreight = x.AirFreight,
                SeaFreight = x.SeaFreight,
                Currency = x.Currency,
                Status = x.Status,
                DisplayOrder = x.DisplayOrder,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                CreatedByName = x.CreatedByName,
                UpdatedByName = x.UpdatedByName

            }).ToList(),
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PriceListCrossDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new PriceListCross
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PriceListCrossEditCommand request, CancellationToken cancellationToken)
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
            var listUpdate = new List<PriceListCrossDetail>();
            var listDelete = new List<PriceListCrossDetail>();
            foreach (var item in obj.PriceListCrossDetail)
            {
                var detail = request.Detail.FirstOrDefault(x => x.Id.Equals(item.Id));
                if (detail is not null)
                {
                    item.PriceListCrossId = request.Id;
                    item.PriceListCross = request.Name;
                    item.Note = detail.Note;
                    item.CommodityGroupId = detail.CommodityGroupId;
                    item.CommodityGroupCode = detail.CommodityGroupCode;
                    item.CommodityGroupName = detail.CommodityGroupName;
                    item.AirFreight = detail.AirFreight;
                    item.SeaFreight = detail.SeaFreight;
                    item.Currency = detail.Currency;
                    item.Status = request.Status;
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

            var listAdd = new List<PriceListCrossDetail>();
            for (int i = 0; i < request.Detail.Count; i++)
            {
                var detail = obj.PriceListCrossDetail.FirstOrDefault(x => x.Id.Equals(request.Detail[i].Id));
                if (detail is null)
                {
                    listAdd.Add(new PriceListCrossDetail()
                    {
                        Id = Guid.NewGuid(),
                        PriceListCrossId = request.Id,
                        PriceListCross = request.Name,
                        CommodityGroupId = request.Detail[i].CommodityGroupId,
                        CommodityGroupCode = request.Detail[i].CommodityGroupCode,
                        CommodityGroupName = request.Detail[i].CommodityGroupName,
                        AirFreight = request.Detail[i].AirFreight,
                        SeaFreight = request.Detail[i].SeaFreight,
                        Currency = request.Detail[i].Currency,
                        Note = request.Detail[i].Note,
                        Status = request.Detail[i].Status,
                        DisplayOrder = request.Detail[i].DisplayOrder,
                        CreatedDate = DateTime.Now,
                        CreatedBy = _context.GetUserId(),
                        CreatedByName = _context.UserName,
                    });
                }
            }

            obj.PriceListCrossDetail = listUpdate;

            if (listDelete.Count > 0 || listAdd.Count > 0)
            {
                _priceListCrossDetailRepository.Remove(listDelete);
                _priceListCrossDetailRepository.Add(listAdd);
                await Commit(_priceListCrossDetailRepository.UnitOfWork);
            }
        }
        else
        {
            obj.PriceListCrossDetail.Clear();
        }
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;

        obj.Id = request.Id;
        obj.Code = request.Code;
        obj.Name = request.Name;
        obj.Description = request.Description;
        obj.Default = request.Default;
        obj.DisplayOrder = request.DisplayOrder;
        obj.Status = request.Status;
        obj.RouterShipping = request.RouterShipping;
        obj.RouterShippingId = request.RouterShippingId;
        obj.UpdatedDate = updatedDate;
        obj.UpdatedBy = updatedBy;
        obj.UpdatedByName = updateName;

        _repository.Update(obj);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PriceListCrossSortCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<PriceListCross> list = request.SortList.Select(x => new PriceListCross()
        {
            Id = x.Id,
            DisplayOrder = x.SortOrder
        });
        _repository.Sort(list);
        return await Commit(_repository.UnitOfWork);
    }
}
