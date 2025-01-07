using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.SO.Models;

public class EmailBody
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public string BodyText { get; set; } = string.Empty;
}
