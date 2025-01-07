using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class CustomerAddressDto
{
    public Guid? Id { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool? ShippingDefault { get; set; }
    public bool? BillingDefault { get; set; }
    public string? Note { get; set; }
    public int? Status { get; set; }
    public int? SortOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
}
