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

public class RptTinhHinhThucHienDonBanHangRepository : IRptTinhHinhThucHienDonBanHangRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<RptTinhHinhThucHienDonBanHang> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public RptTinhHinhThucHienDonBanHangRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<RptTinhHinhThucHienDonBanHang>();
    }

    public void Add(RptTinhHinhThucHienDonBanHang item)
    {
        DbSet.Add(item);
    }

    public void Add(IEnumerable<RptTinhHinhThucHienDonBanHang> t)
    {
        DbSet.AddRange(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<RptTinhHinhThucHienDonBanHang>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<RptTinhHinhThucHienDonBanHang> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);

    }

    public void Remove(RptTinhHinhThucHienDonBanHang item)
    {
        DbSet.Remove(item);
    }
    public void Remove(IEnumerable<RptTinhHinhThucHienDonBanHang> t)
    {
        DbSet.RemoveRange(t);
    }

    public void Update(RptTinhHinhThucHienDonBanHang item)
    {
        DbSet.Update(item);
    }
    public void Update(IEnumerable<RptTinhHinhThucHienDonBanHang> t)
    {
        DbSet.UpdateRange(t);
    }
    public async Task<(IEnumerable<RptTinhHinhThucHienDonBanHang>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.CustomerCode.Contains(keyword) || x.CustomerName.Contains(keyword) || x.ProductCode.Contains(keyword) || x.ProductName.Contains(keyword));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }
    public async Task<IEnumerable<RptTinhHinhThucHienDonBanHang>> GetByParentId(Guid id)
    {
        return await DbSet.Where(x => x.ReportId == id).ToListAsync();
    }
}
