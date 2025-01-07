using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public class CustomerContactRepository : ICustomerContactRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<CustomerContact> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public CustomerContactRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<CustomerContact>();
    }

    public void Add(CustomerContact CustomerContact)
    {
        DbSet.Add(CustomerContact);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<CustomerContact>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<CustomerContact> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(CustomerContact CustomerContact)
    {
        DbSet.Remove(CustomerContact);
    }

    public void Update(CustomerContact CustomerContact)
    {
        DbSet.Update(CustomerContact);
    }

    public async Task<IEnumerable<CustomerContact>> Filter(string? keyword, int? status, Guid? customerId, int pagesize, int pageindex)
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

    public async Task<IEnumerable<CustomerContact>> GetListCbx(int? status, Guid? customerId)
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

    public async Task<IEnumerable<CustomerContact>> Filter(int? status, Guid? customerId)
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
    public void Add(IEnumerable<CustomerContact> items)
    {
        DbSet.AddRange(items);
    }
    public void Update(IEnumerable<CustomerContact> items)
    {
        DbSet.UpdateRange(items);
    }
    public void Remove(IEnumerable<CustomerContact> items)
    {
        DbSet.RemoveRange(items);
    }
    public async Task<IEnumerable<CustomerContact>> GetByParentId(Guid id)
    {
        return await DbSet.Where(x => x.CustomerId == id || x.EmployeeId == id).OrderBy(x => x.SortOrder).ToListAsync();
    }
}
