using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class EmailBodyDto
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public string BodyText { get; set; } = string.Empty;
}
