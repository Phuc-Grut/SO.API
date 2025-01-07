using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IDeliveryProductRepository : IRepository<DeliveryProduct>
{
    void Update(IEnumerable<DeliveryProduct> t);
    void Add(IEnumerable<DeliveryProduct> t);
    void Remove(IEnumerable<DeliveryProduct> t);
    Task<DeliveryProduct> GetByCode(string code);
    Task<IEnumerable<DeliveryProduct>> GetListCbx(int? status);
    Task<(IEnumerable<DeliveryProduct>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<DeliveryProduct>> GetByDeliveryProductId(Guid id);
    Task<IEnumerable<DeliveryProduct>> GetByOrderId(Guid id);
    Task<IEnumerable<DeliveryProduct>> GetByDeliveryProductId(List<Guid> listId);
    Task<IEnumerable<DeliveryProduct>> GetListDetaiId(string id);
    Task<(IEnumerable<DeliveryProduct>, int)> Filter(IFopRequest request);
}
