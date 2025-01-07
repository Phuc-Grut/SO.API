using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

internal class RequestPurchaseRepository : IRequestPurchaseRepository
{
    protected readonly SqlCoreContext Db;

    protected readonly DbSet<RequestPurchase> DbSet;

    //public RequestPurchaseRepository(IServiceProvider serviceProvider)
    //{
    //    var scope = serviceProvider.CreateScope();
    //    Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
    //    DbSet = Db.Set<RequestPurchase>();
    //}
    public RequestPurchaseRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<RequestPurchase>();
    }

    public IUnitOfWork UnitOfWork => Db;

    public void Add(RequestPurchase t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<(IEnumerable<RequestPurchase>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.Include(x => x.RequestPurchaseProduct).AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || x.RequestByName.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
            if (item.Key.Equals("employeeId"))
            {
                query = query.Where(x => x.RequestBy == new Guid(item.Value + ""));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<IEnumerable<RequestPurchase>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<RequestPurchase> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<RequestPurchase> GetById(Guid id)
    {
        return await DbSet.Include(x => x.RequestPurchaseProduct).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<RequestPurchase>> GetListCbx(int? status)
    {
        return await DbSet.ToListAsync();
    }

    public void Remove(RequestPurchase t)
    {
        DbSet.Remove(t);
    }

    public void Update(RequestPurchase t)
    {
        DbSet.Update(t);
    }
    public void Purchase(RequestPurchase t)
    {
        Db.Entry(t).Property(x => x.Podate).IsModified = true;
        Db.Entry(t).Property(x => x.Postatus).IsModified = true;
    }

    public async Task<RequestPurchase> CheckPO(string listPOCode)
    {
        return await DbSet.Include(x => x.RequestPurchaseProduct).FirstOrDefaultAsync(x => listPOCode.Contains(x.PurchaseRequestCode));
    }

    public async Task<IEnumerable<RequestPurchase>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();

        foreach (var item in filter)
        {
            if (item.Key.Equals("accountId"))
            {
                query = query.Where(x => x.RequestBy == (Guid)item.Value);
            }
        }
        return await query.ToListAsync();
    }

    public async Task<RequestPurchase> GetRemoveOrderId(Guid requestId, Guid? orderId)
    {
        return await DbSet
            .Include(x => x.RequestPurchaseProduct)
            .FirstOrDefaultAsync(x => x.Id == requestId &&
                                       x.RequestPurchaseProduct.Any(p => p.OrderId == orderId.Value));
    }

}
