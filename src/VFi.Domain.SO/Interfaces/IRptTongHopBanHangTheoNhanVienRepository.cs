using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IRptTongHopBanHangTheoNhanVienRepository : IRepository<RptTongHopBanHangTheoNhanVien>
{
    Task<(IEnumerable<RptTongHopBanHangTheoNhanVien>, int)> Filter(string? keyword, IFopRequest request);
    Task<IEnumerable<RptTongHopBanHangTheoNhanVien>> GetByParentId(Guid id);
    void Add(IEnumerable<RptTongHopBanHangTheoNhanVien> t);
    void Update(IEnumerable<RptTongHopBanHangTheoNhanVien> t);
    void Remove(IEnumerable<RptTongHopBanHangTheoNhanVien> t);
}
