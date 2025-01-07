using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IRptTongHopBanHangTheoNhomKhachHangRepository : IRepository<RptTongHopBanHangTheoNhomKhachHang>
{
    Task<(IEnumerable<RptTongHopBanHangTheoNhomKhachHang>, int)> Filter(string? keyword, IFopRequest request);
    Task<IEnumerable<RptTongHopBanHangTheoNhomKhachHang>> GetByParentId(Guid id);
    void Add(IEnumerable<RptTongHopBanHangTheoNhomKhachHang> t);
    void Update(IEnumerable<RptTongHopBanHangTheoNhomKhachHang> t);
    void Remove(IEnumerable<RptTongHopBanHangTheoNhomKhachHang> t);
}
