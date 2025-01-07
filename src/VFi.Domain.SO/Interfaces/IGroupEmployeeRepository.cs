using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IGroupEmployeeRepository : IRepository<GroupEmployee>
{
    Task<GroupEmployee> GetByCode(string code);
    Task<(IEnumerable<GroupEmployee>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<GroupEmployee>> GetListCbx(Dictionary<string, object> filter);
    Task<IEnumerable<GroupEmployee>> GetByListId(List<Guid>? listId);
    Task<IEnumerable<GroupEmployee>> Filter(IEnumerable<string>? name);

}
