using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IPriceListPurchaseRepository : IRepository<PriceListPurchase>
{
    bool CheckByCode(string? code, Guid? id);
    bool CheckUsing(Guid id);
    Task<(IEnumerable<PriceListPurchase>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<PriceListPurchase?> GetDefault();
    Task<IEnumerable<PriceListPurchase>> GetListCbx(Dictionary<string, object> filter);
    void Update(IEnumerable<PriceListPurchase> item);
    void Sort(IEnumerable<PriceListPurchase> item);
}
