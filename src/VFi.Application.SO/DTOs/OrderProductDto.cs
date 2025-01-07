using VFi.NetDevPack.Utilities;

namespace VFi.Application.SO.DTOs;

public partial class OrderProductDto
{


    public OrderProductDto(string? calculation = null, decimal? exchangeRate = null)
    {
        this.Calculation = calculation;
        this.ExchangeRate = exchangeRate;
    }
    private string? Calculation { get; set; }
    private decimal? ExchangeRate { get; set; }
    public Guid? Id { get; set; }
    public Guid? OrderId { get; set; }
    public Guid? Guid { get; set; }
    public string? OrderCode { get; set; }
    public Guid? ContractId { get; set; }
    public string? ContractName { get; set; }
    public Guid? QuotationId { get; set; }
    public string? QuotationName { get; set; }
    public Guid ProductId { get; set; }
    public string ProductCode { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string? ProductImage { get; set; }
    public decimal? ProductPrice { get; set; }
    public string? ProductCurrency { get; set; }
    public string? Origin { get; set; }
    public Guid? WarehouseId { get; set; }
    public string? WarehouseCode { get; set; }
    public string? WarehouseName { get; set; }
    public decimal? StockQuantity { get; set; }
    public decimal? TotalStockQuantity { get; set; }
    public decimal? ReservedQuantity { get; set; }
    public decimal? PlannedQuantity { get; set; }
    public string? Currency { get; set; }
    public Guid? PriceListId { get; set; }
    public string? PriceListName { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? DiscountAmountDistribution { get; set; }
    public decimal? DiscountAmountDistributionQD
    {
        get
        {
            return DiscountAmountDistribution * Utilities.GetExchangeRate(Calculation, ExchangeRate);
        }
    }
    public int? DiscountType { get; set; }
    public double? DiscountPercent { get; set; }
    public decimal? AmountDiscount { get; set; }
    public decimal? AmountDiscountQD
    {
        get
        {
            return AmountDiscount * Utilities.GetExchangeRate(Calculation, ExchangeRate);
        }
    }
    public double? DiscountTotal { get; set; }
    public double? TaxRate { get; set; }
    public string? Tax { get; set; }
    public string? TaxCode { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string? Note { get; set; }
    public decimal? QuantitySales { get; set; }
    public decimal? QuantitySaleActual { get; set; }
    public decimal? QuantityReturned { get; set; }
    public decimal? QuantityReturnedActual { get; set; }
    public decimal? QuantityExported { get; set; }
    public decimal? QuantityPurchased { get; set; }
    public double? QuantityProductioned { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public int? DeliveryStatus { get; set; }
    public int? DeliveryQuantity { get; set; }
    public bool? IsDiscount { get; set; }
    public double? RemainQty { get; set; }
    public double? SellQty { get; set; }
    public int? SortOrder { get; set; }
    public decimal? AmountNoDiscount
    {
        get
        {
            return Quantity * UnitPrice;
        }
    }
    public decimal? AmountNoDiscountQD
    {
        get
        {
            return AmountNoDiscount * Utilities.GetExchangeRate(Calculation, ExchangeRate);
        }
    }
    public decimal? TotalAmountDiscount
    {
        get
        {
            return (DiscountAmountDistribution ?? 0) + (AmountDiscount ?? 0);
        }
    }
    public decimal? TotalAmountDiscountQD
    {
        get
        {
            return TotalAmountDiscount * Utilities.GetExchangeRate(Calculation, ExchangeRate);
        }
    }
    public decimal? AmountNoTax
    {
        get
        {
            return AmountNoDiscount - TotalAmountDiscount;
        }
    }
    public decimal? AmountNoTaxQD
    {
        get
        {
            return AmountNoTax * Utilities.GetExchangeRate(Calculation, ExchangeRate);
        }
    }
    public decimal? AmountTax
    {
        get
        {
            if (Domain.SO.Constants.OrderCurrency._currencyListRound.Contains(Currency!))
            {
                return Math.Ceiling(AmountNoTax.GetValueOrDefault() * (decimal)(TaxRate ?? 0) / 100);
            }
            return AmountNoTax * (decimal)(TaxRate ?? 0) / 100;
        }
    }
    public decimal? AmountTaxQD
    {
        get
        {
            return AmountTax * Utilities.GetExchangeRate(Calculation, ExchangeRate);
        }
    }
    public decimal? TotalAmountTax
    {
        get
        {
            return AmountNoTax + AmountTax;
        }
    }
    public decimal? TotalAmountTaxQD
    {
        get
        {
            return TotalAmountTax * Utilities.GetExchangeRate(Calculation, ExchangeRate);
        }
    }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public decimal? ReturnAmount { get; set; }
    public decimal? SaleAmount { get; set; }
    public string? SpecificationCode1 { get; set; }
    public string? SpecificationCode2 { get; set; }
    public string? SpecificationCode3 { get; set; }
    public string? SpecificationCode4 { get; set; }
    public string? SpecificationCode5 { get; set; }
    public string? SpecificationCode6 { get; set; }
    public string? SpecificationCode7 { get; set; }
    public string? SpecificationCode8 { get; set; }
    public string? SpecificationCode9 { get; set; }
    public string? SpecificationCode10 { get; set; }
    public string? SpecificationCodeJson { get; set; }
    public DateTime? OrderDate { get; set; }
    public int? Status { get; set; }
}
public class DeleteOrderProductDto
{
    public Guid Id { get; set; }
}
public class OrderProducReferenceDto
{

    public string? Code { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public decimal? ExchangeRate { get; set; }
    public DateTime? OrderDate { get; set; }
    public int? Status { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public Guid? Id { get; set; }
    public Guid? OrderId { get; set; }
    public Guid? Guid { get; set; }
    public string? OrderCode { get; set; }
    public Guid? ContractId { get; set; }
    public string? ContractName { get; set; }
    public Guid? QuotationId { get; set; }
    public string? QuotationName { get; set; }
    public Guid ProductId { get; set; }
    public string ProductCode { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string? ProductImage { get; set; }
    public decimal? ProductPrice { get; set; }
    public string? ProductCurrency { get; set; }
    public string? Origin { get; set; }
    public Guid? WarehouseId { get; set; }
    public string? WarehouseCode { get; set; }
    public string? WarehouseName { get; set; }
    public decimal? StockQuantity { get; set; }
    public decimal? TotalStockQuantity { get; set; }
    public decimal? ReservedQuantity { get; set; }
    public decimal? PlannedQuantity { get; set; }
    public Guid? PriceListId { get; set; }
    public string? PriceListName { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? DiscountAmountDistribution { get; set; }
    public int? DiscountType { get; set; }
    public double? DiscountPercent { get; set; }
    public decimal? AmountDiscount { get; set; }
    public double? DiscountTotal { get; set; }
    public double? TaxRate { get; set; }
    public string? Tax { get; set; }
    public string? TaxCode { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string? Note { get; set; }
    public decimal? QuantityReturned { get; set; }
    public decimal? QuantitySales { get; set; }
    public decimal? QuantityReturnedActual { get; set; }
    public decimal? QuantityExported { get; set; }
    public decimal? QuantityPurchased { get; set; }
    public double? QuantityProductioned { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public int? DeliveryStatus { get; set; }
    public int? DeliveryQuantity { get; set; }
    public bool? IsDiscount { get; set; }
    public double? RemainQty { get; set; }
    public double? SellQty { get; set; }
    public int? SortOrder { get; set; }
    public decimal? AmountNoDiscount
    {
        get
        {
            return Quantity * UnitPrice;
        }
    }
    public decimal? TotalAmountDiscount
    {
        get
        {
            return (DiscountAmountDistribution ?? 0) + (AmountDiscount ?? 0);
        }
    }
    public decimal? AmountNoTax
    {
        get
        {
            return AmountNoDiscount - TotalAmountDiscount;
        }
    }
    public decimal? AmountTax
    {
        get
        {
            return AmountNoTax * (decimal)(TaxRate ?? 0) / 100;
        }
    }
    public decimal? TotalAmountTax
    {
        get
        {
            return AmountNoTax + AmountTax;
        }
    }
}
