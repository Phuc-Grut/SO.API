using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public class CustomerBankRepository : ICustomerBankRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<CustomerBank> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public CustomerBankRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<CustomerBank>();

    }

    public void Add(CustomerBank CustomerBank)
    {
        DbSet.Add(CustomerBank);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<CustomerBank>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<CustomerBank> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(CustomerBank CustomerBank)
    {
        DbSet.Remove(CustomerBank);
    }

    public void Update(CustomerBank CustomerBank)
    {
        DbSet.Update(CustomerBank);
    }

    public async Task<IEnumerable<CustomerBank>> Filter(string? keyword, int? status, Guid? customerId, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%"));
        }
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        if (customerId != null)
        {
            query = query.Where(x => x.CustomerId == customerId);
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<int> FilterCount(string? keyword, int? status, Guid? customerId)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%"));
        }
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        if (customerId != null)
        {
            query = query.Where(x => x.CustomerId == customerId);
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<CustomerBank>> GetListCbx(int? status, Guid? customerId)
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

    public async Task<IEnumerable<CustomerBank>> Filter(int? status, Guid? customerId)
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
    public void Add(IEnumerable<CustomerBank> items)
    {
        DbSet.AddRange(items);
    }
    public void Update(IEnumerable<CustomerBank> items)
    {
        DbSet.UpdateRange(items);
    }
    public void Remove(IEnumerable<CustomerBank> items)
    {
        DbSet.RemoveRange(items);
    }

    public async Task<IEnumerable<CustomerBank>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.AccountNumber.Contains(keyword) || x.AccountName.Contains(keyword));
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
            query = query.Where(x => x.AccountNumber.Contains(keyword) || x.AccountName.Contains(keyword));
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

    public async Task<IEnumerable<CustomerBank>> GetByParentId(Guid id)
    {
        return await DbSet.Where(x => x.CustomerId == id || x.EmployeeId == id).OrderBy(x => x.SortOrder).ToListAsync();
    }
}
