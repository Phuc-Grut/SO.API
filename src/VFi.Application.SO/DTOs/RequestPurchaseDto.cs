using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Application.SO.DTOs;
using VFi.NetDevPack.Domain;

namespace VFi.Application.SO.DTOs;

public class RequestPurchaseDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public Guid? RequestBy { get; set; }
    public string? RequestByName { get; set; }
    public string? RequestByEmail { get; set; }
    public DateTime? RequestDate { get; set; }
    /// <summary>
    /// Mục đích đề nghị
    /// </summary>
    public string? CurrencyCode { get; set; }
    public string? CurrencyName { get; set; }
    public string? Calculation { get; set; }
    public decimal? ExchangeRate { get; set; }
    public string? Proposal { get; set; }
    public string? Note { get; set; }
    public DateTime? ApproveDate { get; set; }
    public Guid? ApproveBy { get; set; }
    public string? ApproveByName { get; set; }
    public string? ApproveComment { get; set; }
    /// <summary>
    /// Trạng thái: 1-Sử dụng, 0-Không sử dụng
    /// </summary>
    public int? Status { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    /// <summary>
    /// Số lượng đề nghị
    /// </summary>
    public double? QuantityRequest { get; set; }
    /// <summary>
    /// Số lượng duyệt
    /// </summary>
    public double? QuantityApproved { get; set; }
    public int? StatusPurchase { get; set; }
    public double? QuantityPurchased { get; set; }
    public string? PurchaseRequestCode { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public DateTime? Podate { get; set; }
    public int? Postatus { get; set; }
    public List<FileDto>? File { get; set; }
    public List<RequestPurchaseProductDto>? Details { get; set; }

}
public class RequestPurchaseValidateDto
{
    public UInt32 Row { get; set; }
    public bool IsValid
    {
        get
        {
            return Errors.Count == 0;
        }
    }
    public List<string> Errors { get; set; } = new List<string>();
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public Guid? UnitId { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public string? QuantityRequest { get; set; }
    public string? UnitPrice { get; set; }
    public string? StockQuantity { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int? PriorityLevel { get; set; }
    public string? Note { get; set; }
    public string? UnitType { get; set; }
}

public class RequestPurchaseValidateField
{
    public string? Field { get; set; } = null!;
    public int? IndexColumn { get; set; }
}
