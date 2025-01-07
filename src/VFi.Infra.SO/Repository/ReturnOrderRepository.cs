using System;
using System.Collections.Generic;
using System.Linq;
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

namespace VFi.Infra.SO.Repository;

internal class ReturnOrderRepository : IReturnOrderRepository
{
    protected readonly SqlCoreContext Db;

    protected readonly DbSet<ReturnOrder> DbSet;

    public ReturnOrderRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<ReturnOrder>();
    }

    public IUnitOfWork UnitOfWork => Db;

    public void Add(ReturnOrder t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<(IEnumerable<ReturnOrder>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || x.CustomerName.Contains(keyword) || x.CreatedByName.Contains(keyword) ||
            x.CurrencyName.Contains(keyword) || x.WarehouseName.Contains(keyword) || x.OrderCode.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
            if (item.Key.Equals("employeeId"))
            {
                query = query.Where(x => x.AccountId == new Guid(item.Value + ""));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }


    public async Task<IEnumerable<ReturnOrder>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ReturnOrder> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<ReturnOrder> GetById(Guid id)
    {
        return await DbSet.Include(x => x.PaymentInvoice).Include(x => x.ReturnOrderProduct).ThenInclude(y => y.OrderProduct).ThenInclude(z => z.Order).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<ReturnOrder>> GetListCbx(int? status)
    {
        return await DbSet.ToListAsync();
    }

    public void Remove(ReturnOrder t)
    {
        DbSet.Remove(t);
    }

    public void Update(ReturnOrder t)
    {
        DbSet.Update(t);
    }
    public void UploadFile(ReturnOrder t)
    {
        Db.Entry(t).Property(x => x.File).IsModified = true;
    }

    public async Task<IEnumerable<ReturnOrder>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();

        foreach (var item in filter)
        {
            if (item.Key.Equals("accountId"))
            {
                query = query.Where(x => x.AccountId == (Guid)item.Value);
            }
        }
        return await query.ToListAsync();
    }
}
