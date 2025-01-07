using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public partial class DocumentDto
{
    public Guid RefId { get; set; }
    public string? RefCode { get; set; }
    public string? RefResourceCode { get; set; }
}
