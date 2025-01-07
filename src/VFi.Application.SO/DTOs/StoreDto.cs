

using VFi.Domain.SO.Models;

namespace VFi.Application.SO.DTOs;

public class StoreDto
{
    public Guid Id { get; set; }

    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public int? DisplayOrder { get; set; }
    public string UpdatedByName { get; set; }
    public string CreatedByName { get; set; }
    public int? Status { get; set; }
}
