using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface ICustomerBankRepository : IRepository<CustomerBank>
{
    Task<IEnumerable<CustomerBank>> Filter(string? keyword, int? status, Guid? customerId, int pagesize, int pageindex);
    Task<IEnumerable<CustomerBank>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);

    Task<int> FilterCount(string? keyword, int? status, Guid? customerId);
    Task<IEnumerable<CustomerBank>> GetListCbx(int? status, Guid? customerId);
    Task<IEnumerable<CustomerBank>> Filter(int? status, Guid? customerId);
    void Add(IEnumerable<CustomerBank> items);
    void Update(IEnumerable<CustomerBank> items);
    void Remove(IEnumerable<CustomerBank> t);
    Task<IEnumerable<CustomerBank>> GetByParentId(Guid id);
}
