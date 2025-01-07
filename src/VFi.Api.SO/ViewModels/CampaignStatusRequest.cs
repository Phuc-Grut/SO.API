using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class CampaignStatusRequest : FopPagingRequest
{
    [FromQuery(Name = "$campaignId")]
    public Guid? CampaignId { get; set; }
}

public class AddCampaignStatusRequest
{
    public Guid? CampaignId { get; set; }

    public string? Name { get; set; }

    public string? Color { get; set; }

    public string? TextColor { get; set; }

    public string? Description { get; set; }

    public bool? IsDefault { get; set; }

    public bool? IsClose { get; set; }

    public int? Status { get; set; }

    public int? DisplayOrder { get; set; }

}
public class EditCampaignStatusRequest : AddCampaignStatusRequest
{
    public Guid? Id { get; set; }
}
