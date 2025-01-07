using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class LeadActivityCommand : Command
{
    public Guid Id { get; set; }

    public Guid? LeadId { get; set; }

    public Guid? CampaignId { get; set; }

    public string? Campaign { get; set; }

    public string? Type { get; set; }

    public string? Name { get; set; }

    public string? Body { get; set; }

    public DateTime? ActualDate { get; set; }

    public string Attachment { get; set; }

    public int? Status { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string CreatedByName { get; set; }
}

public class LeadActivityAddCommand : LeadActivityCommand
{
    public LeadActivityAddCommand(
        Guid id,
        Guid? leadId,
        Guid? campaignId,
        string? campaign,
        string? type,
        string? name,
        string? body,
        DateTime? actualDate,
        string? attachment,
        int? status
        )
    {
        Id = id;
        LeadId = leadId;
        Name = name;
        CampaignId = campaignId;
        Campaign = campaign;
        Type = type;
        Name = name;
        Body = body;
        ActualDate = actualDate;
        Attachment = attachment;
        Status = status;
    }
    public bool IsValid(ILeadActivityRepository _context)
    {
        ValidationResult = new LeadActivityAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class LeadActivityEditCommand : LeadActivityCommand
{
    public List<ListId>? Delete { get; set; }
    public LeadActivityEditCommand(
        Guid id,
        Guid? leadId,
        Guid? campaignId,
        string? campaign,
        string? type,
        string? name,
        string? body,
        DateTime? actualDate,
        string? attachment,
        int? status
        )
    {
        Id = id;
        LeadId = leadId;
        Name = name;
        CampaignId = campaignId;
        Campaign = campaign;
        Type = type;
        Name = name;
        Body = body;
        ActualDate = actualDate;
        Attachment = attachment;
        Status = status;
    }
    public bool IsValid(ILeadActivityRepository _context)
    {
        ValidationResult = new LeadActivityEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class LeadActivityDeleteCommand : LeadActivityCommand
{
    public LeadActivityDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ILeadActivityRepository _context)
    {
        ValidationResult = new LeadActivityDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class LeadActivitySortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public LeadActivitySortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
