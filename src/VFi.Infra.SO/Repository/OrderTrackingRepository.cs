using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class OrderTrackingRepository : IOrderTrackingRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<OrderTracking> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public OrderTrackingRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<OrderTracking>();
    }
    public void Add(OrderTracking t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<OrderTracking>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<OrderTracking> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<OrderTracking>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("orderId"))
            {
                query = query.Where(x => x.OrderId.Equals(item.Value + ""));
            }
        }
        return await query.ToListAsync();
    }

    public void Remove(OrderTracking t)
    {
        DbSet.Remove(t);
    }

    public void Update(OrderTracking t)
    {
        DbSet.Update(t);
    }
    public void Update(IEnumerable<OrderTracking> details)
    {
        DbSet.UpdateRange(details);
    }
    public void Add(IEnumerable<OrderTracking> details)
    {
        DbSet.AddRange(details);
    }
    public void Remove(IEnumerable<OrderTracking> t)
    {
        DbSet.RemoveRange(t);
    }
}
