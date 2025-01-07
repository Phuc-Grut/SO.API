using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class ContractTermCommandHandler : CommandHandler, IRequestHandler<ContractTermAddCommand, ValidationResult>,
    IRequestHandler<ContractTermDeleteCommand, ValidationResult>,
    IRequestHandler<ContractTermEditCommand, ValidationResult>,
    IRequestHandler<ContractTermSortCommand, ValidationResult>
{
    private readonly IContractTermRepository _repository;
    private readonly IContextUser _context;
    private readonly IContractRepository _contractRepository;

    public ContractTermCommandHandler(IContractTermRepository contractTermRepository, IContextUser contextUser, IContractRepository contractRepository)
    {
        _repository = contractTermRepository;
        _context = contextUser;
        _contractRepository = contractRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(ContractTermAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new ContractTerm
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Status = request.Status,
            CreatedDate = createdDate,
            CreatedBy = createdBy,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ContractTermDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var filter = new Dictionary<string, object> { { "contractTermId", request.Id } };
        var contracts = await _contractRepository.Filter(filter);
        if (contracts.Any())
        {
            return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
        }
        var item = new ContractTerm
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ContractTermEditCommand request, CancellationToken cancellationToken)
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

    public async Task<ValidationResult> Handle(ContractTermSortCommand request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetAll();

        List<ContractTerm> list = new List<ContractTerm>();

        foreach (var sort in request.SortList)
        {
            ContractTerm obj = data.FirstOrDefault(c => c.Id == sort.Id);
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
