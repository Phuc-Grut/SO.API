using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IPromotionProductBuyRepository : IRepository<PromotionProductBuy>
{
    void Update(IEnumerable<PromotionProductBuy> items);
    void Add(IEnumerable<PromotionProductBuy> items);
    void Remove(IEnumerable<PromotionProductBuy> t);
}
