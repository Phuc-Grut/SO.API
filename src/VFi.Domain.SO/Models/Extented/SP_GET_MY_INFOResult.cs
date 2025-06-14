﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VFi.Domain.SO.Models
{
    public partial class SP_GET_MY_INFOResult
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public int? Level { get; set; }
        public bool? BidActive { get; set; }
        public int? BidQuantity { get; set; }
        public string IdName { get; set; }
        public string IdNumber { get; set; }
        public string IdImage1 { get; set; }
        public string IdImage2 { get; set; }
        public int? IdStatus { get; set; }
        public bool? TranActive { get; set; }
        public decimal? Cash { get; set; }
        public decimal? CashHold { get; set; }
        public decimal? CashHoldBid { get; set; }
    }
}
