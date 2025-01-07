using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class ReturnOrderDto
{
    public Guid? Id { get; set; }
    public string Code { get; set; } = null!;
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public Guid? WarehouseId { get; set; }
    public string? WarehouseCode { get; set; }
    public string? WarehouseName { get; set; }
    public DateTime? ReturnDate { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public string? Calculation { get; set; }
    public decimal? ExchangeRate { get; set; }
    public int? TypeDiscount { get; set; }
    public double? DiscountRate { get; set; }
    public int? TypeCriteria { get; set; }
    public decimal? AmountDiscount { get; set; }
    public int? PaymentStatus { get; set; }
    public Guid? ApproveBy { get; set; }
    public DateTime? ApproveDate { get; set; }
    public string? ApproveByName { get; set; }
    public string? ApproveComment { get; set; }
    public List<FileDto>? File { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public List<ReturnOrderProductDto>? ReturnOrderProduct { get; set; }
    public List<PaymentInvoiceDto>? PaymentInvoice { get; set; }
}
public class ReturnOrderValidateDto
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
    public string? UnitPrice { get; set; }
    public string? QuantityReturn { get; set; }
    public string? DiscountPercent { get; set; }
    public string? StockQuantity { get; set; }
    public string? Note { get; set; }
    public string? UnitType { get; set; }
    public string? ReasonName { get; set; }
    public Guid? ReasonId { get; set; }
    public string? TaxCategoryId { get; set; }
    public string? Tax { get; set; }
    public string? TaxCode { get; set; }
    public string? TaxRate { get; set; }
}
