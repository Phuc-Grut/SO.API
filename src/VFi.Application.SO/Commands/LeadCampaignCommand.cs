using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using DocumentFormat.OpenXml.Office2010.Excel;
using FluentValidation;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class LeadCampaignCommand : Command
{
    public Guid Id { get; set; }

    public Guid? LeadId { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Name { get; set; }

    public Guid? CampaignId { get; set; }

    public string? Campaign { get; set; }

    public Guid? StateId { get; set; }

    public string? State { get; set; }

    public Guid? Leader { get; set; }

    public string? LeaderName { get; set; }

    public string? Member { get; set; }

    public int? Status { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? CreatedByName { get; set; }

    public string? UpdatedByName { get; set; }
}

public class UpdateStatusCommand : Command
{
    public Guid Id { get; set; }

    public int? Status { get; set; }
}

public class LeadCampaignAddCommand : LeadCampaignCommand
{
    public LeadCampaignAddCommand(
        Guid id,
        Guid? leadId,
        string? email,
        string? phone,
        string? name,
        Guid? campaignId,
        string? campaign,
        Guid? stateId,
        string? state,
        Guid? leader,
        string? leaderName,
        string? member,
        int? status
        )
    {
        Id = id;
        LeadId = leadId;
        Email = email;
        Phone = phone;
        Name = name;
        CampaignId = campaignId;
        Campaign = campaign;
        StateId = stateId;
        State = state;
        Leader = leader;
        LeaderName = leaderName;
        Member = member;
        Status = status;
    }
    public bool IsValid(ILeadCampaignRepository _context)
    {
        ValidationResult = new LeadCampaignAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class LeadCampaignEditCommand : LeadCampaignCommand
{
    public List<ListId>? Delete { get; set; }
    public LeadCampaignEditCommand(
        Guid id,
        Guid? leadId,
        string? email,
        string? phone,
        string? name,
        Guid? campaignId,
        string? campaign,
        Guid? stateId,
        string? state,
        Guid? leader,
        string? leaderName,
        string? member,
        int? status
        )
    {
        Id = id;
        LeadId = leadId;
        Email = email;
        Phone = phone;
        Name = name;
        CampaignId = campaignId;
        Campaign = campaign;
        StateId = stateId;
        State = state;
        Leader = leader;
        LeaderName = leaderName;
        Member = member;
        Status = status;
    }
    public bool IsValid(ILeadCampaignRepository _context)
    {
        ValidationResult = new LeadCampaignEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class LeadCampaignDeleteCommand : LeadCampaignCommand
{
    public LeadCampaignDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ILeadCampaignRepository _context)
    {
        ValidationResult = new LeadCampaignDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class LeadCampaignSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public LeadCampaignSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
public class AddLeadCampaignCommand : Command
{
    public List<LeadCampaignCommand> Data { get; set; }
    public AddLeadCampaignCommand(
          List<LeadCampaignCommand> data
       )
    {
        Data = data;
    }
}
public class EditLeadCampaignCommand : LeadCampaignCommand
{
    public List<Guid> Data { get; set; }
    public EditLeadCampaignCommand(
        Guid? leadId,
        string? email,
        string? phone,
        string? name,
        Guid? campaignId,
        string? campaign,
        Guid? stateId,
        string? state,
        Guid? leader,
        string? leaderName,
        string? member,
        int? status,
        List<Guid> data
       )
    {
        LeadId = leadId;
        Email = email;
        Phone = phone;
        Name = name;
        CampaignId = campaignId;
        Campaign = campaign;
        StateId = stateId;
        State = state;
        Leader = leader;
        LeaderName = leaderName;
        Member = member;
        Status = status;
        Data = data;
    }
}

public class EditStatusCommand : UpdateStatusCommand
{
    public List<UpdateStatusCommand> Data { get; set; }
    public EditStatusCommand(
        List<UpdateStatusCommand> data
       )
    {
        Data = data;
    }
}

public class EditStateCommand : LeadCampaignCommand
{
    public EditStateCommand(
        Guid id,
        Guid? stateId,
        string? state
       )
    {
        Id = id;
        StateId = stateId;
        State = state;
    }
}

public class DeleteLeadCampaignCommand : Command
{
    public List<Guid> Data { get; set; }
    public DeleteLeadCampaignCommand(
        List<Guid> data
       )
    {
        Data = data;
    }
}

public class LeadCampaignSendEmailCommand : Command
{
    public string Campaign { get; set; }
    public string SenderCode { get; set; }
    public string SenderName { get; set; }
    public string Subject { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string CC { get; set; }
    public string BCC { get; set; }
    public string Body { get; set; }
    public string TemplateCode { get; set; }

    public LeadCampaignSendEmailCommand(
        string campaign,
        string senderCode,
        string senderName,
        string subject,
        string from,
        string to,
        string cC,
        string bCC,
        string body,
        string templateCode)
    {
        Campaign = campaign;
        SenderCode = senderCode;
        SenderName = senderName;
        Subject = subject;
        From = from;
        To = to;
        CC = cC;
        BCC = bCC;
        Body = body;
        TemplateCode = templateCode;
    }
}
