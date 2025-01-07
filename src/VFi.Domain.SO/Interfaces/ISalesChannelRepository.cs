using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface ISalesChannelRepository : IRepository<SalesChannel>
{
    Task<SalesChannel> GetByCode(string code);
    Task<IEnumerable<SalesChannel>> GetListCbx(int? status);
    Task<(IEnumerable<SalesChannel>, int)> Filter(string? keyword, int? status, IFopRequest request);

}
