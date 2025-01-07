using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IShippingMethodRepository : IRepository<ShippingMethod>
{
    Task<ShippingMethod> GetByCode(string code);
    Task<(IEnumerable<ShippingMethod>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<ShippingMethod>> GetListCbx(int? status);
}
