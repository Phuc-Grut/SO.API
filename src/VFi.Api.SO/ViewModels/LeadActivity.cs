using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class LeadActivityRequest : FopPagingRequest
{
}

public class AddLeadActivityRequest
{
    public Guid? LeadId { get; set; }

    public Guid? CampaignId { get; set; }

    public string? Campaign { get; set; }

    public string? Type { get; set; }

    public string? Name { get; set; }

    public string? Body { get; set; }

    public DateTime? ActualDate { get; set; }

    public List<FileRequest>? Attachment { get; set; }

    public int? Status { get; set; }
}
public class EditLeadActivityRequest : AddLeadActivityRequest
{
    public Guid Id { get; set; }
}
