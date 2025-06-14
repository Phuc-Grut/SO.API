﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace VFi.Domain.SO.Models
{
    public partial class Lead : Entity, IAggregateRoot
    {
        public Guid Id { get; set; }

        public string Source { get; set; }

        public string Code { get; set; }

        public string Image { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Country { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public string Ward { get; set; }

        public string ZipCode { get; set; }

        public string Address { get; set; }

        public string Website { get; set; }

        public string TaxCode { get; set; }

        public string BusinessSector { get; set; }

        public string Company { get; set; }

        public string CompanyPhone { get; set; }

        public string CompanyName { get; set; }

        public string CompanySize { get; set; }

        public decimal? Capital { get; set; }

        public DateTime? EstablishedDate { get; set; }

        public string Tags { get; set; }

        public string Note { get; set; }

        public int? Status { get; set; }

        public Guid? GroupId { get; set; }

        public string Group { get; set; }

        public Guid? EmployeeId { get; set; }

        public string Employee { get; set; }

        public Guid? GroupEmployeeId { get; set; }

        public string GroupEmployee { get; set; }

        public int? Gender { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public int? Day { get; set; }

        public string Facebook { get; set; }

        public string Zalo { get; set; }

        public decimal? RevenueTarget { get; set; }

        public int? Revenue { get; set; }

        public string Scale { get; set; }

        public int? Difficult { get; set; }

        public int? Point { get; set; }

        public int? Priority { get; set; }

        public string Demand { get; set; }

        public string DynamicData { get; set; }

        public int? Converted { get; set; }

        public string CustomerCode { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string CreatedByName { get; set; }

        public string UpdatedByName { get; set; }

        public virtual ICollection<LeadActivity> LeadActivities { get; set; } = new List<LeadActivity>();

        public virtual ICollection<LeadCampaign> LeadCampaigns { get; set; } = new List<LeadCampaign>();

        public virtual ICollection<LeadProfile> LeadProfiles { get; set; } = new List<LeadProfile>();

        public virtual ICollection<LeadTopicSuggest> LeadTopicSuggests { get; set; } = new List<LeadTopicSuggest>();
    }
}