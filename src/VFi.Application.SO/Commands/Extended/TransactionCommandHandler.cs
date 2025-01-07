using System.Net;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Consul;
using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class TransactionCommandHandler : CommandHandler, IRequestHandler<TransactionAddCommand, ValidationResult>, IRequestHandler<TransactionDeleteCommand, ValidationResult>, IRequestHandler<TransactionEditCommand, ValidationResult>
{
    private readonly ITransactionRepository _repository;
    private readonly IContextUser _context;

    public TransactionCommandHandler(ITransactionRepository repository, IContextUser contextUser)
    {
        _repository = repository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(TransactionAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new Transaction
        {

            Id = request.Id,
            Code = request.Code,
            Amount = request.Amount,
            AccountId = request.AccountId,
            WalletId = request.WalletId,
            Type = request.Type,
            ObjectRef = request.ObjectRef,
            AuthorizeRef = request.AuthorizeRef,
            TransactionRef = request.TransactionRef,
            Source = request.Source,
            MetaData = request.MetaData,
            Status = request.Status,
            ParentId = request.ParentId,
            //ApplyDate = request.ApplyDate,
            RawData = request.RawData,
            Currency = request.Currency,
            AuthorizationTransactionId = request.AuthorizationTransactionId,
            AuthorizationTransactionCode = request.AuthorizationTransactionCode,
            AuthorizationTransactionResult = request.AuthorizationTransactionResult,
            CaptureTransactionId = request.CaptureTransactionId,
            CaptureTransactionResult = request.CaptureTransactionResult,
            RefundTransactionId = request.RefundTransactionId,
            RefundTransactionResult = request.RefundTransactionResult,
            TransactionDate = request.TransactionDate
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(TransactionDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new Transaction
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(TransactionEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.Id = request.Id;
        item.Code = request.Code;
        item.Amount = request.Amount;
        item.AccountId = request.AccountId;
        item.WalletId = request.WalletId;
        item.Type = request.Type;
        item.ObjectRef = request.ObjectRef;
        item.AuthorizeRef = request.AuthorizeRef;
        item.TransactionRef = request.TransactionRef;
        item.Source = request.Source;
        item.MetaData = request.MetaData;
        item.Status = request.Status;
        item.ParentId = request.ParentId;
        //item.ApplyDate = request.ApplyDate;
        item.RawData = request.RawData;
        item.Currency = request.Currency;
        item.AuthorizationTransactionId = request.AuthorizationTransactionId;
        item.AuthorizationTransactionCode = request.AuthorizationTransactionCode;
        item.AuthorizationTransactionResult = request.AuthorizationTransactionResult;
        item.CaptureTransactionId = request.CaptureTransactionId;
        item.CaptureTransactionResult = request.CaptureTransactionResult;
        item.RefundTransactionId = request.RefundTransactionId;
        item.RefundTransactionResult = request.RefundTransactionResult;
        item.TransactionDate = request.TransactionDate;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
