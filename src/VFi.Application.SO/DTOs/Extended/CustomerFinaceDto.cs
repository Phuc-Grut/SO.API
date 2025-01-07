using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class CustomerFinaceDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public Guid? CurrencyId { get; set; }
    public string Currency { get; set; }
    public string CurrencyName { get; set; }

    public Guid? PriceListId { get; set; }
    public string PriceListName { get; set; }
    public Guid? PriceListPurchaseId { get; set; }
    public string PriceListPurchaseName { get; set; }
    public decimal? DebtLimit { get; set; }
    public decimal? RemainingDebt { get; set; }

    public List<CustomerPriceListCrossDto> CustomerPriceListCross { get; set; }

    public List<Guid> PriceListCross { get; set; }
}
