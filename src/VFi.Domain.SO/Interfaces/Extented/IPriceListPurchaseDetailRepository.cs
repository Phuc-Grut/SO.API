using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IPriceListPurchaseDetailRepository : IRepository<PriceListPurchaseDetail>
{
    Task<IEnumerable<PriceListPurchaseDetail>> GetListListBox(Dictionary<string, object> filter, string? keyword);
    Task<(IEnumerable<PriceListPurchaseDetail>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<PriceListPurchaseDetail>> Filter(Guid id);
    void Update(IEnumerable<PriceListPurchaseDetail> items);
    void Add(IEnumerable<PriceListPurchaseDetail> items);
    void Remove(IEnumerable<PriceListPurchaseDetail> t);
}
