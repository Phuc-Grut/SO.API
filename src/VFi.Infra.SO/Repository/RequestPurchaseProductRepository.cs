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

public class RequestPurchaseProductRepository : IRequestPurchaseProductRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<RequestPurchaseProduct> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    //public RequestPurchaseProductRepository(IServiceProvider serviceProvider)
    //{
    //    var scope = serviceProvider.CreateScope();
    //    Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
    //    DbSet = Db.Set<RequestPurchaseProduct>();
    //}
    public RequestPurchaseProductRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<RequestPurchaseProduct>();
    }

    public void Add(RequestPurchaseProduct productAttributeOption)
    {
        DbSet.Add(productAttributeOption);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<RequestPurchaseProduct>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<RequestPurchaseProduct> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(RequestPurchaseProduct productAttributeOption)
    {
        DbSet.Remove(productAttributeOption);
    }

    public void Update(RequestPurchaseProduct productAttributeOption)
    {
        DbSet.Update(productAttributeOption);
    }

    public async Task<IEnumerable<RequestPurchaseProduct>> GetListListBox(Dictionary<string, object> filter, string? keyword)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("requestPurchaseId"))
            {
                query = query.Where(x => x.RequestPurchaseId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.ToListAsync();
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<RequestPurchaseProduct>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("requestPurchaseId"))
            {
                query = query.Where(x => x.RequestPurchaseId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("orderProductId"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => list.Contains(x.OrderProductId.ToString()));
            }
            if (item.Key.Equals("orderIds"))
            {
                var orderIds = item.Value as List<Guid>;
                if (orderIds != null && orderIds.Any())
                {
                    query = query.Where(x => orderIds.Contains(x.OrderId ?? Guid.Empty));
                }
            }
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public async Task<(IEnumerable<RequestPurchaseProduct>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.Include(x => x.RequestPurchase).AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.RequestPurchase.Code.Contains(keyword));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }
    public void Update(IEnumerable<RequestPurchaseProduct> details)
    {
        DbSet.UpdateRange(details);
    }
    public void Add(IEnumerable<RequestPurchaseProduct> details)
    {
        DbSet.AddRange(details);
    }
    public void Remove(IEnumerable<RequestPurchaseProduct> t)
    {
        DbSet.RemoveRange(t);
    }
    public async Task<IEnumerable<RequestPurchaseProduct>> GetByOrderId(string code)
    {
        return await DbSet.Where(x => x.OrderCode == code).Include(x => x.RequestPurchase).ToListAsync();
    }
    public async Task<IEnumerable<RequestPurchaseProduct>> GetByRequestPurchaseId(Guid id)
    {
        return await DbSet.Where(x => x.RequestPurchaseId == id).ToListAsync();
    }
    public async Task<IEnumerable<RequestPurchaseProduct>> GetByOrderIds(IEnumerable<Guid> ids)
    {
        return await DbSet.Include(x => x.RequestPurchase)
            .Where(x => x.OrderId.HasValue && ids.Contains(x.OrderId.Value)).ToListAsync();
    }
}
