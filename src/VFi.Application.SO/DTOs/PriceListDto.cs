using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class PriceListDto
{
    public Guid? Id { get; set; }
    public string Code { get; set; } = null!;
    public string? Name { get; set; }
    public string Description { get; set; }
    public int? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public List<PriceListDetailDto>? PriceListDetail { get; set; }
}
public class PriceListParams
{
    public int? Status { get; set; }
    public DateTime? Date { get; set; }
    public Guid? ProductId { get; set; }
    public double? Quantity { get; set; }
    public string? Currency { get; set; }
}
public class PriceListListBoxDto
{
    public Guid? Value { get; set; }
    public string Key { get; set; } = null!;
    public string? Label { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public List<PriceListDetailDto>? Details { get; set; }
}
public class PriceListValidateDto
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
    public Guid? UnitId { get; set; }
    public string? UnitCode { get; set; }
    public string? ProductPrice { get; set; }
    public string? CurrencyCode { get; set; }
    public string? UnitName { get; set; }
    public string? UnitType { get; set; }
    public int? Type { get; set; }
    public string? FixPrice { get; set; }
    public string? QuantityMin { get; set; }

}
