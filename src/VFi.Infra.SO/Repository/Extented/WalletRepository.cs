using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class WalletRepository : IWalletRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Wallet> DbSet;
    protected readonly DbSet<WalletTransaction> DbSetTransaction;
    public IUnitOfWork UnitOfWork => Db;
    public WalletRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Wallet>();
        DbSetTransaction = Db.Set<WalletTransaction>();
    }

    public void Add(Wallet t)
    {
        DbSet.Add(t);
    }
    public void AddTransaction(WalletTransaction t)
    {
        DbSetTransaction.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Wallet>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.Select(x => x);
        if (!String.IsNullOrEmpty(keyword))
        {
            query = DbSet.Where(x => x.WalletCode.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("code"))
            {
                query = query.Where(x => x.WalletCode.Equals(item.Value));
            }
            if (item.Key.Equals("accountid"))
            {
                query = query.Where(x => x.AccountId.Equals(Guid.Parse(item.Value.ToString())));
            }
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<(IEnumerable<Wallet>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x => x.WalletCode.Contains(keyword));
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.WalletCode.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("code"))
            {
                query = query.Where(x => x.WalletCode.Equals(item.Value));
            }
            if (item.Key.Equals("accountid"))
            {
                query = query.Where(x => x.AccountId.Equals(Guid.Parse(item.Value.ToString())));
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<Wallet>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Wallet> GetByCode(Guid accountId, string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.WalletCode.Equals(code) && x.AccountId.Equals(accountId));
    }


    public async Task<Wallet> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<Wallet>> GetListBox(Dictionary<string, object> filter)
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
                query = query.Where(x => x.WalletCode.Contains(item.Value.ToString()));
            }
        }
        return await query.ToListAsync();
    }

    public void Remove(Wallet t)
    {
        DbSet.Remove(t);
    }

    public void Update(Wallet t)
    {
        DbSet.Update(t);
    }
}
