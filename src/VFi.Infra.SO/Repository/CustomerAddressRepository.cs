using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public class CustomerAddressRepository : ICustomerAddressRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<CustomerAddress> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public CustomerAddressRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<CustomerAddress>();
    }

    public void Add(CustomerAddress CustomerAddress)
    {
        DbSet.Add(CustomerAddress);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<CustomerAddress>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<CustomerAddress> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }


    public void Remove(CustomerAddress CustomerAddress)
    {
        DbSet.Remove(CustomerAddress);
    }

    public void Update(CustomerAddress CustomerAddress)
    {
        DbSet.Update(CustomerAddress);
    }

    public async Task<IEnumerable<CustomerAddress>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Address.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId == Guid.Parse(item.Value.ToString()));
            }
            if (item.Key.Equals("accountId"))
            {

            }
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Address.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId == Guid.Parse(item.Value.ToString()));
            }
            if (item.Key.Equals("accountId"))
            {
                var customerid = Db.Set<Customer>().Where(x => x.AccountId.Equals(Guid.Parse(item.Value.ToString()))).Select(x => x.Id).ToList();
                if (customerid.Any())
                {
                    query = query.Where(x => x.CustomerId == customerid.First());
                }
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<CustomerAddress>> GetListCbx(int? status, Guid? customerId)
    {
        var query = DbSet.AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        if (customerId != null)
        {
            query = query.Where(x => x.CustomerId == customerId);
        }
        return await query.ToListAsync();
    }

    public void Add(IEnumerable<CustomerAddress> items)
    {
        DbSet.AddRange(items);
    }
    public void Update(IEnumerable<CustomerAddress> items)
    {
        DbSet.UpdateRange(items);
    }
    public void Remove(IEnumerable<CustomerAddress> items)
    {
        DbSet.RemoveRange(items);
    }

    public async Task<IEnumerable<CustomerAddress>> GetByParentId(Guid id)
    {
        return await DbSet.Where(x => x.CustomerId == id || x.EmployeeId == id).OrderBy(x => x.SortOrder).ToListAsync();
    }
}
