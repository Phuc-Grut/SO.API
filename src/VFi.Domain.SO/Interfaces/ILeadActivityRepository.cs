using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface ILeadActivityRepository : IRepository<LeadActivity>
{
    Task<(IEnumerable<LeadActivity>, int)> Filter(string? keyword, int? status, IFopRequest request);
    void Update(IEnumerable<LeadActivity> t);
    bool CheckUsing(Guid id);
}
