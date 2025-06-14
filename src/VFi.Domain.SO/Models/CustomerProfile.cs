﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace VFi.Domain.SO.Models
{
    public partial class CustomerProfile : Entity, IAggregateRoot
    {
        public Guid CustomerId { get; set; }
        public string Group { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public string CreatedByName { get; set; }
        public Guid? AccountId { get; set; }

        public  Customer Customer { get; set; }
    }
}