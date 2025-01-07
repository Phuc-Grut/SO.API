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

internal class WalletTransactionCommandHandler : CommandHandler, IRequestHandler<WalletTransactionAddCommand, ValidationResult>, IRequestHandler<WalletTransactionDeleteCommand, ValidationResult>, IRequestHandler<WalletTransactionEditCommand, ValidationResult>
{
    private readonly IWalletTransactionRepository _repository;
    private readonly IContextUser _context;

    public WalletTransactionCommandHandler(IWalletTransactionRepository repository, IContextUser contextUser)
    {
        _repository = repository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(WalletTransactionAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new WalletTransaction
        {

            Id = request.Id,
            Code = request.Code,
            Amount = request.Amount,
            AccountId = request.AccountId,
            WalletId = request.WalletId,
            Type = request.Type,
            Method = request.Method,
            Status = request.Status,
            ApplyDate = request.ApplyDate,
            RawData = request.RawData,
            Balance = request.Balance,
            RefId = request.RefId,
            RefType = request.RefType,
            RefCode = request.RefCode,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(WalletTransactionDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new WalletTransaction
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(WalletTransactionEditCommand request, CancellationToken cancellationToken)
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
        item.Method = request.Method;
        item.Status = request.Status;
        item.ApplyDate = request.ApplyDate;
        item.RawData = request.RawData;
        item.Balance = request.Balance;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
