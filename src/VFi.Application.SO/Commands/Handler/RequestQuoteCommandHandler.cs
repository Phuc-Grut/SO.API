using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class RequestQuoteCommandHandler : CommandHandler, IRequestHandler<RequestQuoteAddCommand, ValidationResult>,
                                                            IRequestHandler<RequestQuoteDeleteCommand, ValidationResult>,
                                                            IRequestHandler<RequestQuoteEditCommand, ValidationResult>,
                                                            IRequestHandler<UpdateStatusRequestQuoteCommand, ValidationResult>
{
    private readonly IRequestQuoteRepository _repository;
    private readonly IContextUser _context;

    public RequestQuoteCommandHandler(IRequestQuoteRepository RequestQuoteRepository, IContextUser contextUser)
    {
        _repository = RequestQuoteRepository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(RequestQuoteAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createdBy = _context.GetUserId();
        var createdByname = _context.UserClaims.FullName;
        var createdDate = DateTime.Now;
        var RequestQuote = new RequestQuote
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            RequestDate = request.RequestDate,
            DueDate = request.DueDate,
            StoreId = request.StoreId,
            StoreCode = request.StoreCode,
            StoreName = request.StoreName,
            CustomerId = request.CustomerId,
            CustomerCode = request.CustomerCode,
            CustomerName = request.CustomerName,
            Phone = request.Phone,
            Email = request.Email,
            Address = request.Address,
            EmployeeId = request.EmployeeId,
            EmployeeName = request.EmployeeName,
            Note = request.Note,
            Status = request.Status,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createdByname,
            ChannelId = request.ChannelId,
            ChannelCode = request.ChannelCode,
            ChannelName = request.ChannelName
        };

        //add domain event
        //RequestQuote.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _repository.Add(RequestQuote);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(RequestQuoteDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var RequestQuote = new RequestQuote
        {
            Id = request.Id
        };

        //add domain event
        //RequestQuote.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _repository.Remove(RequestQuote);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(RequestQuoteEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Request is not exist") } };
        }
        var updatedBy = _context.GetUserId();
        var updatedByname = _context.UserClaims.FullName;
        var updatedDate = DateTime.Now;

        item.Code = request.Code;
        item.Name = request.Name;
        item.Description = request.Description;
        item.RequestDate = request.RequestDate;
        item.DueDate = request.DueDate;
        item.StoreId = request.StoreId;
        item.StoreCode = request.StoreCode;
        item.StoreName = request.StoreName;
        item.CustomerId = request.CustomerId;
        item.CustomerCode = request.CustomerCode;
        item.CustomerName = request.CustomerName;
        item.Phone = request.Phone;
        item.Email = request.Email;
        item.Address = request.Address;
        item.EmployeeId = request.EmployeeId;
        item.EmployeeName = request.EmployeeName;
        item.Note = request.Note;
        item.Status = request.Status;
        item.UpdatedBy = updatedBy;
        item.UpdatedDate = updatedDate;
        item.UpdatedByName = updatedByname;
        item.ChannelId = request.ChannelId;
        item.ChannelCode = request.ChannelCode;
        item.ChannelName = request.ChannelName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(UpdateStatusRequestQuoteCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Request is not exist") } };
        }
        var obj = new RequestQuote
        {
            Id = request.Id,
            Status = request.Status
        };
        _repository.UpdateStatus(obj);
        return await Commit(_repository.UnitOfWork);
    }
}
