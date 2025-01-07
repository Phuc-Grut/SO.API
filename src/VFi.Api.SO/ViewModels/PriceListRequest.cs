using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddPriceListRequest
{
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public int? DisplayOrder { get; set; }
    public List<AddPriceListDetailRequest>? PriceListDetail { get; set; }
}
public class EditPriceListRequest : AddPriceListRequest
{
    public Guid Id { get; set; }
}
public class PriceListPagingRequest : FilterQuery
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
}
public class PriceListListBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$date")]
    public DateTime? Date { get; set; }
    [FromQuery(Name = "$productId")]
    public Guid? ProductId { get; set; }
    [FromQuery(Name = "$quantity")]
    public double? Quantity { get; set; }
    [FromQuery(Name = "$currency")]
    public string? Currency { get; set; }
    public PriceListParams ToBaseQuery() => new PriceListParams
    {
        Status = Status,
        Date = Date,
        ProductId = ProductId,
        Quantity = Quantity,
        Currency = Currency
    };
}
