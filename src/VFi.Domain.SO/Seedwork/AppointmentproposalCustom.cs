using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.SO.Seedwork;

public class AppointmentproposalCustom
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public Guid EmployeeId { get; set; }
    public string? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }
    public Guid DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public Guid JobId { get; set; }
    public string? JobName { get; set; }
    public Guid JobNewId { get; set; }
    public string? JobNewName { get; set; }
    public string Reason { get; set; } = null!;
    public string? Content { get; set; }
    public string? IdeaHr { get; set; }
    public string? IdeaManager { get; set; }
    public int? Status { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
