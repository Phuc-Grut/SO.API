using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository.Extentded;

public class OrderExpressRepository : IOrderExpressRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<OrderExpress> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public OrderExpressRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<OrderExpress>();
    }

    public void Add(OrderExpress t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<OrderExpress>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.Select(x => x);
        if (!string.IsNullOrEmpty(keyword))
        {
            query = DbSet.Where(x => x.Note.Contains(keyword));
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

    public async Task<(IEnumerable<OrderExpress>, int)> Filter(string? keyword, int? status, IFopRequest request)
    {
        var query = DbSet.Include(x => x.OrderExpressDetail).Include(x => x.OrderServiceAdd).Include(x => x.PaymentInvoice).AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x => x.Code.Contains(keyword) || x.Note.Contains(keyword));

        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x => x.Code.Contains(keyword) || x.Note.Contains(keyword));
        if (status is not null)
            query = query.Where(x => x.Status == status);
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<(IEnumerable<OrderExpress>, int)> Filter(string? keyword, Guid customerId, int? status, IFopRequest request)
    {
        var query = DbSet.Include(x => x.OrderExpressDetail).Include(x => x.OrderServiceAdd).Include(x => x.PaymentInvoice).AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x => (x.Code.Contains(keyword) || x.Note.Contains(keyword)));

        query = query.Where(x => x.CustomerId.Equals(customerId));

        if (status is not null)
            query = query.Where(x => x.Status == status);
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Note.Contains(keyword));
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

    public async Task<IEnumerable<OrderExpress>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<OrderExpress> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<OrderExpress> GetById(Guid id)
    {
        return await DbSet
            .Include(x => x.OrderExpressDetail)
            .Include(x => x.OrderServiceAdd)
            .Include(x => x.PaymentInvoice)
            .Include(x => x.OrderInvoice)
            .Include(x => x.OrderTracking)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<OrderExpress>> GetListBox(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("code"))
            {
                query = query.Where(x => x.Code.Contains(item.Value.ToString()));
            }
        }
        return await query.ToListAsync();
    }

    public void Remove(OrderExpress t)
    {
        DbSet.Remove(t);
    }

    public void Update(OrderExpress t)
    {
        DbSet.Update(t);
    }

    public async Task<OrderExpress?> GetByCode(Guid customerId, string code)
    {
        return await DbSet
            .Include(x => x.OrderExpressDetail)
            .Include(x => x.OrderServiceAdd)
            .Include(x => x.PaymentInvoice)
            .SingleOrDefaultAsync(x => x.CustomerId.Equals(customerId) && x.Code.Equals(code));
    }
    public void Approve(OrderExpress t)
    {
        Db.Entry(t).Property(x => x.Status).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveDate).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveBy).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveByName).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveComment).IsModified = true;
    }
}
