namespace VFi.Application.SO.DTOs;

public class RptTinhHinhThucHienDonBanHangDto
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
    public decimal? Quantity { get; set; }
    public decimal? QuantityDelivery { get; set; }
    public decimal? QuantityRemain
    {
        get
        {
            return (Quantity ?? 0) - (QuantityDelivery ?? 0);
        }
    }
    public decimal? SaleOrder { get; set; }
    public decimal? SaleMade { get; set; }
    public decimal? SaleRemain
    {
        get
        {
            return (SaleOrder ?? 0) - (SaleMade ?? 0);
        }
    }
}
