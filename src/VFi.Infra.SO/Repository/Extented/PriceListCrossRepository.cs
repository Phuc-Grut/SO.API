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

public class PriceListCrossRepository : IPriceListCrossRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<PriceListCross> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public PriceListCrossRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PriceListCross>();
    }
    public void Add(PriceListCross t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<PriceListCross>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.Select(x => x);
        if (!String.IsNullOrEmpty(keyword))
        {
            query = DbSet.Where(x => x.Name.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("code"))
            {
                query = query.Where(x => x.Code.Equals(item.Value));
            }
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<(IEnumerable<PriceListCross>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("code"))
            {
                query = query.Where(x => x.Code.Equals(item.Value));
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<PriceListCross>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<PriceListCross> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<PriceListCross> GetById(Guid id)
    {
        return await DbSet.Include(x => x.PriceListCrossDetail).FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<PriceListCross> GetDefaultByRouterShipping(Guid routerShipping)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Default == true && x.RouterShippingId.Equals(routerShipping));
    }

    public async Task<IEnumerable<PriceListCross>> GetListBox(string keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }
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
            if (item.Key.Equals("code"))
            {
                query = query.Where(x => x.Code.Contains(item.Value.ToString()));
            }
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }

    public void Remove(PriceListCross t)
    {
        DbSet.Remove(t);
    }

    public void Update(PriceListCross t)
    {
        DbSet.Update(t);
    }

    public void Sort(IEnumerable<PriceListCross> t)
    {
        foreach (PriceListCross item in t)
        {
            Db.Attach(item);
            Db.Entry(item).Property("DisplayOrder").IsModified = true;
        }
    }

}
