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

public class OrderFulfillmentDetailRepository : IOrderFulfillmentDetailRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<OrderFulfillmentDetail> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public OrderFulfillmentDetailRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<OrderFulfillmentDetail>();
    }

    public void Add(OrderFulfillmentDetail t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<OrderFulfillmentDetail>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.Select(x => x);
        if (!String.IsNullOrEmpty(keyword))
        {
            query = DbSet.Where(x => x.ProductImage.Contains(keyword));
        }
        foreach (var item in filter)
        {

            if (item.Key.Equals("productCode"))
            {
                query = query.Where(x => x.ProductCode.Equals(item.Value));
            }
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<(IEnumerable<OrderFulfillmentDetail>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x => x.ProductCode.Contains(keyword) || x.ProductName.Contains(keyword));
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.ProductName.Contains(keyword));
        }
        foreach (var item in filter)
        {

            if (item.Key.Equals("productCode"))
            {
                query = query.Where(x => x.ProductCode.Equals(item.Value));
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<OrderFulfillmentDetail>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<OrderFulfillmentDetail> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.ProductCode.Equals(code));
    }

    public async Task<OrderFulfillmentDetail> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<OrderFulfillmentDetail>> GetListBox(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {

            if (item.Key.Equals("productCode"))
            {
                query = query.Where(x => x.ProductCode.Contains(item.Value.ToString()));
            }
        }
        return await query.ToListAsync();
    }

    public void Remove(OrderFulfillmentDetail t)
    {
        DbSet.Remove(t);
    }

    public void Update(OrderFulfillmentDetail t)
    {
        DbSet.Update(t);
    }
}
