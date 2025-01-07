using System;
using System.Drawing;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Consul;
using FluentValidation.Results;
using MassTransit.Internals;
using MassTransit.Internals.GraphValidation;
using MassTransit.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VFi.Application.SO.Queries;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using VFi.NetDevPack.Messaging;
using VFi.NetDevPack.Models;
using static System.Net.Mime.MediaTypeNames;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VFi.Application.SO.Commands;

internal class LeadCommandHandler : CommandHandler, IRequestHandler<LeadAddCommand, ValidationResult>,
                                                       IRequestHandler<LeadDeleteCommand, ValidationResult>,
                                                       IRequestHandler<LeadEditCommand, ValidationResult>,
                                                       IRequestHandler<ConvertLeadCommand, ValidationResult>,
                                                       IRequestHandler<LeadEmailNotifyCommand, ValidationResult>
{
    private readonly IMediatorHandler _mediator;
    private readonly ILeadRepository _repository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IContextUser _context;
    private readonly IEmailMasterRepository _emailMasterRepository;

    public LeadCommandHandler(ILeadRepository LeadRepository, IContextUser contextUser, ICustomerRepository customerRepository, IMediatorHandler mediator, IEmailMasterRepository emailMasterRepository)
    {
        _repository = LeadRepository;
        _customerRepository = customerRepository;
        _context = contextUser;
        _mediator = mediator;
        _emailMasterRepository = emailMasterRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(LeadAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new Lead
        {
            Id = request.Id,
            Source = request.Source,
            Code = request.Code,
            Image = request.Image,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Country = request.Country,
            Province = request.Province,
            District = request.District,
            Ward = request.Ward,
            ZipCode = request.ZipCode,
            Address = request.Address,
            Website = request.Website,
            TaxCode = request.TaxCode,
            BusinessSector = request.BusinessSector,
            Company = request.Company,
            CompanyPhone = request.CompanyPhone,
            CompanyName = request.CompanyName,
            CompanySize = request.CompanySize,
            Capital = request.Capital,
            EstablishedDate = request.EstablishedDate,
            Tags = request.Tags,
            Note = request.Note,
            Status = request.Status,
            GroupId = request.GroupId,
            Group = request.Group,
            EmployeeId = request.EmployeeId,
            Employee = request.Employee,
            GroupEmployeeId = request.GroupEmployeeId,
            GroupEmployee = request.GroupEmployee,
            Gender = request.Gender,
            Year = request.Year,
            Month = request.Month,
            Day = request.Day,
            Facebook = request.Facebook,
            Zalo = request.Zalo,
            RevenueTarget = request.RevenueTarget,
            Revenue = request.Revenue,
            Scale = request.Scale,
            Difficult = request.Difficult,
            Point = request.Point,
            Priority = request.Priority,
            Demand = request.Demand,
            DynamicData = request.DynamicData,
            Converted = request.Converted,
            CreatedDate = createdDate,
            CreatedBy = createdBy,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(LeadDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new Lead
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(LeadEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Lead is not exist") } };
        }
        item.Source = request.Source;
        item.Code = request.Code;
        item.Image = request.Image;
        item.Name = request.Name;
        item.Email = request.Email;
        item.Phone = request.Phone;
        item.Country = request.Country;
        item.Province = request.Province;
        item.District = request.District;
        item.Ward = request.Ward;
        item.ZipCode = request.ZipCode;
        item.Address = request.Address;
        item.Website = request.Website;
        item.TaxCode = request.TaxCode;
        item.BusinessSector = request.BusinessSector;
        item.Company = request.Company;
        item.CompanyPhone = request.CompanyPhone;
        item.CompanyName = request.CompanyName;
        item.CompanySize = request.CompanySize;
        item.Capital = request.Capital;
        item.EstablishedDate = request.EstablishedDate;
        item.Tags = request.Tags;
        item.Note = request.Note;
        item.Status = request.Status;
        item.GroupId = request.GroupId;
        item.Group = request.Group;
        item.EmployeeId = request.EmployeeId;
        item.Employee = request.Employee;
        item.GroupEmployeeId = request.GroupEmployeeId;
        item.GroupEmployee = request.GroupEmployee;
        item.Gender = request.Gender;
        item.Year = request.Year;
        item.Month = request.Month;
        item.Day = request.Day;
        item.Facebook = request.Facebook;
        item.Zalo = request.Zalo;
        item.RevenueTarget = request.RevenueTarget;
        item.Revenue = request.Revenue;
        item.Scale = request.Scale;
        item.Difficult = request.Difficult;
        item.Point = request.Point;
        item.Priority = request.Priority;
        item.Demand = request.Demand;
        item.DynamicData = request.DynamicData;
        item.Converted = request.Converted;
        item.CustomerCode = request.CustomerCode;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;
        item.UpdatedDate = updatedDate;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ConvertLeadCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Lead is not exist") } };
        }
        item.CustomerCode = request.CustomerCode;
        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(LeadEmailNotifyCommand request, CancellationToken cancellationToken)
    {
        var emailNotify = new EmailNotify
        {
            SenderCode = request.SenderCode,
            SenderName = request.SenderName,
            Subject = request.Subject,
            From = request.From,
            To = request.To,
            CC = request.CC,
            BCC = request.BCC,
            Body = request.Body,
            TemplateCode = request.TemplateCode,
        };

        _emailMasterRepository.SendEmail(emailNotify);

        return new ValidationResult();
    }
}
