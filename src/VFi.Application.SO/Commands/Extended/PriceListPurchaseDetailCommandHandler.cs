using System.Net;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Consul;
using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class PriceListPurchaseDetailCommandHandler : CommandHandler, IRequestHandler<PriceListPurchaseDetailAddCommand, ValidationResult>, IRequestHandler<PriceListPurchaseDetailDeleteCommand, ValidationResult>, IRequestHandler<PriceListPurchaseDetailEditCommand, ValidationResult>
{
    private readonly IPriceListPurchaseDetailRepository _repository;
    private readonly IContextUser _context;

    public PriceListPurchaseDetailCommandHandler(IPriceListPurchaseDetailRepository repository, IContextUser contextUser)
    {
        _repository = repository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(PriceListPurchaseDetailAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new PriceListPurchaseDetail
        {

            Id = request.Id,
            PriceListPurchaseId = request.PriceListPurchaseId,
            PriceListPurchase = request.PriceListPurchase,
            Note = request.Note,
            PurchaseGroupId = request.PurchaseGroupId,
            PurchaseGroupName = request.PurchaseGroup,
            BuyFee = request.BuyFee,
            BuyFeeMin = request.BuyFeeMin,
            Currency = request.Currency,
            Status = request.Status,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PriceListPurchaseDetailDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new PriceListPurchaseDetail
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PriceListPurchaseDetailEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.Id = request.Id;
        item.PriceListPurchaseId = request.PriceListPurchaseId;
        item.PriceListPurchase = request.PriceListPurchase;
        item.Note = request.Note;
        item.PurchaseGroupId = request.PurchaseGroupId;
        item.PurchaseGroupCode = request.PurchaseGroup;
        item.PurchaseGroupName = request.PurchaseGroup;
        item.BuyFee = request.BuyFee;
        item.BuyFeeMin = request.BuyFeeMin;
        item.Currency = request.Currency;
        item.Status = request.Status;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
