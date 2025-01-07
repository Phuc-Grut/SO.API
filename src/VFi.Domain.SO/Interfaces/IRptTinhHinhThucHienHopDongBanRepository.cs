using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IRptTinhHinhThucHienHopDongBanRepository : IRepository<RptTinhHinhThucHienHopDongBan>
{
    Task<(IEnumerable<RptTinhHinhThucHienHopDongBan>, int)> Filter(string? keyword, IFopRequest request);
    Task<IEnumerable<RptTinhHinhThucHienHopDongBan>> GetByParentId(Guid id);
    void Add(IEnumerable<RptTinhHinhThucHienHopDongBan> t);
    void Update(IEnumerable<RptTinhHinhThucHienHopDongBan> t);
    void Remove(IEnumerable<RptTinhHinhThucHienHopDongBan> t);
}
