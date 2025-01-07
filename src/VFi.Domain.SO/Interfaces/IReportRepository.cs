using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IReportRepository : IRepository<Report>
{
    Task<(IEnumerable<Report>, int)> Filter(string? keyword, IFopRequest request);
}
