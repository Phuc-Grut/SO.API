using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

internal class PriceListPurchaseRepository : IPriceListPurchaseRepository
{
    protected readonly SqlCoreContext Db;

    protected readonly DbSet<PriceListPurchase> DbSet;

    public PriceListPurchaseRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PriceListPurchase>();
    }

    public IUnitOfWork UnitOfWork => Db;

    public void Add(PriceListPurchase t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<(IEnumerable<PriceListPurchase>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword) || x.Code.Contains(keyword));
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

    public async Task<IEnumerable<PriceListPurchase>> GetAll()
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
               Db.Customer.Any(x => x.Id.Equals(id))
            || Db.Order.Any(x => x.Id.Equals(id))
            || Db.Quotation.Any(x => x.Id.Equals(id))
            || Db.OrderProduct.Any(x => x.Id.Equals(id))
            );
    }

    public async Task<PriceListPurchase?> GetDefault()
    {
        return await DbSet.Include(x => x.PriceListPurchaseDetail).FirstOrDefaultAsync(x => x.Default == true);
    }

    public async Task<PriceListPurchase> GetById(Guid id)
    {
        return await DbSet.Include(x => x.PriceListPurchaseDetail).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<PriceListPurchase>> GetListCbx(Dictionary<string, object> filter)
    {
        var query = DbSet.Include(x => x.PriceListPurchaseDetail).AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("priceListPurchaseId"))
            {
                query = query.Where(x => x.PriceListPurchaseDetail.Any(x => x.PriceListPurchaseId.Equals(item.Value)));
            }
            if (item.Key.Equals("priceListPurchaseId") && filter.ContainsKey("buyFee") && filter["buyFee"] is not null)
            {
                query = query.Where(x => x.PriceListPurchaseDetail.Any(x => x.PriceListPurchaseId.Equals(item.Value)));
            }
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }

    public void Remove(PriceListPurchase t)
    {
        DbSet.Remove(t);
    }

    public void Update(PriceListPurchase t)
    {
        DbSet.Update(t);
    }
    public void Update(IEnumerable<PriceListPurchase> item)
    {
        DbSet.UpdateRange(item);
    }
    public void Sort(IEnumerable<PriceListPurchase> t)
    {
        foreach (PriceListPurchase item in t)
        {
            Db.Attach(item);
            Db.Entry(item).Property("DisplayOrder").IsModified = true;
        }
    }
}
