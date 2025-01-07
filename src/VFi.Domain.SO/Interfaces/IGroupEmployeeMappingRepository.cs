using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IGroupEmployeeMappingRepository : IRepository<GroupEmployeeMapping>
{
    Task<IEnumerable<GroupEmployeeMapping>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<GroupEmployeeMapping>> Filter(Guid id);
    void Update(IEnumerable<GroupEmployeeMapping> items);
    void Add(IEnumerable<GroupEmployeeMapping> items);
    void Remove(IEnumerable<GroupEmployeeMapping> t);
}
