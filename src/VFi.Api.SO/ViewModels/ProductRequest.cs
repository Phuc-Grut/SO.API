namespace VFi.Api.SO.ViewModels;

public class ProductRequest
{

    public Guid ProductId { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public string Origin { get; set; }
    public string UnitType { get; set; }
    public string UnitCode { get; set; }
    public string UnitName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public float TaxRate { get; set; }
    public string Note { get; set; }


}
