using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class SendTransactionDto
{
    public Guid Id { get; set; }
    public string? SenderCode { get; set; }
    public string? Subject { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public string? Cc { get; set; }
    public string? Bcc { get; set; }
    public string? Content { get; set; }
    public string? Plaintext { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public string Product { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public DateTime? SendDate { get; set; }
    public string? Priority { get; set; }
    public string? Group { get; set; }
    public string Campaign { get; set; }
    public int? Open { get; set; }
    public int? SendPriority { get; set; }
    public int? Click { get; set; }
    public string ClickUrl { get; set; }
    public DateTime? OpenDate { get; set; }
    public string Order { get; set; }
    public bool? Bounce { get; set; }
}

public class SendTransactionLogDto
{
    public Guid Id { get; set; }
    public string? Subject { get; set; }
    public DateTime? SendDate { get; set; }
    public int? Open { get; set; }
    public int? Click { get; set; }
    public string Campaign { get; set; }
    public string Order { get; set; }
}
