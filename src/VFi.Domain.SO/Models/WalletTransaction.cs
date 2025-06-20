﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace VFi.Domain.SO.Models
{
    public partial class WalletTransaction : Entity, IAggregateRoot
    {
        public Guid AccountId { get; set; }
        public Guid? WalletId { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// DEPOSIT, PAY_ORDER, PAY, REFUND_PAY_ORDER
        /// </summary>
        public string Type { get; set; }
        public string Method { get; set; }
        public decimal Amount { get; set; }
        public decimal? Balance { get; set; }
        public string Note { get; set; }
        public DateTime? ApplyDate { get; set; }
        public int Status { get; set; }
        public int? RefundStatus { get; set; }
        public decimal? RefundAmount { get; set; }
        public string RawData { get; set; }
        public Guid? RefId { get; set; }
        public string RefType { get; set; }
        public string RefCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedByName { get; set; }
        public string CreatedByName { get; set; }
        public string Document { get; set; }
        public string Hash { get; set; }
        public Guid? PaymentInvoiceId { get; set; }
        public string PaymentInvoiceCode { get; set; }

        public  Wallet Wallet { get; set; }
    }
}