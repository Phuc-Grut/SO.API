using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class DashboardRequest
{
    [FromQuery(Name = "$currency")]
    public string? Currency { get; set; }
    [FromQuery(Name = "$startDate")]
    public DateTime? StartDate { get; set; }
    [FromQuery(Name = "$endDate")]
    public DateTime? EndDate { get; set; }
    public DashboardParams ToBaseQuery() => new DashboardParams
    {
        Currency = Currency,
        StartDate = StartDate,
        EndDate = EndDate
    };
}
public class DashboardSaleProductTimeRequest
{
    public string? Currency { get; set; }
    public int Year { get; set; }
    public int Type { get; set; }
}
