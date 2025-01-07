using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using VFi.NetDevPack.Utilities;

namespace VFi.Infra.SO.Repository;

internal class PriceListRepository : IPriceListRepository
{
    protected readonly SqlCoreContext Db;

    protected readonly DbSet<PriceList> DbSet;

    public PriceListRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PriceList>();
    }

    public IUnitOfWork UnitOfWork => Db;

    public void Add(PriceList t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<(IEnumerable<PriceList>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%") || x.Code.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<IEnumerable<PriceList>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public bool CheckByCode(string? code, Guid? id)
    {
        if (id is null)
        {
            return !DbSet.Any(x => x.Code.Equals(code));
        }
        else
        {
            return !DbSet.Any(x => x.Code.Equals(code) && x.Id != id);
        }
    }

    public bool CheckUsing(Guid id)
    {
        return !(
               Db.Customer.Any(x => x.PriceListId.Equals(id))
            || Db.Order.Any(x => x.PriceListId.Equals(id))
            || Db.Quotation.Any(x => x.PriceListId.Equals(id))
            || Db.OrderProduct.Any(x => x.PriceListId.Equals(id))
            );
    }

    public async Task<PriceList> GetById(Guid id)
    {
        return await DbSet.Include(x => x.PriceListDetail).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<PriceList>> GetListCbx(Dictionary<string, object> filter)
    {
        var query = DbSet.Include(x => x.PriceListDetail).AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("date"))
            {
                var date = item.Value != "" ? Convert.ToDateTime(item.Value) : DateTime.Now;
                query = query.Where(x => x.StartDate <= date && (x.EndDate >= date || x.EndDate == null));
            }
            if (item.Key.Equals("productId"))
            {
                query = query.Where(x => x.PriceListDetail.Any(x => x.ProductId.Equals(item.Value)));
            }
            if (item.Key.Equals("quantity_productId") && filter.ContainsKey("quantity") && filter["quantity"] is not null)
            {
                query = query.Where(x => x.PriceListDetail.Any(x => x.ProductId.Equals(item.Value) && x.QuantityMin <= Convert.ToDouble(filter["quantity"])));
            }
            if (item.Key.Equals("currency"))
            {
                query = query.Where(x => x.Currency.Equals(item.Value));
            }
        }
        return await query.ToListAsync();
    }

    public void Remove(PriceList t)
    {
        DbSet.Remove(t);
    }

    public void Update(PriceList t)
    {
        DbSet.Update(t);
    }
    public void Update(IEnumerable<PriceList> item)
    {
        DbSet.UpdateRange(item);
    }
}
