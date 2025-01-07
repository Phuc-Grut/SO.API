using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IProductionOrderRepository : IRepository<ProductionOrder>
{
    Task<ProductionOrder> GetByCode(string code);
    Task<IEnumerable<ProductionOrder>> GetListCbx(int? status);
    Task<(IEnumerable<ProductionOrder>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<ProductionOrder>> Filter(Dictionary<string, object> filter);
}
