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

namespace VFi.Infra.SO.Repository;

public class PriceListDetailRepository : IPriceListDetailRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<PriceListDetail> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public PriceListDetailRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PriceListDetail>();
    }

    public void Add(PriceListDetail productAttributeOption)
    {
        DbSet.Add(productAttributeOption);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<PriceListDetail>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<PriceListDetail> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(PriceListDetail productAttributeOption)
    {
        DbSet.Remove(productAttributeOption);
    }

    public void Update(PriceListDetail productAttributeOption)
    {
        DbSet.Update(productAttributeOption);
    }

    public async Task<IEnumerable<PriceListDetail>> GetListListBox(Dictionary<string, object> filter, string? keyword)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.ProductName.Contains(keyword) || x.ProductCode.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("priceListId"))
            {
                query = query.Where(x => x.PriceListId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.ToListAsync();
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<PriceListDetail>> Filter(Guid id)
    {
        return await DbSet.Where(x => x.PriceListId == id).OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public void Update(IEnumerable<PriceListDetail> details)
    {
        DbSet.UpdateRange(details);
    }
    public void Add(IEnumerable<PriceListDetail> details)
    {
        DbSet.AddRange(details);
    }
    public void Remove(IEnumerable<PriceListDetail> t)
    {
        DbSet.RemoveRange(t);
    }
}
