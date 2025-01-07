using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;

namespace VFi.Infra.SO.Repository;

public class CustomerBusinessMappingRepository : ICustomerBusinessMappingRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<CustomerBusinessMapping> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public CustomerBusinessMappingRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<CustomerBusinessMapping>();
    }

    public void Add(CustomerBusinessMapping CustomerBusinessMapping)
    {
        DbSet.Add(CustomerBusinessMapping);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<CustomerBusinessMapping>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("businessId"))
            {
                query = query.Where(x => x.BusinessId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("businessId"))
            {
                query = query.Where(x => x.BusinessId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<CustomerBusinessMapping>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<CustomerBusinessMapping> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(CustomerBusinessMapping CustomerBusinessMapping)
    {
        DbSet.Remove(CustomerBusinessMapping);
    }

    public void Update(CustomerBusinessMapping CustomerBusinessMapping)
    {
        DbSet.Update(CustomerBusinessMapping);
    }

    public async Task<IEnumerable<CustomerBusinessMapping>> GetListListBox(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("businessId"))
            {
                query = query.Where(x => x.BusinessId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.ToListAsync();
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<CustomerBusinessMapping>> Filter(Guid id)
    {
        return await DbSet.Include(x => x.Business).Where(x => x.CustomerId == id).OrderBy(x => x.CreatedDate).ToListAsync();
    }
    public void Add(IEnumerable<CustomerBusinessMapping> options)
    {
        DbSet.AddRange(options);
    }
    public void Remove(IEnumerable<CustomerBusinessMapping> t)
    {
        DbSet.RemoveRange(t);
    }
}
