using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddServiceAddRequest
{
    public string? Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int CalculationMethod { get; set; }
    public decimal? Price { get; set; }
    public string? PriceSyntax { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? PayLater { get; set; }
    public int Status { get; set; }
    public string? Tags { get; set; }
    public string? Currency { get; set; }
    public string CurrencyName { get; set; }
    public int DisplayOrder { get; set; }

}
public class EditServiceAddRequest : AddServiceAddRequest
{
    public string Id { get; set; }
}
public class ListBoxServiceAddRequest : ListBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
}
public class PagingServiceAddRequest : PagingRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
}
