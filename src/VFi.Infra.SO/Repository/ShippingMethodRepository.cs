using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class ShippingMethodRepository : IShippingMethodRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ShippingMethod> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ShippingMethodRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<ShippingMethod>();
    }

    public void Add(ShippingMethod ShippingMethod)
    {
        DbSet.Add(ShippingMethod);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<ShippingMethod>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ShippingMethod> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(ShippingMethod ShippingMethod)
    {
        DbSet.Remove(ShippingMethod);
    }

    public void Update(ShippingMethod ShippingMethod)
    {
        DbSet.Update(ShippingMethod);
    }

    public async Task<ShippingMethod> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<(IEnumerable<ShippingMethod>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%") || x.Code.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<IEnumerable<ShippingMethod>> GetListCbx(int? status)
    {
        return await DbSet.Where(x => x.Status == status).ToListAsync();
    }
}
