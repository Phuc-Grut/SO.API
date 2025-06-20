﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class ExpenseRepository : IExpenseRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Expense> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ExpenseRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Expense>();
    }

    public void Add(Expense Expense)
    {
        DbSet.Add(Expense);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Expense>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Expense> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(Expense Expense)
    {
        DbSet.Remove(Expense);
    }

    public void Update(Expense Expense)
    {
        DbSet.Update(Expense);
    }

    public async Task<Expense> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<(IEnumerable<Expense>, int)> Filter(string? keyword, int? status, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
        }
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.OrderBy(x => x.DisplayOrder).ToListAsync(), totalCount);
    }


    public async Task<IEnumerable<Expense>> GetListCbx(int? status)
    {
        var query = DbSet.AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public void Update(IEnumerable<Expense> t)
    {
        DbSet.UpdateRange(t);
    }
}
