using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface ICustomerGroupRepository : IRepository<CustomerGroup>
{
    Task<CustomerGroup> GetByCode(string code);
    Task<(IEnumerable<CustomerGroup>, int)> Filter(string? keyword, int? status, IFopRequest request);
    Task<IEnumerable<CustomerGroup>> GetListCbx(int? status);
    Task<IEnumerable<CustomerGroup>> Filter(List<Guid> listId);
    Task<bool> IsNotBeingUsed(Guid id);
    void Update(IEnumerable<CustomerGroup> t);
    Task<IEnumerable<CustomerGroup>> Filter(IEnumerable<string>? name);

}
