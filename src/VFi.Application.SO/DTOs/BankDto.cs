using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class BankDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Qrbin { get; set; }
    public string ShortName { get; set; }
    public string Name { get; set; }
    public string EnglishName { get; set; }
    public string Address { get; set; }
    public int DisplayOrder { get; set; }
    public int Status { get; set; }
    public string Note { get; set; }
    public string Image { get; set; }
    public Guid? CreatedBy { get; set; }
    public string CreatedByName { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string UpdatedByName { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
