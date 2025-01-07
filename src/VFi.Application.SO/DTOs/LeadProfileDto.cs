using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;

namespace VFi.Application.SO.DTOs;

public class LeadProfileDto
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
