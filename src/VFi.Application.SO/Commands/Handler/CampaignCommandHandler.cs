using System.Net;
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

internal class CampaignCommandHandler : CommandHandler, IRequestHandler<CampaignAddCommand, ValidationResult>,
                                                       IRequestHandler<CampaignDeleteCommand, ValidationResult>,
                                                       IRequestHandler<CampaignEditCommand, ValidationResult>
{
    private readonly ICampaignRepository _repository;
    private readonly ICampaignStatusRepository _campaignStatusRepository;
    private readonly IContextUser _context;

    public CampaignCommandHandler(ICampaignRepository campaignRepository, ICampaignStatusRepository campaignStatusRepository, IContextUser contextUser)
    {
        _repository = campaignRepository;
        _campaignStatusRepository = campaignStatusRepository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(CampaignAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new Campaign
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
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
        if (!result.IsValid)
            return result;

        if (request.Details?.Count > 0)
        {
            List<CampaignStatus> list = new List<CampaignStatus>();
            var i = 1;
            foreach (var x in request.Details)
            {
                var detail = new CampaignStatus()
                {
                    Id = x.Id,
                    CampaignId = request.Id,
                    Name = x.Name,
                    Color = x.Color,
                    TextColor = x.TextColor,
                    IsDefault = x.IsDefault,
                    IsClose = x.IsClose,
                    Description = x.Description,
                    Status = x.Status,
                    DisplayOrder = i,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.Now,
                    CreatedByName = createName
                };
                list.Add(detail);
                i++;
            }
            _campaignStatusRepository.Add(list);
            _ = await CommitNoCheck(_campaignStatusRepository.UnitOfWork);
        }
        return result;
    }

    public async Task<ValidationResult> Handle(CampaignDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new Campaign
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CampaignEditCommand request, CancellationToken cancellationToken)
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
        item.Code = request.Code;
        item.Name = request.Name;
        item.Description = request.Description;
        item.StartDate = request.StartDate;
        item.EndDate = request.EndDate;
        item.Leader = request.Leader;
        item.LeaderName = request.LeaderName;
        item.Member = request.Member;
        item.Status = request.Status;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;
        item.UpdatedDate = updatedDate;

        _repository.Update(item);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        if (request.Delete?.Count > 0)
        {
            List<CampaignStatus> list = new List<CampaignStatus>();
            foreach (var d in request.Delete)
            {
                var o = item.CampaignStatuses.FirstOrDefault(x => x.Id.Equals(d.Id));
                if (o is not null)
                {
                    list.Add(o);
                }
            }
            _campaignStatusRepository.Remove(list);
            _ = await CommitNoCheck(_campaignStatusRepository.UnitOfWork);
        }
        if (request.Details?.Count > 0)
        {
            List<CampaignStatus> listAdd = new List<CampaignStatus>();
            List<CampaignStatus> listUpdate = new List<CampaignStatus>();
            var i = 1;
            foreach (var x in request.Details)
            {
                var obj = item.CampaignStatuses.FirstOrDefault(o => o.Id.Equals(x.Id));
                if (obj is not null)
                {
                    obj.CampaignId = request.Id;
                    obj.Name = x.Name;
                    obj.Color = x.Color;
                    obj.TextColor = x.TextColor;
                    obj.IsDefault = x.IsDefault;
                    obj.IsClose = x.IsClose;
                    obj.Description = x.Description;
                    obj.Status = x.Status;
                    obj.DisplayOrder = i;
                    obj.UpdatedBy = updatedBy;
                    obj.UpdatedDate = DateTime.Now;
                    obj.UpdatedByName = updateName;
                    listUpdate.Add(obj);
                }
                else
                {
                    listAdd.Add(new CampaignStatus()
                    {
                        Id = (Guid)x.Id,
                        CampaignId = request.Id,
                        Name = x.Name,
                        Color = x.Color,
                        TextColor = x.TextColor,
                        IsDefault = x.IsDefault,
                        IsClose = x.IsClose,
                        Description = x.Description,
                        Status = x.Status,
                        DisplayOrder = i,
                        CreatedBy = updatedBy,
                        CreatedDate = DateTime.Now,
                        CreatedByName = updateName,
                    });
                }
                i++;
            }
            if (listAdd.Count > 0)
            {
                _campaignStatusRepository.Add(listAdd);
            }
            if (listUpdate.Count > 0)
            {
                _campaignStatusRepository.Update(listUpdate);
            }
            _ = await CommitNoCheck(_campaignStatusRepository.UnitOfWork);
        }

        return result;
    }

}
