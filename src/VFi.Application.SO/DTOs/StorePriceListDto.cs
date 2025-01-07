using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class StorePriceListDto
{
    public Guid? Id { get; set; }
    public Guid? StoreId { get; set; }
    public Guid? PriceListId { get; set; }
    public string? PriceListName { get; set; }
    public bool? Default { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public int? DisplayOrder { get; set; }
    public string? UpdatedByName { get; set; }
    public string? CreatedByName { get; set; }
}
