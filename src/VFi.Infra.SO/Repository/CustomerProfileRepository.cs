using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public class CustomerProfileRepository : ICustomerProfileRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<CustomerProfile> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public CustomerProfileRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<CustomerProfile>();
    }

    public void Add(CustomerProfile customerProfile)
    {
        DbSet.Add(customerProfile);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<CustomerProfile>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<CustomerProfile> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(CustomerProfile customerProfile)
    {
        DbSet.Remove(customerProfile);
    }

    public void Update(CustomerProfile customerProfile)
    {
        DbSet.Update(customerProfile);
    }

    public async Task<IEnumerable<CustomerProfile>> Filter(string? keyword, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Key.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<int> FilterCount(string? keyword)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Key.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
        }
        return await query.CountAsync();
    }


    public void Update(IEnumerable<CustomerProfile> profiles)
    {
        DbSet.UpdateRange(profiles);
    }

    public Task<CustomerProfile> GetByAccountId(string accountId)
    {
        throw new NotImplementedException();
    }

    public Task<CustomerProfile> GetByKey(string accountId, string key)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CustomerProfile>> GetByGroup(string accountId, string group)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CustomerProfile>> Filter(string? keyword, int? status, int pagesize, int pageindex)
    {
        throw new NotImplementedException();
    }

    public Task<int> FilterCount(string? keyword, int? status)
    {
        throw new NotImplementedException();
    }
}
