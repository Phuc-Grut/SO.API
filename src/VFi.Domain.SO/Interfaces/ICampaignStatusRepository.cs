using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface ICampaignStatusRepository : IRepository<CampaignStatus>
{
    Task<(IEnumerable<CampaignStatus>, int)> Filter(string? keyword, int? status, Guid? campaignId, IFopRequest request);
    Task<IEnumerable<CampaignStatus>> GetListCbx(int? status);
    void Add(IEnumerable<CampaignStatus> items);
    void Update(IEnumerable<CampaignStatus> items);
    void Remove(IEnumerable<CampaignStatus> items);
    bool CheckUsing(Guid id);
}
