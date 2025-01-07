using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IPromotionGroupRepository : IRepository<PromotionGroup>
{
    Task<PromotionGroup> GetByCode(string code);
    Task<(IEnumerable<PromotionGroup>, int)> Filter(string? keyword, int? status, IFopRequest request);
    Task<IEnumerable<PromotionGroup>> GetListCbx(int? status);
}
