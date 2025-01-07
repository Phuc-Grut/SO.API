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

internal class PromotionRepository : IPromotionRepository
{
    protected readonly SqlCoreContext Db;

    protected readonly DbSet<Promotion> DbSet;

    public PromotionRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Promotion>();
    }

    public IUnitOfWork UnitOfWork => Db;

    public void Add(Promotion t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Promotion>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = DbSet.Where(x => x.Code.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("date"))
            {
                var date = item.Value != "" ? Convert.ToDateTime(item.Value) : DateTime.Now;
                query = query.Where(x => x.StartDate <= date && x.EndDate >= date);
            }
        }
        return await query.OrderBy(x => x.Code).Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }
    public async Task<(IEnumerable<Promotion>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request, Guid? promotionGroupId)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || x.Code.Contains(keyword));
        }
        if (promotionGroupId != null)
        {
            query = query.Where(x => x.PromotionGroupId == promotionGroupId);
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
    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = DbSet.Where(x => x.Code.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("date"))
            {
                var date = item.Value != "" ? Convert.ToDateTime(item.Value) : DateTime.Now;
                query = query.Where(x => x.StartDate <= date && x.EndDate >= date);
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<Promotion>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Promotion> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<Promotion> GetById(Guid id)
    {
        return await DbSet.Include(x => x.PromotionByValue).ThenInclude(x => x.PromotionProductBuy).Include(x => x.PromotionByValue).ThenInclude(y => y.PromotionProduct).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Promotion>> GetListCbx(int? status)
    {
        return await DbSet.ToListAsync();
    }

    public void Remove(Promotion t)
    {
        DbSet.Remove(t);
    }

    public void Update(Promotion t)
    {
        DbSet.Update(t);
    }
}
