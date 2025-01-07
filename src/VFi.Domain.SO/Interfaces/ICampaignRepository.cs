using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface ICampaignRepository : IRepository<Campaign>
{
    Task<Campaign> GetByCode(string code);
    Task<(IEnumerable<Campaign>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<Campaign>> GetListCbx(int? status);
    void Update(IEnumerable<Campaign> t);
    bool CheckUsing(Guid id);
}
