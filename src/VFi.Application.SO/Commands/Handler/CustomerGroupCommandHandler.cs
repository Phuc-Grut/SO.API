using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class CustomerGroupCommandHandler : CommandHandler, IRequestHandler<CustomerGroupAddCommand, ValidationResult>,
                                                            IRequestHandler<CustomerGroupDeleteCommand, ValidationResult>,
                                                            IRequestHandler<CustomerGroupEditCommand, ValidationResult>,
                                                            IRequestHandler<CustomerGroupSortCommand, ValidationResult>
{
    private readonly ICustomerGroupRepository _repository;
    private readonly IContextUser _context;

    public CustomerGroupCommandHandler(ICustomerGroupRepository customerGroupRepository, IContextUser contextUser)
    {
        _repository = customerGroupRepository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(CustomerGroupAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new CustomerGroup
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Status = request.Status,
            DisplayOrder = request.DisplayOrder,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };
        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerGroupDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new CustomerGroup
        {
            Id = request.Id
        };
        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerGroupEditCommand request, CancellationToken cancellationToken)
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
        item.DisplayOrder = request.DisplayOrder;
        item.Status = request.Status;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerGroupSortCommand request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetAll();

        List<CustomerGroup> list = new List<CustomerGroup>();

        foreach (var sort in request.SortList)
        {
            CustomerGroup obj = data.FirstOrDefault(c => c.Id == sort.Id);
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
