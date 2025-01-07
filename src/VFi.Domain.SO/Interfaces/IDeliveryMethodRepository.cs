using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IDeliveryMethodRepository : IRepository<DeliveryMethod>
{
    Task<DeliveryMethod> GetByCode(string code);
    Task<IEnumerable<DeliveryMethod>> GetListCbx(int? status);
    Task<(IEnumerable<DeliveryMethod>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);

}
