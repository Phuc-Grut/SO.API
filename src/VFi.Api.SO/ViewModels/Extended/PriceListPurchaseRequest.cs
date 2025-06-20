﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.Application.SO.DTOs;
using VFi.NetDevPack.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace VFi.Api.SO.ViewModels
{

    public class AddPriceListPurchaseRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? Default { get; set; }
        public List<EditPriceListPurchaseDetailRequest> Details { get; set; } = new List<EditPriceListPurchaseDetailRequest>();
    }

    public class EditPriceListPurchaseRequest : AddPriceListPurchaseRequest
    {
    }

    public class PriceListPurchasePagingRequest : FilterQuery
    {
    }

    public class PriceListPurchaseAccountPagingRequest : FilterQuery
    {
        [FromQuery(Name = "$accountId")]
        public Guid AccountId { get; set; }
    }

    public class PriceListPurchaseListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$date")]
        public DateTime? Date { get; set; }
        [FromQuery(Name = "$Id")]
        public Guid? ProductId { get; set; }
        [FromQuery(Name = "$quantity")]
        public double? Quantity { get; set; }
        public PriceListPurchaseParams ToBaseQuery() => new PriceListPurchaseParams
        {
            Status = Status,
            Date = Date,
            ProductId = ProductId,
            Quantity = Quantity
        };
    }
}
