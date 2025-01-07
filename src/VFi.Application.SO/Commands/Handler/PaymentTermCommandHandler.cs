using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class PaymentTermCommandHandler : CommandHandler, IRequestHandler<AddPaymentTermCommand, ValidationResult>, IRequestHandler<DeletePaymentTermCommand, ValidationResult>, IRequestHandler<EditPaymentTermCommand, ValidationResult>
{
    private readonly IPaymentTermRepository _repository;
    private readonly IContextUser _context;

    public PaymentTermCommandHandler(IContextUser contextUser, IPaymentTermRepository paymentTermRepository)
    {
        _context = contextUser;
        _repository = paymentTermRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(AddPaymentTermCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new PaymentTerm
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Day = request.Day,
            Percent = request.Percent,
            Value = request.Value,
            Status = request.Status,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName,
        };

        _repository.Add(item);

        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(DeletePaymentTermCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var item = new PaymentTerm
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(EditPaymentTermCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var paymentTerm = await _repository.GetById(request.Id);
        paymentTerm.Code = request.Code;
        paymentTerm.Name = request.Name;
        paymentTerm.Description = request.Description;
        paymentTerm.Day = request.Day;
        paymentTerm.Percent = request.Percent;
        paymentTerm.Value = request.Value;
        paymentTerm.Status = request.Status;
        paymentTerm.UpdatedDate = updatedDate;
        paymentTerm.UpdatedBy = updatedBy;
        paymentTerm.UpdatedByName = updateName;

        _repository.Update(paymentTerm);

        return await Commit(_repository.UnitOfWork);
    }
}
