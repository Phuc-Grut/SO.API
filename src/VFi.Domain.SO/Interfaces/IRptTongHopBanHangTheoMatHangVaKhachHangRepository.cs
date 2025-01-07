using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IRptTongHopBanHangTheoMatHangVaKhachHangRepository : IRepository<RptTongHopBanHangTheoMatHangVaKhachHang>
{
    Task<(IEnumerable<RptTongHopBanHangTheoMatHangVaKhachHang>, int)> Filter(string? keyword, IFopRequest request);
    Task<IEnumerable<RptTongHopBanHangTheoMatHangVaKhachHang>> GetByParentId(Guid id);
    void Add(IEnumerable<RptTongHopBanHangTheoMatHangVaKhachHang> t);
    void Update(IEnumerable<RptTongHopBanHangTheoMatHangVaKhachHang> t);
    void Remove(IEnumerable<RptTongHopBanHangTheoMatHangVaKhachHang> t);
}
