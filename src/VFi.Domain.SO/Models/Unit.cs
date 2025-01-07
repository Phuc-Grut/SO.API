using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.NetDevPack.Domain;

namespace VFi.Domain.SO.Models;

public partial class Unit : Entity, IAggregateRoot
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? NamePlural { get; set; }
    public string? Description { get; set; }
    public string? DisplayLocale { get; set; }
    public int? DisplayOrder { get; set; }
    public int? IsDefault { get; set; }
    public Guid? GroupUnitId { get; set; }
    public string? GroupUnitName { get; set; }
    public string? GroupUnitCode { get; set; }
    public decimal? Rate { get; set; }
    public int? Status { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedByName { get; set; }
}
