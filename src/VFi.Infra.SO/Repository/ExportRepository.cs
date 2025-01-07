using System.Linq;
using System.Security.Principal;
using Consul;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class ExportRepository : IExportRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Export> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ExportRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Export>();
    }

    public void Add(Export Export)
    {
        DbSet.Add(Export);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Export>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Export> GetById(Guid id)
    {
        return await DbSet.Include(x => x.ExportProducts).FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Remove(Export Export)
    {
        DbSet.Remove(Export);
    }

    public void Update(Export Export)
    {
        DbSet.Update(Export);
    }
    public async Task<Export> GetByExportWarehouseId(Guid exportWarehouseId)
    {
        return await DbSet.Include(x => x.ExportProducts).FirstOrDefaultAsync(x => x.ExportWarehouseId.Equals(exportWarehouseId));
    }
    public async Task<Export> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<IEnumerable<Export>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.Select(x => x);
        if (!String.IsNullOrEmpty(keyword))
        {
            query = DbSet.Where(x => x.Code.Contains(keyword));
        }

        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<IEnumerable<Export>> GetListCbx(int? status)
    {
        return await DbSet.ToListAsync();
    }
    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword));
        }

        return await query.CountAsync();
    }

    public async Task<(IEnumerable<Export>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.Include(x => x.ExportProducts).Include(x => x.ExportWarehouse).ThenInclude(x => x.Order).AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || x.ExportWarehouse.Order.Code.Contains(keyword));
        }

        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }
    public void Approve(Export t)
    {
        Db.Entry(t).Property(x => x.Status).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveDate).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveBy).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveByName).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveComment).IsModified = true;
    }
}
