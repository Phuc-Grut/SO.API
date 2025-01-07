using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class CampaignRequest : FopPagingRequest
{
    public string? EmployeeId { get; set; }
}

public class ADDCampaignRequest
{
    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid? Leader { get; set; }

    public string? LeaderName { get; set; }

    public List<string> Member { get; set; }

    public int? Status { get; set; }
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }

}
public class AddCampaignRequest : ADDCampaignRequest
{
    public List<AddCampaignStatusRequest>? Details { get; set; }
}
public class EditCampaignRequest : ADDCampaignRequest
{
    public Guid Id { get; set; }
    public List<EditCampaignStatusRequest>? Details { get; set; }
    public List<IdRequest>? Delete { get; set; }
}
