using System.Linq;
using System.Security.Principal;
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

public class StorePriceListRepository : IStorePriceListRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<StorePriceList> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public StorePriceListRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<StorePriceList>();
    }

    public void Add(StorePriceList StorePriceList)
    {
        DbSet.Add(StorePriceList);
        //DbSet.Add(Contact);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<StorePriceList>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<StorePriceList> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(StorePriceList StorePriceList)
    {
        DbSet.Remove(StorePriceList);
    }

    public void Update(StorePriceList StorePriceList)
    {
        DbSet.Update(StorePriceList);
    }

    public async Task<StorePriceList> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.CreatedByName.Equals(code));
    }
    public async Task<IEnumerable<StorePriceList>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.Select(x => x);
        if (!String.IsNullOrEmpty(keyword))
        {
            query = DbSet.Where(x => x.CreatedByName.Contains(keyword));
        }

        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<IEnumerable<StorePriceList>> GetListCbx(int? status)
    {
        return await DbSet.ToListAsync();
    }
    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.CreatedByName.Contains(keyword));
        }

        return await query.CountAsync();
    }

    public async Task<(IEnumerable<StorePriceList>, int)> Filter(IFopRequest request)
    {
        var (filtered, totalCount) = DbSet.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public void Remove(IEnumerable<StorePriceList> t)
    {
        DbSet.RemoveRange(t);
    }
    public void Add(IEnumerable<StorePriceList> details)
    {
        DbSet.AddRange(details);
    }
    public void Update(IEnumerable<StorePriceList> details)
    {
        DbSet.UpdateRange(details);
    }
}
