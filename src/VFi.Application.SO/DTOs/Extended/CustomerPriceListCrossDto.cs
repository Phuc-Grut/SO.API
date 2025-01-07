using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class CustomerPriceListCrossDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid PriceListCrossId { get; set; }
    public string PriceListCrossName { get; set; }
    public Guid RouterShippingId { get; set; }
    public string RouterShipping { get; set; }
}
