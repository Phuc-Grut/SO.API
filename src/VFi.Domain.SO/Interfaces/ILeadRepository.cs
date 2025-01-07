using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface ILeadRepository : IRepository<Lead>
{
    Task<Lead> GetByCode(string code);
    Task<(IEnumerable<Lead>, int)> Filter(string? keyword, int? status, int? convert, string? tags, string? id, IFopRequest request);
    Task<IEnumerable<Lead>> GetListCbx(int? status);
    Task<IEnumerable<Lead>> Filter(Dictionary<string, object> filter);
    void Add(IEnumerable<Lead> items);
    void Update(IEnumerable<Lead> items);
}
