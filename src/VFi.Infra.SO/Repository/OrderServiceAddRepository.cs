using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VFi.Infra.SO.Repository;

public class OrderServiceAddRepository : IOrderServiceAddRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<OrderServiceAdd> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public OrderServiceAddRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<OrderServiceAdd>();
    }

    public void Add(OrderServiceAdd OrderServiceAdd)
    {
        DbSet.Add(OrderServiceAdd);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<OrderServiceAdd>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("orderId"))
            {
                query = query.Where(x => x.OrderId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("serviceAddId"))
            {
                query = query.Where(x => x.ServiceAddId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("orderId"))
            {
                query = query.Where(x => x.OrderId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("serviceAddId"))
            {
                query = query.Where(x => x.ServiceAddId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<OrderServiceAdd>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<OrderServiceAdd> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(OrderServiceAdd OrderServiceAdd)
    {
        DbSet.Remove(OrderServiceAdd);
    }

    public void Update(OrderServiceAdd OrderServiceAdd)
    {
        DbSet.Update(OrderServiceAdd);
    }

    public async Task<IEnumerable<OrderServiceAdd>> GetListListBox(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("orderId"))
            {
                query = query.Where(x => x.OrderId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("serviceAddId"))
            {
                query = query.Where(x => x.ServiceAddId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.ToListAsync();
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<OrderServiceAdd>> Filter(Dictionary<string, object> filter)
    {

        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("orderId"))
            {
                query = query.Where(x => x.OrderId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("quotationId"))
            {
                query = query.Where(x => x.QuotationId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.OrderBy(x => x.CreatedDate).ToListAsync();
    }
    public void Add(IEnumerable<OrderServiceAdd> items)
    {
        DbSet.AddRange(items);
    }
    public void Update(IEnumerable<OrderServiceAdd> items)
    {
        DbSet.UpdateRange(items);
    }
    public void Remove(IEnumerable<OrderServiceAdd> t)
    {
        DbSet.RemoveRange(t);
    }
}
