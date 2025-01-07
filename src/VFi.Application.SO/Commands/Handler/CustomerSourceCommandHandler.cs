using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class CustomerSourceCommandHandler : CommandHandler, IRequestHandler<CustomerSourceAddCommand, ValidationResult>,
                                                            IRequestHandler<CustomerSourceDeleteCommand, ValidationResult>,
                                                            IRequestHandler<CustomerSourceEditCommand, ValidationResult>,
                                                            IRequestHandler<CustomerSourceSortCommand, ValidationResult>
{
    private readonly ICustomerSourceRepository _repository;
    private readonly IContextUser _context;
    private readonly ICustomerRepository _customerRepository;

    public CustomerSourceCommandHandler(ICustomerSourceRepository CustomerSourceRepository, IContextUser contextUser, ICustomerRepository customerRepository)
    {
        _repository = CustomerSourceRepository;
        _context = contextUser;
        _customerRepository = customerRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(CustomerSourceAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new CustomerSource
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            DisplayOrder = request.DisplayOrder,
            Status = request.Status,
            CreatedDate = createdDate,
            CreatedBy = createdBy,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerSourceDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;

        var filter = new Dictionary<string, object> { { "customerSourceId", request.Id } };

        var customers = await _customerRepository.Filter(filter);

        if (customers.Any())
        {
            return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
        }

        var item = new CustomerSource
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerSourceEditCommand request, CancellationToken cancellationToken)
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
        item.DisplayOrder = request.DisplayOrder;
        item.Status = request.Status;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerSourceSortCommand request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetAll();

        List<CustomerSource> list = new List<CustomerSource>();

        foreach (var sort in request.SortList)
        {
            CustomerSource obj = data.FirstOrDefault(c => c.Id == sort.Id);
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
