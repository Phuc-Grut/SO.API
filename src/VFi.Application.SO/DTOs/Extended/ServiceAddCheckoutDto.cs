namespace VFi.Application.SO.DTOs;

public class ServiceAddCheckoutDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int CalculationMethod { get; set; }
    public decimal? Price { get; set; }
    public string? PriceSyntax { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int DisplayOrder { get; set; }
    public string? Currency { get; set; }
}
