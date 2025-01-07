using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IPromotionByValueRepository : IRepository<PromotionByValue>
{
    Task<IEnumerable<PromotionByValue>> Filter(Guid id);
    void Update(IEnumerable<PromotionByValue> items);
    void Add(IEnumerable<PromotionByValue> items);
    void Remove(IEnumerable<PromotionByValue> t);
}
