using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IPromotionProductRepository : IRepository<PromotionProduct>
{
    void Update(IEnumerable<PromotionProduct> items);
    void Add(IEnumerable<PromotionProduct> items);
    void Remove(IEnumerable<PromotionProduct> t);
}
