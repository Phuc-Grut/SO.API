using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddPromotionRequest
{
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
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
    public string? CustomerGroups { get; set; }
    public string? Customers { get; set; }
    public List<AddPromotionByValueRequest>? Details { get; set; }
}
public class EditPromotionRequest : AddPromotionRequest
{
    public Guid Id { get; set; }
    public List<DeletePromotionByValueRequest>? Deletes { get; set; }
    public List<DeletePromotionProductRequest>? DeleteBonus { get; set; }
    public List<DeletePromotionProductRequest>? DeleteBuy { get; set; }
}
public class PromotionPagingRequest : PagingRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$date")]
    public DateTime? Date { get; set; }
    public PromotionParams ToBaseQuery() => new PromotionParams
    {
        Status = Status,
        Date = Date
    };
}
