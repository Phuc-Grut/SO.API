#nullable disable
using System;
using System.Collections.Generic;
using VFi.NetDevPack.Domain;

namespace VFi.Domain.SO.Models;

public partial class INVENTORY_DETAIL_BY_LISTID
{
    public Guid Id { get; set; }
    public Guid? WarehouseId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public Guid? ProductId { get; set; }
    public decimal? StockQuantity { get; set; }
    public decimal? ReservedQuantity { get; set; }
    public decimal? PlannedQuantity { get; set; }
}
