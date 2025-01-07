namespace VFi.Application.SO.DTOs;

public class RptSoChiTietBanHangDto
{
    public Guid Id { get; set; }
    public Guid? ReportId { get; set; }
    public string? OrderCode { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public double? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? AmountNoDiscount
    {
        get
        {
            return (decimal)(Quantity ?? 0) * (UnitPrice ?? 0);
        }
    }

    public decimal? AmountDiscount { get; set; }
    public double? TaxRate { get; set; }
    public decimal? AmountTax
    {
        get
        {
            return (AmountNoDiscount - AmountDiscount) * (decimal)(TaxRate ?? 0) / 100;
        }
    }
    public decimal? TotalAmountTax
    {
        get
        {
            return AmountNoDiscount - AmountDiscount + AmountTax;
        }
    }

    public double? ReturnQuantity { get; set; }
    public decimal? ReturnAmount { get; set; }
    public decimal? SaleAmount { get; set; }
}
