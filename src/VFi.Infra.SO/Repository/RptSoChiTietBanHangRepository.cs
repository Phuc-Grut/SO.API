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

public class RptSoChiTietBanHangRepository : IRptSoChiTietBanHangRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<RptSoChiTietBanHang> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public RptSoChiTietBanHangRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<RptSoChiTietBanHang>();
    }

    public void Add(RptSoChiTietBanHang item)
    {
        DbSet.Add(item);
    }

    public void Add(IEnumerable<RptSoChiTietBanHang> t)
    {
        DbSet.AddRange(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<RptSoChiTietBanHang>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<RptSoChiTietBanHang> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);

    }

    public void Remove(RptSoChiTietBanHang item)
    {
        DbSet.Remove(item);
    }
    public void Remove(IEnumerable<RptSoChiTietBanHang> t)
    {
        DbSet.RemoveRange(t);
    }

    public void Update(RptSoChiTietBanHang item)
    {
        DbSet.Update(item);
    }
    public void Update(IEnumerable<RptSoChiTietBanHang> t)
    {
        DbSet.UpdateRange(t);
    }
    public async Task<(IEnumerable<RptSoChiTietBanHang>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.ProductName.Contains(keyword) || x.ProductCode.Contains(keyword));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }
    public async Task<IEnumerable<RptSoChiTietBanHang>> GetByParentId(Guid id)
    {
        return await DbSet.Where(x => x.ReportId == id).ToListAsync();
    }
}
