using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class SurchargeGroupCommandHandler : CommandHandler,
    IRequestHandler<SurchargeGroupAddCommand, ValidationResult>,
    IRequestHandler<SurchargeGroupDeleteCommand, ValidationResult>,
    IRequestHandler<SurchargeGroupEditCommand, ValidationResult>,
    IRequestHandler<SurchargeGroupSortCommand, ValidationResult>
{
    private readonly ISurchargeGroupRepository _repository;
    private readonly IContextUser _context;

    public SurchargeGroupCommandHandler(ISurchargeGroupRepository repository, IContextUser contextUser)
    {
        _repository = repository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(SurchargeGroupAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new SurchargeGroup
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Note = request.Note,
            DisplayOrder = request.DisplayOrder,
            Status = request.Status,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(SurchargeGroupDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new SurchargeGroup
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(SurchargeGroupEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.Id = request.Id;
        item.Code = request.Code;
        item.Name = request.Name;
        item.Description = request.Description;
        item.Note = request.Note;
        item.DisplayOrder = request.DisplayOrder;
        item.Status = request.Status;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
    public async Task<ValidationResult> Handle(SurchargeGroupSortCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<SurchargeGroup> list = request.SortList.Select(x => new SurchargeGroup()
        {
            Id = x.Id,
            DisplayOrder = x.SortOrder
        });
        _repository.Sort(list);
        return await Commit(_repository.UnitOfWork);
    }
}
