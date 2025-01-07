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

public class RptTinhHinhThucHienHopDongBanRepository : IRptTinhHinhThucHienHopDongBanRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<RptTinhHinhThucHienHopDongBan> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public RptTinhHinhThucHienHopDongBanRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<RptTinhHinhThucHienHopDongBan>();
    }

    public void Add(RptTinhHinhThucHienHopDongBan item)
    {
        DbSet.Add(item);
    }

    public void Add(IEnumerable<RptTinhHinhThucHienHopDongBan> t)
    {
        DbSet.AddRange(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<RptTinhHinhThucHienHopDongBan>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<RptTinhHinhThucHienHopDongBan> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);

    }

    public void Remove(RptTinhHinhThucHienHopDongBan item)
    {
        DbSet.Remove(item);
    }
    public void Remove(IEnumerable<RptTinhHinhThucHienHopDongBan> t)
    {
        DbSet.RemoveRange(t);
    }

    public void Update(RptTinhHinhThucHienHopDongBan item)
    {
        DbSet.Update(item);
    }
    public void Update(IEnumerable<RptTinhHinhThucHienHopDongBan> t)
    {
        DbSet.UpdateRange(t);
    }
    public async Task<(IEnumerable<RptTinhHinhThucHienHopDongBan>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.CustomerCode.Contains(keyword) || x.CustomerName.Contains(keyword) || x.ProductCode.Contains(keyword) || x.ProductName.Contains(keyword));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }
    public async Task<IEnumerable<RptTinhHinhThucHienHopDongBan>> GetByParentId(Guid id)
    {
        return await DbSet.Where(x => x.ReportId == id).ToListAsync();
    }
}
