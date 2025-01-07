using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class BusinessRepository : IBusinessRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Business> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public BusinessRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Business>();
    }
    public void Add(Business Business)
    {
        DbSet.Add(Business);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Business>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Business> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }
    public async Task<IEnumerable<Business>> GetById(IEnumerable<CustomerBusinessMapping> mapping)
    {
        var query = DbSet.AsQueryable();
        var data = mapping.ToList().Select(x => x.BusinessId).ToList();
        return await query.Where(x => data.Contains(x.Id)).ToListAsync();
    }

    public void Remove(Business Business)
    {
        DbSet.Remove(Business);
    }

    public void Update(Business Business)
    {
        DbSet.Update(Business);
    }

    public async Task<Business> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<(IEnumerable<Business>, int)> Filter(string? keyword, int? status, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
        }
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.OrderBy(x => x.DisplayOrder).ToListAsync(), totalCount);
    }


    public async Task<IEnumerable<Business>> GetListCbx(int? status)
    {
        var query = DbSet.AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public void Update(IEnumerable<Business> t)
    {
        DbSet.UpdateRange(t);
    }

    public async Task<IEnumerable<Business>> Filter(IEnumerable<string>? name)
    {
        var a = name?.Select(x => x.ToLower());
        var query = DbSet.Where(x => a.Contains(x.Name.ToLower()));
        return await query.ToListAsync();
    }
}
