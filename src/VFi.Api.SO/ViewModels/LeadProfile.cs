using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class LeadProfileRequest : FopPagingRequest
{
}

public class AddLeadProfileRequest
{
    public Guid? LeadId { get; set; }

    public string? Key { get; set; }

    public string? Value { get; set; }

    public string? Description { get; set; }

}
public class EditLeadProfileRequest : AddLeadProfileRequest
{
    public Guid Id { get; set; }
}
