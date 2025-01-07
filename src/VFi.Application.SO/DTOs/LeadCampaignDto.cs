using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;

namespace VFi.Application.SO.DTOs;

public class LeadCampaignDto
{
    public Guid Id { get; set; }

    public Guid? LeadId { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string Name { get; set; }

    public Guid? CampaignId { get; set; }

    public string Campaign { get; set; }

    /// <summary>
    /// Bước chăm sóc
    /// </summary>
    public Guid? StateId { get; set; }

    public string State { get; set; }

    public DateTime? StateDate { get; set; }

    public Guid? Leader { get; set; }

    public string LeaderName { get; set; }

    public string Image { get; set; }

    public List<string> Member { get; set; }

    public List<Employee> ListMember { get; set; }

    public int? Status { get; set; }

    public string CustomerCode { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string CreatedByName { get; set; }

    public string UpdatedByName { get; set; }


}
