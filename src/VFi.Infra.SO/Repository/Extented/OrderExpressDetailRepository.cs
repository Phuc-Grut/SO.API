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

public class OrderExpressDetailRepository : IOrderExpressDetailRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<OrderExpressDetail> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public OrderExpressDetailRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<OrderExpressDetail>();
    }

    public void Add(OrderExpressDetail t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<OrderExpressDetail>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.Select(x => x);
        if (!String.IsNullOrEmpty(keyword))
        {
            query = DbSet.Where(x => x.ProductName.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("productName"))
            {
                query = query.Where(x => x.ProductName.Equals(item.Value));
            }
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<(IEnumerable<OrderExpressDetail>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x => x.ProductName.Contains(keyword) || x.Note.Contains(keyword));
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
            if (item.Key.Equals("productName"))
            {
                query = query.Where(x => x.ProductName.Equals(item.Value));
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<OrderExpressDetail>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<OrderExpressDetail> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.ProductImage.Equals(code));
    }

    public async Task<OrderExpressDetail> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<OrderExpressDetail>> GetListBox(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("code"))
            {
                query = query.Where(x => x.ProductName.Contains(item.Value.ToString()));
            }
        }
        return await query.ToListAsync();
    }

    public void Remove(OrderExpressDetail t)
    {
        DbSet.Remove(t);
    }

    public void Update(OrderExpressDetail t)
    {
        DbSet.Update(t);
    }

    public void Add(IEnumerable<OrderExpressDetail> items)
    {
        DbSet.AddRange(items);
    }

    public void Update(IEnumerable<OrderExpressDetail> items)
    {
        DbSet.UpdateRange(items);
    }

    public void Remove(IEnumerable<OrderExpressDetail> items)
    {
        DbSet.RemoveRange(items);
    }
}
