using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Consul;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentValidation.Results;
using MassTransit.Internals.GraphValidation;
using MediatR;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;
using static System.Net.Mime.MediaTypeNames;

namespace VFi.Application.SO.Commands;

internal class LeadActivityCommandHandler : CommandHandler, IRequestHandler<LeadActivityAddCommand, ValidationResult>,
                                                       IRequestHandler<LeadActivityDeleteCommand, ValidationResult>,
                                                       IRequestHandler<LeadActivityEditCommand, ValidationResult>
{
    private readonly ILeadActivityRepository _repository;
    private readonly IContextUser _context;

    public LeadActivityCommandHandler(ILeadActivityRepository LeadActivityRepository, IContextUser contextUser)
    {
        _repository = LeadActivityRepository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(LeadActivityAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new LeadActivity
        {
            Id = request.Id,
            LeadId = request.LeadId,
            CampaignId = request.CampaignId,
            Campaign = request.Campaign,
            Type = request.Type,
            Name = createName,
            Body = request.Body,
            ActualDate = request.ActualDate,
            Attachment = request.Attachment,
            Status = request.Status,
            CreatedDate = createdDate,
            CreatedBy = createdBy,
            CreatedByName = createName
        };

        _repository.Add(item);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        return result;
    }

    public async Task<ValidationResult> Handle(LeadActivityDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new LeadActivity
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(LeadActivityEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Code not exists") } };
        }
        item.LeadId = request.LeadId;
        item.CampaignId = request.CampaignId;
        item.Campaign = request.Campaign;
        item.Type = request.Type;
        item.Name = request.Name;
        item.Body = request.Body;
        item.ActualDate = request.ActualDate;
        item.Attachment = request.Attachment;
        item.Status = request.Status;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;
        item.UpdatedDate = updatedDate;

        _repository.Update(item);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        return result;
    }

}
