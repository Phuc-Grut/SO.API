using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IReasonRepository : IRepository<Reason>
{
    Task<Reason> GetByCode(string code);
    Task<Reason> GetByName(string name);
    Task<(IEnumerable<Reason>, int)> Filter(string? keyword, int? status, IFopRequest request);
    Task<IEnumerable<Reason>> GetListCbx(int? status);
}
