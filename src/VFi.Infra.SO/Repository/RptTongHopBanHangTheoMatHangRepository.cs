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

public class RptTongHopBanHangTheoMatHangRepository : IRptTongHopBanHangTheoMatHangRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<RptTongHopBanHangTheoMatHang> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public RptTongHopBanHangTheoMatHangRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<RptTongHopBanHangTheoMatHang>();
    }

    public void Add(RptTongHopBanHangTheoMatHang item)
    {
        DbSet.Add(item);
    }

    public void Add(IEnumerable<RptTongHopBanHangTheoMatHang> t)
    {
        DbSet.AddRange(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<RptTongHopBanHangTheoMatHang>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<RptTongHopBanHangTheoMatHang> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);

    }

    public void Remove(RptTongHopBanHangTheoMatHang item)
    {
        DbSet.Remove(item);
    }
    public void Remove(IEnumerable<RptTongHopBanHangTheoMatHang> t)
    {
        DbSet.RemoveRange(t);
    }

    public void Update(RptTongHopBanHangTheoMatHang item)
    {
        DbSet.Update(item);
    }
    public void Update(IEnumerable<RptTongHopBanHangTheoMatHang> t)
    {
        DbSet.UpdateRange(t);
    }
    public async Task<(IEnumerable<RptTongHopBanHangTheoMatHang>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.ProductName.Contains(keyword) || x.ProductCode.Contains(keyword));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }
    public async Task<IEnumerable<RptTongHopBanHangTheoMatHang>> GetByParentId(Guid id)
    {
        return await DbSet.Where(x => x.ReportId == id).ToListAsync();
    }
}
