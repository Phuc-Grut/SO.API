using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;

namespace VFi.Application.SO.DTOs;

public class CampaignDto
{
    public Guid Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid? Leader { get; set; }

    public string LeaderName { get; set; }

    public List<string> Member { get; set; }

    public List<Employee> ListMember { get; set; }

    public int? Status { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string CreatedByName { get; set; }

    public string UpdatedByName { get; set; }
    public List<CampaignStatusDto>? Details { get; set; }
    public int? CountLeadCampaigns { get; set; }
}
