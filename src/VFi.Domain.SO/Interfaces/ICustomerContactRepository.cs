using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface ICustomerContactRepository : IRepository<CustomerContact>
{
    Task<IEnumerable<CustomerContact>> Filter(string? keyword, int? status, Guid? customerId, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, int? status, Guid? customerId);
    Task<IEnumerable<CustomerContact>> GetListCbx(int? status, Guid? customerId);
    Task<IEnumerable<CustomerContact>> Filter(int? status, Guid? customerId);
    void Add(IEnumerable<CustomerContact> items);
    void Update(IEnumerable<CustomerContact> items);
    void Remove(IEnumerable<CustomerContact> t);
    Task<IEnumerable<CustomerContact>> GetByParentId(Guid id);

}
