using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IShippingCarrierRepository : IRepository<ShippingCarrier>
{
    Task<ShippingCarrier> GetByCode(string code);
    Task<(IEnumerable<ShippingCarrier>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<ShippingCarrier>> GetListCbx(int? status, string? country);

}
