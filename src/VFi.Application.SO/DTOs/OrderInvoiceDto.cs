using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class OrderInvoiceDto
{
    public Guid? Id { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public Guid? OrderExpressId { get; set; }
    public string? OrderExpressCode { get; set; }
    public string? Serial { get; set; }
    public string? Symbol { get; set; }
    public string? Number { get; set; }
    public decimal? Value { get; set; }
    public DateTime? Date { get; set; }
    public string? Note { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
}
