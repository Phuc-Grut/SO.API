using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class PaymentTermDetailDto
{
    public Guid Id { get; set; }
    public Guid? PaymentTermId { get; set; }
    public int Type { get; set; }
    public decimal? Value { get; set; }
    public decimal? Duration { get; set; }
    public int? Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}
public class DeletePaymentTermDetailDto
{
    public Guid Id { get; set; }
}
