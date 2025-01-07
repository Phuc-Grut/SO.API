using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface ISalesDiscountProductRepository : IRepository<SalesDiscountProduct>
{
    void Add(IEnumerable<SalesDiscountProduct> t);
    void Update(IEnumerable<SalesDiscountProduct> t);
    void Remove(IEnumerable<SalesDiscountProduct> t);
    Task<IEnumerable<SalesDiscountProduct>> GetByParentId(Guid id);
    Task<IEnumerable<SalesDiscountProduct>> GetByParentId(List<Guid> id);
    Task<IEnumerable<SalesDiscountProduct>> GetByPurchaseProductId(Guid id);
    Task<IEnumerable<SalesDiscountProduct>> GetByOrderId(string code);
}
