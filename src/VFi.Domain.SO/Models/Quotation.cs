﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace VFi.Domain.SO.Models
{
    public partial class Quotation : Entity, IAggregateRoot
    {
        public Quotation()
        {
            Contract = new HashSet<Contract>();
            Order = new HashSet<Order>();
            OrderProduct = new HashSet<OrderProduct>();
            OrderServiceAdd = new HashSet<OrderServiceAdd>();
            QuotationAttachment = new HashSet<QuotationAttachment>();
        }
         
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public Guid? CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Guid? StoreId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public Guid? ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string DeliveryNote { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryCountry { get; set; }
        public string DeliveryProvince { get; set; }
        public string DeliveryDistrict { get; set; }
        public string DeliveryWard { get; set; }
        public int? DeliveryStatus { get; set; }
        public bool? IsBill { get; set; }
        public string BillName { get; set; }
        public string BillAddress { get; set; }
        public string BillCountry { get; set; }
        public string BillProvince { get; set; }
        public string BillDistrict { get; set; }
        public string BillWard { get; set; }
        public int? BillStatus { get; set; }
        public Guid? ShippingMethodId { get; set; }
        public string ShippingMethodCode { get; set; }
        public string ShippingMethodName { get; set; }
        public Guid? DeliveryMethodId { get; set; }
        public string DeliveryMethodCode { get; set; }
        public string DeliveryMethodName { get; set; }
        /// <summary>
        /// Ngày nhận hàng dự kiến
        /// </summary>
        public DateTime? ExpectedDate { get; set; }
        public string Currency { get; set; }
        public string CurrencyName { get; set; }
        public string Calculation { get; set; }
        public decimal? ExchangeRate { get; set; }
        public Guid? PriceListId { get; set; }
        public string PriceListName { get; set; }
        public Guid? RequestQuoteId { get; set; }
        public string RequestQuoteCode { get; set; }
        public Guid? ContractId { get; set; }
        public Guid? SaleOrderId { get; set; }
        public Guid? QuotationTermId { get; set; }
        public string QuotationTermContent { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public Guid? GroupEmployeeId { get; set; }
        public string GroupEmployeeName { get; set; }
        public Guid? AccountId { get; set; }
        public string AccountName { get; set; }
        public int? TypeDiscount { get; set; }
        public double? DiscountRate { get; set; }
        public int? TypeCriteria { get; set; }
        public decimal? AmountDiscount { get; set; }
        public string Note { get; set; }
        public DateTime? ApproveDate { get; set; }
        public Guid? ApproveBy { get; set; }
        public string ApproveByName { get; set; }
        public string ApproveComment { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }
        public string PurchaseGroup { get; set; }
        public int? BuyFee { get; set; }
        public string RouterShipping { get; set; }
        public string CommodityGroup { get; set; }
        public decimal? AirFreight { get; set; }
        public decimal? SeaFreight { get; set; }
        public decimal? Surcharge { get; set; }
        public decimal? TotalAmountTax { get; set; }
        public string File { get; set; }
        public Guid? OldId { get; set; }
        public string? OldCode { get; set; }
        public  RequestQuote RequestQuote { get; set; }
        public  ShippingMethod ShippingMethod { get; set; }
        public  ICollection<Contract> Contract { get; set; }
        public  ICollection<Order> Order { get; set; }
        public  ICollection<OrderProduct> OrderProduct { get; set; }
        public  ICollection<OrderServiceAdd> OrderServiceAdd { get; set; }
        public  ICollection<QuotationAttachment> QuotationAttachment { get; set; }
    }
}