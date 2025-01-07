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

public class RptTongHopBanHangTheoNhomKhachHangRepository : IRptTongHopBanHangTheoNhomKhachHangRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<RptTongHopBanHangTheoNhomKhachHang> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public RptTongHopBanHangTheoNhomKhachHangRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<RptTongHopBanHangTheoNhomKhachHang>();
    }

    public void Add(RptTongHopBanHangTheoNhomKhachHang item)
    {
        DbSet.Add(item);
    }

    public void Add(IEnumerable<RptTongHopBanHangTheoNhomKhachHang> t)
    {
        DbSet.AddRange(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<RptTongHopBanHangTheoNhomKhachHang>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<RptTongHopBanHangTheoNhomKhachHang> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);

    }

    public void Remove(RptTongHopBanHangTheoNhomKhachHang item)
    {
        DbSet.Remove(item);
    }
    public void Remove(IEnumerable<RptTongHopBanHangTheoNhomKhachHang> t)
    {
        DbSet.RemoveRange(t);
    }

    public void Update(RptTongHopBanHangTheoNhomKhachHang item)
    {
        DbSet.Update(item);
    }
    public void Update(IEnumerable<RptTongHopBanHangTheoNhomKhachHang> t)
    {
        DbSet.UpdateRange(t);
    }
    public async Task<(IEnumerable<RptTongHopBanHangTheoNhomKhachHang>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }
    public async Task<IEnumerable<RptTongHopBanHangTheoNhomKhachHang>> GetByParentId(Guid id)
    {
        return await DbSet.Where(x => x.ReportId == id).ToListAsync();
    }
}
