#nullable disable
using System;
using System.Collections.Generic;
using VFi.NetDevPack.Domain;

namespace VFi.Domain.SO.Models;

public partial class QuantityPuchased : Entity, IAggregateRoot
{
    public Guid? ProductId { get; set; }
    public string UnitType { get; set; }
    public string UnitCode { get; set; }
    public double? QuantityPurchased { get; set; }
    public double? QuantityApproved { get; set; }
    public string PurchaseRequestCode { get; set; }
}
