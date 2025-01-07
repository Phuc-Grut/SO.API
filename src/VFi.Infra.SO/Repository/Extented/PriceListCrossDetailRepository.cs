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

public class PriceListCrossDetailRepository : IPriceListCrossDetailRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<PriceListCrossDetail> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public PriceListCrossDetailRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PriceListCrossDetail>();
    }
    public void Add(PriceListCrossDetail t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<PriceListCrossDetail>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.Select(x => x);
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.CommodityGroupCode.Contains(keyword) || x.CommodityGroupName.Contains(keyword));
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

    public async Task<(IEnumerable<PriceListCrossDetail>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x => x.CommodityGroupCode.Contains(keyword) || x.CommodityGroupName.Contains(keyword) || x.Note.Contains(keyword));
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<(IEnumerable<PriceListCrossDetail>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.CommodityGroupCode.Contains(keyword) || x.CommodityGroupName.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
            else if (item.Key.Equals("priceListCrossId"))
            {
                query = query.Where(x => x.PriceListCrossId.Equals(item.Value));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.OrderBy(x => x.DisplayOrder).ToListAsync(), totalCount);
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.CommodityGroupCode.Contains(keyword) || x.CommodityGroupName.Contains(keyword));
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

    public async Task<IEnumerable<PriceListCrossDetail>> GetAll()
    {
        return await DbSet.ToListAsync();
    }



    public async Task<PriceListCrossDetail> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<PriceListCrossDetail>> GetListBox(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {

        }
        return await query.ToListAsync();
    }

    public void Remove(PriceListCrossDetail t)
    {
        DbSet.Remove(t);
    }

    public void Update(PriceListCrossDetail t)
    {
        DbSet.Update(t);
    }

    public void Update(IEnumerable<PriceListCrossDetail> details)
    {
        DbSet.UpdateRange(details);
    }
    public void Add(IEnumerable<PriceListCrossDetail> details)
    {
        DbSet.AddRange(details);
    }
    public void Remove(IEnumerable<PriceListCrossDetail> t)
    {
        DbSet.RemoveRange(t);
    }
}
