namespace VFi.Application.SO.DTOs;

public class RptTongHopBanHangTheoKhachHangDto
{
    public Guid Id { get; set; }
    public Guid? ReportId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public decimal? AmountNoDiscount { get; set; }
    public decimal? AmountDiscount { get; set; }
    public decimal? AmountTax { get; set; }
    public decimal? TotalAmountTax
    {
        get
        {
            return (AmountNoDiscount ?? 0) + (AmountTax ?? 0);
        }
    }
    public decimal? ReturnAmount { get; set; }
    public decimal? SaleAmount { get; set; }
    public decimal? NetSale
    {
        get
        {
            return (AmountNoDiscount ?? 0) - (AmountDiscount ?? 0) + (AmountTax ?? 0) - (ReturnAmount ?? 0) - (SaleAmount ?? 0);
        }
    }
}
