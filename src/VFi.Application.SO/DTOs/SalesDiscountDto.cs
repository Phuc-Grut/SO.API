using VFi.NetDevPack.Utilities;

namespace VFi.Application.SO.DTOs;

public partial class SalesDiscountDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public Guid? CustomerAddressId { get; set; }
    public string? CustomerAddress { get; set; }
    public string? SalesOrderCode { get; set; }
    public Guid? SalesOrderId { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencyName { get; set; }
    public decimal? ExchangeRate { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }
    public string? Note { get; set; }
    public int? TypeDiscount { get; set; }
    public int? Status { get; set; }
    public DateTime? DiscountDate { get; set; }
    public int? PaymentStatus { get; set; }
    public Guid? ApproveBy { get; set; }
    public DateTime? ApproveDate { get; set; }
    public string? ApproveByName { get; set; }
    public string? ApproveComment { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public decimal? AmountNoTax { get; set; }
    public decimal? AmountTax { get; set; }
    public decimal? TotalAmount { get; set; }
    public List<DocumentDto>? Reference { get; set; }
    public List<FileDto>? File { get; set; }
    public ICollection<SalesDiscountProductDto>? ListDetails { get; set; }
    public List<PaymentInvoiceDto>? PaymentInvoice { get; set; }
}
public class DeleteByIdDto
{
    public Guid Id { get; set; }
}


public class SalesDiscountValidateDto
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
    public string? Quantity { get; set; }
    public string? StockQuantity { get; set; }
    public string? UnitPrice { get; set; }
    public string? TaxCategoryId { get; set; }
    public string? TaxName { get; set; }
    public string? TaxCode { get; set; }
    public string? DiscountPercent { get; set; }

    public string? TaxRate { get; set; }
    public string? ReasonDiscount { get; set; }
    public string? UnitType { get; set; }
}
