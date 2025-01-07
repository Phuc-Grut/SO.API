#nullable disable
using System;
using System.Collections.Generic;
using VFi.NetDevPack.Domain;

namespace VFi.Domain.SO.Models;

public partial class Currency : Entity, IAggregateRoot
{
    public Currency()
    {
        ExchangeRate = new HashSet<ExchangeRate>();
    }

    public string Code { get; set; }
    public string Name { get; set; }
    public string Locale { get; set; }
    public string CustomFormatting { get; set; }
    public int DisplayOrder { get; set; }
    public int? Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    public ICollection<ExchangeRate> ExchangeRate { get; set; }
}
