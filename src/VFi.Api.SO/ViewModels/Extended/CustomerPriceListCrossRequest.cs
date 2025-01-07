using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class CustomerPriceListCrossRequest
{
    public Guid? Id { get; set; }
    public Guid PriceListCrossId { get; set; }
    public string PriceListCrossName { get; set; }
    public Guid RouterShippingId { get; set; }
    public string RouterShipping { get; set; }
}
