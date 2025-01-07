using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;

namespace VFi.Infra.SO.Repository;

public class CustomerGroupMappingRepository : ICustomerGroupMappingRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<CustomerGroupMapping> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public CustomerGroupMappingRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<CustomerGroupMapping>();
    }

    public void Add(CustomerGroupMapping CustomerGroupMapping)
    {
        DbSet.Add(CustomerGroupMapping);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<CustomerGroupMapping>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("customerGroupId"))
            {
                query = query.Where(x => x.CustomerGroupId.Equals(new Guid(item.Value + "")));
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
            if (item.Key.Equals("customerGroupId"))
            {
                query = query.Where(x => x.CustomerGroupId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<CustomerGroupMapping>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<CustomerGroupMapping> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(CustomerGroupMapping CustomerGroupMapping)
    {
        DbSet.Remove(CustomerGroupMapping);
    }

    public void Update(CustomerGroupMapping CustomerGroupMapping)
    {
        DbSet.Update(CustomerGroupMapping);
    }

    public async Task<IEnumerable<CustomerGroupMapping>> GetListListBox(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("customerGroupId"))
            {
                query = query.Where(x => x.CustomerGroupId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.ToListAsync();
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<CustomerGroupMapping>> Filter(Guid id)
    {
        return await DbSet.Where(x => x.CustomerId == id || x.CustomerGroupId == id).Include(x => x.CustomerGroup).OrderBy(x => x.CreatedDate).ToListAsync();
    }
    public void Add(IEnumerable<CustomerGroupMapping> options)
    {
        DbSet.AddRange(options);
    }
    public void Remove(IEnumerable<CustomerGroupMapping> t)
    {
        DbSet.RemoveRange(t);
    }
}
