using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class ShippingCarrierRepository : IShippingCarrierRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ShippingCarrier> DbSet;
    public IUnitOfWork UnitOfWork => Db;

    public ShippingCarrierRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<ShippingCarrier>();
    }

    public void Add(ShippingCarrier ShippingCarrier)
    {
        DbSet.Add(ShippingCarrier);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<ShippingCarrier>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ShippingCarrier> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(ShippingCarrier ShippingCarrier)
    {
        DbSet.Remove(ShippingCarrier);
    }

    public void Update(ShippingCarrier ShippingCarrier)
    {
        DbSet.Update(ShippingCarrier);
    }

    public async Task<ShippingCarrier> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<(IEnumerable<ShippingCarrier>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
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

            if (item.Key.Equals("country"))
            {
                query = query.Where(x => x.Country.Equals(item.Value));
            }
        }

        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<IEnumerable<ShippingCarrier>> GetListCbx(int? status = 1, string? country = "")
    {
        if (string.IsNullOrEmpty(country))
        {
            return await DbSet.Where(x => x.Status == status).ToListAsync();
        }

        return await DbSet.Where(x => x.Status == status && x.Country.Equals(country)).ToListAsync();
    }
}