using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Consul;
using VFi.Application.SO.Commands.Validations;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class CustomerBankCommand : Command
{


    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string? Name { get; set; }
    public string? BankCode { get; set; }
    public string? BankName { get; set; }
    public string? BankBranch { get; set; }
    public string? AccountName { get; set; }
    public string? AccountNumber { get; set; }
    public bool? Default { get; set; }
    public int? Status { get; set; }
    public int? SortOrder { get; set; }
}

public class CustomerBankAddCommand : CustomerBankCommand
{
    public CustomerBankAddCommand()
    {
    }

    public CustomerBankAddCommand(
        Guid id,
        Guid customerId,
        string? name,
        string? bankCode,
        string? bankName,
        string? bankBranch,
        string? accountName,
        string? accountNumber,
        bool? @default,
        int? status,
        int? sortOrder,
        Guid? createdBy,
        string createdByName
        )
    {
        Id = id;
        CustomerId = customerId;
        Name = name;
        BankCode = bankCode;
        BankName = bankName;
        BankBranch = bankBranch;
        AccountName = accountName;
        AccountNumber = accountNumber;
        Default = @default;
        Status = status;
        SortOrder = sortOrder;
        CreatedBy = createdBy;
        CreatedByName = createdByName;
    }
    public Guid? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }

    public bool IsValid(ICustomerBankRepository _context)
    {
        ValidationResult = new CustomerBankAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerBankEditCommand : CustomerBankCommand
{
    public CustomerBankEditCommand(
        Guid id,
        Guid customerId,
        string? name,
        string? bankCode,
        string? bankName,
        string? bankBranch,
        string? accountName,
        string? accountNumber,
        bool? @default,
        int? status,
        int? sortOrder,
        Guid? updatedBy,
        string updatedByName
        )
    {
        Id = id;
        CustomerId = customerId;
        Name = name;
        BankCode = bankCode;
        BankName = bankName;
        BankBranch = bankBranch;
        AccountName = accountName;
        AccountNumber = accountNumber;
        Default = @default;
        Status = status;
        SortOrder = sortOrder;
        UpdatedBy = updatedBy;
        UpdatedByName = updatedByName;
    }
    public CustomerBankEditCommand()
    {
    }

    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }

    public bool IsValid(ICustomerBankRepository _context)
    {
        ValidationResult = new CustomerBankEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CustomerBankDeleteCommand : CustomerBankCommand
{
    public CustomerBankDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ICustomerBankRepository _context)
    {
        ValidationResult = new CustomerBankDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
