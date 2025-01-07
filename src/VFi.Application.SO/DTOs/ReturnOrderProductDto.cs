using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class ReturnOrderProductDto
{
    public Guid? Id { get; set; }
    public Guid? ReturnOrderId { get; set; }
    public Guid? OrderProductId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductCode { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public decimal? QuantityReturn { get; set; }
    public decimal? UnitPrice { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public decimal? DiscountAmountDistribution { get; set; }
    public int? DiscountType { get; set; }
    public double? DiscountPercent { get; set; }
    public decimal? AmountDiscount { get; set; }
    public double? TaxRate { get; set; }
    public string? Tax { get; set; }
    public string? TaxCode { get; set; }
    public Guid? ReasonId { get; set; }
    public string? ReasonName { get; set; }
    public Guid? WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
    public decimal? AmountNoDiscount
    {
        get
        {
            return QuantityReturn * UnitPrice;
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
    public decimal? QuantityRemain { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
}
public class DeleteReturnOrderProductDto
{
    public Guid Id { get; set; }
}
