using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class RptTongHopBanHangTheoKhachHangRepository : IRptTongHopBanHangTheoKhachHangRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<RptTongHopBanHangTheoKhachHang> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public RptTongHopBanHangTheoKhachHangRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<RptTongHopBanHangTheoKhachHang>();
    }

    public void Add(RptTongHopBanHangTheoKhachHang item)
    {
        DbSet.Add(item);
    }

    public void Add(IEnumerable<RptTongHopBanHangTheoKhachHang> t)
    {
        DbSet.AddRange(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<RptTongHopBanHangTheoKhachHang>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<RptTongHopBanHangTheoKhachHang> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);

    }

    public void Remove(RptTongHopBanHangTheoKhachHang item)
    {
        DbSet.Remove(item);
    }
    public void Remove(IEnumerable<RptTongHopBanHangTheoKhachHang> t)
    {
        DbSet.RemoveRange(t);
    }

    public void Update(RptTongHopBanHangTheoKhachHang item)
    {
        DbSet.Update(item);
    }
    public void Update(IEnumerable<RptTongHopBanHangTheoKhachHang> t)
    {
        DbSet.UpdateRange(t);
    }
    public async Task<(IEnumerable<RptTongHopBanHangTheoKhachHang>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.CustomerCode.Contains(keyword) || x.CustomerName.Contains(keyword));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }
    public async Task<IEnumerable<RptTongHopBanHangTheoKhachHang>> GetByParentId(Guid id)
    {
        return await DbSet.Where(x => x.ReportId == id).ToListAsync();
    }
}
