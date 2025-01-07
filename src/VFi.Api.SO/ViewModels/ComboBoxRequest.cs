using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;

namespace VFi.Api.SO.ViewModels;

public class ComboBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$groupId")]
    public Guid? GroupId { get; set; }
    [FromQuery(Name = "$keyWord")]
    public string? KeyWord { get; set; }
    [FromQuery(Name = "$country")]
    public string? Country { get; set; }
}

public class ComboBoxHaveCategoryRequest : ComboBoxRequest
{
    [FromQuery(Name = "$category")]
    public string? Category { get; set; }
}
public class ComboBoxStatusRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
}
