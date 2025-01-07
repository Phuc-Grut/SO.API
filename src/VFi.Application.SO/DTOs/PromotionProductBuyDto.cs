using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class PromotionProductBuyDto
{
    public Guid? Id { get; set; }
    public Guid? PromotionByValueId { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public double? Quantity { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }
    public string? CreatedByName { get; set; }
}
public class DeletePromotionProductBuyDto
{
    public Guid Id { get; set; }
}
