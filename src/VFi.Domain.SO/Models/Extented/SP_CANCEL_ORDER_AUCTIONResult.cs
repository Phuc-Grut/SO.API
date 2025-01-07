using System.ComponentModel.DataAnnotations;

namespace VFi.Domain.SO.Models;
public partial class SP_CANCEL_ORDER_AUCTIONResult
{
    [Key]
    public Guid Id { get; set; }
    public string Code { get; set; }
    public decimal? Paid { get; set; }
    public int? Status { get; set; }
}
