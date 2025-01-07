using FluentValidation.Results;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class ReturnIdResult : ValidationResult
{
    public Guid? Id { get; set; }
    public bool IsValid { get; set; }
}
public class IdRequest
{
    public Guid Id { get; set; }
}
public class ListIdRequest
{
    public List<Guid>? ListId { get; set; }
}
public class DateToDateRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? WarehouseId { get; set; }
}
public class LocationRequest
{
    public Guid? WarehouseId { get; set; }
    public Guid? AreaId { get; set; }
    public Guid? ShelfId { get; set; }
    public Guid? AsideId { get; set; }
    public Guid? LocationId { get; set; }
}
