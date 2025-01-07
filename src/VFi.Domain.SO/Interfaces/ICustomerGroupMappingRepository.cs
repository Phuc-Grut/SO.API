using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface ICustomerGroupMappingRepository : IRepository<CustomerGroupMapping>
{
    Task<IEnumerable<CustomerGroupMapping>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<CustomerGroupMapping>> GetListListBox(Dictionary<string, object> filter);
    Task<IEnumerable<CustomerGroupMapping>> Filter(Guid id);
    void Add(IEnumerable<CustomerGroupMapping> items);
    void Remove(IEnumerable<CustomerGroupMapping> t);
}
