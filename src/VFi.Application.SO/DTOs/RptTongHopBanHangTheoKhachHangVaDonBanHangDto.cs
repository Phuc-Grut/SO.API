namespace VFi.Application.SO.DTOs;

public class RptTongHopBanHangTheoKhachHangVaDonBanHangDto
{
    public Guid Id { get; set; }
    public Guid? ReportId { get; set; }
    public Guid? ParentId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? OrderCode { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal? TotalAmountTax { get; set; }
    public decimal? PaymentAmount { get; set; }
    public decimal? RemainAmount
    {
        get
        {
            return (TotalAmountTax - PaymentAmount);
        }
    }
    public List<RptTongHopBanHangTheoKhachHangVaDonBanHangDto>? Children { get; set; }
}
