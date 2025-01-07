using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Messaging;
using VFi.NetDevPack.Models;
using VFi.NetDevPack.Utilities;

namespace VFi.Application.SO.Commands;

public class CustomerExCommandHandler : CommandHandler,
                                                        IRequestHandler<CustomerExIdentityEditCommand, ValidationResult>,
                                                        IRequestHandler<SignupNoPassCommand, ValidationResult>,
                                                        IRequestHandler<CustomerExActiveBidCommand, ValidationResult>,
                                                         IRequestHandler<CustomerExEditCommand, ValidationResult>,
                                                        IRequestHandler<CustomerExDeactiveBidCommand, ValidationResult>,
                                                        IRequestHandler<CustomerExEditBidQuantityCommand, ValidationResult>,
                                                        IRequestHandler<CustomerExActiveBidHoldCommand, ValidationResult>,
                                                        IRequestHandler<CustomerExDeactiveBidHoldCommand, ValidationResult>,
                                                        IRequestHandler<CustomerExUpdateFinanceCommand, ValidationResult>,
                                                        IRequestHandler<CustomerExUpdateIdInfoActiveCommand, ValidationResult>,
                                                        IRequestHandler<CustomerExUpBidQuantityHoldCommand, ValidationResult>

{
    private readonly IAccountRepository _accountRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerGroupMappingRepository _groupMapping;
    private readonly ICustomerBusinessMappingRepository _businessMapping;
    private readonly ICustomerAddressRepository _addressRepository;
    private readonly ICustomerContactRepository _contactRepository;
    private readonly ICustomerBankRepository _bankRepository;
    private readonly ICustomerPriceListCrossRepository _customerPriceListCrossRepository;
    private readonly IWalletRepository _repositoryWallet;
    private readonly IWalletTransactionRepository _repositoryWalletTrans;
    private readonly ISOContextProcedures _repository;
    private readonly ISOExtProcedures _repositoryEx;
    private readonly IContextUser _context;
    public CustomerExCommandHandler(
        ICustomerRepository customerRepository, IAccountRepository accountRepository,
        ICustomerGroupMappingRepository groupMapping,
        ICustomerBusinessMappingRepository businessMappingRepository,
        ICustomerAddressRepository addressRepository,
        ICustomerContactRepository contactRepository,
        ICustomerBankRepository bankRepository,
        IWalletRepository repositoryWallet,
        IWalletTransactionRepository repositoryWalletTrans,
        ISOContextProcedures repositoryProcedure, ISOExtProcedures repositoryEx,
        IContextUser contextUser,
        ICustomerPriceListCrossRepository customerPriceListCrossRepository
        )
    {
        _customerRepository = customerRepository;
        _accountRepository = accountRepository;
        _groupMapping = groupMapping;
        _businessMapping = businessMappingRepository;
        _addressRepository = addressRepository;
        _contactRepository = contactRepository;
        _bankRepository = bankRepository;
        _repositoryWallet = repositoryWallet;
        _repositoryWalletTrans = repositoryWalletTrans;
        _repository = repositoryProcedure;
        _repositoryEx = repositoryEx;
        _context = contextUser;
        _customerPriceListCrossRepository = customerPriceListCrossRepository;
    }
    public void Dispose()
    {
        // _customerRepository.Dispose();
    }

    public async Task<ValidationResult> Handle(SignupNoPassCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_accountRepository))
            return request.ValidationResult;

        var name = request.Name;
        var token = JsonConvert.SerializeObject(new { Id = request.Id, Name = request.Name, Email = request.Email, Phone = request.Phone, Timestamp = request.Timestamp }).EncryptString("");
        var mailMerge = JsonConvert.SerializeObject(new { Id = request.Id, Name = name, Token = token, Phone = request.Phone, Email = request.Email });
        var emailNotify = new EmailNotify();
        emailNotify.SenderCode = "MG-SMTP";
        emailNotify.SenderName = "Megabuy Japan";
        emailNotify.Subject = "Xác thực đăng ký";
        emailNotify.To = request.Email;
        emailNotify.CC = "";
        emailNotify.BCC = "";
        emailNotify.Body = mailMerge;
        emailNotify.TemplateCode = "SIGNUP-NOPASS";
        _ = await _accountRepository.SendEmail(emailNotify);

        return request.ValidationResult;
    }
    public async Task<ValidationResult> Handle(CustomerExEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_customerRepository))
            return request.ValidationResult;
        var item = await _customerRepository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Customer is not exist") } };
        }
        item.Image = request.Image;
        item.Name = request.Name;
        item.Phone = request.Phone;
        item.Email = request.Email;

        item.Gender = request.Gender;
        item.Year = request.Year;
        item.Month = request.Month;
        item.Day = request.Day;
        if (!item.AccountPhoneVerified.HasValue || (item.AccountPhoneVerified.HasValue && !item.AccountPhoneVerified.Value))
        {
            item.AccountPhone = request.Phone;
        }
        if (!item.AccountEmailVerified.HasValue || (item.AccountEmailVerified.HasValue && !item.AccountEmailVerified.Value))
        {
            item.AccountEmail = request.Email;
        }
        _customerRepository.Update(item);
        return await Commit(_customerRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerExIdentityEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_customerRepository))
            return request.ValidationResult;
        var item = await _customerRepository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Customer is not exist") } };
        }
        item.IdName = request.IdName;
        item.IdNumber = request.IdNumber;
        item.IdDate = request.IdDate;
        item.IdIssuer = request.IdIssuer;
        item.IdImage1 = request.IdImage1;
        item.IdImage2 = request.IdImage2;
        if (request.IdStatus.HasValue)
            item.IdStatus = request.IdStatus;


        _customerRepository.Update(item);
        return await Commit(_customerRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerExActiveBidCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_customerRepository))
            return request.ValidationResult;
        var item = await _customerRepository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Customer is not exist") } };
        }
        item.BidActive = true;
        item.BidQuantity = request.BidQuantity;
        _customerRepository.Update(item);
        return await Commit(_customerRepository.UnitOfWork);
    }
    public async Task<ValidationResult> Handle(CustomerExDeactiveBidCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_customerRepository))
            return request.ValidationResult;
        var item = await _customerRepository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Customer is not exist") } };
        }
        item.BidActive = false;
        item.BidQuantity = 0;
        _customerRepository.Update(item);
        return await Commit(_customerRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerExUpdateFinanceCommand request, CancellationToken cancellationToken)
    {
        var item = await _customerRepository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Customer is not exist") } };
        }
        if (request.PriceListCross.Any())
        {
            var listRemove = item.CustomerPriceListCross.Where(x => !request.PriceListCross.Any(y => y.PriceListCrossId.Equals(x.PriceListCrossId)));
            var listAdd = request.PriceListCross.Where(x => !item.CustomerPriceListCross.Any(y => y.PriceListCrossId.Equals(x.PriceListCrossId))).Select(x =>
            {
                return new CustomerPriceListCross()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = request.Id,
                    PriceListCrossId = x.PriceListCrossId,
                    PriceListCrossName = x.PriceListCrossName,
                    RouterShipping = x.RouterShipping,
                    RouterShippingId = x.RouterShippingId,
                };
            });
            if (listAdd.Any() || listRemove.Any())
            {
                _customerPriceListCrossRepository.Add(listAdd);
                _customerPriceListCrossRepository.Remove(listRemove.Select(x => new CustomerPriceListCross() { Id = x.Id }));
                await Commit(_customerPriceListCrossRepository.UnitOfWork);
            }
        }

        item.CustomerPriceListCross.Clear();
        item.PriceListPurchaseId = request.PriceListPurchaseId;
        item.PriceListPurchaseName = request.PriceListPurchaseName;
        item.CurrencyId = request.CurrencyId;
        item.Currency = request.Currency;
        item.CurrencyName = request.CurrencyName;
        item.PriceListId = request.PriceListId;
        item.PriceListName = request.PriceListName;
        item.DebtLimit = request.DebtLimit;
        item.RemainingDebt = request.RemainingDebt;

        _customerRepository.Update(item);
        return await CommitNoCheck(_customerRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerExEditBidQuantityCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_customerRepository))
            return request.ValidationResult;
        var item = await _customerRepository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Customer is not exist") } };
        }
        item.BidQuantity = request.BidQuantity;
        _customerRepository.Update(item);
        return await Commit(_customerRepository.UnitOfWork);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ValidationResult> Handle(CustomerExActiveBidHoldCommand cmd, CancellationToken cancellationToken)
    {
        if (!cmd.IsValid(_customerRepository))
            return cmd.ValidationResult;

        var item = await _customerRepository.GetById(cmd.Id);
        var userName = _context.UserClaims.FullName;
        var userId = _context.GetUserId();
        var accountId = item.AccountId.Value;

        var result = await _repositoryEx.SP_HOLD_BID_WALLETAsync(accountId, cmd.WalletCode, "", cmd.Amount, cmd.BidQuantity, "", "", "", userId, userName);
        if (result is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("", "Customer Active Bid failed") } };
        }
        return new ValidationResult();

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ValidationResult> Handle(CustomerExDeactiveBidHoldCommand cmd, CancellationToken cancellationToken)
    {
        if (!cmd.IsValid(_customerRepository))
            return cmd.ValidationResult;


        var item = await _customerRepository.GetById(cmd.Id);
        var userName = _context.UserClaims.FullName;
        var userId = _context.GetUserId();
        var accountId = item.AccountId.Value;

        var result = await _repositoryEx.SP_DEACTIVE_BID_WALLETAsync(accountId, userId, userName);
        if (result is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("", "Customer Deactive Bid failed") } };
        }
        return new ValidationResult();

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ValidationResult> Handle(CustomerExUpBidQuantityHoldCommand cmd, CancellationToken cancellationToken)
    {
        if (!cmd.IsValid(_customerRepository))
            return cmd.ValidationResult;
        var item = await _customerRepository.GetById(cmd.Id);
        var userName = _context.UserClaims.FullName;
        var userId = _context.GetUserId();
        var accountId = item.AccountId.Value;

        var result = await _repositoryEx.SP_HOLD_BID_WALLETAsync(accountId, cmd.WalletCode, "", cmd.Amount, cmd.BidQuantity, "", "", "", userId, userName);
        if (result is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("", "Customer Active Bid failed") } };
        }
        return new ValidationResult();

    }
    public async Task<ValidationResult> Handle(CustomerExUpdateIdInfoActiveCommand request, CancellationToken cancellationToken)
    {
        var item = await _customerRepository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Customer is not exist") } };
        }
        item.TranActive = request.TranActive;
        item.IdStatus = request.IdStatus;
        _customerRepository.Update(item);
        return await Commit(_customerRepository.UnitOfWork);
    }

}
