using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class CustomerContactCommandHandler : CommandHandler, IRequestHandler<CustomerContactAddCommand, ValidationResult>, IRequestHandler<CustomerContactDeleteCommand, ValidationResult>, IRequestHandler<CustomerContactEditCommand, ValidationResult>
{
    private readonly ICustomerContactRepository _repository;

    public CustomerContactCommandHandler(ICustomerContactRepository customerContactRepository)
    {
        _repository = customerContactRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(CustomerContactAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new CustomerContact
        {
            Id = request.Id,
            CustomerId = (Guid)request.CustomerId,
            Name = request.Name,
            Gender = request.Gender,
            Phone = request.Phone,
            Email = request.Email,
            Facebook = request.Facebook,
            Tags = request.Tags,
            Address = request.Address,
            Status = request.Status,
            SortOrder = request.SortOrder,
            CreatedDate = DateTime.Now,
            CreatedBy = request.CreatedBy,
            CreatedByName = request.CreatedByName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerContactDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new CustomerContact
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerContactEditCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Contract is not exist") } };
        }
        item.CustomerId = (Guid)request.CustomerId;
        item.Name = request.Name;
        item.Gender = request.Gender;
        item.Phone = request.Phone;
        item.Email = request.Email;
        item.Facebook = request.Facebook;
        item.Tags = request.Tags;
        item.Address = request.Address;
        item.Status = request.Status;
        item.SortOrder = request.SortOrder;
        item.UpdatedBy = request.UpdatedBy;
        item.UpdatedDate = DateTime.Now;
        item.UpdatedByName = request.UpdatedByName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
