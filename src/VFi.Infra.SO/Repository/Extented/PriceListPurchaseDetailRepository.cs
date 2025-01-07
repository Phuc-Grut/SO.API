using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul.Filtering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class PriceListPurchaseDetailRepository : IPriceListPurchaseDetailRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<PriceListPurchaseDetail> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public PriceListPurchaseDetailRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PriceListPurchaseDetail>();
    }

    public void Add(PriceListPurchaseDetail productAttributeOption)
    {
        DbSet.Add(productAttributeOption);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<PriceListPurchaseDetail>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<PriceListPurchaseDetail> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(PriceListPurchaseDetail productAttributeOption)
    {
        DbSet.Remove(productAttributeOption);
    }

    public void Update(PriceListPurchaseDetail productAttributeOption)
    {
        DbSet.Update(productAttributeOption);
    }

    public async Task<IEnumerable<PriceListPurchaseDetail>> GetListListBox(Dictionary<string, object> filter, string? keyword)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.PurchaseGroupCode.Contains(keyword) || x.PurchaseGroupName.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("priceListPurchaseId"))
            {
                query = query.Where(x => x.PriceListPurchaseId.Equals(item.Value));
            }
            else if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status.Equals(item.Value));
            }
        }
        return await query.ToListAsync();
    }

    public async Task<(IEnumerable<PriceListPurchaseDetail>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.PurchaseGroupCode.Contains(keyword) || x.PurchaseGroupName.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
            else if (item.Key.Equals("priceListPurchaseId"))
            {
                query = query.Where(x => x.PriceListPurchaseId.Equals(item.Value));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.OrderBy(x => x.DisplayOrder).ToListAsync(), totalCount);
    }

    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public void Update(IEnumerable<PriceListPurchaseDetail> details)
    {
        DbSet.UpdateRange(details);
    }
    public void Add(IEnumerable<PriceListPurchaseDetail> details)
    {
        DbSet.AddRange(details);
    }
    public void Remove(IEnumerable<PriceListPurchaseDetail> t)
    {
        DbSet.RemoveRange(t);
    }
    public async Task<IEnumerable<PriceListPurchaseDetail>> Filter(Guid id)
    {
        return await DbSet.Where(x => x.Id == id).OrderBy(x => x.Id).ToListAsync();
    }
}
