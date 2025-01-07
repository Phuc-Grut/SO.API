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

internal class PriceListCrossDetailCommandHandler : CommandHandler, IRequestHandler<PriceListCrossDetailAddCommand, ValidationResult>, IRequestHandler<PriceListCrossDetailDeleteCommand, ValidationResult>, IRequestHandler<PriceListCrossDetailEditCommand, ValidationResult>
{
    private readonly IPriceListCrossDetailRepository _repository;
    private readonly IContextUser _context;

    public PriceListCrossDetailCommandHandler(IPriceListCrossDetailRepository repository, IContextUser contextUser)
    {
        _repository = repository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(PriceListCrossDetailAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new PriceListCrossDetail
        {
            Id = request.Id,
            PriceListCrossId = request.PriceListCrossId,
            PriceListCross = request.PriceListCross,
            Note = request.Note,
            AirFreight = request.AirFreight,
            SeaFreight = request.SeaFreight,
            Currency = request.Currency,
            Status = request.Status,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PriceListCrossDetailDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new PriceListCrossDetail
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PriceListCrossDetailEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.Id = request.Id;
        item.PriceListCrossId = request.PriceListCrossId;
        item.PriceListCross = request.PriceListCross;
        item.Note = request.Note;
        item.AirFreight = request.AirFreight;
        item.SeaFreight = request.SeaFreight;
        item.Currency = request.Currency;
        item.Status = request.Status;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
