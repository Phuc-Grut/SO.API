using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface ILeadProfileRepository : IRepository<LeadProfile>
{
    Task<(IEnumerable<LeadProfile>, int)> Filter(string? keyword, int? status, IFopRequest request);
    void Update(IEnumerable<LeadProfile> t);
    bool CheckUsing(Guid id);
}
