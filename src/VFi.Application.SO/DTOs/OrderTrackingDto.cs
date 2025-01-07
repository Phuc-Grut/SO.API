using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class OrderTrackingDto
{
    public Guid? Id { get; set; }
    public Guid? OrderId { get; set; }
    public Guid? OrderExpressId { get; set; }
    public string? Name { get; set; }
    public int? Status { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public DateTime? TrackingDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
}
public class DeleteOrderTrackingDto
{
    public Guid Id { get; set; }
}
public class OrderTrackingQueryParams
{
    public int? Status { get; set; }
    public Guid? OrderId { get; set; }
}
