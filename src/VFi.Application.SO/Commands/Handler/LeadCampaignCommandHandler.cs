using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Consul;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentValidation.Results;
using MassTransit.Internals.GraphValidation;
using MediatR;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;
using static System.Net.Mime.MediaTypeNames;
using static MassTransit.ValidationResultExtensions;

namespace VFi.Application.SO.Commands;

internal class LeadCampaignCommandHandler : CommandHandler, IRequestHandler<LeadCampaignAddCommand, ValidationResult>,
                                                       IRequestHandler<LeadCampaignDeleteCommand, ValidationResult>,
                                                       IRequestHandler<LeadCampaignEditCommand, ValidationResult>,
                                                       IRequestHandler<AddLeadCampaignCommand, ValidationResult>,
                                                       IRequestHandler<EditLeadCampaignCommand, ValidationResult>,
                                                       IRequestHandler<EditStatusCommand, ValidationResult>,
                                                       IRequestHandler<EditStateCommand, ValidationResult>,
                                                       IRequestHandler<DeleteLeadCampaignCommand, ValidationResult>,
                                                       IRequestHandler<LeadCampaignSendEmailCommand, ValidationResult>
{
    private readonly ILeadCampaignRepository _repository;
    private readonly ICampaignStatusRepository _campaignStatusRepository;
    private readonly IContextUser _context;
    private readonly IEmailMasterRepository _emailMasterRepository;

    public LeadCampaignCommandHandler(ILeadCampaignRepository LeadCampaignRepository, ICampaignStatusRepository campaignStatusRepository, IContextUser contextUser, IEmailMasterRepository emailMasterRepository)
    {
        _repository = LeadCampaignRepository;
        _campaignStatusRepository = campaignStatusRepository;
        _context = contextUser;
        _emailMasterRepository = emailMasterRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(LeadCampaignAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new LeadCampaign
        {
            Id = request.Id,
            LeadId = request.LeadId,
            Email = request.Email,
            Phone = request.Phone,
            Name = request.Name,
            CampaignId = request.CampaignId,
            Campaign = request.Campaign,
            StateId = request.StateId,
            State = request.State,
            Leader = request.Leader,
            LeaderName = request.LeaderName,
            Member = request.Member,
            Status = request.Status,
            CreatedDate = createdDate,
            CreatedBy = createdBy,
            CreatedByName = createName
        };
        _repository.Add(item);
        var result = await Commit(_repository.UnitOfWork);
        return result;
    }

    public async Task<ValidationResult> Handle(LeadCampaignDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new LeadCampaign
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(LeadCampaignEditCommand request, CancellationToken cancellationToken)
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
        item.Email = request.Email;
        item.Phone = request.Phone;
        item.Name = request.Name;
        item.CampaignId = request.CampaignId;
        item.Campaign = request.Campaign;
        if (request.StateId is not null)
        {
            var itemcampaign = await _campaignStatusRepository.GetById((Guid)request.StateId);
            if (itemcampaign is not null)
            {
                item.StateId = request.StateId;
                item.State = itemcampaign.Name;
                item.StateDate = DateTime.Now;
            }
        }
        item.Leader = request.Leader;
        item.LeaderName = request.LeaderName;
        item.Member = request.Member;
        item.Status = request.Status;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;
        item.UpdatedDate = updatedDate;

        _repository.Update(item);
        var result = await Commit(_repository.UnitOfWork);

        return result;
    }
    public async Task<ValidationResult> Handle(AddLeadCampaignCommand request, CancellationToken cancellationToken)
    {
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        List<LeadCampaign> list = new List<LeadCampaign>();
        foreach (var x in request.Data)
        {
            var detail = new LeadCampaign()
            {
                Id = x.Id,
                LeadId = x.LeadId,
                Email = x.Email,
                Phone = x.Phone,
                Name = x.Name,
                CampaignId = x.CampaignId,
                Campaign = x.Campaign,
                StateId = x.StateId,
                State = x.State,
                Leader = x.Leader,
                LeaderName = x.LeaderName,
                Member = x.Member,
                Status = x.Status,
                CreatedDate = createdDate,
                CreatedBy = createdBy,
                CreatedByName = createName
            };
            list.Add(detail);
        }
        _repository.Add(list);
        var result = await CommitNoCheck(_repository.UnitOfWork);
        return result;
    }

    public async Task<ValidationResult> Handle(EditLeadCampaignCommand request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        filter.Add("id", string.Join(",", request.Data.ToList()));
        var listData = await _repository.Filter(filter);
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        List<LeadCampaign> list = new List<LeadCampaign>();
        foreach (var item in listData)
        {
            item.LeadId = request.LeadId ?? item.LeadId;
            item.Email = request.Email ?? item.Email;
            item.Phone = request.Phone ?? item.Phone;
            item.Name = request.Name ?? item.Name;
            item.CampaignId = request.CampaignId ?? item.CampaignId;
            item.Campaign = request.Campaign ?? item.Campaign;
            item.StateId = request.StateId ?? item.StateId;
            item.State = request.State ?? item.State;
            item.Leader = request.Leader ?? item.Leader;
            item.LeaderName = request.LeaderName ?? item.LeaderName;
            item.Member = request.Member ?? item.Member;
            item.Status = request.Status ?? item.Status;
            item.UpdatedBy = updatedBy;
            item.UpdatedByName = updateName;
            item.UpdatedDate = updatedDate;
            list.Add(item);
        }
        _repository.Update(list);
        var result = await CommitNoCheck(_repository.UnitOfWork);
        return result;
    }

    public async Task<ValidationResult> Handle(EditStatusCommand request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        filter.Add("id", string.Join(",", request.Data.Select(x => x.Id).ToList()));
        var listData = await _repository.Filter(filter);
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        List<LeadCampaign> list = new List<LeadCampaign>();
        foreach (var item in listData)
        {
            var obj = request.Data.Where(x => x.Id == item.Id).SingleOrDefault();
            if (obj is not null)
            {
                item.Status = obj.Status;
                item.UpdatedBy = updatedBy;
                item.UpdatedByName = updateName;
                item.UpdatedDate = updatedDate;
                list.Add(item);
            }
        }
        _repository.Update(list);
        var result = await CommitNoCheck(_repository.UnitOfWork);
        return result;
    }

    public async Task<ValidationResult> Handle(EditStateCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Code not exists") } };
        }
        item.StateId = request.StateId;
        item.State = request.State;
        item.StateDate = DateTime.Now;
        _repository.Update(item);
        var result = await Commit(_repository.UnitOfWork);

        return result;
    }
    public async Task<ValidationResult> Handle(DeleteLeadCampaignCommand request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        filter.Add("id", string.Join(",", request.Data.ToList()));
        var listData = await _repository.Filter(filter);
        List<LeadCampaign> list = new List<LeadCampaign>();
        foreach (var item in listData)
        {
            list.Add(item);
        }
        _repository.Remove(list);
        var result = await CommitNoCheck(_repository.UnitOfWork);
        return result;
    }

    public async Task<ValidationResult> Handle(LeadCampaignSendEmailCommand request, CancellationToken cancellationToken)
    {
        var campaignSendEmail = new CampaignSendEmail
        {
            Campaign = request.Campaign,
            SenderCode = request.SenderCode,
            SenderName = request.SenderName,
            Subject = request.Subject,
            From = request.From,
            To = request.To,
            CC = request.CC,
            BCC = request.BCC,
            Body = request.Body,
        };

        _emailMasterRepository.CampaignSendEmail(campaignSendEmail);

        return new ValidationResult();
    }
}
