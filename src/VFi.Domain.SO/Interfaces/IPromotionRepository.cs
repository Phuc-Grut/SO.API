using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IPromotionRepository : IRepository<Promotion>
{
    Task<Promotion> GetByCode(string code);
    Task<IEnumerable<Promotion>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<(IEnumerable<Promotion>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request, Guid? promotionGroupId);
    Task<IEnumerable<Promotion>> GetListCbx(int? status);
}
