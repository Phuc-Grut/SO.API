using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class CustomerPriceListCrossRepository : ICustomerPriceListCrossRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<CustomerPriceListCross> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public CustomerPriceListCrossRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<CustomerPriceListCross>();
    }


    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<CustomerPriceListCross>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<CustomerPriceListCross> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(CustomerPriceListCross CustomerPriceListCross)
    {
        DbSet.Remove(CustomerPriceListCross);
    }

    public void Add(CustomerPriceListCross CustomerPriceListCross)
    {
        DbSet.Add(CustomerPriceListCross);
    }

    public void Add(IEnumerable<CustomerPriceListCross> t)
    {
        DbSet.AddRange(t);
    }

    public void Remove(IEnumerable<CustomerPriceListCross> t)
    {
        DbSet.RemoveRange(t);
    }

    public void Update(CustomerPriceListCross t)
    {
        DbSet.Update(t);
    }
}
