using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IRptTongHopBanHangTheoKhachHangVaDonBanHangRepository : IRepository<RptTongHopBanHangTheoKhachHangVaDonBanHang>
{
    Task<(IEnumerable<RptTongHopBanHangTheoKhachHangVaDonBanHang>, int)> Filter(string? keyword, IFopRequest request);
    Task<IEnumerable<RptTongHopBanHangTheoKhachHangVaDonBanHang>> GetByParentId(Guid id);
    void Add(IEnumerable<RptTongHopBanHangTheoKhachHangVaDonBanHang> t);
    void Update(IEnumerable<RptTongHopBanHangTheoKhachHangVaDonBanHang> t);
    void Remove(IEnumerable<RptTongHopBanHangTheoKhachHangVaDonBanHang> t);
}
