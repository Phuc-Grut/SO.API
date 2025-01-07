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

public class RptTongHopBanHangTheoNhanVienRepository : IRptTongHopBanHangTheoNhanVienRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<RptTongHopBanHangTheoNhanVien> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public RptTongHopBanHangTheoNhanVienRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<RptTongHopBanHangTheoNhanVien>();
    }

    public void Add(RptTongHopBanHangTheoNhanVien item)
    {
        DbSet.Add(item);
    }

    public void Add(IEnumerable<RptTongHopBanHangTheoNhanVien> t)
    {
        DbSet.AddRange(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<RptTongHopBanHangTheoNhanVien>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<RptTongHopBanHangTheoNhanVien> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);

    }

    public void Remove(RptTongHopBanHangTheoNhanVien item)
    {
        DbSet.Remove(item);
    }
    public void Remove(IEnumerable<RptTongHopBanHangTheoNhanVien> t)
    {
        DbSet.RemoveRange(t);
    }

    public void Update(RptTongHopBanHangTheoNhanVien item)
    {
        DbSet.Update(item);
    }
    public void Update(IEnumerable<RptTongHopBanHangTheoNhanVien> t)
    {
        DbSet.UpdateRange(t);
    }
    public async Task<(IEnumerable<RptTongHopBanHangTheoNhanVien>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.EmployeeCode.Contains(keyword) || x.EmployeeName.Contains(keyword));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }
    public async Task<IEnumerable<RptTongHopBanHangTheoNhanVien>> GetByParentId(Guid id)
    {
        return await DbSet.Where(x => x.ReportId == id).ToListAsync();
    }
}
