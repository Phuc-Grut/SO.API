using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<(IEnumerable<Employee>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<Employee>> Filter(Dictionary<string, object> filter);
    Task<IEnumerable<Employee>> GetListCbx(int? status, Guid? groupId);
    Task<Employee> GetByIdQuery(Guid id);
    Task<Employee> GetByAccountId(Guid? accountId);
    Task<Employee> GetByCode(string code);
    Task<IEnumerable<Employee>> Filter(IEnumerable<string>? name);
    Task<IEnumerable<Employee>> GetByListId(string? listId);

}
