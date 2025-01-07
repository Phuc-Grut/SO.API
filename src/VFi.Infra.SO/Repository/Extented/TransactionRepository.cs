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

public class TransactionRepository : ITransactionRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Transaction> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public TransactionRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Transaction>();
    }

    public void Add(Transaction t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Transaction>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.Select(x => x);
        if (!String.IsNullOrEmpty(keyword))
        {
            query = DbSet.Where(x => x.RawData.Contains(keyword));
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

    public async Task<(IEnumerable<Transaction>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x => x.Code.Contains(keyword) || x.RawData.Contains(keyword));
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.RawData.Contains(keyword));
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

    public async Task<IEnumerable<Transaction>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Transaction> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<Transaction> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<Transaction>> GetListBox(Dictionary<string, object> filter)
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

    public void Remove(Transaction t)
    {
        DbSet.Remove(t);
    }

    public void Update(Transaction t)
    {
        DbSet.Update(t);
    }
}
