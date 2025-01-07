using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface ICustomerAddressRepository : IRepository<CustomerAddress>
{
    Task<IEnumerable<CustomerAddress>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    //Task<int> FilterCount(string? keyword, int? status, Guid? customerId);
    Task<IEnumerable<CustomerAddress>> GetListCbx(int? status, Guid? customerId);
    //Task<IEnumerable<CustomerAddress>> Filter( int? status, Guid? customerId);
    void Add(IEnumerable<CustomerAddress> items);
    void Update(IEnumerable<CustomerAddress> items);
    void Remove(IEnumerable<CustomerAddress> t);
    Task<IEnumerable<CustomerAddress>> GetByParentId(Guid id);
}
