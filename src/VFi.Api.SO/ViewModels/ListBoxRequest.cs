using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;

namespace VFi.Api.SO.ViewModels;

public class ListBoxRequest
{
    [FromQuery(Name = "$keyword")]
    public string? Keyword { get; set; }
}
