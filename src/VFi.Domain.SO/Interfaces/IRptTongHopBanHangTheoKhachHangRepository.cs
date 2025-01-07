using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IRptTongHopBanHangTheoKhachHangRepository : IRepository<RptTongHopBanHangTheoKhachHang>
{
    Task<(IEnumerable<RptTongHopBanHangTheoKhachHang>, int)> Filter(string? keyword, IFopRequest request);
    Task<IEnumerable<RptTongHopBanHangTheoKhachHang>> GetByParentId(Guid id);
    void Add(IEnumerable<RptTongHopBanHangTheoKhachHang> t);
    void Update(IEnumerable<RptTongHopBanHangTheoKhachHang> t);
    void Remove(IEnumerable<RptTongHopBanHangTheoKhachHang> t);
}
