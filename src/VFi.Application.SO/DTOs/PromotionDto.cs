using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;

namespace VFi.Application.SO.DTOs;

public class PromotionDto
{
    public Guid Id { get; set; }
    public Guid? PromotionGroupId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string? Stores { get; set; }
    public string? SalesChannel { get; set; }
    public bool? ApplyTogether { get; set; }
    public bool? ApplyAllCustomer { get; set; }
    public int? Type { get; set; }
    public int? PromotionMethod { get; set; }
    public bool? UsingCode { get; set; }
    public bool? ApplyBirthday { get; set; }
    public string? PromotionalCode { get; set; }
    public bool? IsLimit { get; set; }
    public double? PromotionLimit { get; set; }
    public bool? Applytax { get; set; }
    public int? DisplayType { get; set; }
    public int? PromotionBase { get; set; }
    public int? ObjectApply { get; set; }
    public int? Condition { get; set; }
    public int? Apply { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }
    public string? CreatedByName { get; set; }
    public List<PromotionByValueDto>? Details { get; set; }
    public List<PromotionCustomerGroupDto>? ListGroup { get; set; }
    public string? Groups { get; set; }
    public List<PromotionCustomerDto>? ListCustomer { get; set; }
    public string? CustomerName { get; set; }
}
public class PromotionCustomerGroupDto
{
    public Guid? Value { get; set; }
}
public class PromotionCustomerDto
{
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
}
public class PromotionParams
{
    public int? Status { get; set; }
    public DateTime? Date { get; set; }
}
