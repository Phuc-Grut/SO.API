using FluentValidation.Results;
using MediatR;
using VFi.Application.SO.Commands;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CRM.Commands;

internal class SalesChannelCommandHandler : CommandHandler,
                                    IRequestHandler<SalesChannelAddCommand, ValidationResult>,
                                    IRequestHandler<SalesChannelEditCommand, ValidationResult>,
                                    IRequestHandler<SalesChannelDeleteCommand, ValidationResult>
{
    private readonly ISalesChannelRepository _repository;
    private readonly IContextUser _context;

    public SalesChannelCommandHandler(ISalesChannelRepository salesChannelRepository, IContextUser contextUser)
    {
        _repository = salesChannelRepository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(SalesChannelAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new SalesChannel
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Status = request.Status,
            IsDefault = request.IsDefault,
            DisplayOrder = request.DisplayOrder,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(SalesChannelDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new SalesChannel
        {
            Id = request.Id,
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(SalesChannelEditCommand request, CancellationToken cancellationToken)
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
        item.IsDefault = request.IsDefault;
        item.DisplayOrder = request.DisplayOrder;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
