using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;

namespace VFi.Infra.SO.Repository;

public class PromotionCustomerGroupRepository : IPromotionCustomerGroupRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<PromotionCustomerGroup> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public PromotionCustomerGroupRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PromotionCustomerGroup>();
    }

    public void Add(PromotionCustomerGroup PromotionCustomerGroup)
    {
        DbSet.Add(PromotionCustomerGroup);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<PromotionCustomerGroup>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("customerGroupId"))
            {
                query = query.Where(x => x.CustomerGroupId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("promotionId"))
            {
                query = query.Where(x => x.PromotionId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("customerGroupId"))
            {
                query = query.Where(x => x.CustomerGroupId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("promotionId"))
            {
                query = query.Where(x => x.PromotionId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<PromotionCustomerGroup>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<PromotionCustomerGroup> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(PromotionCustomerGroup PromotionCustomerGroup)
    {
        DbSet.Remove(PromotionCustomerGroup);
    }

    public void Update(PromotionCustomerGroup PromotionCustomerGroup)
    {
        DbSet.Update(PromotionCustomerGroup);
    }

    public async Task<IEnumerable<PromotionCustomerGroup>> GetListListBox(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("customerGroupId"))
            {
                query = query.Where(x => x.CustomerGroupId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("promotionId"))
            {
                query = query.Where(x => x.PromotionId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.ToListAsync();
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<PromotionCustomerGroup>> Filter(Guid id)
    {
        return await DbSet.Where(x => x.PromotionId == id || x.CustomerGroupId == id).Include(x => x.CustomerGroup).ToListAsync();
    }
    public void Add(IEnumerable<PromotionCustomerGroup> options)
    {
        DbSet.AddRange(options);
    }
    public void Remove(IEnumerable<PromotionCustomerGroup> t)
    {
        DbSet.RemoveRange(t);
    }
}
