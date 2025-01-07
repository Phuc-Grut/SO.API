using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IExportWarehouseRepository : IRepository<ExportWarehouse>
{

    Task<ExportWarehouse> GetByCode(string code);
    Task<IEnumerable<ExportWarehouse>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<IEnumerable<ExportWarehouse>> Filter(Dictionary<string, object> filter);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<ExportWarehouse>> GetListCbx(int? status);
    Task<(IEnumerable<ExportWarehouse>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    void Approve(ExportWarehouse t);

}

