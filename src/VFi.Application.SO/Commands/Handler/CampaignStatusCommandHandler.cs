using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Consul;
using FluentValidation.Results;
using MassTransit.Internals.GraphValidation;
using MediatR;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;
using static System.Net.Mime.MediaTypeNames;

namespace VFi.Application.SO.Commands;

internal class CampaignStatusCommandHandler : CommandHandler, IRequestHandler<CampaignStatusAddCommand, ValidationResult>,
                                                       IRequestHandler<CampaignStatusDeleteCommand, ValidationResult>,
                                                       IRequestHandler<CampaignStatusEditCommand, ValidationResult>,
                                                       IRequestHandler<CampaignStatusSortCommand, ValidationResult>
{
    private readonly ICampaignStatusRepository _CampaignStatusRepository;
    private readonly IContextUser _context;

    public CampaignStatusCommandHandler(ICampaignStatusRepository CampaignStatusRepository, IContextUser contextUser)
    {
        _CampaignStatusRepository = CampaignStatusRepository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _CampaignStatusRepository.Dispose();
    }

    public async Task<ValidationResult> Handle(CampaignStatusAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_CampaignStatusRepository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new CampaignStatus
        {
            Id = request.Id,
            CampaignId = request.CampaignId,
            Name = request.Name,
            Color = request.Color,
            Description = request.Description,
            IsDefault = request.IsDefault,
            IsClose = request.IsClose,
            Status = request.Status,
            DisplayOrder = request.DisplayOrder,
            CreatedDate = createdDate,
            CreatedBy = createdBy,
            CreatedByName = createName
        };

        _CampaignStatusRepository.Add(item);
        return await Commit(_CampaignStatusRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CampaignStatusDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_CampaignStatusRepository))
            return request.ValidationResult;
        var item = new CampaignStatus
        {
            Id = request.Id
        };

        _CampaignStatusRepository.Remove(item);
        return await Commit(_CampaignStatusRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CampaignStatusEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_CampaignStatusRepository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _CampaignStatusRepository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "CampaignStatus is not exist") } };
        }
        item.CampaignId = request.CampaignId;
        item.Name = request.Name;
        item.Color = request.Color;
        item.Description = request.Description;
        item.IsDefault = request.IsDefault;
        item.IsClose = request.IsClose;
        item.Status = request.Status;
        item.DisplayOrder = request.DisplayOrder;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;
        item.UpdatedDate = updatedDate;

        _CampaignStatusRepository.Update(item);
        return await Commit(_CampaignStatusRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CampaignStatusSortCommand request, CancellationToken cancellationToken)
    {
        var data = await _CampaignStatusRepository.GetAll();

        List<CampaignStatus> list = new List<CampaignStatus>();

        foreach (var sort in request.SortList)
        {
            CampaignStatus obj = data.FirstOrDefault(c => c.Id == sort.Id);
            if (obj != null)
            {
                obj.DisplayOrder = sort.SortOrder;
                list.Add(obj);
            }
        }
        _CampaignStatusRepository.Update(list);
        return await Commit(_CampaignStatusRepository.UnitOfWork);
    }
}
