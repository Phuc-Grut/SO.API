﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace VFi.Domain.SO.Models
{
    public partial class Promotion : Entity, IAggregateRoot
    {
        public Promotion()
        {
            PromotionByValue = new HashSet<PromotionByValue>();
            PromotionCustomer = new HashSet<PromotionCustomer>();
            PromotionCustomerGroup = new HashSet<PromotionCustomerGroup>();
        }
         
        public Guid? PromotionGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string Stores { get; set; }
        public string SalesChannel { get; set; }
        public bool? ApplyTogether { get; set; }
        public bool? ApplyAllCustomer { get; set; }
        public int? Type { get; set; }
        public int? PromotionMethod { get; set; }
        public bool? UsingCode { get; set; }
        public bool? ApplyBirthday { get; set; }
        public string PromotionalCode { get; set; }
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
        public string UpdatedByName { get; set; }
        public string CreatedByName { get; set; }

        public  PromotionGroup PromotionGroup { get; set; }
        public  ICollection<PromotionByValue> PromotionByValue { get; set; }
        public  ICollection<PromotionCustomer> PromotionCustomer { get; set; }
        public  ICollection<PromotionCustomerGroup> PromotionCustomerGroup { get; set; }
    }
}