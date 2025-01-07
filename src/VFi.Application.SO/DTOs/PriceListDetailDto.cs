using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class PriceListDetailDto
{
    public Guid? Id { get; set; }
    public Guid PriceListId { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public decimal? ProductPrice { get; set; }
    public string? ProductCurrency { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencyName { get; set; }
    public int? QuantityMin { get; set; }
    public int? Type { get; set; }
    public decimal? FixPrice { get; set; }
    public int? TypeDiscount { get; set; }
    public double? DiscountRate { get; set; }
    public decimal? DiscountValue { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
}
public class DeletePriceListDetailDto
{
    public Guid Id { get; set; }
}
