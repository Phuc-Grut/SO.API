using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IProductionOrdersDetailRepository : IRepository<ProductionOrdersDetail>
{
    Task<(IEnumerable<ProductionOrdersDetail>, int)> Filter(string? keyword, IFopRequest request, int? type, int? productOrderStatus);
    Task<(IEnumerable<ProductionOrdersDetail>, int)> Filter(string? keyword, IFopRequest request);
    Task<int> FilterCount(string? keyword, int? status, string? machineId);
    Task<IEnumerable<ProductionOrdersDetail>> GetAll(Guid id);
    void Add(IEnumerable<ProductionOrdersDetail> items);
    void Update(IEnumerable<ProductionOrdersDetail> items);
    void Remove(IEnumerable<ProductionOrdersDetail> items);

}
