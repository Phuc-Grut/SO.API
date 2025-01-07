using Consul;
using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class PaymentInvoiceCommandHandler : CommandHandler,
    IRequestHandler<PaymentInvoiceAddCommand, ValidationResult>,
    IRequestHandler<PaymentInvoiceDeleteCommand, ValidationResult>,
    IRequestHandler<PaymentInvoiceEditCommand, ValidationResult>,
    IRequestHandler<PaymentInvoiceChangeLockedCommand, ValidationResult>
{
    private readonly IPaymentInvoiceRepository _repository;
    private readonly IContextUser _context;

    public PaymentInvoiceCommandHandler(IPaymentInvoiceRepository PaymentInvoiceRepository, IContextUser contextUser)
    {
        _repository = PaymentInvoiceRepository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(PaymentInvoiceAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var obj = new PaymentInvoice
        {
            Id = request.Id,
            Type = request.Type,
            Code = request.Code,
            OrderId = request.OrderId,
            OrderCode = request.OrderCode,
            SaleDiscountId = request.SaleDiscountId,
            ReturnOrderId = request.ReturnOrderId,
            Description = request.Description,
            Amount = request.Amount,
            Currency = request.Currency,
            CurrencyName = request.CurrencyName,
            Calculation = request.Calculation,
            ExchangeRate = request.ExchangeRate,
            PaymentDate = request.PaymentDate,
            PaymentMethodName = request.PaymentMethodName,
            PaymentMethodCode = request.PaymentMethodCode,
            PaymentMethodId = request.PaymentMethodId,
            BankName = request.BankName,
            BankAccount = request.BankAccount,
            BankNumber = request.BankNumber,
            PaymentCode = request.PaymentCode,
            PaymentNote = request.PaymentNote,
            Note = request.Note,
            Status = request.Status,
            PaymentStatus = request.PaymentStatus,
            AccountId = request.AccountId,
            AccountName = request.AccountName,
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName,
            File = request.File,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(obj);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PaymentInvoiceDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var PaymentInvoice = new PaymentInvoice
        {
            Id = request.Id
        };

        _repository.Remove(PaymentInvoice);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PaymentInvoiceEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = await _repository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "PaymentInvoice is not exist") } };
        }
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        item.Type = request.Type;
        item.Code = request.Code;
        item.OrderId = request.OrderId;
        item.OrderCode = request.OrderCode;
        item.SaleDiscountId = request.SaleDiscountId;
        item.ReturnOrderId = request.ReturnOrderId;
        item.Description = request.Description;
        item.Amount = request.Amount;
        item.Currency = request.Currency;
        item.CurrencyName = request.CurrencyName;
        item.Calculation = request.Calculation;
        item.ExchangeRate = request.ExchangeRate;
        item.PaymentDate = request.PaymentDate;
        item.PaymentMethodName = request.PaymentMethodName;
        item.PaymentMethodCode = request.PaymentMethodCode;
        item.PaymentMethodId = request.PaymentMethodId;
        item.BankName = request.BankName;
        item.BankAccount = request.BankAccount;
        item.BankNumber = request.BankNumber;
        item.PaymentCode = request.PaymentCode;
        item.PaymentNote = request.PaymentNote;
        item.Note = request.Note;
        item.Status = request.Status;
        item.PaymentStatus = request.PaymentStatus;
        item.AccountId = request.AccountId;
        item.AccountName = request.AccountName;
        item.CustomerId = request.CustomerId;
        item.CustomerName = request.CustomerName;
        item.File = request.File;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;
        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
    public async Task<ValidationResult> Handle(PaymentInvoiceChangeLockedCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Payment invoice is not exist") } };
        }
        var obj = new PaymentInvoice
        {
            Id = request.Id,
            Locked = request.Locked
        };
        _repository.Changelocked(obj);
        return await Commit(_repository.UnitOfWork);
    }
}
