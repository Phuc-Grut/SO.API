using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IRptTongHopBanHangTheoMatHangRepository : IRepository<RptTongHopBanHangTheoMatHang>
{
    Task<(IEnumerable<RptTongHopBanHangTheoMatHang>, int)> Filter(string? keyword, IFopRequest request);
    Task<IEnumerable<RptTongHopBanHangTheoMatHang>> GetByParentId(Guid id);
    void Add(IEnumerable<RptTongHopBanHangTheoMatHang> t);
    void Update(IEnumerable<RptTongHopBanHangTheoMatHang> t);
    void Remove(IEnumerable<RptTongHopBanHangTheoMatHang> t);
}
