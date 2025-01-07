using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IExportWarehouseProductRepository : IRepository<ExportWarehouseProduct>
{
    Task<IEnumerable<ExportWarehouseProduct>> GetListListBox(Dictionary<string, object> filter, string? keyword);
    Task<IEnumerable<ExportWarehouseProduct>> Filter(Dictionary<string, object> filter);
    Task<(IEnumerable<ExportWarehouseProduct>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    void Update(IEnumerable<ExportWarehouseProduct> items);
    void Add(IEnumerable<ExportWarehouseProduct> items);
    void Remove(IEnumerable<ExportWarehouseProduct> t);
    Task<IEnumerable<ExportWarehouseProduct>> GetByOrderId(string code);
    Task<IEnumerable<ExportWarehouseProduct>> GetByOrderIds(IEnumerable<Guid> ids);
    Task<IEnumerable<ExportWarehouseProduct>> GetByExportWarehouseId(Guid exportWarehouseId);
}
