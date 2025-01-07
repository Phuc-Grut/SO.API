using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class DeliveryMethodCommandHandler : CommandHandler, IRequestHandler<DeliveryMethodAddCommand, ValidationResult>, IRequestHandler<DeliveryMethodDeleteCommand, ValidationResult>, IRequestHandler<DeliveryMethodEditCommand, ValidationResult>
{
    private readonly IDeliveryMethodRepository _repository;
    private readonly IContextUser _context;

    public DeliveryMethodCommandHandler(IDeliveryMethodRepository DeliveryMethodRepository, IContextUser contextUser)
    {
        _repository = DeliveryMethodRepository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(DeliveryMethodAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new DeliveryMethod
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Status = request.Status,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(DeliveryMethodDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new DeliveryMethod
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(DeliveryMethodEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.Code = request.Code;
        item.Name = request.Name;
        item.Description = request.Description;
        item.Status = request.Status;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;


        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
