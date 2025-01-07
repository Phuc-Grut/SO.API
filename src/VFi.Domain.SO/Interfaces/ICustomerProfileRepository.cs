using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface ICustomerProfileRepository : IRepository<CustomerProfile>
{
    Task<CustomerProfile> GetByAccountId(string accountId);
    Task<CustomerProfile> GetByKey(string accountId, string key);
    Task<IEnumerable<CustomerProfile>> GetByGroup(string accountId, string group);
    Task<IEnumerable<CustomerProfile>> Filter(string? keyword, int? status, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, int? status);

}
