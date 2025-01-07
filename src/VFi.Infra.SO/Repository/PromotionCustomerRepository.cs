using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;

namespace VFi.Infra.SO.Repository;

public class PromotionCustomerRepository : IPromotionCustomerRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<PromotionCustomer> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public PromotionCustomerRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PromotionCustomer>();
    }

    public void Add(PromotionCustomer PromotionCustomer)
    {
        DbSet.Add(PromotionCustomer);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<PromotionCustomer>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId.Equals(new Guid(item.Value + "")));
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
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("promotionId"))
            {
                query = query.Where(x => x.PromotionId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<PromotionCustomer>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<PromotionCustomer> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(PromotionCustomer PromotionCustomer)
    {
        DbSet.Remove(PromotionCustomer);
    }

    public void Update(PromotionCustomer PromotionCustomer)
    {
        DbSet.Update(PromotionCustomer);
    }

    public async Task<IEnumerable<PromotionCustomer>> GetListListBox(Dictionary<string, object> filter)
    {
        var query = DbSet.Include(x => x.Customer).AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId.Equals(new Guid(item.Value + "")));
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

    public async Task<IEnumerable<PromotionCustomer>> Filter(Guid id)
    {
        return await DbSet.Include(x => x.Customer).Where(x => x.PromotionId == id).ToListAsync();
    }
    public void Add(IEnumerable<PromotionCustomer> options)
    {
        DbSet.AddRange(options);
    }
    public void Remove(IEnumerable<PromotionCustomer> t)
    {
        DbSet.RemoveRange(t);
    }
}
