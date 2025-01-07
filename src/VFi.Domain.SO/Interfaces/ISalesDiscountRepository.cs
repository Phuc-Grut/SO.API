using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface ISalesDiscountRepository : IRepository<SalesDiscount>
{
    Task<SalesDiscount> GetByCode(string code);
    Task<SalesDiscount> GetById(Guid id);
    Task<IEnumerable<SalesDiscount>> GetByOrder(Guid id);
    Task<IEnumerable<SalesDiscount>> GetByOrder(string ordercode, bool? getDetail);
    Task<IEnumerable<SalesDiscount>> GetByOrder(List<string> ordercode, bool? getDetail);
    Task<(IEnumerable<SalesDiscount>, int)> Filter(string? keyword, Guid? vendorId, int? status, IFopRequest request);
    Task<IEnumerable<SalesDiscountProduct>> Filter(Dictionary<string, object> filter);

}
