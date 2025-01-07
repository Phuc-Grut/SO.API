using DocumentFormat.OpenXml.Office.Word;
using FluentValidation.Results;
using MassTransit.Mediator;
using MediatR;
using VFi.Application.SO.Events;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands.Handler;

internal class WalletCommandHandler : CommandHandler, IRequestHandler<WalletAddCommand, ValidationResult>,
    IRequestHandler<WalletDeleteCommand, ValidationResult>,
    IRequestHandler<WalletEditCommand, ValidationResult>,
     IRequestHandler<DepositWalletFromBankCommand, ValidationResult>,
     IRequestHandler<DepositWalletCommand, ValidationResult>,
     IRequestHandler<WithdrawWalletCommand, ValidationResult>,
     IRequestHandler<HoldWalletCommand, ValidationResult>,
     IRequestHandler<HoldBidWalletCommand, ValidationResult>, IRequestHandler<RefundHoldBidWalletCommand, ValidationResult>,
     IRequestHandler<RefundHoldWalletCommand, ValidationResult>
{
    private readonly IEventRepository eventRepository;
    private readonly IWalletRepository _repository;
    private readonly IMyRepository _repositoryMy;
    private readonly ICustomerRepository _repositoryCustomer;
    private readonly IWalletTransactionRepository _repositoryWalletTrans;
    private readonly IContextUser _context;
    private readonly ISOExtProcedures _procedures;
    public WalletCommandHandler(IWalletRepository repository, ICustomerRepository repositoryCustomer,
        IWalletTransactionRepository repositoryWalletTrans, ISOExtProcedures repositoryProcedure, IMyRepository repositoryMy,
        IContextUser contextUser, IEventRepository eventRepository)
    {
        _repository = repository;
        _repositoryWalletTrans = repositoryWalletTrans;
        _repositoryCustomer = repositoryCustomer;
        _procedures = repositoryProcedure;
        _repositoryMy = repositoryMy;
        _context = contextUser;
        this.eventRepository = eventRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(WalletAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new Wallet
        {

            Id = request.Id,
            AccountId = request.AccountId,
            WalletCode = request.WalletCode,
            Cash = request.Cash,
            CashHold = request.CashHold,
            Status = request.Status,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(WalletDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new Wallet
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }


    public async Task<ValidationResult> Handle(WalletEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.Id = request.Id;
        item.AccountId = request.AccountId;
        item.WalletCode = request.WalletCode;
        item.Cash = request.Cash;
        item.CashHold = request.CashHold;
        item.Status = request.Status;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }



    public async Task<ValidationResult> Handle(DepositWalletFromBankCommand cmd, CancellationToken cancellationToken)
    {
        string customerName = "";
        if (!cmd.IsValid(_repository))
            return cmd.ValidationResult;

        if (!cmd.AccountId.HasValue)
        {
            var customer = (await _repositoryCustomer.GetByCode(cmd.CustomerCode));
            cmd.AccountId = customer.AccountId;
            customerName = customer.Code + " / " + customer.Name;
        }

        var userName = _context.UserClaims.Username;
        var userId = _context.GetUserId();
        var result = await _procedures.SP_DEPOSIT_FROM_BANKAsync(
            cmd.AccountId,
            cmd.WalletCode,
            cmd.Amount,
            cmd.PaymentCode,
            cmd.PaymentNote,
            cmd.PaymentDate,
            cmd.BankName,
            cmd.BankAccount,
            cmd.BankNumber,
            userId,
            userName);
        if (result is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("", "Deposit wallet failed") } };
        }
        try
        {
            _repositoryMy.DepositPushNotify(cmd.AccountId.Value, cmd.Amount, cmd.PaymentCode, cmd.PaymentNote);
        }
        catch (Exception)
        { }
        try
        {
            var message = new VFi.Domain.SO.Events.PaymentNotifyQueueEvent();
            // message.Data = notification.Data;
            // message.Tenant = notification.Tenant;
            //message.Data_Zone = notification.Data_Zone;
            message.PaymentType = "Nạp tiền";
            message.CustomerName = customerName;
            message.Body = cmd.PaymentNote;
            message.Date = cmd.PaymentDate.ToString("yyy/MM/dd HH:mm");
            message.Amount = cmd.Amount.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
            await eventRepository.PaymentNotify(message);

        }
        catch (Exception)
        { }
        return new ValidationResult();

    }
    public async Task<ValidationResult> Handle(DepositWalletCommand cmd, CancellationToken cancellationToken)
    {
        if (!cmd.IsValid(_repository))
            return cmd.ValidationResult;

        if (!cmd.AccountId.HasValue)
        {
            cmd.AccountId = (await _repositoryCustomer.GetByCode(cmd.CustomerCode)).AccountId;
        }

        var userName = _context.UserClaims.Username;
        var userId = _context.GetUserId();
        var result = await _procedures.SP_DEPOSIT_WALLETAsync(
            cmd.AccountId,
            cmd.WalletCode,
            cmd.Amount,
            cmd.PaymentCode,
            cmd.PaymentNote,
            cmd.PaymentDate,
            cmd.Document,
            userId,
            userName);
        if (result is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("", "Deposit wallet failed") } };
        }
        try
        {
            _repositoryMy.DepositPushNotify(cmd.AccountId.Value, cmd.Amount, cmd.PaymentCode, cmd.PaymentNote);
        }
        catch (Exception)
        { }
        return new ValidationResult();
    }

    public async Task<ValidationResult> Handle(WithdrawWalletCommand cmd, CancellationToken cancellationToken)
    {
        if (!cmd.IsValid(_repository))
            return cmd.ValidationResult;
        var userName = _context.UserClaims.FullName;
        var userId = _context.GetUserId();
        //var result = await _procedures.SP_WITHDRAW_WALLETAsync(cmd.AccountId, cmd.WalletCode, cmd.TransactionCode, cmd.Amount, cmd.Method, cmd.Note, cmd.RawData, cmd.Document, userId, userName);
        var result = await _procedures.SP_CREATE_INVOICE_WITHDRAWAsync(cmd.AccountId, cmd.WalletCode, cmd.Amount, cmd.Method, cmd.Note, cmd.RawData, cmd.Document, userId, userName);
        if (result is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("", "Deposit wallet failed") } };
        }
        return new ValidationResult();

    }



    public async Task<ValidationResult> Handle(HoldWalletCommand cmd, CancellationToken cancellationToken)
    {
        if (!cmd.IsValid(_repository))
            return cmd.ValidationResult;

        var userName = _context.UserClaims.FullName;
        var userId = _context.GetUserId();

        var result = await _procedures.SP_HOLD_WALLETAsync(cmd.AccountId, cmd.WalletCode, cmd.TransactionCode, cmd.Amount, cmd.Note, cmd.RawData, cmd.RefId, cmd.RefDate, cmd.RefType, "", userId, userName);
        if (result is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("", "Deposit wallet failed") } };
        }
        return new ValidationResult();

    }

    public async Task<ValidationResult> Handle(RefundHoldWalletCommand cmd, CancellationToken cancellationToken)
    {
        if (!cmd.IsValid(_repository))
            return cmd.ValidationResult;

        var userName = _context.UserClaims.FullName;
        var userId = _context.GetUserId();

        var result = await _procedures.SP_REFUND_HOLD_WALLETAsync(cmd.AccountId, cmd.WalletCode, cmd.TransactionCode, cmd.RefId, cmd.Note, cmd.RawData, "", userId, userName);
        if (result is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("", "Deposit wallet failed") } };
        }
        return new ValidationResult();

    }

    public async Task<ValidationResult> Handle(HoldBidWalletCommand cmd, CancellationToken cancellationToken)
    {
        if (!cmd.IsValid(_repository))
            return cmd.ValidationResult;

        var userName = _context.UserClaims.FullName;
        var userId = _context.GetUserId();

        var result = await _procedures.SP_ADD_HOLD_BID_WALLETAsync(cmd.AccountId, cmd.WalletCode, cmd.TransactionCode, cmd.Amount, cmd.Note, userId, userName);
        if (result is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("", "Hold bid wallet failed") } };
        }
        return new ValidationResult();

    }
    public async Task<ValidationResult> Handle(RefundHoldBidWalletCommand cmd, CancellationToken cancellationToken)
    {
        if (!cmd.IsValid(_repository))
            return cmd.ValidationResult;

        var userName = _context.UserClaims.FullName;
        var userId = _context.GetUserId();

        var result = await _procedures.SP_REFUND_HOLD_BID_WALLETAsync(cmd.AccountId, cmd.RefId, cmd.TransactionCode, cmd.Note, userId, userName);
        if (result is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("", "Refund hold wallet failed") } };
        }
        return new ValidationResult();

    }


}
