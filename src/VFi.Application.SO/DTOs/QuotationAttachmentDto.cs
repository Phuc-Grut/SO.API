using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class QuotationAttachmentDto
{
    public Guid Id { get; set; }
    public Guid? QuotationId { get; set; }
    public string? Name { get; set; }
    public string? Path { get; set; }
    public string? AttachType { get; set; }
    public int? DisplayOrder { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
}
public class DeleteQuotationAttachmentDto
{
    public Guid Id { get; set; }
}
