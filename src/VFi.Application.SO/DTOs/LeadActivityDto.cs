using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;

namespace VFi.Application.SO.DTOs;

public class LeadActivityDto
{
    public Guid Id { get; set; }

    public Guid? LeadId { get; set; }

    public Guid? CampaignId { get; set; }

    public string? Campaign { get; set; }

    public string? Type { get; set; }

    public string? Name { get; set; }

    public string? Body { get; set; }

    public DateTime? ActualDate { get; set; }

    public List<FileDto>? Attachment { get; set; }

    public int? Status { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? CreatedByName { get; set; }

    public string? UpdatedByName { get; set; }
}
