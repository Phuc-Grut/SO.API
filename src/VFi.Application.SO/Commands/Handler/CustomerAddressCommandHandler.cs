using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class CustomerAddressCommandHandler : CommandHandler, IRequestHandler<CustomerAddressAddCommand, ValidationResult>, IRequestHandler<CustomerAddressDeleteCommand, ValidationResult>, IRequestHandler<CustomerAddressEditCommand, ValidationResult>
{
    private readonly ICustomerAddressRepository _repository;

    public CustomerAddressCommandHandler(ICustomerAddressRepository CustomerAddressRepository)
    {
        _repository = CustomerAddressRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(CustomerAddressAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new CustomerAddress
        {
            Id = request.Id,
            CustomerId = (Guid)request.CustomerId,
            Name = request.Name,
            Country = request.Country,
            Province = request.Province,
            District = request.District,
            Ward = request.Ward,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email,
            ShippingDefault = request.ShippingDefault,
            BillingDefault = request.BillingDefault,
            Status = request.Status,
            SortOrder = request.SortOrder,
            CreatedDate = DateTime.Now,
            CreatedBy = request.CreatedBy,
            CreatedByName = request.CreatedByName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerAddressDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new CustomerAddress
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerAddressEditCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Address is not exist") } };
        }

        item.CustomerId = (Guid)request.CustomerId;
        item.Name = request.Name;
        item.Country = request.Country;
        item.Province = request.Province;
        item.District = request.District;
        item.Ward = request.Ward;
        item.Address = request.Address;
        item.Phone = request.Phone;
        item.Email = request.Email;
        item.ShippingDefault = request.ShippingDefault;
        item.BillingDefault = request.BillingDefault;
        item.Status = request.Status;
        item.SortOrder = request.SortOrder;
        item.UpdatedBy = request.UpdatedBy;
        item.UpdatedDate = DateTime.Now;
        item.UpdatedByName = request.UpdatedByName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
