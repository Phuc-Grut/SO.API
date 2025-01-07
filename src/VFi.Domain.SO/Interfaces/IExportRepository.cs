using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IExportRepository : IRepository<Export>
{
    Task<Export> GetByExportWarehouseId(Guid ExportWarehouseId);
    Task<Export> GetByCode(string code);
    Task<IEnumerable<Export>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<Export>> GetListCbx(int? status);
    Task<(IEnumerable<Export>, int)> Filter(string? keyword, IFopRequest request);
    void Approve(Export t);

}

