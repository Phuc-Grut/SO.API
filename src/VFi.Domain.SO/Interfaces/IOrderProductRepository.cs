using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IOrderProductRepository : IRepository<OrderProduct>
{
    Task<IEnumerable<OrderProduct>> GetListListBox(Dictionary<string, object> filter, string? keyword);
    Task<IEnumerable<OrderProduct>> Filter(Dictionary<string, object> filter);
    void Update(IEnumerable<OrderProduct> items);
    void Add(IEnumerable<OrderProduct> items);
    void Remove(IEnumerable<OrderProduct> t);
    Task<IEnumerable<OrderProduct>> GetById(IEnumerable<ExportWarehouseProduct> exportWarehouseProduct);
    Task<IEnumerable<OrderProduct>> GetByOrderIds(IEnumerable<Guid> orderIds);
    Task<(IEnumerable<OrderProduct>, int)> Filter(string keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<OrderProduct>> GetContractByOrderId(string code);
    Task<IEnumerable<OrderProduct>> GetQuotationByOrderId(string code);
}
