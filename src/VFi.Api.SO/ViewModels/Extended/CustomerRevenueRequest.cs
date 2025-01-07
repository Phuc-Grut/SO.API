namespace VFi.Api.SO.ViewModels;

public class CustomerRevenueRequest
{
    public int? BackHour { set; get; }
    public Guid? AccountId { set; get; }
    public Guid? CustomerId { set; get; }
}
