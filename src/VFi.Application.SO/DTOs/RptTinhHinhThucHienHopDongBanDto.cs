namespace VFi.Application.SO.DTOs;

public class RptTinhHinhThucHienHopDongBanDto
{
    public Guid Id { get; set; }
    public Guid? ReportId { get; set; }
    public string? Code { get; set; }
    public DateTime? Date { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? QuantitySold { get; set; }
    public decimal? QuantityRemain
    {
        get
        {
            return (Quantity ?? 0) - (QuantitySold ?? 0);
        }
    }
    public decimal? SaleContract { get; set; }
    public decimal? SaleMade { get; set; }
    public decimal? SaleRemain
    {
        get
        {
            return (SaleContract ?? 0) - (SaleMade ?? 0);
        }
    }
}
