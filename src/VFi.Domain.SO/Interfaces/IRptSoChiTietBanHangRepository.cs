using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IRptSoChiTietBanHangRepository : IRepository<RptSoChiTietBanHang>
{
    Task<(IEnumerable<RptSoChiTietBanHang>, int)> Filter(string? keyword, IFopRequest request);
    Task<IEnumerable<RptSoChiTietBanHang>> GetByParentId(Guid id);
    void Add(IEnumerable<RptSoChiTietBanHang> t);
    void Update(IEnumerable<RptSoChiTietBanHang> t);
    void Remove(IEnumerable<RptSoChiTietBanHang> t);
}
