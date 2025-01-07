using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;

namespace VFi.Application.SO.DTOs;

public class QuotationDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public Guid? StoreId { get; set; }
    public string? StoreCode { get; set; }
    public string? StoreName { get; set; }
    public Guid? ChannelId { get; set; }
    public string? ChannelName { get; set; }
    public string? DeliveryNote { get; set; }
    public string? DeliveryName { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryCountry { get; set; }
    public string? DeliveryProvince { get; set; }
    public string? DeliveryDistrict { get; set; }
    public string? DeliveryWard { get; set; }
    public int? DeliveryStatus { get; set; }
    public bool? IsBill { get; set; }
    public string? BillName { get; set; }
    public string? BillAddress { get; set; }
    public string? BillCountry { get; set; }
    public string? BillProvince { get; set; }
    public string? BillDistrict { get; set; }
    public string? BillWard { get; set; }
    public int? BillStatus { get; set; }
    public Guid? ShippingMethodId { get; set; }
    public string? ShippingMethodCode { get; set; }
    public string? ShippingMethodName { get; set; }
    public Guid? DeliveryMethodId { get; set; }
    public string? DeliveryMethodCode { get; set; }
    public string? DeliveryMethodName { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public string? Calculation { get; set; }
    public decimal? ExchangeRate { get; set; }
    public Guid? PriceListId { get; set; }
    public string? PriceListName { get; set; }
    public Guid? RequestQuoteId { get; set; }
    public string? RequestQuoteCode { get; set; }
    public Guid? ContractId { get; set; }
    public Guid? SaleOrderId { get; set; }
    public Guid? QuotationTermId { get; set; }
    public string? TermName { get; set; }
    public string? QuotationTermContent { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? ExpiredDate { get; set; }
    public Guid? GroupEmployeeId { get; set; }
    public string? GroupEmployeeName { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public int? TypeDiscount { get; set; }
    public double? DiscountRate { get; set; }
    public int? TypeCriteria { get; set; }
    public decimal? AmountDiscount { get; set; }
    public string? Note { get; set; }
    public DateTime? ApproveDate { get; set; }
    public Guid? ApproveBy { get; set; }
    public string? ApproveComment { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public decimal? TotalAmountTax { get; set; }
    public List<FileDto>? File { get; set; }
    public decimal? TotalDiscountAmount { get; set; }
    public double? TotalAmount { get; set; }
    public decimal? TotalServiceAmount { get; set; }
    public Guid? OldId { get; set; }
    public string? OldCode { get; set; }
    public List<OrderProductDto>? OrderProduct { get; set; }
    public List<OrderServiceAddDto>? OrderServiceAdd { get; set; }
}
public class QuotationParams
{
    public int? Status { get; set; }
    public DateTime? Date { get; set; }
    public Guid? CustomerId { get; set; }
    public int? IsContract { get; set; }
    public int? IsOrder { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Currency { get; set; }
}
public class QuotationListBoxDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public DateTime? Date { get; set; }
    public Guid? ContractId { get; set; }
    public string? ContractCode { get; set; }
    public string? ContractName { get; set; }
    public string? AccountName { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public Guid? PriceListId { get; set; }
    public string? PriceListName { get; set; }
    public int? TypeDiscount { get; set; }
    public double? DiscountRate { get; set; }
    public int? TypeCriteria { get; set; }
    public decimal? AmountDiscount { get; set; }
    public decimal? TotalAmountTax { get; set; }
    public int? Status { get; set; }
}
public class QuotationPrintDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string? CustomerName { get; set; }
    public string? Address { get; set; }
    public string? Date { get; set; }
    public string? ExpiredDate { get; set; }
    public string? Note { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public string? CreatedDateString { get; set; }
    public string? PrintedDateString { get; set; }
    public string? TotalAmountNoTax { get; set; }
    public string? TotalTaxValue { get; set; }
    public string? TotalDiscount { get; set; }
    public string? TotalAmount { get; set; }
    public string? TotalAmountText { get; set; }
    public List<QuotationDetailPrintDto>? Details { get; set; }
    public List<OrderServiceAddPrintDto>? OrderServiceAdd { get; set; }
}
public class QuotationDetailPrintDto
{
    public Guid? Id { get; set; }
    public string ProductCode { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string? UnitName { get; set; }
    public string? Tax { get; set; }
    public string? Quantity { get; set; }
    public string? UnitPrice { get; set; }
    public string? AmountNoVat { get; set; }
    public decimal? AmountNoDiscount { get; set; }
    public decimal? DiscountAmountDistribution { get; set; }
    public decimal? TotalAmountDiscount { get; set; }
    public decimal? AmountNoTax { get; set; }
    public decimal? AmountTax { get; set; }
    public decimal? TotalAmountTax { get; set; }
    public int? SortOrder { get; set; }
}
public class QuotationValidateDto
{
    public UInt32 Row { get; set; }
    public bool IsValid
    {
        get
        {
            return Errors.Count == 0;
        }
    }
    public List<string> Errors { get; set; } = new List<string>();
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? StockQuantity { get; set; }
    public Guid? UnitId { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public string? Quantity { get; set; }
    public string? UnitPrice { get; set; }
    public string? Note { get; set; }
    public string? TaxCategoryId { get; set; }
    public string? Tax { get; set; }
    public string? TaxCode { get; set; }
    public string? DiscountPercent { get; set; }

    public string? TaxRate { get; set; }
}
