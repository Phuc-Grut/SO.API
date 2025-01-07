using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using VFi.Application.SO.Commands.Validations;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class RequestQuoteCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? StoreId { get; set; }
    public string? StoreCode { get; set; }
    public string? StoreName { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? Note { get; set; }
    public int? Status { get; set; }
    public Guid? ChannelId { get; set; }
    public string? ChannelCode { get; set; }
    public string? ChannelName { get; set; }
}

public class RequestQuoteAddCommand : RequestQuoteCommand
{
    public RequestQuoteAddCommand(
        Guid id,
        string? code,
        string? name,
        string? description,
        DateTime? requestDate,
        DateTime? dueDate,
        Guid? storeId,
        string? storeCode,
        string? storeName,
        Guid? customerId,
        string? customerCode,
        string? customerName,
        string? phone,
        string? email,
        string? address,
        Guid? employeeId,
        string? employeeName,
        string? note,
        int? status,
        Guid? channelId,
        string? channelCode,
        string? channelName)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        RequestDate = requestDate;
        DueDate = dueDate;
        StoreId = storeId;
        StoreCode = storeCode;
        StoreName = storeName;
        CustomerId = customerId;
        CustomerCode = customerCode;
        CustomerName = customerName;
        Phone = phone;
        Email = email;
        Address = address;
        EmployeeId = employeeId;
        EmployeeName = employeeName;
        Note = note;
        Status = status;
        ChannelId = channelId;
        ChannelCode = channelCode;
        ChannelName = channelName;
    }
    public bool IsValid(IRequestQuoteRepository _context)
    {
        ValidationResult = new RequestQuoteAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class RequestQuoteEditCommand : RequestQuoteCommand
{
    public RequestQuoteEditCommand(
        Guid id,
        string? code,
        string? name,
        string? description,
        DateTime? requestDate,
        DateTime? dueDate,
        Guid? storeId,
        string? storeCode,
        string? storeName,
        Guid? customerId,
        string? customerCode,
        string? customerName,
        string? phone,
        string? email,
        string? address,
        Guid? employeeId,
        string? employeeName,
        string? note,
        int? status,
        Guid? channelId,
        string? channelCode,
        string? channelName)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        RequestDate = requestDate;
        DueDate = dueDate;
        StoreId = storeId;
        StoreCode = storeCode;
        StoreName = storeName;
        CustomerId = customerId;
        CustomerCode = customerCode;
        CustomerName = customerName;
        Phone = phone;
        Email = email;
        Address = address;
        EmployeeId = employeeId;
        EmployeeName = employeeName;
        Note = note;
        Status = status;
        ChannelId = channelId;
        ChannelCode = channelCode;
        ChannelName = channelName;
    }
    public bool IsValid(IRequestQuoteRepository _context)
    {
        ValidationResult = new RequestQuoteEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class RequestQuoteDeleteCommand : RequestQuoteCommand
{
    public RequestQuoteDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IRequestQuoteRepository _context)
    {
        ValidationResult = new RequestQuoteDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class UpdateStatusRequestQuoteCommand : Command
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public UpdateStatusRequestQuoteCommand(
        Guid id,
        int? status
    )
    {
        Id = id;
        Status = status;
    }
}
