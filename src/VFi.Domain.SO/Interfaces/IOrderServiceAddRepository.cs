using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IOrderServiceAddRepository : IRepository<OrderServiceAdd>
{
    Task<IEnumerable<OrderServiceAdd>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<OrderServiceAdd>> GetListListBox(Dictionary<string, object> filter);
    Task<IEnumerable<OrderServiceAdd>> Filter(Dictionary<string, object> filter);
    void Add(IEnumerable<OrderServiceAdd> items);
    void Update(IEnumerable<OrderServiceAdd> items);
    void Remove(IEnumerable<OrderServiceAdd> t);
}
