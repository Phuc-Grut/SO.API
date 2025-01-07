using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using FluentValidation;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class SignupNoPassCommand : Command
{
    public SignupNoPassCommand(Guid id, string name, string email, string phone)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsValid(IAccountRepository _context)
    {
        ValidationResult = new SignupNoPassCommandValidation<SignupNoPassCommand>(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class CustomerExUpdateFinanceCommand : Command
{
    public CustomerExUpdateFinanceCommand(Guid id,
                                        Guid? priceListPurchaseId,
                                        string priceListPurchaseName,
                                        Guid? currencyId,
                                        string currency,
                                        string currencyName,
                                        Guid? priceListId,
                                        string priceListName,
                                        decimal? debtLimit,
                                        decimal? remainingDebt,
                                        IEnumerable<CustomerPriceListCrossDto>? priceListCross)
    {
        Id = id;
        PriceListPurchaseId = priceListPurchaseId;
        PriceListPurchaseName = priceListPurchaseName;
        CurrencyId = currencyId;
        Currency = currency;
        CurrencyName = currencyName;
        PriceListId = priceListId;
        PriceListName = priceListName;
        DebtLimit = debtLimit;
        RemainingDebt = remainingDebt;
        PriceListCross = priceListCross;
    }

    public Guid Id { get; set; }
    public Guid? PriceListPurchaseId { get; set; }
    public string PriceListPurchaseName { get; set; }
    public Guid? CurrencyId { get; set; }
    public string Currency { get; set; }
    public string CurrencyName { get; set; }
    public Guid? PriceListId { get; set; }
    public string PriceListName { get; set; }
    public decimal? DebtLimit { get; set; }
    public decimal? RemainingDebt { get; set; }
    public IEnumerable<CustomerPriceListCrossDto> PriceListCross { get; set; } = Enumerable.Empty<CustomerPriceListCrossDto>();
}
public class CustomerExEditCommand : Command
{
    public Guid Id { get; set; }
    public string? Image { get; set; }

    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public int? Gender { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public CustomerExEditCommand()
    {

    }
    public bool IsValid(ICustomerRepository _context)
    {
        ValidationResult = new CustomerExEditCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerExIdentityEditCommand : Command
{
    public Guid Id { get; set; }
    public string IdName { get; set; }
    public string IdNumber { get; set; }
    public DateTime? IdDate { get; set; }
    public string IdIssuer { get; set; }
    public string IdImage1 { get; set; }
    public string IdImage2 { get; set; }
    public int? IdStatus { get; set; }

    public CustomerExIdentityEditCommand()
    {

    }
    public bool IsValid(ICustomerRepository _context)
    {
        ValidationResult = new CustomerExIdentityEditCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class CustomerExActiveBidCommand : Command
{
    public Guid Id { get; set; }
    public int BidQuantity { get; set; }
    public CustomerExActiveBidCommand()
    {

    }
    public bool IsValid(ICustomerRepository _context)
    {
        ValidationResult = new CustomerExActiveBidValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerExEditBidQuantityCommand : Command
{
    public Guid Id { get; set; }
    public int BidQuantity { get; set; }
    public CustomerExEditBidQuantityCommand()
    {

    }
    public bool IsValid(ICustomerRepository _context)
    {
        ValidationResult = new CustomerExEditBidQuantityValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class CustomerExDeactiveBidCommand : Command
{
    public Guid Id { get; set; }
    public CustomerExDeactiveBidCommand()
    {

    }
    public bool IsValid(ICustomerRepository _context)
    {
        ValidationResult = new CustomerExDeactiveBidValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class CustomerExActiveBidHoldCommand : Command
{
    public Guid Id { get; set; }
    public int BidQuantity { get; set; }
    public decimal Amount { get; set; }
    public string WalletCode { get; set; }
    public CustomerExActiveBidHoldCommand()
    {

    }
    public bool IsValid(ICustomerRepository _context)
    {
        ValidationResult = new CustomerExActiveBidHoldValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class CustomerExDeactiveBidHoldCommand : Command
{
    public Guid Id { get; set; }
    public string WalletCode { get; set; }
    public CustomerExDeactiveBidHoldCommand()
    {

    }
    public bool IsValid(ICustomerRepository _context)
    {
        ValidationResult = new CustomerExDeactiveBidHoldValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class CustomerExUpBidQuantityHoldCommand : Command
{
    public Guid Id { get; set; }
    public int BidQuantity { get; set; }
    public decimal Amount { get; set; }
    public string WalletCode { get; set; }
    public CustomerExUpBidQuantityHoldCommand()
    {

    }
    public bool IsValid(ICustomerRepository _context)
    {
        ValidationResult = new CustomerExUpBidQuantityHoldValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerExUpdateIdInfoActiveCommand : Command
{
    public Guid Id { get; set; }
    public bool TranActive { get; set; }
    public int IdStatus { get; set; }
    public CustomerExUpdateIdInfoActiveCommand(Guid id, bool tranActive, int idStatus)
    {
        Id = id;
        TranActive = tranActive;
        IdStatus = idStatus;
    }
}
