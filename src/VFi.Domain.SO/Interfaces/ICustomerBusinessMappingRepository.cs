using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface ICustomerBusinessMappingRepository : IRepository<CustomerBusinessMapping>
{
    Task<IEnumerable<CustomerBusinessMapping>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<CustomerBusinessMapping>> GetListListBox(Dictionary<string, object> filter);
    Task<IEnumerable<CustomerBusinessMapping>> Filter(Guid id);
    void Add(IEnumerable<CustomerBusinessMapping> items);
    void Remove(IEnumerable<CustomerBusinessMapping> t);
}
