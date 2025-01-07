using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class LeadCampaignRequest : FopPagingRequest
{
    [FromQuery(Name = "$isState")]
    public int? IsState { get; set; }
    [FromQuery(Name = "$leader")]
    public Guid? Leader { get; set; }
    [FromQuery(Name = "$campaignId")]
    public Guid? CampaignId { get; set; }
    [FromQuery(Name = "$employeeId")]
    public string? EmployeeId { get; set; }
}

public class AddLeadCampaignRequest
{
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

    public List<string> Member { get; set; }

    public int? Status { get; set; }

}
public class UpdateStatusRequest
{
    public Guid Id { get; set; }

    public int? Status { get; set; }

}
public class EditLeadCampaignRequest : AddLeadCampaignRequest
{
    public Guid Id { get; set; }
}
public class AddLead
{
    public List<AddLeadCampaignRequest> Data { get; set; }
}
public class EditLead : AddLeadCampaignRequest
{
    public Guid Id { get; set; }
    public List<Guid> Data { get; set; }
}
public class EditStatus
{
    public List<UpdateStatusRequest> Data { get; set; }
}
public class EditState
{
    public Guid Id { get; set; }
    public Guid? StateId { get; set; }
    public string? State { get; set; }
}
public class DeleteLead
{
    public List<Guid> Data { get; set; }
}

public class LeadCampaignSendTransactionRequest
{
    public string? Keyword { get; set; }
    public string? Campaign { get; set; }
    public string? To { get; set; }
}

public class LeadCampaignSendEmailRequest : EmailNotifyRequest
{
    public string Campaign { get; set; }
}
