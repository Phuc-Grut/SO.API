using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class PriceListSurchargeRepository : IPriceListSurchargeRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<PriceListSurcharge> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public PriceListSurchargeRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PriceListSurcharge>();
    }
    public void Add(PriceListSurcharge t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<PriceListSurcharge>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.Select(x => x);
        if (!String.IsNullOrEmpty(keyword))
        {
            query = DbSet.Where(x => x.SurchargeGroup.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<(IEnumerable<PriceListSurcharge>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x => x.SurchargeGroup.Contains(keyword));
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.SurchargeGroup.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<PriceListSurcharge>> GetAll()
    {
        return await DbSet.ToListAsync();
    }


    public async Task<PriceListSurcharge> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<PriceListSurcharge>> GetListBox(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            else if (item.Key.Equals("routerShippingId"))
            {
                query = query.Where(x => x.RouterShippingId.Equals(item.Value));
            }
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }

    public void Remove(PriceListSurcharge t)
    {
        DbSet.Remove(t);
    }

    public void Update(PriceListSurcharge t)
    {
        DbSet.Update(t);
    }

    public void Add(IEnumerable<PriceListSurcharge> details)
    {
        DbSet.AddRange(details);
    }
    public void Sort(IEnumerable<PriceListSurcharge> t)
    {
        foreach (PriceListSurcharge item in t)
        {
            Db.Attach(item);
            Db.Entry(item).Property("DisplayOrder").IsModified = true;
        }
    }
}
