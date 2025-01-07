using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class CustomerBankCommandHandler : CommandHandler, IRequestHandler<CustomerBankAddCommand, ValidationResult>, IRequestHandler<CustomerBankDeleteCommand, ValidationResult>, IRequestHandler<CustomerBankEditCommand, ValidationResult>
{
    private readonly ICustomerBankRepository _repository;

    public CustomerBankCommandHandler(ICustomerBankRepository repository)
    {
        _repository = repository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(CustomerBankAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new CustomerBank
        {
            Id = request.Id,
            CustomerId = (Guid)request.CustomerId,
            BankCode = request.BankCode,
            BankName = request.BankName,
            Name = request.Name,
            BankBranch = request.BankBranch,
            AccountName = request.AccountName,
            AccountNumber = request.AccountNumber,
            Default = request.Default,
            Status = request.Status,
            SortOrder = request.SortOrder,
            CreatedDate = DateTime.Now,
            CreatedBy = request.CreatedBy,
            CreatedByName = request.CreatedByName
        };

        //add domain event
        //CustomerBank.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerBankDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new CustomerBank
        {
            Id = request.Id
        };
        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerBankEditCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Account Banking is not exist") } };
        }
        item.CustomerId = (Guid)request.CustomerId;
        item.Name = request.Name;
        item.BankCode = request.BankCode;
        item.BankName = request.BankName;
        item.BankBranch = request.BankBranch;
        item.AccountNumber = request.AccountNumber;
        item.AccountName = request.AccountName;
        item.Default = request.Default;
        item.Status = request.Status;
        item.SortOrder = request.SortOrder;
        item.UpdatedBy = request.UpdatedBy;
        item.UpdatedDate = DateTime.Now;
        item.UpdatedByName = request.UpdatedByName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
