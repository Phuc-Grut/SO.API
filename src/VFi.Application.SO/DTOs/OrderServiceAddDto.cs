using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class OrderServiceAddDto
{
    public Guid? Id { get; set; }
    public Guid? OrderId { get; set; }
    public Guid? OrderExpressId { get; set; }
    public Guid? QuotationId { get; set; }
    public Guid? ServiceAddId { get; set; }
    public string? ServiceAddName { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public string? Calculation { get; set; }
    public int? Status { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedByName { get; set; }
    public string? CreatedByName { get; set; }
    public decimal? ExchangeRate { get; set; }
    public int? DisplayOrder { get; set; }
}
public class DeleteOrderServiceAddDto
{
    public Guid Id { get; set; }
}
