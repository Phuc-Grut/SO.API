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

public class PromotionByValueRepository : IPromotionByValueRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<PromotionByValue> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public PromotionByValueRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PromotionByValue>();
    }

    public void Add(PromotionByValue productAttributeOption)
    {
        DbSet.Add(productAttributeOption);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<PromotionByValue>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<PromotionByValue> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(PromotionByValue productAttributeOption)
    {
        DbSet.Remove(productAttributeOption);
    }

    public void Update(PromotionByValue productAttributeOption)
    {
        DbSet.Update(productAttributeOption);
    }

    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<PromotionByValue>> Filter(Guid id)
    {
        return await DbSet.Where(x => x.PromotionId == id).ToListAsync();
    }
    public void Update(IEnumerable<PromotionByValue> details)
    {
        DbSet.UpdateRange(details);
    }
    public void Add(IEnumerable<PromotionByValue> details)
    {
        DbSet.AddRange(details);
    }
    public void Remove(IEnumerable<PromotionByValue> t)
    {
        DbSet.RemoveRange(t);
    }
}
