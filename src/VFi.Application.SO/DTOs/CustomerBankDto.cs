using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;

namespace VFi.Application.SO.DTOs;

public class CustomerBankDto
{
    public Guid? Id { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? Name { get; set; }
    public string? BankCode { get; set; }
    public string? BankName { get; set; }
    public string? BankBranch { get; set; }
    public string? AccountName { get; set; }
    public string? AccountNumber { get; set; }
    public bool? Default { get; set; }
    public int? Status { get; set; }
    public int? SortOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }

}
