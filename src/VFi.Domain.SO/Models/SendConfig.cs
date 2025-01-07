using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.SO.Models;

public class SendConfig
{
}

public class SendConfigCombobox
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? From { get; set; }
    public string? FromName { get; set; }
}
