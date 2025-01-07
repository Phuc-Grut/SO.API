using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using MassTransit.Internals.GraphValidation;
using VFi.Application.SO.Commands.Validations;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class PromotionCommand : Command
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
    public string? CustomerGroups { get; set; }
    public string? Customers { get; set; }
    public List<PromotionByValueDto>? Detail { get; set; }
}

public class AddPromotionCommand : PromotionCommand
{
    public Guid? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public AddPromotionCommand(
        Guid id,
        Guid? promotionGroupId,
        string? code,
        string? name,
        string? description,
        int? status,
        DateTime? startDate,
        DateTime? endDate,
        TimeSpan? startTime,
        TimeSpan? endTime,
        string? stores,
        string? salesChannel,
        bool? applyTogether,
        bool? applyAllCustomer,
        int? type,
        int? promotionMethod,
        bool? usingCode,
        bool? applyBirthday,
        string? promotionalCode,
        bool? isLimit,
        double? promotionLimit,
        bool? applytax,
        int? displayType,
        int? promotionBase,
        int? objectApply,
        int? condition,
        int? apply,
        Guid? createdBy,
        string? createdByName,
        string? customerGroups,
        string? customers,
        List<PromotionByValueDto>? detail
        )
    {
        Id = id;
        PromotionGroupId = promotionGroupId;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        StartDate = startDate;
        EndDate = endDate;
        StartTime = startTime;
        EndTime = endTime;
        Stores = stores;
        SalesChannel = salesChannel;
        ApplyTogether = applyTogether;
        ApplyAllCustomer = applyAllCustomer;
        Type = type;
        PromotionMethod = promotionMethod;
        UsingCode = usingCode;
        ApplyBirthday = applyBirthday;
        PromotionalCode = promotionalCode;
        IsLimit = isLimit;
        PromotionLimit = promotionLimit;
        Applytax = applytax;
        DisplayType = displayType;
        PromotionBase = promotionBase;
        ObjectApply = objectApply;
        Condition = condition;
        Apply = apply;
        CreatedBy = createdBy;
        CreatedByName = createdByName;
        CustomerGroups = customerGroups;
        Customers = customers;
        Detail = detail;
    }

    public bool IsValid(IPromotionRepository _context)
    {
        ValidationResult = new AddPromotionValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class EditPromotionCommand : PromotionCommand
{
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }
    public List<DeletePromotionByValueDto>? Delete { get; set; }
    public List<DeletePromotionProductDto>? DeleteBonus { get; set; }
    public List<DeletePromotionProductDto>? DeleteBuy { get; set; }
    public EditPromotionCommand(
        Guid id,
        Guid? promotionGroupId,
        string? code,
        string? name,
        string? description,
        int? status,
        DateTime? startDate,
        DateTime? endDate,
        TimeSpan? startTime,
        TimeSpan? endTime,
        string? stores,
        string? salesChannel,
        bool? applyTogether,
        bool? applyAllCustomer,
        int? type,
        int? promotionMethod,
        bool? usingCode,
        bool? applyBirthday,
        string? promotionalCode,
        bool? isLimit,
        double? promotionLimit,
        bool? applytax,
        int? displayType,
        int? promotionBase,
        int? objectApply,
        int? condition,
        int? apply,
        Guid? updatedBy,
        string? updatedByName,
        string? customerGroups,
        string? customers,
        List<PromotionByValueDto>? detail,
        List<DeletePromotionByValueDto>? delete,
        List<DeletePromotionProductDto>? deleteBonus,
        List<DeletePromotionProductDto>? deleteBuy
        )
    {
        Id = id;
        PromotionGroupId = promotionGroupId;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        StartDate = startDate;
        EndDate = endDate;
        StartTime = startTime;
        EndTime = endTime;
        Stores = stores;
        SalesChannel = salesChannel;
        ApplyTogether = applyTogether;
        ApplyAllCustomer = applyAllCustomer;
        Type = type;
        PromotionMethod = promotionMethod;
        UsingCode = usingCode;
        ApplyBirthday = applyBirthday;
        PromotionalCode = promotionalCode;
        IsLimit = isLimit;
        PromotionLimit = promotionLimit;
        Applytax = applytax;
        DisplayType = displayType;
        PromotionBase = promotionBase;
        ObjectApply = objectApply;
        Condition = condition;
        Apply = apply;
        UpdatedBy = updatedBy;
        UpdatedByName = updatedByName;
        CustomerGroups = customerGroups;
        Customers = customers;
        Detail = detail;
        Delete = delete;
        DeleteBonus = deleteBonus;
        DeleteBuy = deleteBuy;
    }

    public override bool IsValid()
    {
        ValidationResult = new EditPromotionValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class DeletePromotionCommand : PromotionCommand
{
    public DeletePromotionCommand(Guid id)
    {
        Id = id;
    }
    public override bool IsValid()
    {
        ValidationResult = new DetelePromotionValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
