using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VFi.Application.SO.Commands;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CRM.Commands;

internal class ContractTypeCommandHandler : CommandHandler,
                                    IRequestHandler<ContractTypeAddCommand, ValidationResult>,
                                    IRequestHandler<ContractTypeEditCommand, ValidationResult>,
                                    IRequestHandler<ContractTypeDeleteCommand, ValidationResult>
{
    private readonly IContractTypeRepository _repository;
    private readonly IContractRepository _contractRepository;
    private readonly IContextUser _context;

    public ContractTypeCommandHandler(IContractTypeRepository ContractTypeRepository, IContextUser contextUser, IContractRepository contractRepository)
    {
        _repository = ContractTypeRepository;
        _context = contextUser;
        _contractRepository = contractRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(ContractTypeAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new ContractType
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

    public async Task<ValidationResult> Handle(ContractTypeDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var filter = new Dictionary<string, object> { { "contractTypeId", request.Id } };

        var contracts = await _contractRepository.Filter(filter);

        if (contracts.Any())
        {
            return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
        }

        var item = new ContractType
        {
            Id = request.Id,
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ContractTypeEditCommand request, CancellationToken cancellationToken)
    {
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = await _repository.GetById(request.Id);

        item.Code = request.Code;
        item.Name = request.Name;
        item.Description = request.Description;
        item.Status = request.Status;
        item.UpdatedBy = updatedBy;
        item.UpdatedDate = updatedDate;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
