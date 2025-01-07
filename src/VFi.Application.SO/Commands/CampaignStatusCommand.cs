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

public class CampaignStatusCommand : Command
{
    public Guid Id { get; set; }

    public Guid? CampaignId { get; set; }

    public string? Name { get; set; }

    public string? Color { get; set; }

    public string? Description { get; set; }

    public bool? IsDefault { get; set; }

    public bool? IsClose { get; set; }

    public int? Status { get; set; }

    public int? DisplayOrder { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? CreatedByName { get; set; }

    public string? UpdatedByName { get; set; }
}

public class CampaignStatusAddCommand : CampaignStatusCommand
{
    public CampaignStatusAddCommand(
        Guid id,
        Guid? campaignId,
        string? name,
        string? color,
        string? description,
        bool? isDefault,
        bool? isClose,
        int? status,
        int? displayOrder
        )
    {
        Id = id;
        CampaignId = campaignId;
        Name = name;
        Color = color;
        Description = description;
        IsDefault = isDefault;
        IsClose = isClose;
        Status = status;
        DisplayOrder = displayOrder;
    }
    public bool IsValid(ICampaignStatusRepository _context)
    {
        ValidationResult = new CampaignStatusAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CampaignStatusEditCommand : CampaignStatusCommand
{

    public CampaignStatusEditCommand(
        Guid id,
        Guid? campaignId,
        string? name,
        string? color,
        string? description,
        bool? isDefault,
        bool? isClose,
        int? status,
        int? displayOrder)
    {
        Id = id;
        CampaignId = campaignId;
        Name = name;
        Color = color;
        Description = description;
        IsDefault = isDefault;
        IsClose = isClose;
        Status = status;
        DisplayOrder = displayOrder;
    }
    public bool IsValid(ICampaignStatusRepository _context)
    {
        ValidationResult = new CampaignStatusEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CampaignStatusDeleteCommand : CampaignStatusCommand
{
    public CampaignStatusDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ICampaignStatusRepository _context)
    {
        ValidationResult = new CampaignStatusDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class CampaignStatusSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public CampaignStatusSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
