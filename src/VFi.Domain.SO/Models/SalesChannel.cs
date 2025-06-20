﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace VFi.Domain.SO.Models
{
    public partial class SalesChannel : Entity, IAggregateRoot
    {
        public SalesChannel()
        {
            Order = new HashSet<Order>();
            RequestQuote = new HashSet<RequestQuote>();
        }
         
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public bool? IsDefault { get; set; }
        public int? DisplayOrder { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }

        public  ICollection<Order> Order { get; set; }
        public  ICollection<RequestQuote> RequestQuote { get; set; }
    }
}