﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace VFi.Domain.SO.Models
{
    public partial class PriceListSurcharge : Entity, IAggregateRoot
    {
        public string Note { get; set; }
        public Guid RouterShippingId { get; set; }
        public string RouterShipping { get; set; }
        public Guid? SurchargeGroupId { get; set; }
        public string SurchargeGroup { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public int? Status { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }
        public int? DisplayOrder { get; set; }

        public  RouteShipping RouterShippingNavigation { get; set; }
        public  SurchargeGroup SurchargeGroupNavigation { get; set; }
    }
}