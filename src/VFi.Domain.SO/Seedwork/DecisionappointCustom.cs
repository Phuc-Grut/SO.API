using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.SO.Seedwork;

public class DecisionappointCustom
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public Guid? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public Guid JobNewId { get; set; }
    public Guid? SignerId { get; set; }
    public string? SignerName { get; set; }
    public Guid AppointId { get; set; }
    public DateTime? AppointDate { get; set; }
    public string? Reason { get; set; }
    public string? Note { get; set; }
    public string? Content { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
