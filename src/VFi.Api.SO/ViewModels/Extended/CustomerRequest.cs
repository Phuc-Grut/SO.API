using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Domain;

namespace VFi.Api.SO.ViewModels;

public class UpdateFinanceExCustomerRequest
{
    public Guid? PriceListPurchaseId { get; set; }
    public string PriceListPurchaseName { get; set; }
    public Guid? CurrencyId { get; set; }
    public string Currency { get; set; }
    public string CurrencyName { get; set; }
    public Guid? PriceListId { get; set; }
    public string PriceListName { get; set; }
    public decimal? DebtLimit { get; set; }
    public decimal? RemainingDebt { get; set; }
    public List<CustomerPriceListCrossRequest> CustomerPriceListCross { get; set; } = new List<CustomerPriceListCrossRequest>();
}
public class UpdateIdInfoCustomerRequest
{
    public bool TranActive { get; set; }
    public int IdStatus { get; set; }
}
