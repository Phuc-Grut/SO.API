using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class SalesChannelDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public bool? IsDefault { get; set; }
    public int? DisplayOrder { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
}
