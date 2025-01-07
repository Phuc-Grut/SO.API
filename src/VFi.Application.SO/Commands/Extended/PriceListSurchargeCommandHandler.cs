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

internal class PriceListSurchargeCommandHandler : CommandHandler, IRequestHandler<PriceListSurchargeAddCommand, ValidationResult>,
    IRequestHandler<PriceListSurchargeDeleteCommand, ValidationResult>,
    IRequestHandler<PriceListSurchargeSortCommand, ValidationResult>,
    IRequestHandler<PriceListSurchargeEditCommand, ValidationResult>
{
    private readonly IPriceListSurchargeRepository _repository;
    private readonly IContextUser _context;

    public PriceListSurchargeCommandHandler(IPriceListSurchargeRepository repository, IContextUser contextUser)
    {
        _repository = repository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(PriceListSurchargeAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;

        if (request.Details.Any())
        {
            var datas = request.Details.Select(x => new PriceListSurcharge()
            {
                Id = Guid.NewGuid(),
                Currency = x.Currency,
                Note = x.Note,
                Price = x.Price,
                RouterShipping = request.RouterShipping,
                RouterShippingId = request.RouterShippingId,
                Status = x.Status,
                SurchargeGroup = x.SurchargeGroup,
                SurchargeGroupId = x.SurchargeGroupId,
                CreatedBy = createdBy,
                CreatedByName = createName,
                CreatedDate = createdDate
            });
            _repository.Add(datas);
            return await Commit(_repository.UnitOfWork);
        }
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PriceListSurchargeDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = await _repository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Router shipping is not exist") } };
        }

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PriceListSurchargeEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;

        var item = await _repository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Router shipping is not exist") } };
        }

        item.Status = request.Status;
        item.RouterShippingId = request.RouterShippingId;
        item.RouterShipping = request.RouterShipping;
        item.SurchargeGroup = request.SurchargeGroup;
        item.Currency = request.Currency;
        item.Note = request.Note;
        item.Price = request.Price;
        item.SurchargeGroupId = request.SurchargeGroupId;
        item.UpdatedByName = _context.UserClaims.FullName;
        item.UpdatedBy = _context.GetUserId();
        item.UpdatedDate = DateTime.Now;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PriceListSurchargeSortCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<PriceListSurcharge> list = request.SortList.Select(x => new PriceListSurcharge()
        {
            Id = x.Id,
            DisplayOrder = x.SortOrder
        });
        _repository.Sort(list);
        return await Commit(_repository.UnitOfWork);
    }
}
