using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Application.SO.DTOs;

namespace VFi.Application.SO.DTOs;

public class SendConfigDto
{
}

public class SendConfigComboboxDto : ComboBoxDto
{
    public string? From { get; set; }
    public string? FromName { get; set; }
}
