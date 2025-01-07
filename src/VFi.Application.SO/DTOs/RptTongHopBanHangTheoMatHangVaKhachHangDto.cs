namespace VFi.Application.SO.DTOs;

public class RptTongHopBanHangTheoMatHangVaKhachHangDto
{
    public Guid Id { get; set; }
    public Guid? ReportId { get; set; }
    public Guid? ParentId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public decimal? Quantity { get; set; }
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
    public double? ReturnQuantity { get; set; }
    public decimal? ReturnAmount { get; set; }
    public decimal? SaleAmount { get; set; }
    public decimal? NetSale
    {
        get
        {
            return (AmountNoDiscount ?? 0) - (AmountDiscount ?? 0) + (AmountTax ?? 0) - (ReturnAmount ?? 0) - (SaleAmount ?? 0);
        }
    }
    public List<RptTongHopBanHangTheoMatHangVaKhachHangDto>? Children { get; set; }
}
