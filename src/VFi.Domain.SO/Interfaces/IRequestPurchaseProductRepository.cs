using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IRequestPurchaseProductRepository : IRepository<RequestPurchaseProduct>
{
    Task<IEnumerable<RequestPurchaseProduct>> GetListListBox(Dictionary<string, object> filter, string? keyword);
    Task<IEnumerable<RequestPurchaseProduct>> Filter(Dictionary<string, object> filter);
    Task<(IEnumerable<RequestPurchaseProduct>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    void Update(IEnumerable<RequestPurchaseProduct> items);
    void Add(IEnumerable<RequestPurchaseProduct> items);
    void Remove(IEnumerable<RequestPurchaseProduct> t);
    Task<IEnumerable<RequestPurchaseProduct>> GetByOrderId(string code);
    Task<IEnumerable<RequestPurchaseProduct>> GetByOrderIds(IEnumerable<Guid> ids);
    Task<IEnumerable<RequestPurchaseProduct>> GetByRequestPurchaseId(Guid id);
}
