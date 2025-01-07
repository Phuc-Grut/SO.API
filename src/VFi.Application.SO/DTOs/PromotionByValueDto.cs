using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class PromotionByValueDto
{
    public Guid? Id { get; set; }
    public Guid? PromotionId { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public int? Type { get; set; }
    public decimal? MinOrderPrice { get; set; }
    public decimal? LimitTotalValue { get; set; }
    public double? DiscountPercent { get; set; }
    public decimal? ReduceAmount { get; set; }
    public decimal? FixPrice { get; set; }
    public int? TypeBonus { get; set; }
    public int? TypeBuy { get; set; }
    public double? Quantity { get; set; }
    public double? QuantityBuy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }
    public string? CreatedByName { get; set; }
    public List<PromotionProductDto>? ProductBonus { get; set; }
    public List<PromotionProductBuyDto>? ProductBuy { get; set; }
}
public class DeletePromotionByValueDto
{
    public Guid Id { get; set; }
}
