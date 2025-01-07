using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class SendTemplateDto
{
}
public class SendTemplateComboboxDto : ComboBoxDto
{
    public string Description { get; set; }
    public int Type { get; set; }
    public int Status { get; set; }
}
public class SendTemplateListboxDto : ComboBoxDto
{
    public string? Description { get; set; }
    public int? Type { get; set; }
    public int? Status { get; set; }
}
