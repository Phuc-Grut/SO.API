using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IExportProductRepository : IRepository<ExportProduct>
{
    Task<IEnumerable<ExportProduct>> GetListListBox(Dictionary<string, object> filter, string? keyword);
    Task<IEnumerable<ExportProduct>> Filter(Dictionary<string, object> filter);
    void Update(IEnumerable<ExportProduct> items);
    void Add(IEnumerable<ExportProduct> items);
    void Remove(IEnumerable<ExportProduct> t);
}
