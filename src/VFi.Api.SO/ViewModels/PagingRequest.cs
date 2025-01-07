using Microsoft.AspNetCore.Mvc;

namespace VFi.Api.SO.ViewModels;

public class PagingRequest
{
    [FromQuery(Name = "$skip")]
    public int Skip { get; set; }
    [FromQuery(Name = "$top")]
    public int Top { get; set; }
    [FromQuery(Name = "$keyword")]
    public string? Keyword { get; set; }
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
}
public class FilterInfor
{
    public string? Key { get; set; }
    public string? Value { get; set; }
    public string? Ope { get; set; }
}

public class PagingFilterRequest : PagingRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$customerGroupId")]
    public string? CustomerGroupId { get; set; }
    [FromQuery(Name = "$groupId")]
    public string? GroupId { get; set; }
    [FromQuery(Name = "$filter")]
    public Dictionary<string, object>? Filter { get; set; }
}
public class PagingFilterRequestQuery : PagingRequest
{
    [FromQuery(Name = "$parentId")]
    public string? ParentId { get; set; }
    [FromQuery(Name = "$startDate")]
    public DateTime? StartDate { get; set; }
    [FromQuery(Name = "$endDate")]
    public DateTime? EndDate { get; set; }

    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
}
public class FilterQuery
{
    public string? Filter { get; set; }
    public string? Keyword { get; set; }

    public string? Order { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }
    public int? Status { get; set; }
    public int? DomesticStatus { get; set; }
}


public class PagingFilterHaveCategoryRequest : PagingFilterRequest
{
    [FromQuery(Name = "$category")]
    public string? Category { get; set; }
}
public class PagingFilterHaveDateRequest : PagingFilterHaveCategoryRequest
{
    [FromQuery(Name = "$startDate")]
    public DateTime? StartDate { get; set; }
    [FromQuery(Name = "$endDate")]
    public DateTime? EndDate { get; set; }
}
public class FopPagingRequest
{
    [FromQuery(Name = "$filter")]
    public string? Filter { get; set; }

    [FromQuery(Name = "$keyword")]
    public string? Keyword { get; set; }

    [FromQuery(Name = "$order")]
    public string? Order { get; set; }

    [FromQuery(Name = "$pageNumber")]
    public int PageNumber { get; set; }

    [FromQuery(Name = "$pageSize")]
    public int PageSize { get; set; }

    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
}
