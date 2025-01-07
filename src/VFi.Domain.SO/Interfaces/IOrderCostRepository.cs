using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IOrderCostRepository : IRepository<OrderCost>
{
    //Task<IEnumerable<OrderCost>> Filter(Guid id, int pagesize, int pageindex);
    //Task<int> FilterCount(Guid id);
    //Task<IEnumerable<OrderCost>> GetAll(Guid id);

    Task<OrderCost> GetByCode(string code);
    Task<IEnumerable<OrderCost>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<OrderCost>> GetListCbx(int? status);
    Task<(IEnumerable<OrderCost>, int)> Filter(IFopRequest request);
}
