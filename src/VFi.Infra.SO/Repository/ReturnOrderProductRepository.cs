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

namespace VFi.Infra.SO.Repository;

public class ReturnOrderProductRepository : IReturnOrderProductRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ReturnOrderProduct> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ReturnOrderProductRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<ReturnOrderProduct>();
    }

    public void Add(ReturnOrderProduct productAttributeOption)
    {
        DbSet.Add(productAttributeOption);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<ReturnOrderProduct>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ReturnOrderProduct> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(ReturnOrderProduct productAttributeOption)
    {
        DbSet.Remove(productAttributeOption);
    }

    public void Update(ReturnOrderProduct productAttributeOption)
    {
        DbSet.Update(productAttributeOption);
    }

    public async Task<IEnumerable<ReturnOrderProduct>> GetListListBox(Dictionary<string, object> filter, string? keyword)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("returnOrderId"))
            {
                query = query.Where(x => x.ReturnOrderId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.ToListAsync();
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<ReturnOrderProduct>> Filter(Guid id)
    {
        return await DbSet.Where(x => x.ReturnOrderId == id).ToListAsync();
    }
    public void Update(IEnumerable<ReturnOrderProduct> details)
    {
        DbSet.UpdateRange(details);
    }
    public void Add(IEnumerable<ReturnOrderProduct> details)
    {
        DbSet.AddRange(details);
    }
    public void Remove(IEnumerable<ReturnOrderProduct> t)
    {
        DbSet.RemoveRange(t);
    }
    public async Task<IEnumerable<ReturnOrderProduct>> GetByParentId(Guid id)
    {
        return await DbSet.Where(x => x.OrderProductId == id || x.ReturnOrderId == id).ToListAsync();
    }
    public async Task<IEnumerable<ReturnOrderProduct>> GetByOrderId(string code)
    {
        return await DbSet.Include(x => x.OrderProduct).ThenInclude(y => y.Order).Where(x => x.OrderProduct.Order.Code == code).Include(x => x.ReturnOrder).ToListAsync();
    }

}
