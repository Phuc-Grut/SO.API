using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IReturnOrderProductRepository : IRepository<ReturnOrderProduct>
{
    Task<IEnumerable<ReturnOrderProduct>> GetListListBox(Dictionary<string, object> filter, string? keyword);
    Task<IEnumerable<ReturnOrderProduct>> GetByParentId(Guid id);
    Task<IEnumerable<ReturnOrderProduct>> Filter(Guid id);
    void Update(IEnumerable<ReturnOrderProduct> items);
    void Add(IEnumerable<ReturnOrderProduct> items);
    void Remove(IEnumerable<ReturnOrderProduct> t);
    Task<IEnumerable<ReturnOrderProduct>> GetByOrderId(string code);
}
