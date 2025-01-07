using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface ILeadCampaignRepository : IRepository<LeadCampaign>
{
    Task<(IEnumerable<LeadCampaign>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<LeadCampaign>> Filter(Dictionary<string, object> filter);
    Task<IEnumerable<LeadCampaign>> GetListCbx(int? status);
    void Add(IEnumerable<LeadCampaign> items);
    void Update(IEnumerable<LeadCampaign> items);
    void Remove(IEnumerable<LeadCampaign> items);
    bool CheckUsing(Guid id);
}
