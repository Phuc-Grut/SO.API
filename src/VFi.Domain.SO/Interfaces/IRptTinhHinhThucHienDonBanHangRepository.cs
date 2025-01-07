using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IRptTinhHinhThucHienDonBanHangRepository : IRepository<RptTinhHinhThucHienDonBanHang>
{
    Task<(IEnumerable<RptTinhHinhThucHienDonBanHang>, int)> Filter(string? keyword, IFopRequest request);
    Task<IEnumerable<RptTinhHinhThucHienDonBanHang>> GetByParentId(Guid id);
    void Add(IEnumerable<RptTinhHinhThucHienDonBanHang> t);
    void Update(IEnumerable<RptTinhHinhThucHienDonBanHang> t);
    void Remove(IEnumerable<RptTinhHinhThucHienDonBanHang> t);
}
