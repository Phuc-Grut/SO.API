using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class GroupEmployeeMappingDto
{
    public Guid? Id { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? GroupEmployeeId { get; set; }
    public string? GroupEmployeeName { get; set; }
    public bool? IsLeader { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CreatedByName { get; set; }
}
public class DeleteGroupEmployeeMappingDto
{
    public Guid Id { get; set; }
}
