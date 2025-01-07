using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class RequestPurchaseProductRequest
{
    public Guid? Id { get; set; }
    public Guid RequestPurchaseId { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public Guid? OrderProductId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public string? ProductImage { get; set; }
    public string? SourceLink { get; set; }
    public decimal? ShippingFee { get; set; }
    public string? BidUsername { get; set; }
    public string? Origin { get; set; }
    /// <summary>
    /// Đơn vị tính
    /// </summary>
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    /// <summary>
    /// Số lượng đề nghị
    /// </summary>
    public double QuantityRequest { get; set; }
    /// <summary>
    /// Số lượng duyệt
    /// </summary>
    public double? QuantityApproved { get; set; }
    /// <summary>
    /// Đơn giá trước VAT
    /// </summary>
    public decimal? UnitPrice { get; set; }
    public string? Currency { get; set; }
    /// <summary>
    /// Ngày cần hàng
    /// </summary>
    public DateTime? DeliveryDate { get; set; }
    /// <summary>
    /// Mức độ ưu tiên: 0--Bình thường, 1--Gấp
    /// </summary>
    public int? PriorityLevel { get; set; }
    public string? Note { get; set; }
    public string? VendorCode { get; set; }
    public string? VendorName { get; set; }
    public int? Status { get; set; }
    public int? StatusPurchase { get; set; }
    public double? QuantityPurchase { get; set; }
    public double? QuantityPurchased { get; set; }
}

public class DeleteRequestPurchaseProductRequest
{
    public Guid Id { get; set; }
}

public class RequestPurchaseAddOrdersRequest
{
    public Guid Id { get; set; }
    public List<Guid>? OrderIds {  get; set; } 
}
