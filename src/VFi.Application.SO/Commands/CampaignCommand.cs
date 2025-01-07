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

public class CampaignCommand : Command
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

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
    public List<CampaignStatusDto>? Details { get; set; }
}

public class CampaignAddCommand : CampaignCommand
{
    public CampaignAddCommand(
        Guid id,
        string? code,
        string? name,
        string? description,
        DateTime? startDate,
        DateTime? endDate,
        Guid? leader,
        string? leaderName,
        string? member,
        int? status,
        List<CampaignStatusDto>? details
        )
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Leader = leader;
        LeaderName = leaderName;
        Member = member;
        Status = status;
        Details = details;
    }
    public bool IsValid(ICampaignRepository _context)
    {
        ValidationResult = new CampaignAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CampaignEditCommand : CampaignCommand
{
    public List<ListId>? Delete { get; set; }
    public CampaignEditCommand(
        Guid id,
        string? code,
        string? name,
        string? description,
        DateTime? startDate,
        DateTime? endDate,
        Guid? leader,
        string? leaderName,
        string? member,
        int? status,
        List<CampaignStatusDto>? details,
        List<ListId>? delete
        )
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Leader = leader;
        LeaderName = leaderName;
        Member = member;
        Status = status;
        Details = details;
        Delete = delete;
    }
    public bool IsValid(ICampaignRepository _context)
    {
        ValidationResult = new CampaignEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CampaignDeleteCommand : CampaignCommand
{
    public CampaignDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ICampaignRepository _context)
    {
        ValidationResult = new CampaignDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class CampaignSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public CampaignSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
