using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class StoreRepository : IStoreRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Store> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public StoreRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Store>();
    }

    public void Add(Store store)
    {
        DbSet.Add(store);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Store>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Store> GetById(Guid id)
    {
        return await DbSet.Include(x => x.StorePriceList).FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Remove(Store store)
    {
        DbSet.Remove(store);
    }

    public void Update(Store store)
    {
        DbSet.Update(store);
    }
    public async Task<bool> CheckExist(string? code, Guid? id)
    {
        if (id == null)
        {
            if (String.IsNullOrEmpty(code))
            {
                return false;
            }
            return await DbSet.AnyAsync(x => x.Code.Equals(code));
        }
        return await DbSet.AnyAsync(x => x.Code.Equals(code) && x.Id != id);
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }
    public async Task<(IEnumerable<Store>, int)> Filter(Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.OrderBy(x => x.DisplayOrder).ToListAsync(), totalCount);
    }

    public async Task<IEnumerable<Store>> GetListListBox(int? status)
    {
        var query = DbSet.AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public void Update(IEnumerable<Store> stores)
    {
        DbSet.UpdateRange(stores);
    }
}
