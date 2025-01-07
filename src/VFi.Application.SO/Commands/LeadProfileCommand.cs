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

public class LeadProfileCommand : Command
{
    public Guid Id { get; set; }

    public Guid? LeadId { get; set; }

    public string? Key { get; set; }

    public string? Value { get; set; }

    public string? Description { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? CreatedByName { get; set; }

    public string? UpdatedByName { get; set; }
}

public class LeadProfileAddCommand : LeadProfileCommand
{
    public LeadProfileAddCommand(
        Guid id,
        Guid? leadId,
        string? key,
        string? value,
        string? description
     )
    {
        Id = id;
        LeadId = leadId;
        Key = key;
        Value = value;
        Description = description;
    }
    public bool IsValid(ILeadProfileRepository _context)
    {
        ValidationResult = new LeadProfileAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class LeadProfileEditCommand : LeadProfileCommand
{
    public List<ListId>? Delete { get; set; }
    public LeadProfileEditCommand(
        Guid id,
        Guid? leadId,
        string? key,
        string? value,
        string? description
        )
    {
        Id = id;
        LeadId = leadId;
        Key = key;
        Value = value;
        Description = description;
    }
    public bool IsValid(ILeadProfileRepository _context)
    {
        ValidationResult = new LeadProfileEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class LeadProfileDeleteCommand : LeadProfileCommand
{
    public LeadProfileDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ILeadProfileRepository _context)
    {
        ValidationResult = new LeadProfileDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class LeadProfileSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public LeadProfileSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}
