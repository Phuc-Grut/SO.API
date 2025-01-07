using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class CustomerGroupRepository : ICustomerGroupRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<CustomerGroup> DbSet;
    protected readonly DbSet<CustomerGroupMapping> DbSetCustomerGroupMapping;
    protected readonly DbSet<PromotionCustomerGroup> DbSetPromotionCustomerGroup;

    public IUnitOfWork UnitOfWork => Db;
    public CustomerGroupRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<CustomerGroup>();
        DbSetCustomerGroupMapping = Db.Set<CustomerGroupMapping>();
        DbSetPromotionCustomerGroup = Db.Set<PromotionCustomerGroup>();
    }

    public void Add(CustomerGroup CustomerGroup)
    {
        DbSet.Add(CustomerGroup);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<CustomerGroup>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<CustomerGroup> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<CustomerGroup>> Filter(List<Guid> listId)
    {
        var query = DbSet.AsQueryable();
        query = query.Where(c => listId.Contains(c.Id));
        return await query.ToListAsync();
    }

    public void Remove(CustomerGroup CustomerGroup)
    {
        DbSet.Remove(CustomerGroup);
    }

    public void Update(CustomerGroup CustomerGroup)
    {
        DbSet.Update(CustomerGroup);
    }

    public async Task<CustomerGroup> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<(IEnumerable<CustomerGroup>, int)> Filter(string? keyword, int? status, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
        }
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.OrderBy(x => x.DisplayOrder).ToListAsync(), totalCount);
    }

    public async Task<IEnumerable<CustomerGroup>> GetListCbx(int? status)
    {
        var query = DbSet.AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public void Update(IEnumerable<CustomerGroup> t)
    {
        DbSet.UpdateRange(t);
    }

    public async Task<bool> IsNotBeingUsed(Guid id)
    {
        if (await DbSetCustomerGroupMapping.AnyAsync(x => x.CustomerGroupId == id))
        {
            return false;
        }
        if (await DbSetPromotionCustomerGroup.AnyAsync(x => x.CustomerGroupId == id))
        {
            return false;
        }
        return true;
    }

    public async Task<IEnumerable<CustomerGroup>> Filter(IEnumerable<string>? name)
    {
        var query = DbSet.Where(x => name.Any(y => y.Equals((x.Name.ToLower()))));
        return await query.ToListAsync();
    }
}
